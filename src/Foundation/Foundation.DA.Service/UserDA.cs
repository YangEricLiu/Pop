using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SE.DSP.Foundation.DA.Service
{
    using System.Data;
    using System.Data.Common;

    using SE.DSP.Foundation.Infrastructure.BaseClass;


    using SE.DSP.Foundation.Infrastructure.Structure;
    using SE.DSP.Foundation.Infrastructure.Enumerations;
    using SE.DSP.Foundation.Infrastructure.Utils;
    using SE.DSP.Foundation.Infrastructure.Constant;
    using SE.DSP.Foundation.Infrastructure.BE.Entities;
    using SE.DSP.Foundation.DA.Interface;
    using System.Configuration;

    public class UserDA : DABase, IUserDA
    {
        public const String UserColumns = "Id,UserType,Name,RealName,Password,PasswordToken,PasswordTokenDate,Comment,Title,Telephone,Email,Status,DemoStatus,UpdateUser,UpdateTime,CAST(Version AS bigint) AS Version,SpId";
        public const String UserTable = "[User]";

        public UserDA() { }

        private Microsoft.Practices.EnterpriseLibrary.Data.Database db = Database;

        public long CreateUser(UserEntity entity, ConnectionOption option = null)
        {
            if (entity == null) return -1;


            const string sql = @"INSERT INTO [User] (Name,UserType,RealName,Password,PasswordToken,PasswordTokenDate,Comment,Title,Telephone,Email,Status,DemoStatus,UpdateUser,UpdateTime, SpId) 
VALUES (@Name,@UserType,@RealName,@Password,@PasswordToken,@PasswordTokenDate,@Comment,@Title,@Telephone,@Email,@Status,@DemoStatus,@UpdateUser,@UpdateTime, @SpId); 
SELECT SCOPE_IDENTITY();";
            var cmd = GenerateCUCommand(sql, entity);
            return Convert.ToInt64(db.ExecuteScalar(cmd));
        }
        public void UpdateUser(UserEntity entity, ConnectionOption option = null)
        {
            if (entity == null) return;



            const string sql = @"UPDATE [User] 
SET Name=@Name,UserType=@UserType,RealName=@RealName,Password=@Password,PasswordToken=@PasswordToken,PasswordTokenDate=@PasswordTokenDate,Comment=@Comment,Title=@Title,Telephone=@Telephone,Email=@Email,Status=@Status,DemoStatus=@DemoStatus,UpdateUser=@UpdateUser,UpdateTime=@UpdateTime,SpId=@SpId 
WHERE Id=@Id;";
            var cmd = GenerateCUCommand(sql, entity);
            db.ExecuteNonQuery(cmd);
        }

        public void DeleteUser(UserFilter filter, ConnectionOption option = null)
        {
            if (filter == null) return;

            var sql = @"DELETE FROM [User] WHERE 1=1 " + GenerateWhereCondition(filter, true, false);
            var command = GenerateRCommand(sql, filter);
            db.ExecuteNonQuery(command);
        }

        public void CreateUserCustomer(UserCustomerEntity[] entity, ConnectionOption option = null)
        {
            if (entity == null || entity.Length == 0) return;

            var sql = new StringBuilder("INSERT INTO UserCustomer(UserId,HierarchyId,WholeCustomer,UpdateUser,UpdateTime)VALUES");

            foreach (var e in entity)
            {
                var wholeCustomerBit = e.WholeCustomer ? "1" : "0";
                sql.AppendFormat("({0},{1},{2},@UpdateUser,@UpdateTime),", e.UserId, e.CustomerId, wholeCustomerBit);
            }
            sql.Remove(sql.Length - 1, 1);

            var command = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(command, "UpdateUser", DbType.String, entity[0].UpdateUser);
            db.AddInParameter(command, "UpdateTime", DbType.DateTime, entity[0].UpdateTime);
            db.ExecuteNonQuery(command);
        }

        public void DeleteUserCustomer(UserCustomerFilter filter, ConnectionOption option = null)
        {
            if (filter == null) return;

            var sql = new StringBuilder("DELETE FROM UserCustomer WHERE 1=1 ");
            if (filter.UserIds != null && filter.UserIds.Length > 0) sql.AppendFormat(" AND UserId IN ({0})", String.Join(",", filter.UserIds));
            if (filter.CustomerIds != null && filter.CustomerIds.Length > 0) sql.AppendFormat(" AND HierarchyId IN ({0})", String.Join(",", filter.CustomerIds));

            var command = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteNonQuery(command);
        }

        public UserEntity[] RetrieveUsers(UserFilter filter, ConnectionOption option = null)
        {
            if (filter == null) return new UserEntity[] { };
            var sql = String.Format("SELECT {0} FROM {1} WHERE 1=1 {2} ORDER BY Name", UserColumns, UserTable, GenerateWhereCondition(filter, true));
            var command = GenerateRCommand(sql, filter);
            var list = ConvertToUserEntities(db.ExecuteReader(command));
            return list.ToArray();
        }

        public UserEntity[] RetrieveUsers(UserFilter filter, UserOrder order, out long totalCount, ConnectionOption option = null)
        {
            var sql = GenerateSQL(filter, order);
            var command = GenerateRCommand(sql, filter);
            var list = ConvertToUserEntities(db.ExecuteReader(command), out totalCount);

            return list.ToArray();
        }

        public UserCustomerEntity[] RetrieveUserCustomers(UserCustomerFilter filter, ConnectionOption option = null)
        {
            if (filter == null) return new UserCustomerEntity[0];
            var sql = new StringBuilder("SELECT UserId, HierarchyId, WholeCustomer, UpdateTime, UpdateUser, CAST(VERSION AS BIGINT) AS VERSION FROM UserCustomer WHERE 1=1");

            if (filter.UserIds != null && filter.UserIds.Length > 0 && filter.UserIds.All(p=> p != -1)) sql.Append(GenerateWhereCondition("UserId", filter.UserIds, true));
            if (filter.CustomerIds != null && filter.CustomerIds.Length > 0) sql.Append(GenerateWhereCondition("HierarchyId", filter.CustomerIds, true));
            if (filter.WholeCustomer.HasValue) sql.Append(" AND WholeCustomer=" + (filter.WholeCustomer.Value ? 1 : 0));

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var list = new List<UserCustomerEntity>();
            using (var reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    var entity = new UserCustomerEntity
                        {
                            CustomerId = Convert.ToInt64(reader["HierarchyId"]),
                            UserId = Convert.ToInt64(reader["UserId"]),
                            Version = Convert.ToInt64(reader["Version"]),
                            UpdateTime = Convert.ToDateTime(reader["UpdateTime"]),
                            UpdateUser = Convert.ToString(reader["UpdateUser"]),
                        };

                    var ordinal = reader.GetOrdinal("WholeCustomer");
                    if (!reader.IsDBNull(ordinal)) entity.WholeCustomer = reader.GetBoolean(ordinal);
                    list.Add(entity);
                }
            }
            return list.ToArray();
        }

        public void CreateUserRole(UserRoleEntity[] entities, ConnectionOption option = null)
        {
            var sql = new StringBuilder("INSERT INTO UserRole(UserId, RoleId) Values");
            foreach (var entity in entities)
            {
                sql.AppendFormat("({0},{1}),", entity.UserId, entity.RoleId);
            }
            sql.Remove(sql.Length - 1, 1);

            var command = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteNonQuery(command);
        }

        public void DeleteUserRole(RoleFilter filter, ConnectionOption option = null)
        {
            if (filter == null) return;

            var sql = new StringBuilder("DELETE FROM UserRole WHERE 1=1 ");

            if (filter.UserIds != null && filter.UserIds.Length > 0) sql.AppendFormat(" AND UserId IN ({0})", String.Join(",", filter.UserIds));
            if (filter.RoleIds != null && filter.RoleIds.Length > 0) sql.AppendFormat(" AND RoleId IN ({0})", String.Join(",", filter.RoleIds));
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteNonQuery(cmd);
        }


        /// <summary>
        /// Generate command for creating/updating
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private DbCommand GenerateCUCommand(string sql, UserEntity entity)
        {
            if (String.IsNullOrEmpty(sql) || entity == null) return null;

            DbCommand command = db.GetSqlStringCommand(sql);

            if (entity.Id.HasValue)
            {
                db.AddInParameter(command, "Id", DbType.Int64, entity.Id);
            }

            db.AddInParameter(command, "SpId", DbType.Int64, entity.SpId);
            db.AddInParameter(command, "Name", DbType.String, entity.Name);
            db.AddInParameter(command, "RealName", DbType.String, entity.RealName);
            db.AddInParameter(command, "UserType", DbType.Int64, entity.UserType.HasValue ? entity.UserType.Value: (object)DBNull.Value);
            db.AddInParameter(command, "Password", DbType.String, entity.Password);
            db.AddInParameter(command, "PasswordToken", DbType.String, entity.PasswordToken ?? (object)DBNull.Value);
            db.AddInParameter(command, "PasswordTokenDate", DbType.DateTime, entity.PasswordTokenDate ?? (object)DBNull.Value);
            db.AddInParameter(command, "Comment", DbType.String, entity.Comment);
            db.AddInParameter(command, "Title", DbType.Int32, (int)entity.Title);
            db.AddInParameter(command, "Telephone", DbType.String, entity.Telephone);
            //db.AddInParameter(command, "Password", DbType.String, entity.Password);
            db.AddInParameter(command, "Email", DbType.String, entity.Email);
            db.AddInParameter(command, "Status", DbType.Int32, Convert.ToInt32(entity.Status));
            db.AddInParameter(command, "DemoStatus", DbType.Int32, entity.DemoStatus);
            db.AddInParameter(command, "UpdateUser", DbType.String, entity.UpdateUser);
            db.AddInParameter(command, "UpdateTime", DbType.DateTime, entity.UpdateTime);

            return command;
        }

        /// <summary>
        /// Generate commmand for retrieving using sql & filter
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private DbCommand GenerateRCommand(string sql, UserFilter filter)
        {
            if (String.IsNullOrEmpty(sql) || filter == null) return null;

            DbCommand command = db.GetSqlStringCommand(sql);

            if (filter.ExcludeId.HasValue)
            {
                db.AddInParameter(command, "ExcludeId", DbType.Int64, filter.ExcludeId.Value);
            }
            //if (filter.CustomerId.HasValue)
            //{
            //    db.AddInParameter(command, "HierarchyId", DbType.Int64, filter.CustomerId.Value);
            //}
            if (!String.IsNullOrEmpty(filter.LikeRealName))
            {
                db.AddInParameter(command, "LikeRealName", DbType.String, "%" + filter.LikeRealName + "%");
            }
            if (!String.IsNullOrEmpty(filter.RealName))
            {
                db.AddInParameter(command, "RealName", DbType.String, filter.RealName);
            }
            if (!String.IsNullOrEmpty(filter.LikeName))
            {
                db.AddInParameter(command, "LikeName", DbType.String, "%" + filter.LikeName + "%");
            }
            if (!String.IsNullOrEmpty(filter.Name))
            {
                db.AddInParameter(command, "Name", DbType.String, filter.Name);
            }

            //if (filter.RoleId.HasValue)
            //{
            //    db.AddInParameter(command, "UserType", DbType.Int64, filter.RoleId.Value);
            //}

            return command;
        }
        /// <summary>
        /// Generate where condition using tag filter, 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="startWithAnd">If true, the condition generated with be started with and, and vice versa</param>
        /// <param name="needTablePrefix">if true, all fields will contain a prefix "Tag.", and vice versa</param>
        /// <returns></returns>
        private static String GenerateWhereCondition(UserFilter filter, bool startWithAnd = true, bool needTablePrefix = false)
        {
            if (filter == null) return String.Empty;

            var sql = new StringBuilder();
            var tablePrefixStr = needTablePrefix ? UserTable + "." : "";

            if (filter.SpId != -1)
            {
                sql.AppendFormat(" AND {0}SpId=" + filter.SpId, tablePrefixStr);
            }

            if (filter.ExcludeId.HasValue)
            {
                sql.AppendFormat(" AND {0}Id<>@ExcludeId", tablePrefixStr);
            }
            if (filter.UserIds != null && filter.UserIds.Length > 0)
            {
                if (filter.UserIds.Length > 0)
                {
                    sql.Append(GenerateWhereCondition(tablePrefixStr + "Id", filter.UserIds, true));
                }
                else
                {
                    sql.Append(" AND 1<>1");
                }
            }
            if (filter.RoleIds != null && filter.RoleIds.Length > 0)
            {
                sql.AppendFormat(" AND Id IN (SELECT UserId FROM UserRole Where 1=1 {0})", GenerateWhereCondition("RoleId", filter.RoleIds, true));
            }
            if (filter.CustomerIds != null && filter.CustomerIds.Length > 0)
            {
                sql.AppendFormat(" AND Id IN (SELECT UserId FROM UserCustomer Where 1=1 {0})", GenerateWhereCondition("HierarchyId", filter.CustomerIds, true));
            }
            if (!String.IsNullOrEmpty(filter.Name))
            {
                sql.AppendFormat(" AND {0}Name=@Name", tablePrefixStr);
            }
            if (!String.IsNullOrEmpty(filter.RealName))
            {
                sql.AppendFormat(" AND {0}RealName=@RealName", tablePrefixStr);
            }
            if (!String.IsNullOrEmpty(filter.LikeName))
            {
                sql.AppendFormat(" AND {0}Name LIKE @LikeName", tablePrefixStr);
            }
            if (!String.IsNullOrEmpty(filter.LikeRealName))
            {
                sql.AppendFormat(" AND {0}RealName LIKE @LikeRealName", tablePrefixStr);
            }
            if (filter.Title.HasValue)
            {
                sql.AppendFormat(" AND {0}Title={1}", tablePrefixStr, (int)filter.Title.Value);
            }
            if (filter.DemoStatus.HasValue)
            {
                sql.AppendFormat(" AND {0}DemoStatus={1}", tablePrefixStr, (int)filter.DemoStatus.Value);
            }
            if (sql.Length > 4 && !startWithAnd) sql.Remove(0, 4);

            return sql.ToString();
        }

        /// <summary>
        /// Generate sql using <paramref name="filter"/> and <paramref name="order"/>.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private static String GenerateSQL(UserFilter filter, UserOrder order)
        {
            if (order.Equals(default(UserOrder)) || order.Orders == null || order.Orders.Length == 0)
            {
                order.Orders = new[] { new UserOrder.Order { Column = UserColumn.Name, Type = OrderType.ASC } };
            }

            var sbOrderby = new StringBuilder(" ");

            var orders = order.Orders;
            for (int i = 0; i < orders.Length; i++)
            {
                sbOrderby.Append(orders[i].Column);
                sbOrderby.Append(" ");
                sbOrderby.Append(orders[i].Type);

                if (i != orders.Length - 1) sbOrderby.Append(",");
            }
            sbOrderby.Append(" ");

            var limit = int.MaxValue;
            int.TryParse(ConfigurationManager.AppSettings[DAConfigurationKey.TAGLIMITINONEQUERY], out limit);

            var paging = new Paging
            {
                Columns = UserColumns,
                Table = UserTable,
                Start = order.Start,
                Limit = order.Limit == 0 ? limit : order.Limit,
                OrderBy = sbOrderby.ToString(),
                Where = GenerateWhereCondition(filter, false, false)
            };

            var sql = PagingHelper.GeneratePagingSql(paging);
            return sql;
        }
        /// <summary>
        /// Convert reader to entities
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static List<UserEntity> ConvertToUserEntities(IDataReader reader)
        {
            var list = new List<UserEntity>();

            using (reader)
            {
                while (reader.Read())
                {
                    var entity = ReadToEntity(reader);
                    list.Add(entity);
                }
                reader.Close();
            }
            return list;
        }
        /// <summary>
        /// Convert reader to entities
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        private static List<UserEntity> ConvertToUserEntities(IDataReader reader, out long totalCount)
        {
            totalCount = 0;
            var list = new List<UserEntity>();

            using (reader)
            {
                while (reader.Read())
                {
                    var entity = ReadToEntity(reader);
                    list.Add(entity);
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0)) totalCount = reader.GetInt32(0);
                    }
                }
                reader.Close();
            }
            return list;
        }
        /// <summary>
        /// Convert reader to entities
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        private static UserEntity ReadToEntity(IDataReader reader)
        {
            var entity = new UserEntity();

            int ordinal = reader.GetOrdinal("Id");
            entity.Id = reader.GetInt64(ordinal);

            ordinal = reader.GetOrdinal("Name");
            entity.Name = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("SpId");
            entity.SpId = reader.GetInt64(ordinal);

            ordinal = reader.GetOrdinal("RealName");
            entity.RealName = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("Password");
            entity.Password = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("PasswordToken");
            if (!reader.IsDBNull(ordinal)) entity.PasswordToken = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("PasswordTokenDate");
            if (!reader.IsDBNull(ordinal)) entity.PasswordTokenDate = reader.GetDateTime(ordinal);

            ordinal = reader.GetOrdinal("Comment");
            if (!reader.IsDBNull(ordinal)) entity.Comment = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("Telephone");
            if (!reader.IsDBNull(ordinal)) entity.Telephone = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("Title");
            if (!reader.IsDBNull(ordinal)) entity.Title = (UserTitle)reader.GetInt32(ordinal);

            ordinal = reader.GetOrdinal("UserType");
            if (!reader.IsDBNull(ordinal)) entity.UserType = reader.GetInt32(ordinal);

            ordinal = reader.GetOrdinal("Email");
            if (!reader.IsDBNull(ordinal)) entity.Email = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("Status");
            entity.Status = (EntityStatus)reader.GetInt32(ordinal);

            ordinal = reader.GetOrdinal("DemoStatus");
            if (!reader.IsDBNull(ordinal)) entity.DemoStatus = reader.GetInt32(ordinal);

            ordinal = reader.GetOrdinal("UpdateUser");
            entity.UpdateUser = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("UpdateTime");
            entity.UpdateTime = reader.GetDateTime(ordinal);

            ordinal = reader.GetOrdinal("Version");
            entity.Version = reader.GetInt64(ordinal);

            return entity;
        }


        public UserEntity RetrieveUserById(long userId)
        {
            var sql = String.Format("SELECT {0} FROM {1} WHERE Id = @Id", UserColumns, UserTable);
            DbCommand command = db.GetSqlStringCommand(sql);

            db.AddInParameter(command, "Id", DbType.Int64, userId);

            var list = ConvertToUserEntities(db.ExecuteReader(command));
            return list.ToArray()[0];
        }
    }
}
