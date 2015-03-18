using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.Structure;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.DA.Interface;

namespace SE.DSP.Foundation.DA.Service
{
    public class RoleDA : DABase, IRoleDA
    {
        public RoleDA() { }

        private Microsoft.Practices.EnterpriseLibrary.Data.Database db = Database;
        public long CreateRole(RoleEntity entity)
        {
            const String sql = "INSERT INTO Role(SpId, Name, UpdateUser,UpdateTime)VALUES(@SpId,@Name,@UpdateUser,@UpdateTime);SELECT SCOPE_IDENTITY();";
            var cmd = Database.GetSqlStringCommand(sql);
            Database.AddInParameter(cmd, "SpId", DbType.Int64, entity.SpId);
            Database.AddInParameter(cmd, "Name", DbType.String, entity.Name);
            Database.AddInParameter(cmd, "UpdateUser", DbType.String, entity.UpdateUser);
            Database.AddInParameter(cmd, "UpdateTime", DbType.DateTime, entity.UpdateTime);

            return Convert.ToInt64(Database.ExecuteScalar(cmd));
        }

        public void UpdateRole(RoleEntity entity)
        {
            const String sql = "UPDATE Role SET Name=@Name, UpdateUser=@UpdateUser,UpdateTime=@UpdateTime WHERE Id=@Id;";
            var cmd = Database.GetSqlStringCommand(sql);
            Database.AddInParameter(cmd, "Id", DbType.Int64, entity.Id.Value);
            Database.AddInParameter(cmd, "Name", DbType.String, entity.Name);
            Database.AddInParameter(cmd, "UpdateUser", DbType.String, entity.UpdateUser);
            Database.AddInParameter(cmd, "UpdateTime", DbType.DateTime, entity.UpdateTime);

            Database.ExecuteNonQuery(cmd);
        }

        public void DeleteRole(long[] roleIds)
        {
            if (roleIds == null || roleIds.Length == 0) return;
            String sql = @String.Format("DELETE Role WHERE Id IN ({0})", string.Join(",", roleIds));
            var cmd = Database.GetSqlStringCommand(sql);

            Database.ExecuteNonQuery(cmd);
        }

        public RoleEntity[] RetrieveRolesByFilter(RoleFilter filter, ConnectionOption connOp = null)
        {
            //PrepareDB(connOp);
            var sql = new StringBuilder("SELECT Id,SpId, Name, UpdateUser,UpdateTime, CAST(Version AS BIGINT) AS VERSION FROM Role WHERE 1=1");
            if(filter.SpId !=-1) sql.Append(" AND SpId=" + filter.SpId);
            if (filter.UserIds != null && filter.UserIds.Length > 0) sql.AppendFormat(" AND Id IN (SELECT RoleId FROM UserRole WHERE UserId IN ({0})) ", String.Join(",", filter.UserIds));
            if (filter.RoleIds != null && filter.RoleIds.Length > 0) sql.AppendFormat(" AND Id IN ({0})", String.Join(",", filter.RoleIds));
            if (filter.ExcludeId.HasValue) sql.Append(" AND Id <>" + filter.ExcludeId.Value);
            if (!String.IsNullOrEmpty(filter.Name)) sql.AppendFormat(" AND Name=@Name");

            var cmd = db.GetSqlStringCommand(sql.ToString());

            if (!String.IsNullOrEmpty(filter.Name)) db.AddInParameter(cmd, "Name", DbType.String, filter.Name);

            var list = new List<RoleEntity>();
            using (var reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    var role = new RoleEntity
                    {
                        Id = Convert.ToInt64(reader["Id"]),
                        SpId = Convert.ToInt64(reader["SpId"]),
                        Name = Convert.ToString(reader["Name"]),
                        UpdateTime = Convert.ToDateTime(reader["UpdateTime"]),
                        UpdateUser = Convert.ToString(reader["UpdateUser"]),
                        Version = Convert.ToInt64(reader["Version"])
                    };
                    list.Add(role);
                }
            }
            return list.ToArray();
        }

    }
}
