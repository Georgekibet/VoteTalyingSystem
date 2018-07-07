using System;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class WardMcas : MasterEntity<WardMcasRef>
    {
        public WardMcas()
            : base(default(Guid))
        {
            Ward = WardRef.CreateEmpty();
            Candidate = CandidateRef.CreateEmpty();
        }

        public WardMcas(Guid id)
            : base(id)
        {
            Ward = WardRef.CreateEmpty();
            Candidate = CandidateRef.CreateEmpty();
        }

        public WardMcas(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : base(id, dateCreated, dateLastUpdated, status)
        {
            Ward = WardRef.CreateEmpty();
            Candidate = CandidateRef.CreateEmpty();
        }

        public WardRef Ward { get; set; }
        public CandidateRef Candidate { get; set; }

        public override WardMcasRef GetMasterDataRef()
        {
            return new WardMcasRef()
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
    public class WardMcasRef : MasterDataRef
    {
        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.WardMcas; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (Id == Guid.Empty)
                message += "Invalid purchasing clerk ref id,";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static WardMcasRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<WardMcasRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
        }
    }
}