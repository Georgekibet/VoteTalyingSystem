using System;
using Mobile.Common.Util;

namespace Mobile.Common.Storage
{
    public class Transactor
    {
        private readonly IDatabase database;

        public Transactor(IDatabase database)
        {
            this.database = database;
        }

        // Pass in an Action that represents a unit of work that should be transactional. 
        public Result<object> Transact(Action unitOfWork)
        {
            try
            {
                database.BeginTransaction();

                unitOfWork();
                long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                database.Commit();
                long end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                long total = end - start;

                Console.WriteLine("Commit time {0} seconds / {1} milliseconds", total / 1000, total);
                return Result<object>.Success(default(object));
            }
            catch (Exception e)
            {
                database.Rollback();
                return Result<object>.Failure(e, "Error when processing transaction");
            }  
        }
    }
}
