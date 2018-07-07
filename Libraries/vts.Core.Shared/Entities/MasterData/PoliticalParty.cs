using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class PoliticalParty : MasterEntity<PoliticalPartyRef>
    {
        public PoliticalParty() : base(default(Guid))
        {
        }

        public PoliticalParty(Guid id) : base(id)
        {
        }

        public PoliticalParty(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : base(id, dateCreated, dateLastUpdated, status)
        {
        }

        [Required(ErrorMessage = "Party Code is required")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Party Name is required")]
        public string Name { get; set; }

        public string Acronym { get; set; }
        public DateTime DateRegistered { get; set; }
        public Byte[] ApprovedSymbol { get; set; }

        public override PoliticalPartyRef GetMasterDataRef()
        {
            return new PoliticalPartyRef(this.Id, this.Name);
        }

        public override ValidationResultInfo Validate()
        {
            var validationInfo = this.BasicValidation();
            return validationInfo;
        }
    }

    [ComplexType]
    public class PoliticalPartyRef : MasterDataRef
    {
        public PoliticalPartyRef()
        {
        }

        public PoliticalPartyRef(Guid id, string name)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }

        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.PoliticalParty; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(Name))
                message += "Invalid Name";
            if (Id == Guid.Empty)
                message += "Invalid political party id";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static PoliticalPartyRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<PoliticalPartyRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
            this.Name = "";
        }

        #region Equality

        protected bool Equals(PoliticalPartyRef other)
        {
            return Id.Equals(other.Id) && string.Equals(Name, other.Name) && MasterDataType == other.MasterDataType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PoliticalPartyRef)obj);
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
}