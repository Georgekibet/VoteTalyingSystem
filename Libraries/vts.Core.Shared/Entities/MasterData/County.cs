using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class County : MasterEntity<CountyRef>
    {
        public County() 
            : base(default(Guid))
        {
            Initialize();
        }

        public County(Guid id) 
            : base(id)
        {
            Initialize();
        }

        public County(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : base(id, dateCreated, dateLastUpdated, status)
        {
            Initialize();
        }

        [Required(ErrorMessage = "County Name is required")]
        public string Name { get; set; }

       [Required(ErrorMessage = "Region is required")]
        public RegionRef Region { get; set; }

        [Required(ErrorMessage = "County Code is required")]
        public string Code { get; set; }

        public int TotalRegisteredVoters { get; set; }

        public List<CountyGovernors> CountyGovernors { get; set; }

        public List<CountySenators> CountySenators { get; set; }

        public List<CountyWomenReps> CountyWomenReps { get; set; }

        public override CountyRef GetMasterDataRef()
        {
            return new CountyRef(this.Id, this.Name);
        }

        public override ValidationResultInfo Validate()
        {
            var validationInfo = this.BasicValidation();
            return validationInfo;
        }

        public void Initialize()
        {
            CountyGovernors = new List<CountyGovernors>();
            CountySenators = new List<CountySenators>();
            CountyWomenReps = new List<CountyWomenReps>();
        }
    }

    

    [ComplexType]
    public class CountyRef : MasterDataRef
    {
        public CountyRef()
        {
        }

        public CountyRef(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }

        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.County; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(Name))
                message += "Invalid Name";
            if (Id == Guid.Empty)
                message += "Invalid County id";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static CountyRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<CountyRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
            this.Name = "";
        }

        #region Equality

        protected bool Equals(CountyRef other)
        {
            return Id.Equals(other.Id) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CountyRef)obj);
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