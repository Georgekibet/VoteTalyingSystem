using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using vts.Core.Shared.Entities.Master;
using vts.Data.Context;
using vts.Shared.Services;
using vts.Shared.Utility;

namespace vts.Data.Repository.MasterData
{
    public abstract class BaseRepository<T, R> where T : MasterEntity<R> where R : MasterDataRef
    {
        protected string ConnectionString;

        protected IQueryable<T> CtxSetup(IQueryable<T> ctx)
        {
            return ctx.AsNoTracking();
        }

        protected abstract Func<T, List<T>, ValidationResult[]> ValidationFunc { get; }
        protected abstract Func<string, List<T>, List<T>> SearchFunc { get; }

        public BaseRepository(ContextConnection contextConnection)
        {
            ConnectionString = contextConnection.VtsConnectionString;
        }

        protected VtsContext GetVtsContext(string info)
        {
            var ctx = new VtsContext(ConnectionString);
            return ctx;
        }

        public abstract T GetById(Guid id, bool includeDeactivated = false);

        public abstract IEnumerable<T> GetAll(bool includeDeactivated = false);

        public IPaginatedList<T> GetAll(int currentPage, int itemPerPage, string searchText, bool includeDeactivated = false)
        {
            searchText = searchText.ToLower();
            var filtered = SearchFunc(searchText, GetAll(includeDeactivated).ToList());
            return new PaginatedList<T>(filtered.AsQueryable(), currentPage, itemPerPage, filtered.Count());
        }

        public bool GetItemUpdatedSinceDateTime(DateTime dateTime)
        {
            return GetAll(true).Any(n => n.DateLastUpdated > dateTime);
        }

        public DateTime GetLastTimeItemUpdated()
        {
            return GetAll(true).Select(n => n.DateLastUpdated).Max();
        }

        public IEnumerable<T> GetItemUpdated(DateTime dateTime)
        {
            return GetAll(true).Where(n => n.DateLastUpdated > dateTime).ToList();
        }

        public int GetCount(bool includeDeactivated = false)
        {
            return GetAll(includeDeactivated).Count();
        }

        #region Validation

        protected virtual ValidationResultInfo Validate(T itemToValidate, List<T> allItems)
        {
            ValidationResultInfo vri = itemToValidate.BasicValidation();
            if (itemToValidate.Status == EntityStatus.Inactive || itemToValidate.Status == EntityStatus.Deleted)
                return vri;
            if (itemToValidate.Id == Guid.Empty)
                vri.Results.Add(new ValidationResult("Enter Valid  Guid ID"));
            ValidationResult[] validations = ValidationFunc(itemToValidate, allItems);

            if (validations.Any())
                vri.Results.AddRange(validations);
            return vri;
        }

        #endregion Validation
    }
}