using System;
using System.Data.Entity;

namespace Tavarta.DataLayer.DbConfigurations
{
    public class TavartaDbConfiguration : DbConfiguration
    {
        public TavartaDbConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlServerExecutionStrategy(2,TimeSpan.FromSeconds(15)));
        }
    }
}
