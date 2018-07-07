using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vts.Core.Import;
using vts.Core.Import.models;
using vts.Core.Import.Services;
using vts.Shared.Entities.Master;

namespace vts.Core.Contracts
{
    public class DefaultImportServices : IDefaultServicesList
    {
        public DefaultImportServices()
        {
            Services = new DefaultServices()
                .Add<IRegionImportService, RegionImportService>()
                .Add<IImportService<CountyImportModel>, CountyImportService>();
        }

        public DefaultServices Services { get; set; }
    }
}
