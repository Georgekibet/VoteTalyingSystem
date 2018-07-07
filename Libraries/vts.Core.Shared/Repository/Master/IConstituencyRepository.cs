using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Repository
{
    public interface IConstituencyRepository : IMasterRepository<Constituency>
    {
        QueryResult<Constituency> Query(QueryStandard query);
    }
}