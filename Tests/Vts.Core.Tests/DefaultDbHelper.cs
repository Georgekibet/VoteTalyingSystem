using System.Configuration;

namespace Vts.Core.Tests
{
    public class DefaultDbHelper
    {
        public static DbHelper GetDefaultDbTestingHelper()
        {
            string sqlConnection = ConfigurationManager.AppSettings["vtsConnectionString"];
            var cs = new DbHelper(sqlConnection);
            return cs;
        }
    }

    public class DbHelper
    {
        public DbHelper(string vtsConnectionString)
        {
            VtsConnectionstring = vtsConnectionString;
        }

        public string VtsConnectionstring { get; set; }
    }
}