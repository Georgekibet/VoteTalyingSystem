using System;
using System.Collections.Generic;
using System.Linq;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;
using Vts.WebLib.ViewModels;

namespace Vts.WebLib.ViewModelBuilders.RegionViewModelBuilders
{
    public class RegionViewModelBuilder: IRegionViewModelBuilder
    {
        private IRegionRepository _regionRepository;

        public RegionViewModelBuilder(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public List<RegionViewModel> GetAll(bool inactive = false)
        {
            return _regionRepository.GetAll(inactive).ToList()
                .Select(region => new RegionViewModel(region)) as List<RegionViewModel>;
        }

        public RegionViewModel Get(Guid id)
        {
            return new RegionViewModel(_regionRepository.GetById(id));
        }

        public void Save(RegionViewModel regionViewModel)
        {
            _regionRepository.Save(regionViewModel.Region);
        }

        public void SetInactive(Guid id)
        {
            var region = _regionRepository.GetById(id);
            _regionRepository.SetInactive(region);
        }

        public void SetActive(Guid id)
        {
            var region = _regionRepository.GetById(id);
            _regionRepository.SetActive(region);
        }

        public void SetAsDeleted(Guid id)
        {
            var region = _regionRepository.GetById(id);
            _regionRepository.SetAsDeleted(region);
        }

        public QueryResult<RegionViewModel> Query(QueryStandard query)
        {
            var queryResult = _regionRepository.Query(query);
            var results = new QueryResult<RegionViewModel>();
            results.Count = queryResult.Count;
            results.Data = queryResult.Data.Select(Map).ToList();

            return results;
        }

        private RegionViewModel Map(Region region)
        {
            var rvm = new RegionViewModel(region);
            return rvm;
        }
    }
}
