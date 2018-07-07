using System;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class CountyGovernors : MasterEntity<CountyGovernorsRef>
    {
        public CountyGovernors()
            : base(default(Guid))
        {
            County = CountyRef.CreateEmpty();
            Candidate = CandidateRef.CreateEmpty();
        }

        public CountyGovernors(Guid id)
            : base(id)
        {
            County = CountyRef.CreateEmpty();
            Candidate = CandidateRef.CreateEmpty();
        }

        public CountyGovernors(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : base(id, dateCreated, dateLastUpdated, status)
        {
            County = CountyRef.CreateEmpty();
            Candidate = CandidateRef.CreateEmpty();
        }

        public CountyRef County { get; set; }
        public CandidateRef Candidate { get; set; }

        public override CountyGovernorsRef GetMasterDataRef()
        {
            return new CountyGovernorsRef()
            {
                Id = Id
            };
        }

        public override ValidationResultInfo Validate()
        {
            var validationInfo = this.BasicValidation();
            return validationInfo;
        }
    }

    [ComplexType]
    public class CountyGovernorsRef : MasterDataRef
    {
        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.CountyGoverners; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (Id == Guid.Empty)
                message += "Invalid purchasing clerk ref id,";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static CountyGovernorsRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<CountyGovernorsRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
        }
    }
}