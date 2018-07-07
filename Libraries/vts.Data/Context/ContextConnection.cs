namespace vts.Data.Context
{
    public class ContextConnection
    {
        public ContextConnection(string vtsConnectionString)
        {
            VtsConnectionString = vtsConnectionString;
        }

        public string VtsConnectionString { get; set; }
    }
}