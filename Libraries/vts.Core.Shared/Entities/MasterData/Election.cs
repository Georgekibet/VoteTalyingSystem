using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class Election : MasterEntity<ElectionRef>
    {
        public Election() : base(default(Guid))
        {
        }

        public Election(Guid id) : base(id)
        {
        }

        public Election(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : base(id, dateCreated, dateLastUpdated, status)
        {
        }

        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime NominationStartDate { get; set; }
        public DateTime NominationEndDate { get; set; }
        public ElectionType ElectionType { get; set; }
        public string Location { get; set; }

        public override ElectionRef GetMasterDataRef()
        {
            return new ElectionRef(this.Id, this.Name);
        }

        public override ValidationResultInfo Validate()
        {
            var validationInfo = this.BasicValidation();
            return validationInfo;
        }
    }

    [ComplexType]
    public class ElectionRef : MasterDataRef
    {
        public ElectionRef()
        {
        }

        public ElectionRef(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        [Required(ErrorMessage = "Election Name is required")]
        public string Name { get; set; }

        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.Election; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(Name))
                message += "Invalid Name";
            if (Id == Guid.Empty)
                message += "Invalid election id";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static ElectionRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<ElectionRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
            this.Name = "";
        }

        #region Equality

        protected bool Equals(ElectionRef other)
        {
            return Id.Equals(other.Id) && string.Equals(Name, other.Name) && MasterDataType == other.MasterDataType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ElectionRef)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)MasterDataType;
                return hashCode;
            }
        }

        #endregion Equality
    }

    public enum ElectionType
    {
        None = 0,
        ByElection = 1,
        GeneralElection = 2,
    }
}