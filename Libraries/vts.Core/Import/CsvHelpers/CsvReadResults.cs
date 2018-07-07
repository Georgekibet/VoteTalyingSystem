using System.Collections.Generic;

namespace vts.Core.Import.CsvHelpers
{
    public class CsvReadResults<T> where T:class
    {
        public List<T> Imported { get; set; }
        public List<string> Ignored { get; set; }
        public int TotalRecords { get; set; }
    }
}