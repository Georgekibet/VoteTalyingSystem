using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class PollingCentre : MasterEntity<PollingCentreRef>
    {
        public PollingCentre() : base(default(Guid))
        {
        }
        public PollingCentre(Guid id) : base(id)
        {
        }

        public PollingCentre(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : base(id, dateCreated, dateLastUpdated, status)
        {
        }

        [Required(ErrorMessage = "Polling Centre Name is required")]
        public string Name { get; set; }

        public WardRef Ward { get; set; }

        [Required(ErrorMessage = "Polling Centre Code is required")]
        public string Code { get; set; }

        public int RegisteredVoters { get; set; }
        public int Streams { get; set; }
        public PollingCentreType PollingCentreType { get; set; }

        public override PollingCentreRef GetMasterDataRef()
        {
            return new PollingCentreRef(this.Id, this.Name);
        }
        public override ValidationResultInfo Validate()
        {
            var validationInfo = this.BasicValidation();
            return validationInfo;
        }
    }

    [ComplexType]
    public class PollingCentreRef : MasterDataRef
    {
        public PollingCentreRef()
        {
        }

        public PollingCentreRef(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }

        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.PollingCentre; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(Name))
                message += "Invalid Name";
            if (Id == Guid.Empty)
                message += "Invalid polling centre id";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static PollingCentreRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<PollingCentreRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
            this.Name = "";
        }

        #region Equality

        protected bool Equals(PollingCentreRef other)
        {
            return Id.Equals(other.Id) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PollingCentreRef)obj);
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

    //TODO enumarate polling centre types
    public enum PollingCentreType
    {
        None = 0,
    }
}