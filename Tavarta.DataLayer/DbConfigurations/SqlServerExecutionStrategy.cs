using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace Tavarta.DataLayer.DbConfigurations
{
    public class SqlServerExecutionStrategy : DbExecutionStrategy
    {
        private readonly DateTime _establishTime;
        private readonly TimeSpan _maxDelay;

        public SqlServerExecutionStrategy()
        { }

        public SqlServerExecutionStrategy(int maxRetryCount, TimeSpan maxDelay)
        {
            this._establishTime = DateTime.UtcNow;
            this._maxDelay = maxDelay;
        }

        protected override bool ShouldRetryOn(Exception ex)
        {
            var sqlException = ex as SqlException;
            return sqlException != null && sqlException.Errors.Cast<SqlError>().Any(error => error.Number == (int)TypeOfError.Deadlock || error.Number == (int)TypeOfError.Timeout || error.Number == (int)TypeOfError.Timeout2);
        }

        protected override TimeSpan? GetNextDelay(Exception lastException)
        {
            return DateTime.UtcNow.Subtract(_establishTime.Add(this._maxDelay));
        }
    }

    public enum TypeOfError
    {
        Deadlock = 1205,
        Timeout = -1,
        Timeout2 = -2,
        ServiceBusy = 40501,
        TransportLevelAndEstablishedConnection = 10053,
    }
}