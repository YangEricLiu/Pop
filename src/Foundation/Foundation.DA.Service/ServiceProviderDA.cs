using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;



namespace SE.DSP.Foundation.DA.Service
{
    using System.Data;
    using System.Data.Common;




    using SE.DSP.Foundation.Infrastructure.BaseClass;
    using SE.DSP.Foundation.Infrastructure.Structure;
    using SE.DSP.Foundation.Infrastructure.Enumerations;
    using SE.DSP.Foundation.DA.Interface;
    using SE.DSP.Foundation.Infrastructure.BE.Entities;

    public class ServiceProviderDA : DABase, IServiceProviderDA
    {
        public long CreateServiceProvider(ServiceProviderEntity entity)
        {
            const string sql = @"insert into serviceprovider(
      [UserName]
      ,[Name]
      ,[Address]
      ,[Telephone]
      ,[Email]
      ,[StartDate]
      ,[Status]
      ,[Comment]
      ,[DeployStatus]
      ,[UpdateTime]
      ,[UpdateUser]
      ,[CalcStatus]
      ,[Domain])
values(
@UserName,
    @Name,
    @Address,
    @Telephone ,
    @Email ,
    @StartDate,
    @Status ,
    @Comment,
    @DeployStatus,
    @UpdateTime,
    @UpdateUser,
     @CalcStatus,
     @Domain);
SELECT SCOPE_IDENTITY();";
            var cmd = GenerateCUCommand(sql, entity);
            return Convert.ToInt64(Database.ExecuteScalar(cmd));
        }

        public void UpdateServiceProvider(ServiceProviderEntity entity)
        {
            const string sql = @"update serviceprovider set 
    [UserName]=@UserName,
    [Name]=@Name,
    [Address]=@Address,
    [Telephone]=@Telephone ,
    [Email]=@Email ,
    [StartDate]=@StartDate,
    [Status]=@Status ,
    [Comment]=@Comment,
    [DeployStatus]=@DeployStatus,
    [UpdateTime]=@UpdateTime,
    [UpdateUser]=@UpdateUser,
    [CalcStatus]=@CalcStatus,
    [Domain] = @Domain
where id=@Id;";
            var cmd = GenerateCUCommand(sql, entity);
            Database.ExecuteNonQuery(cmd);
        }

        public ServiceProviderEntity[] RetrieveServiceProviders(ServiceProviderFilter filter)
        {
            if (filter == null) return new ServiceProviderEntity[] { };
            var sql = String.Format(@"SELECT [Id]
      ,[UserName]
      ,[Name]
      ,[Address]
      ,[Telephone]
      ,[Email]
      ,[Domain]
      ,[StartDate]
      ,[Status]
      ,[Comment]
      ,[DeployStatus]
      ,[UpdateTime]
      ,[UpdateUser] ,CAST(Version AS bigint) AS Version,CalcStatus FROM ServiceProvider WHERE 1=1 {0}", GenerateWhereCondition(filter));
            var command = GenerateRCommand(sql, filter);
            var list = ConvertToEntities(Database.ExecuteReader(command));
            return list.ToArray();
        }

        /// <summary>
        /// Generate command for creating/updating
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static DbCommand GenerateCUCommand(string sql, ServiceProviderEntity entity)
        {
            if (String.IsNullOrEmpty(sql) || entity == null) return null;

            DbCommand command = Database.GetSqlStringCommand(sql);

            if (entity.Id.HasValue)
            {
                Database.AddInParameter(command, "Id", DbType.Int64, entity.Id);
            }

            Database.AddInParameter(command, "UserName", DbType.String, entity.UserName);
            Database.AddInParameter(command, "Name", DbType.String, entity.Name);
            Database.AddInParameter(command, "Address", DbType.String, entity.Address ?? (object)DBNull.Value);
            Database.AddInParameter(command, "Telephone", DbType.String, entity.Telephone ?? (object)DBNull.Value);
            Database.AddInParameter(command, "Email", DbType.String, entity.Email ?? (object)DBNull.Value);
            Database.AddInParameter(command, "StartDate", DbType.DateTime, entity.StartDate ?? (object)DBNull.Value);
            Database.AddInParameter(command, "Status", DbType.Int32, entity.Status);
            Database.AddInParameter(command, "Domain", DbType.String, entity.Domain);
            Database.AddInParameter(command, "Comment", DbType.String, entity.Comment ?? (object)DBNull.Value);
            Database.AddInParameter(command, "DeployStatus", DbType.Int32, entity.DeployStatus);
            Database.AddInParameter(command, "UpdateUser", DbType.String, entity.UpdateUser);
            Database.AddInParameter(command, "UpdateTime", DbType.DateTime, entity.UpdateTime);
            Database.AddInParameter(command, "CalcStatus", DbType.Int32, entity.CalcStatus ? 1 : 0);
            return command;
        }
        private static DbCommand GenerateRCommand(string sql, ServiceProviderFilter filter)
        {
            if (String.IsNullOrEmpty(sql) || filter == null) return null;

            DbCommand command = Database.GetSqlStringCommand(sql);

            if (filter.ExcludeId.HasValue)
            {
                Database.AddInParameter(command, "ExcludeId", DbType.Int64, filter.ExcludeId.Value);
            }

            return command;
        }

        private static String GenerateWhereCondition(ServiceProviderFilter filter)
        {
            if (filter == null) return String.Empty;

            var sql = new StringBuilder();

            if (filter.ExcludeId.HasValue)
            {
                sql.AppendFormat(" AND Id<>@ExcludeId");
            }

            if (filter.Ids != null && filter.Ids.Length > 0)
            {
                sql.AppendFormat(" AND Id IN ({0})", String.Join(",", filter.Ids));
            }

            if (filter.StatusFilter != null)
            {
                sql.Append(GenerateWhereCondition(filter.StatusFilter, true));
            }

            return sql.ToString();
        }

        public void SetStatus(long[] spids, EntityStatus status, LastUpdateInfo update)
        {
            if (spids == null || spids.Length == 0 || LastUpdateInfo.IsNull(update)) return;

            var sql = @"update ServiceProvider set Status=@Status,UpdateUser=@UpdateUser,UpdateTime=@UpdateTime WHERE " + GenerateWhereCondition("Id", spids, false);

            var cmd = Database.GetSqlStringCommand(sql);
            Database.AddInParameter(cmd, "Status", DbType.Int32, Convert.ToInt32(status));
            Database.AddInParameter(cmd, "UpdateUser", DbType.String, update.User);
            Database.AddInParameter(cmd, "UpdateTime", DbType.DateTime, update.Time);
            Database.ExecuteNonQuery(cmd);

        }


        private static List<ServiceProviderEntity> ConvertToEntities(IDataReader reader)
        {
            var list = new List<ServiceProviderEntity>();

            using (reader)
            {
                while (reader.Read())
                {
                    var entity = new ServiceProviderEntity();

                    int ordinal = reader.GetOrdinal("Id");
                    entity.Id = reader.GetInt64(ordinal);

                    ordinal = reader.GetOrdinal("UserName");
                    if (!reader.IsDBNull(ordinal)) entity.UserName = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("Name");
                    if (!reader.IsDBNull(ordinal)) entity.Name = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("Address");
                    if (!reader.IsDBNull(ordinal)) entity.Address = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("Telephone");
                    if (!reader.IsDBNull(ordinal)) entity.Telephone = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("Email");
                    if (!reader.IsDBNull(ordinal)) entity.Email = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("Domain");
                    if (!reader.IsDBNull(ordinal)) entity.Domain = reader.GetString(ordinal);


                    ordinal = reader.GetOrdinal("Comment");
                    if (!reader.IsDBNull(ordinal)) entity.Comment = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("StartDate");
                    if (!reader.IsDBNull(ordinal)) entity.StartDate = reader.GetDateTime(ordinal);

                    ordinal = reader.GetOrdinal("Status");
                    entity.Status = (EntityStatus)reader.GetInt32(ordinal);

                    ordinal = reader.GetOrdinal("DeployStatus");
                    entity.DeployStatus = (DeployStatus)reader.GetInt32(ordinal);

                    ordinal = reader.GetOrdinal("UpdateUser");
                    entity.UpdateUser = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("UpdateTime");
                    entity.UpdateTime = reader.GetDateTime(ordinal);

                    ordinal = reader.GetOrdinal("Version");
                    entity.Version = reader.GetInt64(ordinal);


                    ordinal = reader.GetOrdinal("CalcStatus");

                    entity.CalcStatus = reader.GetInt32(ordinal) == 1 ? true : false; ;

                    list.Add(entity);
                }

                reader.Close();
            }
            return list;
        }
        /// <summary>
        /// Generate where condition for id list. like fieldName=id, fieldName<>id, fieldName in (id1,id2), fieldName not in (id1,id2)
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="ids"></param>
        /// <param name="startWithAnd"></param>
        /// <returns></returns>
        private static String GenerateWhereCondition(String filedName, IList<long> ids, bool startWithAnd = true)
        {
            if (String.IsNullOrEmpty(filedName) || ids == null || ids.Count == 0) return String.Empty;

            String condition;

            if (ids.Count == 1) condition = filedName + '=' + ids[0];
            else condition = filedName + " IN (" + String.Join(",", ids) + ")";

            return startWithAnd ? " AND " + condition : condition;
        }

    }
}