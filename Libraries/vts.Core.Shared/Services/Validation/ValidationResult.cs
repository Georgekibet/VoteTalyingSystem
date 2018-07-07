using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace vts.Shared.Services
{
    public class ValidationResultInfo
    {
        public ValidationResultInfo()
        {
            Results = new List<ValidationResult>();
        }

        public bool IsValid
        {
            get { return !Results.Any(); }
        }

        public List<ValidationResult> Results { get; set; }
    }
}
