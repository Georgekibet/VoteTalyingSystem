using System.Collections.Generic;
using vts.Core.Repository;
using vts.Core.Shared.Entities.Master;
using vts.Core.TransactionalEntities;
using vts.Core.Workflows;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Core.ResultServices
{
    public interface IReferendumResultService
    {
        void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results);
    }

    public class ReferendumResultService : IReferendumResultService
    {
        private readonly IReferendumResultRepository _referendumResultRepository;
        private readonly IReferendumResultWorkflow _referendumResultWorkflow;

        public ReferendumResultService(IReferendumResultRepository referendumResultRepository, IReferendumResultWorkflow referendumResultWorkflow)
        {
            _referendumResultRepository = referendumResultRepository;
            _referendumResultWorkflow = referendumResultWorkflow;
        }

        public void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results)
        {
            ResultInfo resultInfo = new ResultInfo
            {
                OriginatingPollingCentre = pollingCentre,
                CommandGeneratedByUser = user
            };

            var res = _referendumResultRepository.GetByPollingCentre(pollingCentre);

            if (res == null)
            {
                var resultReference = new ResultReference();
                string reference = resultReference.Generate(pollingCentre.Name, user.Username);
                var newResult = _referendumResultWorkflow.Create(resultInfo, reference);
                var resultWithDetail = _referendumResultWorkflow.AddReferendumResultLineItems(newResult, resultInfo, results);
                var confirmResult = _referendumResultWorkflow.Confirm(resultWithDetail, resultInfo);
                _referendumResultRepository.Save(confirmResult);
            }
            if (res != null)
            {
                var modifiedResult = _referendumResultWorkflow.Modify(res, resultInfo, results);
                _referendumResultRepository.Save(modifiedResult);
            }
        }
    }
}