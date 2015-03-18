using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.BaseClass;


using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.DA.Interface;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using System.Data.Common;

namespace SE.DSP.Foundation.DA.Service
{
    public class DataPrivilegeDA : DABase, IDataPrivilegeDA
    {
        public DataPrivilegeDA() { }

        public DataPrivilegeEntity[] RetrieveDataPrivileges(DataPrivilegeFilter filter)
        {
            if (filter == null) return new DataPrivilegeEntity[0];

            var sql = new StringBuilder(@"SELECT UserId,PrivilegeType,PrivilegeItemId,UpdateUser,HierarchyPath.ToString() AS Path,UpdateTime,CAST(Version AS bigint) AS Version FROM UserDataPrivilege WHERE 1=1");
            sql.Append(GenerateWhereCondition(filter));
            sql.Append(" ORDER BY UserId,PrivilegeType,PrivilegeItemId");
            var cmd = Database.GetSqlStringCommand(sql.ToString());
            var list = new List<DataPrivilegeEntity>();
            using (var reader = Database.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    var entity = new DataPrivilegeEntity();

                    int ordinal = reader.GetOrdinal("UserId");
                    entity.UserId = reader.GetInt64(ordinal);

                    ordinal = reader.GetOrdinal("PrivilegeType");
                    if (!reader.IsDBNull(ordinal)) entity.PrivilegeType = (DataAuthType)reader.GetInt32(ordinal);

                    ordinal = reader.GetOrdinal("PrivilegeItemId");
                    if (!reader.IsDBNull(ordinal)) entity.PrivilegeItemId = reader.GetInt64(ordinal);

                    ordinal = reader.GetOrdinal("UpdateUser");
                    entity.UpdateUser = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("UpdateTime");
                    entity.UpdateTime = reader.GetDateTime(ordinal);

                    ordinal = reader.GetOrdinal("Version");
                    entity.Version = reader.GetInt64(ordinal);
                    list.Add(entity);
                }
            }
            return list.ToArray();
        }

        public UserEntity[] GetUsersByItem(DataAuthType dataAuthType, long itemId)
        {
            var sql = string.Format("SELECT {0} FROM {1} WHERE Id IN (SELECT UserId FROM UserDataPrivilege WHERE PrivilegeType={2} AND PrivilegeItemId={3}) ORDER BY Name", UserDA.UserColumns, UserDA.UserTable, (int)dataAuthType, itemId);
            var command = Database.GetSqlStringCommand(sql);
            var list = UserDA.ConvertToUserEntities(Database.ExecuteReader(command));
            return list.ToArray();
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

        private static String GenerateWhereCondition(DataPrivilegeFilter filter)
        {
            var sql = new StringBuilder();
            if (filter.UserIds != null && filter.UserIds.Length > 0) sql.Append(GenerateWhereCondition("UserId", filter.UserIds, true));
            if (filter.PrivilegeItemIds != null && filter.PrivilegeItemIds.Length > 0) sql.Append(GenerateWhereCondition("PrivilegeItemId", filter.PrivilegeItemIds, true));
            if (filter.PrivilegeType.HasValue) sql.Append(" AND PrivilegeType=" + (int)filter.PrivilegeType.Value);
            if (filter.CustomerIds != null && filter.CustomerIds.Length > 0) sql.AppendFormat(" AND PrivilegeItemId IN (SELECT Id FROM Hierarchy WHERE 1=1 {0})", GenerateWhereCondition("CustomerId", filter.CustomerIds, true));
            return sql.ToString();
        }

        public void CreateDataPrivileges(DataPrivilegeEntity[] entities)
        {
            if (entities == null || entities.Length == 0) return;

            var sql = new StringBuilder(
@"DECLARE @maxId BIGINT
SET @maxId=(SELECT Max(Id) FROM UserDataPrivilege)
INSERT INTO UserDataPrivilege(UserId,PrivilegeType,PrivilegeItemId,HierarchyPath,UpdateUser,UpdateTime)VALUES");

            foreach (var e in entities)
            {
                sql.AppendFormat("({0},{1},{2},'/',@UpdateUser,@UpdateTime),",
                                 e.UserId,
                                 e.PrivilegeType.HasValue ? (object)(int)e.PrivilegeType.Value : DBNull.Value,
                                 e.PrivilegeItemId.HasValue ? (object)(long)e.PrivilegeItemId.Value : DBNull.Value);
            }
            sql.Remove(sql.Length - 1, 1);
            sql.AppendLine();
            sql.Append(
@"IF(@maxId IS NULL) 
BEGIN
UPDATE UserDataPrivilege SET HierarchyPath=(SELECT Path FROM Hierarchy h WHERE h.Id=UserDataPrivilege.PrivilegeItemId) WHERE PrivilegeType=0
END
ELSE
BEGIN
UPDATE UserDataPrivilege SET HierarchyPath=(SELECT Path FROM Hierarchy h WHERE h.Id=UserDataPrivilege.PrivilegeItemId) WHERE PrivilegeType=0 AND Id>@maxId
END");

            var command = Database.GetSqlStringCommand(sql.ToString());
            Database.AddInParameter(command, "UpdateUser", DbType.String, entities[0].UpdateUser);
            Database.AddInParameter(command, "UpdateTime", DbType.DateTime, entities[0].UpdateTime);
            Database.ExecuteNonQuery(command);

        }

        public void DeleteDataPrivileges(DataPrivilegeFilter filter)
        {
            if (filter == null) return;
            var sql = String.Format("DELETE UserDataPrivilege WHERE 1=1 {0}", GenerateWhereCondition(filter));
            var cmd = Database.GetSqlStringCommand(sql);
            Database.ExecuteNonQuery(cmd);
        }


        public void UpdateDataprivileges(long[] privileges)
        {
            var inParams = string.Join<long>(",", privileges);

            string sql =string.Format("UPDATE UserDataPrivilege SET HierarchyPath=(SELECT Path FROM Hierarchy h WHERE h.Id=UserDataPrivilege.PrivilegeItemId) WHERE PrivilegeType=0 and PrivilegeItemId in({0}) ",inParams);

            var cmd = Database.GetSqlStringCommand(sql);
            Database.ExecuteNonQuery(cmd);
        }


        public long[] RetrieveCustomerIdsByPrivilegeItem(long[] itemIds)
        {
            string sql = @"SELECT                                  
                                CustomerId
                               FROM Hierarchy WHERE 1=1 " + GenerateWhereCondition("Id", itemIds);

            DbCommand cmd = Database.GetSqlStringCommand(sql);


            var list = new List<long>();
            using (var reader = Database.ExecuteReader(cmd))
            {
                while (reader.Read())
                {

                    int ordinal = reader.GetOrdinal("CustomerId");
                    long id = reader.GetInt64(ordinal);


                    list.Add(id);
                }
            }

            return list.ToArray();
        }
    }
}
