/*------------------------------Summary------------------------------------------------------
 * Product Name : REM
 * File Name	: BaseDA.cs
 * Author	    : Figo
 * Date Created : 2011-09-28
 * Description  : The base data access abstract class
--------------------------------------------------------------------------------------------*/

using Microsoft.Practices.EnterpriseLibrary.Data;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Structure;
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace SE.DSP.Foundation.Infrastructure.BaseClass
{
    /// <summary>
    /// The base data access abstract class
    /// </summary>
    public abstract class DABase
    {
        /// <summary>
        /// A instance of <see cref="Database" /> for REM.
        /// </summary>
        protected static  Database Database = GetDatabase(GetInformationConnectionString());
        protected static readonly int InsertBatchSize = 500;

        protected static Database GetDatabase(string connectionString)
        {
            return new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(connectionString);
        }


        //public static void SetDatabase(string connectionString)
        //{
        //    Database = GetDatabase(connectionString);
        //}


        //public static void ResetDatabase()
        //{
        //    Database = GetDatabase(GetInformationConnectionString());
        //}

        protected static string GetInformationConnectionString()
        {
            var connstr = System.Configuration.ConfigurationManager.ConnectionStrings["REMInformation"].ConnectionString;

            connstr = connstr
                .Replace("{" + DeploymentConfigKey.SpDbDatabase.ToString() + "}", ConfigHelper.Get(DeploymentConfigKey.SpDbDatabase))
                .Replace("{" + DeploymentConfigKey.SpDbServerIP.ToString() + "}", ConfigHelper.Get(DeploymentConfigKey.SpDbServerIP))
                .Replace("{" + DeploymentConfigKey.SpDbUser.ToString() + "}", ConfigHelper.Get(DeploymentConfigKey.SpDbUser))
                .Replace("{" + DeploymentConfigKey.SpDbPassword.ToString() + "}", ConfigHelper.Get(DeploymentConfigKey.SpDbPassword))
                .Replace("{" + DeploymentConfigKey.SpDbMaxPoolSize.ToString() + "}", ConfigHelper.Get(DeploymentConfigKey.SpDbMaxPoolSize));

            return connstr;
        }



        /// <summary>
        /// convert data by language
        /// </summary>
        /// <typeparam name="T">DtoBase</typeparam>
        /// <param name="entities">Dto collection</param>
        /// <param name="tableName">db Table name</param>
        /// <param name="resKeyFiled">db key column</param>
        /// <param name="resValueField">db column value</param>
        /// <returns>return converted dto collection</returns>
        protected static List<T> ConvertDataSourceByLanguage<T>(IEnumerable<T> entities, string tableName, string resKeyFiled, string resValueField) where T : class //where T : DtoBase,EntityBase
        {
            if (entities == null ||
                string.IsNullOrEmpty(tableName) ||
                string.IsNullOrEmpty(resKeyFiled) ||
                string.IsNullOrEmpty(resValueField))
                return null;

            var tablePrefix = tableName;

            List<string> keys = new List<string>();

            foreach (var temp in entities)
            {
                string tempKey;
                try
                {
                    tempKey = temp.GetType().GetProperty(resKeyFiled).GetValue(temp).ToString();

                    if (tempKey.Contains("(") ||
                      tempKey.Contains(")"))
                    {
                        tempKey = tempKey.Replace("(", " ").Replace(")", " ");
                    }

                    keys.Add(string.Format("{0}_{1}", tablePrefix, tempKey));
                }
                catch (Exception ex)
                {

                }
            }

            var dict = I18nHelper.GetValues(DAContext.Language, I18nResourceType.DB, keys.ToArray());

            foreach (var temp in entities)
            {
                var key = string.Format("{0}_{1}", tablePrefix, temp.GetType().GetProperty(resKeyFiled).GetValue(temp).ToString());

                key = key.Replace("(", " ").Replace(")", " ");

                if (dict.ContainsKey(key))
                {
                    temp.GetType().GetProperty(resValueField).SetValue(temp, dict[key]);
                }

            }
            var iterator = entities.GetEnumerator();

            List<T> list = new List<T>();

            while (iterator.MoveNext())
            {
                list.Add(iterator.Current);
            }

            return list;
        }

        protected static String GenerateWhereCondition(String filedName, IList<long> ids, bool startWithAnd = true)
        {
            if (String.IsNullOrEmpty(filedName) || ids == null) return String.Empty;
            if (ids.Count == 0) return startWithAnd ? " AND 1=0 " : " 1=0 ";

            String condition;

            if (ids.Count == 1) condition = filedName + '=' + ids[0];
            else condition = filedName + " IN (" + String.Join(",", ids) + ")";

            return startWithAnd ? " AND " + condition : condition;
        }


        /// <summary>
        /// Generate where condition for status filter.
        /// </summary>
        /// <param name="filter">Status filter</param>
        /// <param name="startWithAnd">If true, the condition generated with be started with and, and vice versa</param>
        /// <param name="tablePrefix">Table prefix, null by default</param>
        /// <returns></returns>
        protected static String GenerateWhereCondition(StatusFilter filter, bool startWithAnd = true, String tablePrefix = null)
        {
            if (filter == null) return String.Empty;
            var tablePrefixStr = String.IsNullOrEmpty(tablePrefix) ? "" : tablePrefix;

            var statuses = filter.Statuses;
            var excludeStatus = filter.ExcludeStatus;

            if (statuses == null || statuses.Length == 0)
                return string.Empty;

            var condition = new StringBuilder();
            if (startWithAnd) condition.Append(" AND ");
            condition.AppendFormat(" {0}Status", tablePrefixStr);

            if (statuses.Length == 1)
            {
                condition.Append(excludeStatus ? " <> " : " = ");
                condition.Append(Convert.ToInt32(statuses[0]));
            }
            else if (statuses.Length > 1)
            {
                var statusStr = string.Join(",", statuses.Cast<int>());
                condition.Append(excludeStatus ? " NOT IN " : " IN ");
                condition.Append("(" + statusStr + ")");
            }
            return condition.ToString();
        }
    }
}