using System;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class ConstituencyMps : MasterEntity<ConstituencyMpsRef>
    {
        public ConstituencyMps()
            : base(default(Guid))
        {
            Constituency = ConstituencyRef.CreateEmpty();
            Candidate = CandidateRef.CreateEmpty();
        }

        public ConstituencyMps(Guid id)
            : base(id)
        {
            Constituency = ConstituencyRef.CreateEmpty();
            Candidate = CandidateRef.CreateEmpty();
        }

        public ConstituencyMps(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status)
            : base(id, dateCreated, dateLastUpdated, status)
        {
            Constituency = ConstituencyRef.CreateEmpty();
            Candidate = CandidateRef.CreateEmpty();
        }

        public ConstituencyRef Constituency { get; set; }
        public CandidateRef Candidate { get; set; }

        public override ConstituencyMpsRef GetMasterDataRef()
        {
            return new ConstituencyMpsRef()
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
    public class ConstituencyMpsRef : MasterDataRef
    {
        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.ConstituencyMps; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (Id == Guid.Empty)
                message += "Invalid purchasing clerk ref id,";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static ConstituencyMpsRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<ConstituencyMpsRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
        }
    }
}