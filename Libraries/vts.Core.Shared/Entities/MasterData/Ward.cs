using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class Ward : MasterEntity<WardRef>
    {
        public Ward() : base(default(Guid))
        {
            WardMcas = new List<WardMcas>();
        }

        public Ward(Guid id) : base(id)
        {
            WardMcas = new List<WardMcas>();
        }

        public Ward(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : base(id, dateCreated, dateLastUpdated, status)
        {
            WardMcas = new List<WardMcas>();
        }

        [Required(ErrorMessage = "Ward Name is required")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Constituency is required")]
        public ConstituencyRef Constituency { get; set; }

        public string Code { get; set; }

        public List<WardMcas> WardMcas { get; set; }

        public override WardRef GetMasterDataRef()
        {
            return new WardRef(this.Id, this.Name);
        }

        public override ValidationResultInfo Validate()
        {
            var validationInfo = this.BasicValidation();
            return validationInfo;
        }
    }

    [ComplexType]
    public class WardRef : MasterDataRef
    {
        public WardRef()
        {
        }

        public WardRef(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }

        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.Ward; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(Name))
                message += "Invalid Name";
            if (Id == Guid.Empty)
                message += "Invalid Ward id";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static WardRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<WardRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
            this.Name = "";
        }

        #region Equality

        protected bool Equals(WardRef other)
        {
            return Id.Equals(other.Id) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WardRef)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        #endregion Equality
    }
}