using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.Structure;
using SE.DSP.Foundation.DA.Interface;
using SE.DSP.Foundation.Infrastructure.BE.Entities; 

namespace SE.DSP.Foundation.DA.Service
{
    public class RolePrivilegeDA : DABase, IRolePrivilegeDA
    {
        public RolePrivilegeDA()
        {

        }

        private Microsoft.Practices.EnterpriseLibrary.Data.Database db = Database;
        public void CreateRolePrivilege(RolePrivilegeEntity[] entities)
        {
            if (entities == null || entities.Length == 0) return;

            var sql = new StringBuilder("INSERT INTO RolePrivilege(RoleId,PrivilegeCode,UpdateUser,UpdateTime)VALUES");

            foreach (var e in entities)
            {
                sql.AppendFormat("({0},{1},@UpdateUser,@UpdateTime),", e.RoleId, e.PrivilegeCode);
            }
            sql.Remove(sql.Length - 1, 1);

            var command = Database.GetSqlStringCommand(sql.ToString());
            Database.AddInParameter(command, "UpdateUser", DbType.String, entities[0].UpdateUser);
            Database.AddInParameter(command, "UpdateTime", DbType.DateTime, entities[0].UpdateTime);
            Database.ExecuteNonQuery(command);
        }

        public void DeleteRolePrivilege(RoleFilter filter)
        {
            if (filter == null) return;

            var sql = String.Format("DELETE FROM RolePrivilege WHERE RoleId IN (SELECT Id FROM [Role] WHERE 1=1 {0});", GenerateWhereCondition(filter, true));

            var command = Database.GetSqlStringCommand(sql);
            Database.ExecuteNonQuery(command);
        }

        public RolePrivilegeEntity[] RetrieveRolePrivilege(RoleFilter filter)
        {
            if (filter == null) return new RolePrivilegeEntity[] { };
            var sql = String.Format("SELECT RoleId,PrivilegeCode,UpdateUser,UpdateTime,CAST(Version AS BIGINT) AS Version FROM RolePrivilege WHERE 1=1 {0}", GenerateWhereCondition(filter, true));
            var command = Database.GetSqlStringCommand(sql);
            var list = ConvertToEntities(Database.ExecuteReader(command));
            return list.ToArray();
        }

        private static String GenerateWhereCondition(RoleFilter filter, bool startWithAnd = true)
        {
            if (filter == null || ((filter.RoleIds == null || filter.RoleIds.Length == 0) && (filter.UserIds == null || filter.UserIds.Length == 0))) return String.Empty;

            var sql = new StringBuilder();
            if (filter.UserIds != null && filter.UserIds.Length > 0) sql.AppendFormat("AND RoleId IN (Select RoleId FROM UserRole WHERE 1=1 {0})",GenerateWhereCondition("UserId", filter.UserIds, true));
            if (filter.RoleIds != null && filter.RoleIds.Length > 0) sql.Append(GenerateWhereCondition("RoleId", filter.RoleIds, true));
            if (sql.Length > 4 && !startWithAnd) sql.Remove(0, 4);
            return sql.ToString();
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


        public String[] RetrievePrivileges()
        {
            const string sql = "SELECT Code FROM Privilege";
            var command = Database.GetSqlStringCommand(sql);
            var list = new List<String>();
            using (var reader = Database.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    list.Add(Convert.ToString(reader[0]));
                }
            }
            return list.ToArray();
        }

        private List<RolePrivilegeEntity> ConvertToEntities(IDataReader reader)
        {
            var list = new List<RolePrivilegeEntity>();

            using (reader)
            {
                while (reader.Read())
                {
                    var entity = new RolePrivilegeEntity();

                    int ordinal = reader.GetOrdinal("RoleId");
                    entity.RoleId = reader.GetInt64(ordinal);

                    ordinal = reader.GetOrdinal("PrivilegeCode");
                    entity.PrivilegeCode = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("UpdateUser");
                    entity.UpdateUser = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("UpdateTime");
                    entity.UpdateTime = reader.GetDateTime(ordinal);

                    ordinal = reader.GetOrdinal("Version");
                    entity.Version = reader.GetInt64(ordinal);


                    list.Add(entity);
                }
                reader.Close();
            }
            return list;
        }


    }
}
