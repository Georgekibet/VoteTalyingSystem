using System;
using System.Collections.Generic;
using log4net;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;

namespace vts.Core.Import.Services
{

    public interface IRegionImportService
    {
        ImportResult Process(List<Region> importedRegions);
    }

    public class RegionImportService : IRegionImportService
    {
        private ILog _log = LogManager.GetLogger("RegionImportService");
        private readonly IRegionRepository _regionRepository;

        public RegionImportService(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public ImportResult Process(List<Region> importedRegions)
        {
            var succeeded = 0;
            
            foreach (var region in importedRegions)
            {
                
                try
                {
                    region.Id = Guid.NewGuid();
                    _regionRepository.Save(region);
                    succeeded++;
                }
                catch (DomainValidationException e)
                {
                    _log.InfoFormat(
                        "RegionImportService Validation Error: Validation failed for imported record 'region: {0}, code: {1}'. Error Message: {2}",
                        region.Name, region.Code, e.Message);

                }
                catch (Exception e)
                {
                    _log.Info("RegionImportService Error: Error occured while saving imported record. Error Message: "+e.Message);
                }
                
            }
            return new ImportResult {Imported = succeeded,NotImported = importedRegions.Count - succeeded};
            
        }
    }
}
