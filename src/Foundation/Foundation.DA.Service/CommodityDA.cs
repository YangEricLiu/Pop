using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using SE.DSP.Foundation.DA.Interface;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Enumerations;

namespace SE.DSP.Foundation.DA.Service
{
    public class CommodityDA : DABase, ICommodityDA
    {

        public CommodityDA() { }

        #region Retrieve
        public CommodityEntity RetrieveCommodityById(long commodityId)
        {
            string sql = @"SELECT Id,Code,Comment,Status,UpdateUser,UpdateTime,CAST(Version AS bigint) AS Version FROM Commodity WHERE Id=@Id";

            DbCommand command = Database.GetSqlStringCommand(sql);

            Database.AddInParameter(command, "Id", DbType.Int64, commodityId);

            List<CommodityEntity> commodityList = this.ReadCommodityList(Database.ExecuteReader(command));

            return commodityList.Count == 0 ? null : commodityList[0];
        }

        public CommodityEntity[] RetrieveAllCommodity()
        {
            string sql = @"SELECT Id,Code,Comment,Status,UpdateUser,UpdateTime,CAST(Version AS bigint) AS Version FROM Commodity";

            DbCommand command = Database.GetSqlStringCommand(sql);

            List<CommodityEntity> commodityList = this.ReadCommodityList(Database.ExecuteReader(command));

            return commodityList.ToArray();
        }
        #endregion

        private List<CommodityEntity> ReadCommodityList(IDataReader reader)
        {
            List<CommodityEntity> commodityUOMList = new List<CommodityEntity>();

            using (reader)
            {
                while (reader.Read())
                {
                    CommodityEntity commodity = new CommodityEntity();

                    int ordinal = reader.GetOrdinal("Id");
                    commodity.Id = reader.GetInt64(ordinal);

                    ordinal = reader.GetOrdinal("Code");
                    commodity.Code = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("Comment");

                    if (reader.IsDBNull(ordinal))
                    {
                        commodity.Comment = null;
                    }
                    else
                    {
                        commodity.Comment = reader.GetString(ordinal);
                    }

                    ordinal = reader.GetOrdinal("Status");
                    commodity.Status = (EntityStatus)reader.GetInt32(ordinal);

                    ordinal = reader.GetOrdinal("UpdateUser");
                    commodity.UpdateUser = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("UpdateTime");
                    commodity.UpdateTime = reader.GetDateTime(ordinal);

                    ordinal = reader.GetOrdinal("Version");
                    commodity.Version = reader.GetInt64(ordinal);

                    commodityUOMList.Add(commodity);
                }

               

                reader.Close();
            }
            commodityUOMList = ConvertDataSourceByLanguage(commodityUOMList, "Commodity", "Code", "Comment");

            return commodityUOMList;
        }
    }
}