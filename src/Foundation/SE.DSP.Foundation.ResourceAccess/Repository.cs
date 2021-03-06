﻿using PetaPoco;
using SE.DSP.Foundation.DataAccess.CustomQueries;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.DataAccess
{
    public abstract class Repository<TEntity, TIdType> : IRepository<TEntity, TIdType>
    {
        protected readonly Database Db;
        private const string DatabaseConnectionStringName = "REMInformation";
 
        protected Repository()
        {
            this.Db = this.GetDatabase();
        }

        public abstract TEntity GetById(TIdType id);

        public abstract TEntity Add(TEntity entity);

        public abstract TEntity Add(IUnitOfWork unitOfWork, TEntity entity);

        public abstract void Update(TEntity entity);

        public abstract void Update(IUnitOfWork unitOfWork, TEntity entity);

        public abstract void Delete(TIdType id);

        public abstract void Delete(IUnitOfWork unitOfWork, TIdType id);

        public TEntity GetOne(ISingleResultQuery<TEntity> singleResultQuery)
        {
            if (this.Db == null)
            {
                throw new ArgumentException("PetaPoco database can not be null.");
            }

            singleResultQuery.Db = this.Db;

            return singleResultQuery.Execute();
        }

        public IEnumerable<TEntity> GetMany(IMultiResultQuery<TEntity> multiResultQuery)
        {
            if (this.Db == null)
            {
                throw new ArgumentException("PetaPoco database can not be null.");
            }

            multiResultQuery.Db = this.Db;

            return multiResultQuery.Execute();
        }

        public IPaged<TEntity> GetPaged(IPagedResultQuery<TEntity> pagedResultQuery)
        {
            if (this.Db == null)
            {
                throw new ArgumentException("PetaPoco database can not be null.");
            }

            pagedResultQuery.Db = this.Db;

            return pagedResultQuery.Execute();
        }

        public IEnumerable<TEntity> AddMany(IUnitOfWork unitOfWork, IList<TEntity> entities)
        {
            var result = new List<TEntity>();

            foreach (TEntity entity in entities)
            {
                result.Add(this.Add(unitOfWork, entity));
            }

            return result;
        }

        public void UpdateMany(IUnitOfWork unitOfWork, IList<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                this.Update(unitOfWork, entity);
            }
        }

        protected PetaPoco.Database GetDatabese(IUnitOfWork unitOfWork)
        {
            return unitOfWork == null ? this.Db : unitOfWork.Db;
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

