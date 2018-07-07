using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class Constituency : MasterEntity<ConstituencyRef>
    {
        public Constituency() : base(default(Guid))
        {
            ConstituencyMps = new List<ConstituencyMps>();
        }

        public Constituency(Guid id) : base(id)
        {
            ConstituencyMps = new List<ConstituencyMps>();
        }

        public Constituency(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : base(id, dateCreated, dateLastUpdated, status)
        {
            ConstituencyMps = new List<ConstituencyMps>();
        }

        [Required(ErrorMessage = "Constituency Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "County is required")]
        public CountyRef County { get; set; }

        public string Code { get; set; }
        public int TotalRegisteredVoters { get; set; }
        public int PopulationOver18 { get; set; }
        public decimal Area { get; set; }
        public int NumberOfWards { get; set; }
        public int NumberOfPollingStations { get; set; }

        public List<ConstituencyMps> ConstituencyMps { get; set; }

        public override ConstituencyRef GetMasterDataRef()
        {
            return new ConstituencyRef(this.Id, this.Name);
        }

        public override ValidationResultInfo Validate()
        {
            var validationInfo = this.BasicValidation();
            return validationInfo;
        }
    }

    [ComplexType]
    public class ConstituencyRef : MasterDataRef
    {
        public ConstituencyRef()
        {
        }

        public ConstituencyRef(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }

        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.Constituency; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(Name))
                message += "Invalid Name";
            if (Id == Guid.Empty)
                message += "Invalid Constituency id";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static ConstituencyRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<ConstituencyRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
            this.Name = "";
        }

        #region Equality

        protected bool Equals(ConstituencyRef other)
        {
            return Id.Equals(other.Id) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConstituencyRef)obj);
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