using System.Collections.Generic;
using vts.Core.Repository;
using vts.Core.Shared.Entities.Master;
using vts.Core.TransactionalEntities;
using vts.Core.Workflows;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Core.ResultServices
{
    public interface ISenatorialResultService
    {
        void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results);
    }

    public class SenatorialResultService : ISenatorialResultService
    {
        private readonly ISenatorialResultRepository _senatorialResultRepository;
        private readonly ISenatorialResultWorkflow _senatorialResultWorkflow;

        public SenatorialResultService(ISenatorialResultRepository senatorialResultRepository, ISenatorialResultWorkflow senatorialResultWorkflow)
        {
            _senatorialResultRepository = senatorialResultRepository;
            _senatorialResultWorkflow = senatorialResultWorkflow;
        }

        public void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results)
        {
            ResultInfo resultInfo = new ResultInfo
            {
                OriginatingPollingCentre = pollingCentre,
                CommandGeneratedByUser = user
            };

            var res = _senatorialResultRepository.GetByPollingCentre(pollingCentre);

            if (res == null)
            {
                var resultReference = new ResultReference();
                string reference = resultReference.Generate(pollingCentre.Name, user.Username);
                var newResult = _senatorialResultWorkflow.Create(resultInfo, reference);
                var resultWithDetail = _senatorialResultWorkflow.AddSenatorialResultLineItems(newResult, resultInfo, results);
                var confirmResult = _senatorialResultWorkflow.Confirm(resultWithDetail, resultInfo);
                _senatorialResultRepository.Save(confirmResult);
            }
            if (res != null)
            {
                var modifiedResult = _senatorialResultWorkflow.Modify(res, resultInfo, results);
                _senatorialResultRepository.Save(modifiedResult);
            }
        }
    }
}