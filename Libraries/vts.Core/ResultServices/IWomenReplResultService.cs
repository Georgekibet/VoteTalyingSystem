using System.Collections.Generic;
using vts.Core.Repository;
using vts.Core.Shared.Entities.Master;
using vts.Core.TransactionalEntities;
using vts.Core.Workflows;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Core.ResultServices
{
    public interface IWomenRepResultService
    {
        void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results);
    }

    public class WomenRepResultService : IWomenRepResultService
    {
        private readonly IWomenRepResultRepository _womenRepResultRepository;
        private readonly IWomenRepResultWorkflow _womenRepResultWorkflow;

        public WomenRepResultService(IWomenRepResultRepository womenRepResultRepository, IWomenRepResultWorkflow womenRepResultWorkflow)
        {
            _womenRepResultRepository = womenRepResultRepository;
            _womenRepResultWorkflow = womenRepResultWorkflow;
        }

        public void Excecute(UserRef user, PollingCentreRef pollingCentre, List<ResultDetail> results)
        {
            ResultInfo resultInfo = new ResultInfo
            {
                OriginatingPollingCentre = pollingCentre,
                CommandGeneratedByUser = user
            };

            var res = _womenRepResultRepository.GetByPollingCentre(pollingCentre);

            if (res == null)
            {
                var resultReference = new ResultReference();
                string reference = resultReference.Generate(pollingCentre.Name, user.Username);
                var newResult = _womenRepResultWorkflow.Create(resultInfo, reference);
                var resultWithDetail = _womenRepResultWorkflow.AddWomenRepResultLineItems(newResult, resultInfo, results);
                var confirmResult = _womenRepResultWorkflow.Confirm(resultWithDetail, resultInfo);
                _womenRepResultRepository.Save(confirmResult);
            }
            if (res != null)
            {
                var modifiedResult = _womenRepResultWorkflow.Modify(res, resultInfo, results);
                _womenRepResultRepository.Save(modifiedResult);
            }
        }
    }
}