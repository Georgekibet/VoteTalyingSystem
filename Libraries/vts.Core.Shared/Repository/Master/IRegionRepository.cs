using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Repository
{
    public interface IRegionRepository : IMasterRepository<Region>
    {
        Region GetByName(string regionName, bool includeDeactivated = false);
        QueryResult<Region> Query(QueryStandard query);
    }
}