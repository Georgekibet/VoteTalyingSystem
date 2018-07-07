using System;
using System.Collections.Generic;
using vts.Shared.Services;
using vts.Shared.Utility;

namespace vts.Shared.Repository
{
    public interface IMasterRepository<T> : IValidation<T> where T : class
    {
        Guid Save(T entity, bool? isSync = null);
        void SetInactive(T entity);
        void SetActive(T entity);
        void SetAsDeleted(T entity);
        T GetById(Guid id, bool includeDeactivated = false);
        IEnumerable<T> GetAll(bool includeDeactivated = false);
        bool GetItemUpdatedSinceDateTime(DateTime dateTime);
        DateTime GetLastTimeItemUpdated();
        IEnumerable<T> GetItemUpdated(DateTime dateTime);
        int GetCount(bool includeDeactivated = false);
        IPaginatedList<T> GetAll(int currentPage, int itemPerPage, string searchText, bool includeDeactivated = false);

    }
}
