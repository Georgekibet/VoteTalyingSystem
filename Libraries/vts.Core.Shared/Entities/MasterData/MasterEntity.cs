using System;
using vts.Shared.Services;

namespace vts.Core.Shared.Entities.Master
{
    public enum EntityStatus { New = 0, Active = 1, Inactive = 2, Deleted = 3 }

    public abstract class MasterDataRef
    {
        public Guid Id { get; set; }
        public abstract MasterDataCollection MasterDataType { get; }

        public abstract Tuple<bool, string> IsValid();

        protected static T CreateEmpty<T>() where T : MasterDataRef, new()
        {
            T o = new T();
            o.CreatEmptyImpl();
            return o;
        }

        protected abstract void CreatEmptyImpl();
    }

    public abstract class MasterEntity<T> where T : MasterDataRef
    {
        protected MasterEntity(Guid id)
        {
            Id = id;
        }

        protected MasterEntity(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : this(id)
        {
            DateCreated = dateCreated;
            DateLastUpdated = dateLastUpdated;
            Status = status;
        }

        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        internal void _SetDateCreated(DateTime dateCreated)
        {
            DateCreated = dateCreated;
        }

        public DateTime DateLastUpdated { get; set; }

        internal void _SetDateLastUpdated(DateTime dateLastUpdated)
        {
            DateLastUpdated = dateLastUpdated;
        }

        public EntityStatus Status { get; set; }

        internal void _SetStatus(EntityStatus status)
        {
            Status = status;
        }

        public abstract T GetMasterDataRef();
        public abstract ValidationResultInfo Validate();
    }
}