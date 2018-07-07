using System;
using System.Linq;

namespace vts.Shared.Services
{
    public class DomainValidationException : Exception
    {
        public DomainValidationException(ValidationResultInfo validationResults, string message)
            : base(message)
        {
            ValidationResults = validationResults;
            errorMessage = message + "---" +
                                 string.Join("--", validationResults.Results.Select(n => n.ErrorMessage));
        }

        private string errorMessage = "";
        public ValidationResultInfo ValidationResults { get; set; }
        public override string Message { get { return errorMessage; } }
    }
}