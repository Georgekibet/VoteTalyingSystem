using System.Collections.Generic;
using vts.Core.Repository;
using vts.Core.Shared.Entities.Master;
using vts.Core.TransactionalEntities;
using vts.Core.Workflows;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Core.ResultServices
{
    public interface IPresidentialResultService
    {
        void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results);
    }

    public class PresidentialResultService : IPresidentialResultService
    {
        private readonly IPresidentialResultRepository _presidentialResultRepository;
        private readonly IPresidentialResultWorkflow _presidentialResultWorkflow;

        public PresidentialResultService(IPresidentialResultRepository presidentialResultRepository, IPresidentialResultWorkflow presidentialResultWorkflow)
        {
            _presidentialResultRepository = presidentialResultRepository;
            _presidentialResultWorkflow = presidentialResultWorkflow;
        }

        public void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results)
        {
            ResultInfo resultInfo = new ResultInfo
            {
                OriginatingPollingCentre = pollingCentre,
                CommandGeneratedByUser = user
            };

            var res = _presidentialResultRepository.GetByPollingCentre(pollingCentre);

            if (res == null)
            {
                var resultReference = new ResultReference();
                string reference = resultReference.Generate(pollingCentre.Name, user.Username);
                var newResult = _presidentialResultWorkflow.Create(resultInfo, reference);
                var resultWithDetail = _presidentialResultWorkflow.AddPresidentialResultLineItems(newResult, resultInfo, results);
                var confirmResult = _presidentialResultWorkflow.Confirm(resultWithDetail, resultInfo);
                _presidentialResultRepository.Save(confirmResult);
            }
            if (res != null)
            {
                var modifiedResult = _presidentialResultWorkflow.Modify(res, resultInfo, results);
                _presidentialResultRepository.Save(modifiedResult);
            }
        }
    }
}