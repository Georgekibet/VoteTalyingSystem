using System;
using System.Collections.Generic;
using System.Linq;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;
using Vts.WebLib.ViewModels;

namespace Vts.WebLib.ViewModelBuilders.ConstituencyViewModelBuilders.Impl
{
    public class ConstituencyViewModelBuilder: IConstituencyViewModelBuilder
    {
        private IConstituencyRepository _constituencyRepository;
        private ICountyRepository _countyRepository;

        public ConstituencyViewModelBuilder(IConstituencyRepository constituencyRepository, ICountyRepository countyRepository)
        {
            _constituencyRepository = constituencyRepository;
            _countyRepository = countyRepository;
        }

        public List<ConstituencyViewModel> GetAll(bool inactive = false)
        {
            return _constituencyRepository.GetAll(inactive).ToList()
                .Select(c => new ConstituencyViewModel(c)) as List<ConstituencyViewModel>;
        }

        public ConstituencyViewModel Get(Guid id)
        {
            return new ConstituencyViewModel(_constituencyRepository.GetById(id));
        }

        public void Save(ConstituencyViewModel cvm)
        {
            _constituencyRepository.Save(cvm.Constituency);
        }

        public void SetInactive(Guid id)
        {
            var c = _constituencyRepository.GetById(id);
            _constituencyRepository.SetInactive(c);
        }

        public void SetActive(Guid id)
        {
            var c = _constituencyRepository.GetById(id);
            _constituencyRepository.SetActive(c);
        }

        public void SetAsDeleted(Guid id)
        {
            var c = _constituencyRepository.GetById(id);
            _constituencyRepository.SetAsDeleted(c);
        }

        public Dictionary<Guid, string> Counties()
        {
            return _countyRepository.GetAll().OrderBy(n => n.Name)
               .Select(r => new { r.Id, r.Name }).ToList().ToDictionary(d => d.Id, d => d.Name);
        }

        public QueryResult<ConstituencyViewModel> Query(QueryStandard query)
        {
            var queryResult = _constituencyRepository.Query(query);
            var results = new QueryResult<ConstituencyViewModel>();
            results.Count = queryResult.Count;
            results.Data = queryResult.Data.Select(Map).ToList();

            return results;
        }

        private ConstituencyViewModel Map(Constituency constituency)
        {
            var cvm = new ConstituencyViewModel(constituency);
            return cvm;
        }
    }
}
