using System;
using vts.Core.Shared.Entities.Master;

namespace Agrimanagr.Core.Error
{
    public class UncaughtExceptionLogEntry 
    {
        public UncaughtExceptionLogEntry(Exception e)
        
        {
            Message = e.Message;
            Stacktrace = e.StackTrace;
            Timestamp = DateTime.Now;
        }

        public UncaughtExceptionLogEntry()
      
        {
        }

        public bool Acknowledged { get; set; }
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public DateTime Timestamp { get; set; }
    }
}