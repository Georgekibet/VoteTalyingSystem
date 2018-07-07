using System;
using System.Collections.Generic;
using System.Linq;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;
using Vts.WebLib.ViewModels.CountyViewModels;

namespace Vts.WebLib.ViewModelBuilders.CountyViewModelBuilders.Impl
{
    public class CountyViewModelBuilder: ICountyViewModelBuilder
    {
        private ICountyRepository _countyRepository;
        private IRegionRepository _regionRepository;

        public CountyViewModelBuilder(ICountyRepository countyRepository, IRegionRepository regionRepository)
        {
            _countyRepository = countyRepository;
            _regionRepository = regionRepository;
        }

        public List<CountyViewModel> GetAll(bool inactive = false)
        {
            return _countyRepository.GetAll(inactive).ToList()
                .Select(county => new CountyViewModel(county)) as List<CountyViewModel>;
        }

        public CountyViewModel Get(Guid id)
        {
            return new CountyViewModel(_countyRepository.GetById(id));
        }

        public void Save(CountyViewModel countyViewModel)
        {
            _countyRepository.Save(countyViewModel.County);
        }

        public void SetInactive(Guid id)
        {
            var county = _countyRepository.GetById(id);
            _countyRepository.SetInactive(county);
        }

        public void SetActive(Guid id)
        {
            var county = _countyRepository.GetById(id);
            _countyRepository.SetActive(county);

        }

        public void SetAsDeleted(Guid id)
        {
            var county = _countyRepository.GetById(id);
            _countyRepository.SetAsDeleted(county);
        }

        public Dictionary<Guid, string> Regions()
        {
            return _regionRepository.GetAll().OrderBy(n => n.Name)
               .Select(r => new { r.Id, r.Name }).ToList().ToDictionary(d => d.Id, d => d.Name);
        }

        public QueryResult<CountyViewModel> Query(QueryStandard query)
        {
            var queryResult = _countyRepository.Query(query);
            var results = new QueryResult<CountyViewModel>();
            results.Count = queryResult.Count;
            results.Data = queryResult.Data.Select(Map).ToList();

            return results;
        }

        private CountyViewModel Map(County county)
        {
            var cvm = new CountyViewModel(county);
            return cvm;
        }
    }
}
