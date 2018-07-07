using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vts.Core.Import.CsvHelpers
{
    public interface ICsvReadWriteHelper<T> where T : class 
    {
        CsvReadResults<T> ReadCsv(string path);
    }
}
