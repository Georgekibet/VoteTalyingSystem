using System.Collections.Generic;
using vts.Core.Repository;
using vts.Core.Shared.Entities.Master;
using vts.Core.TransactionalEntities;
using vts.Core.Workflows;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Core.ResultServices
{
    public interface IMpResultService
    {
        void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results);
    }

    public class MpResultService : IMpResultService
    {
        private readonly IMpResultRepository _mpResultRepository;
        private readonly IMpResultWorkflow _mpResultWorkflow;

        public MpResultService(IMpResultRepository mpResultRepository, IMpResultWorkflow mpResultWorkflow)
        {
            _mpResultRepository = mpResultRepository;
            _mpResultWorkflow = mpResultWorkflow;
        }

        public void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results)
        {
            ResultInfo resultInfo = new ResultInfo
            {
                OriginatingPollingCentre = pollingCentre,
                CommandGeneratedByUser = user
            };

            var res = _mpResultRepository.GetByPollingCentre(pollingCentre);

            if (res == null)
            {
                var resultReference = new ResultReference();
                string reference = resultReference.Generate(pollingCentre.Name, user.Username);
                var newResult = _mpResultWorkflow.Create(resultInfo, reference);
                var resultWithDetail = _mpResultWorkflow.AddMpResultLineItems(newResult, resultInfo, results);
                var confirmResult = _mpResultWorkflow.Confirm(resultWithDetail, resultInfo);
                _mpResultRepository.Save(confirmResult);
            }
            if (res != null)
            {
                var modifiedResult = _mpResultWorkflow.Modify(res, resultInfo, results);
                _mpResultRepository.Save(modifiedResult);
            }
        }
    }
}