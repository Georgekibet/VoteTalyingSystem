using System.Collections.Generic;
using vts.Core.Repository;
using vts.Core.Shared.Entities.Master;
using vts.Core.TransactionalEntities;
using vts.Core.Workflows;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Core.ResultServices
{
    public interface IMcaResultService
    {
        void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results);
    }

    public class McaResultService : IMcaResultService
    {
        private readonly IMcaResultRepository _mcaResultRepository;
        private readonly IMcaResultWorkflow _mcaResultWorkflow;

        public McaResultService(IMcaResultRepository mcaResultRepository, IMcaResultWorkflow mcaResultWorkflow)
        {
            _mcaResultRepository = mcaResultRepository;
            _mcaResultWorkflow = mcaResultWorkflow;
        }

        public void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results)
        {
            ResultInfo resultInfo = new ResultInfo
            {
                OriginatingPollingCentre = pollingCentre,
                CommandGeneratedByUser = user
            };

            var res = _mcaResultRepository.GetByPollingCentre(pollingCentre);

            if (res == null)
            {
                var resultReference = new ResultReference();
                string reference = resultReference.Generate(pollingCentre.Name, user.Username);
                var newResult = _mcaResultWorkflow.Create(resultInfo, reference);
                var resultWithDetail = _mcaResultWorkflow.AddMcaResultLineItems(newResult, resultInfo, results);
                var confirmResult = _mcaResultWorkflow.Confirm(resultWithDetail, resultInfo);
                _mcaResultRepository.Save(confirmResult);
            }
            if (res != null)
            {
                var modifiedResult = _mcaResultWorkflow.Modify(res, resultInfo, results);
                _mcaResultRepository.Save(modifiedResult);
            }
        }
    }
}