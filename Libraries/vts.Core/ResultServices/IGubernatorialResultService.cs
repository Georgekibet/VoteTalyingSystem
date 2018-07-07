using System.Collections.Generic;
using vts.Core.Repository;
using vts.Core.Shared.Entities.Master;
using vts.Core.TransactionalEntities;
using vts.Core.Workflows;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Core.ResultServices
{
    public interface IGubernatorialResultService
    {
        void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results);
    }

    public class GubernatorialResultService : IGubernatorialResultService
    {
        private readonly IGubernatorialResultRepository _gubernatorialResultRepository;
        private readonly IGubernatorialResultWorkflow _gubernatorialResultWorkflow;

        public GubernatorialResultService(IGubernatorialResultRepository gubernatorialResultRepository, IGubernatorialResultWorkflow presidentialResultWorkflow)
        {
            _gubernatorialResultRepository = gubernatorialResultRepository;
            _gubernatorialResultWorkflow = presidentialResultWorkflow;
        }

        public void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results)
        {
            ResultInfo resultInfo = new ResultInfo
            {
                OriginatingPollingCentre = pollingCentre,
                CommandGeneratedByUser = user
            };

            var res = _gubernatorialResultRepository.GetByPollingCentre(pollingCentre);

            if (res == null)
            {
                var resultReference = new ResultReference();
                string reference = resultReference.Generate(pollingCentre.Name, user.Username);
                var newResult = _gubernatorialResultWorkflow.Create(resultInfo, reference);
                var resultWithDetail = _gubernatorialResultWorkflow.AddGubernatorialResultLineItems(newResult, resultInfo, results);
                var confirmResult = _gubernatorialResultWorkflow.Confirm(resultWithDetail, resultInfo);
                _gubernatorialResultRepository.Save(confirmResult);
            }
            if (res != null)
            {
                var modifiedResult = _gubernatorialResultWorkflow.Modify(res, resultInfo, results);
                _gubernatorialResultRepository.Save(modifiedResult);
            }
        }
    }
}