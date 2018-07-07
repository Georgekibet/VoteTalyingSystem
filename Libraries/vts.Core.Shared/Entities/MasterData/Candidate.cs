using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class Candidate : MasterEntity<CandidateRef>
    {
        public Candidate() : base(default(Guid))
        {
        }

        public Candidate(Guid id) : base(id)
        {
        }

        public Candidate(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : base(id, dateCreated, dateLastUpdated, status)
        {
        }

        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Id card number is required")]
        public string IdCardNumber { get; set; }

        public string PassportNumber { get; set; }

        [Required(ErrorMessage = "Race is required")]
        public RaceRef Race { get; set; }

        [Required(ErrorMessage = "Political party is required")]
        public PoliticalPartyRef PoliticalParty { get; set; }

        [Required(ErrorMessage = "Independence Status is required")]
        public CandidateType CandidateType { get; set; }

        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$",
            ErrorMessage = @"Invalid email")]
        public string EmailAddress { get; set; }

        public byte[] Symbol { get; set; }
        public byte[] Photo { get; set; }
        public string RunningMateName { get; set; }
        public string RunningMateIdCardNumber { get; set; }
        public CandidateStatus CandidateStatus { get; set; }

        public string Fullname()
        {
            return FirstName + " " + MiddleName + " " + Surname;
        }

        public override CandidateRef GetMasterDataRef()
        {
            return new CandidateRef(this.Id, this.Fullname(), this.CandidateType);
        }

        public override ValidationResultInfo Validate()
        {
            var validationInfo = this.BasicValidation();
            return validationInfo;
        }
    }

    [ComplexType]
    public class CandidateRef : MasterDataRef
    {
        public CandidateRef()
        {
        }

        public CandidateRef(Guid id, string fullName, CandidateType candidateType)
        {
            Id = id;
            FullName = fullName;
            CandidateType = candidateType;
        }

        public string FullName { get; set; }

        public CandidateType CandidateType { get; set; }

        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.Candidate; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(FullName))
                message += "Invalid Name";
            if (Id == Guid.Empty)
                message += "Invalid candidate id";
            if ((int)CandidateType == 0)
                message += "Invalid candidate type";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static CandidateRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<CandidateRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
            this.FullName = "";
            this.CandidateType = CandidateType.None;
        }

        #region Equality

        protected bool Equals(CandidateRef other)
        {
            return Id.Equals(other.Id) && string.Equals(FullName, other.FullName) && CandidateType == other.CandidateType && MasterDataType == other.MasterDataType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CandidateRef)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)CandidateType;
                hashCode = (hashCode * 397) ^ (int)MasterDataType;
                return hashCode;
            }
        }

        #endregion Equality
    }

    public enum CandidateType
    {
        None = 0,
        Independent = 1,
        PartyBacked = 2,
    }

    public enum CandidateStatus
    {
        Registered = 0,
        Pending = 1,
        Withdrawn = 2,
        Disqualified = 3,
        Nominated = 4,
        Deceased = 5,
        Deleted = 6,
    }
}