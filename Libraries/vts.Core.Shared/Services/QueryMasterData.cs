using System;
using System.Collections.Generic;
using System.Text;

namespace vts.Shared.Services
{
    public class QueryResult<T> where T : class
    {
        public QueryResult()
        {
            Data = new List<T>();
        }

        public List<T> Data { get; set; }
        public int Count { get; set; }

    }
    public abstract class QueryBase
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }

    }

    public class QueryMasterData : QueryBase
    {
        public QueryMasterData()
        {
            IsFirstSync = false;
        }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Description { get; set; }
        public int PassChange { get; set; }
        public bool IsFirstSync { get; set; }

    }

    public class QueryStandard : QueryBase
    {
        public Guid? Id { get; set; }
        public string Description { get; set; }
        public bool IncludeDeactivated { get; set; }
        public int? UserType { get; set; }
    }
}
