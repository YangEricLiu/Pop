
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.DA.Interface;
using System.Collections.Generic;
using System.Data;

namespace SE.DSP.Foundation.DA.Service
{
    public class IndustryDA : DABase, IIndustryDA
    {
        public IndustryDA() { }

        #region Retrieve
        public IndustryEntity RetrieveIndustryById(long industryId)
        {
            var sql = @"SELECT Id,Code,ParentId,Comment,Status,UpdateUser,UpdateTime,CAST(Version AS bigint) AS Version FROM Industry WHERE Id=@Id";

            var command = Database.GetSqlStringCommand(sql);

            Database.AddInParameter(command, "Id", DbType.Int64, industryId);

            var industryList = this.ReadIndustryList(Database.ExecuteReader(command));

            return industryList.Count == 0 ? null : industryList[0];
        }

        public IndustryEntity[] RetrieveAllIndustries()
        {
            var sql = @"SELECT Id,Code,ParentId,Comment,Status,UpdateUser,UpdateTime,CAST(Version AS bigint) AS Version FROM Industry";

            var command = Database.GetSqlStringCommand(sql);

            var industryList = this.ReadIndustryList(Database.ExecuteReader(command));

            return industryList.ToArray();
        }

        private List<IndustryEntity> ReadIndustryList(IDataReader reader)
        {
            var industryList = new List<IndustryEntity>();

            using (reader)
            {
                while (reader.Read())
                {
                    var industry = new IndustryEntity();

                    int ordinal = reader.GetOrdinal("Id");
                    industry.Id = reader.GetInt64(ordinal);

                    ordinal = reader.GetOrdinal("Code");
                    industry.Code = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("ParentId");

                    if (reader.IsDBNull(ordinal))
                    {
                        industry.ParentId = null;
                    }
                    else
                    {
                        industry.ParentId = reader.GetInt64(ordinal);
                    }

                    ordinal = reader.GetOrdinal("Comment");

                    if (reader.IsDBNull(ordinal))
                    {
                        industry.Comment = null;
                    }
                    else
                    {
                        industry.Comment = reader.GetString(ordinal);
                    }

                    ordinal = reader.GetOrdinal("Status");
                    industry.Status = (EntityStatus)reader.GetInt32(ordinal);

                    ordinal = reader.GetOrdinal("UpdateUser");
                    industry.UpdateUser = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("UpdateTime");
                    industry.UpdateTime = reader.GetDateTime(ordinal);

                    ordinal = reader.GetOrdinal("Version");
                    industry.Version = reader.GetInt64(ordinal);

                    industryList.Add(industry);
                }

                reader.Close();
            }

            industryList =ConvertDataSourceByLanguage(industryList, "Industry", "Code", "Comment");

            return industryList;
        }
        #endregion
    }
}