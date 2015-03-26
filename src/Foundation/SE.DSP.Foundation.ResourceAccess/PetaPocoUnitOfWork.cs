using PetaPoco;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.DataAccess
{
    public class PetaPocoUnitOfWork : IUnitOfWork
    {
        private const string DatabaseConnectionStringName = "REMInformation";
        private readonly Transaction petaTransaction;
        private readonly Database db;

        public PetaPocoUnitOfWork()
        {
            this.db = this.GetDatabase();
            this.petaTransaction = new Transaction(this.db);
        }

        public void Dispose()
        {
            this.petaTransaction.Dispose();
        }

        public Database Db
        {
            get { return this.db; }
        }

        public void Commit()
        {
            this.petaTransaction.Complete();
        }

        private Database GetDatabase()
        {
            var connectionString = this.GetConnectionString();
            return new Database(connectionString, "System.Data.SqlClient");
        }

        private string GetConnectionString()
        {
            var connstr = System.Configuration.ConfigurationManager.ConnectionStrings[DatabaseConnectionStringName].ConnectionString;

            connstr = connstr
                .Replace("{" + DeploymentConfigKey.SpDbDatabase.ToString() + "}", ConfigHelper.Get(DeploymentConfigKey.SpDbDatabase))
                .Replace("{" + DeploymentConfigKey.SpDbServerIP.ToString() + "}", ConfigHelper.Get(DeploymentConfigKey.SpDbServerIP))
                .Replace("{" + DeploymentConfigKey.SpDbUser.ToString() + "}", ConfigHelper.Get(DeploymentConfigKey.SpDbUser))
                .Replace("{" + DeploymentConfigKey.SpDbPassword.ToString() + "}", ConfigHelper.Get(DeploymentConfigKey.SpDbPassword))
                .Replace("{" + DeploymentConfigKey.SpDbMaxPoolSize.ToString() + "}", ConfigHelper.Get(DeploymentConfigKey.SpDbMaxPoolSize));

            return connstr;
        }
    }
}
