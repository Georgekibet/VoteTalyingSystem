using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Repository
{
    public interface ICountyRepository : IMasterRepository<County>
    {
        QueryResult<County> Query(QueryStandard query);
    }
}