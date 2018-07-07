using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vts.Core.Import
{
    public interface IImportService<T> where T: class 
    {
        ImportResult Process(List<T> importedList);
    }
}
