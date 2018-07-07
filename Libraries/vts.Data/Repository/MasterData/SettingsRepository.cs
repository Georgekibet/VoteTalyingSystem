using RefactorThis.GraphDiff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using vts.Core.Shared.Entities.Master;
using vts.Data.Context;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;

namespace vts.Data.Repository.MasterData
{
    public class SettingsRepository : BaseRepository<Settings, SettingsRef>, ISettingsRepository
    {
        public SettingsRepository(ContextConnection contextConnection)
             : base(contextConnection)
        {
        }

        protected override Func<Settings, List<Settings>, ValidationResult[]> ValidationFunc
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                return (itemToCheck, allItems) =>
                {
                    var itemsToCheck = allItems.Where(n => n.Id != itemToCheck.Id);

                    var dupeId = itemsToCheck.Any(n => n.Key == itemToCheck.Key);
                    if (dupeId) validationResults.Add(new ValidationResult("Duplicate Settings Key found"));

                    var validation = itemToCheck.Validate();
                    if (!validation.IsValid)
                        validationResults.AddRange(validation.Results);
                    return validationResults.ToArray();
                };
            }
        }

        protected override Func<string, List<Settings>, List<Settings>> SearchFunc
        {
            get
            {
                return (searchText, allItems) =>
                {
                    if (string.IsNullOrEmpty(searchText)) return allItems;
                    var st = searchText.ToLower();

                    return
                        allItems.Where(
                            n =>
                                n.Key.ToString().ToLower().Contains(st))
                            .ToList();
                };
            }
        }

        public Guid Save(Settings entity, bool? isSync = null)
        {
            var vri = new ValidationResultInfo();

            if (isSync == null || !isSync.Value)
            {
                vri = Validate(entity);
            }

            if (!vri.IsValid)
            {
                throw new DomainValidationException(vri, "Settings not valid");
            }
            DateTime dt = DateTime.Now;
            using (var ctx = GetVtsContext("SettingsRepository"))
            {
                Settings settings = GetById(entity.Id);
                if (settings == null)
                {
                    entity.Status = EntityStatus.Active;
                    entity.DateCreated = dt;
                }
                entity.DateLastUpdated = dt;
                ctx.UpdateGraph(entity);

                ctx.SaveChanges();
            }

            return entity.Id;
        }

        private void SetStatus(Settings entity, EntityStatus status)
        {
            using (var ctx = GetVtsContext("SettingsRepository"))
            {
                Settings c = ctx.Settings.Find(entity.Id);
                if (c != null)
                {
                    if (c.Status == status) return;
                    c.Status = status;
                    c.DateLastUpdated = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        public void SetInactive(Settings entity)
        {
            SetStatus(entity, EntityStatus.Inactive);
        }

        public void SetActive(Settings entity)
        {
            SetStatus(entity, EntityStatus.Active);
        }

        public void SetAsDeleted(Settings entity)
        {
            SetStatus(entity, EntityStatus.Deleted);
        }

        public override Settings GetById(Guid id, bool includeDeactivated = false)
        {
            Settings settings = null;
            using (var ctx = GetVtsContext("SettingsRepository"))
            {
                settings = CtxSetup(ctx.Settings).FirstOrDefault(n => n.Id == id);
            }
            return settings;
        }

        public override IEnumerable<Settings> GetAll(bool includeDeactivated = false)
        {
            List<Settings> settings = new List<Settings>();

            using (var ctx = GetVtsContext("SettingsRepository"))
            {
                if (includeDeactivated)
                    settings =
                    CtxSetup(ctx.Settings)
                        .Where(n => n.Status != EntityStatus.Deleted)
                        .ToList();
                else
                    settings =
                    CtxSetup(ctx.Settings)
                        .Where(n => n.Status != EntityStatus.Deleted && n.Status != EntityStatus.Inactive)
                        .ToList();
            }
            return settings;
        }

        public Settings GetByKey(SettingsKeys key)
        {
            Settings settings = null;
            using (var ctx = GetVtsContext("SettingsRepository"))
            {
                settings = CtxSetup(ctx.Settings).FirstOrDefault(n => n.Key == key);
            }
            return settings;
        }

        public ValidationResultInfo Validate(Settings itemToValidate)
        {
            return base.Validate(itemToValidate, GetAll(true).ToList());
        }
    }
}