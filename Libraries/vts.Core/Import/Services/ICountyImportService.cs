using System;
using System.Collections.Generic;
using log4net;
using vts.Core.Import.models;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;

namespace vts.Core.Import.Services
{
    public class CountyImportService : IImportService<CountyImportModel>
    {
        private ILog _log = LogManager.GetLogger("CountyImportService");
        private ICountyRepository _countyRepository;
        private IRegionRepository _regionRepository;

        public CountyImportService(ICountyRepository countyRepository,IRegionRepository regionRepository)
        {
            _countyRepository = countyRepository;
            _regionRepository = regionRepository;
        }

        public ImportResult Process(List<CountyImportModel> importedList)
        {
            var succeeded = 0;

            foreach (var model in importedList)
            {
                var county = new County();
                
                try
                {
                    var region = _regionRepository.GetByName(model.Name);
                    if (region == null)
                    {
                        _log.InfoFormat(
                        "CountyImportService: Region doesnt exist. Imported record 'County: {0}, code: {1}'. Region: {2}",
                        county.Name, county.Code,model.Region);
                        continue;
                    }
                    county.Id = Guid.NewGuid();
                    county.Code = model.Code;
                    county.Name = model.Name;
                    county.Region = region.GetMasterDataRef();

                    _countyRepository.Save(county);
                    succeeded++;

                }
                catch (DomainValidationException e)
                {
                    _log.InfoFormat(
                        "CountyImportService Validation Error: Validation failed for imported record 'County: {0}, code: {1}'. Error Message: {2}",
                        county.Name, county.Code, e.Message);

                }
                catch (Exception e)
                {
                    _log.Info("CountyImportService Error: Error occured while saving imported record. Error Message: " + e.Message);
                }

            }
            return new ImportResult { Imported = succeeded, NotImported = importedList.Count - succeeded };
        }
    }
}
