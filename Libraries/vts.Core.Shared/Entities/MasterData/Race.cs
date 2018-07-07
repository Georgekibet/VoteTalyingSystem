using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class Race : MasterEntity<RaceRef>
    {
        public Race() : base(default(Guid))
        {
        }

        public Race(Guid id) : base(id)
        {
        }

        public Race(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : base(id, dateCreated, dateLastUpdated, status)
        {
        }

        public string Name { get; set; }
        public RaceType RaceType { get; set; }
        public ElectionRef Election { get; set; }

        public override RaceRef GetMasterDataRef()
        {
            return new RaceRef(this.Id, this.Name, this.RaceType);
        }

        public override ValidationResultInfo Validate()
        {
            var validationInfo = this.BasicValidation();
            return validationInfo;
        }
    }

    [ComplexType]
    public class RaceRef : MasterDataRef
    {
        public RaceRef()
        {
        }

        public RaceRef(Guid id, string name, RaceType raceType)
        {
            Id = id;
            Name = name;
            RaceType = raceType;
        }

        [Required(ErrorMessage = "Race Name is required")]
        public string Name { get; set; }

        public RaceType RaceType { get; set; }

        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.Race; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(Name))
                message += "Invalid Name";
            if (Id == Guid.Empty)
                message += "Invalid race id";
            if ((int)RaceType == 0)
                message += "Invalid race type";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static RaceRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<RaceRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
            this.Name = "";
            this.RaceType = RaceType.None;
        }

        #region Equalty

        protected bool Equals(RaceRef other)
        {
            return Id.Equals(other.Id) && string.Equals(Name, other.Name) && MasterDataType == other.MasterDataType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RaceRef)obj);
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

        #endregion Equalty
    }

    public enum RaceType
    {
        None = 0,
        Presidential = 1,
        Governor = 2,
        Senator = 3,
        WomenRepresentative = 4,
        MemberOfParliament = 5,
        MemberOfCountyAssembly = 6,
        Referendum = 7,
    }
}