using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class Region : MasterEntity<RegionRef>
    {
        public Region() : base(default(Guid))
        {
        }

        public Region(Guid id) : base(id)
        {
        }

        public Region(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : base(id, dateCreated, dateLastUpdated, status)
        {
        }

        [Required(ErrorMessage = "Region Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Region Code is required")]
        public string Code { get; set; }

        public override RegionRef GetMasterDataRef()
        {
            return new RegionRef(this.Id, this.Name);
        }

        public override ValidationResultInfo Validate()
        {
            var validationInfo = this.BasicValidation();
            return validationInfo;
        }
    }

    [ComplexType]
    public class RegionRef : MasterDataRef
    {
        public RegionRef()
        {
        }

        public RegionRef(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }

        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.Region; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(Name))
                message += "Invalid Name";
            if (Id == Guid.Empty)
                message += "Invalid Region id";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static RegionRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<RegionRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
            this.Name = "";
        }

        #region Equality

        protected bool Equals(RegionRef other)
        {
            return Id.Equals(other.Id) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RegionRef)obj);
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