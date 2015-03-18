using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using System;



namespace SE.DSP.Foundation.DA.Service
{
    using SE.DSP.Foundation.DA.Interface;
    using SE.DSP.Foundation.Infrastructure.BaseClass;
    using SE.DSP.Foundation.Infrastructure.BE.Entities;
    using SE.DSP.Foundation.Infrastructure.Enumerations;
    public class UomDA : DABase, IUomDA
    {
        #region Retrieve
        public UomEntity RetrieveUomById(long uomId)
        {
            string sql = @"SELECT Id,Code,Comment,Status,UpdateUser,UpdateTime,CAST(Version AS bigint) AS Version FROM Uom WHERE Id=@Id";

            DbCommand command = Database.GetSqlStringCommand(sql);

            Database.AddInParameter(command, "Id", DbType.Int64, uomId);

            List<UomEntity> uomList = this.ReadUomList(Database.ExecuteReader(command));

            if (uomList.Count == 0)
                return null;

           return uomList[0];
        }

        public UomEntity[] RetrieveAllUom()
        {
            string sql = @"SELECT Id,Code,Comment,Status,UpdateUser,UpdateTime,CAST(Version AS bigint) AS Version FROM Uom";

            DbCommand command = Database.GetSqlStringCommand(sql);


            List<UomEntity> uomList = this.ReadUomList(Database.ExecuteReader(command));

            return uomList.ToArray();
        }

        public UomEntity[] RetrieveUomByCommodityId(long commodityId)
        {
            string sql = @" SELECT Uom.Id,Uom.Code,Uom.Comment,Uom.Status,Uom.UpdateUser,Uom.UpdateTime,CAST(Uom.Version AS bigint) AS Version FROM Uom
	                            INNER JOIN UomGroupRelation on Uom.Id = UomGroupRelation.UomId
	                            INNER JOIN UomGroup on UomGroupRelation.GroupId = UomGroup.Id
                            WHERE UomGroup.CommodityId = @commodityid";

            DbCommand command = Database.GetSqlStringCommand(sql);

            Database.AddInParameter(command, "CommodityId", DbType.Int64, commodityId);

            List<UomEntity> uomList = this.ReadUomList(Database.ExecuteReader(command));

            return uomList.ToArray();
        }

        private List<UomEntity> ReadUomList(IDataReader reader)
        {
            List<UomEntity> uomList = new List<UomEntity>();

            using (reader)
            {
                while (reader.Read())
                {
                    UomEntity uom = new UomEntity();

                    int ordinal = reader.GetOrdinal("Id");
                    uom.Id = reader.GetInt64(ordinal);

                    ordinal = reader.GetOrdinal("Code");
                    uom.Code = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("Comment");

                    if (reader.IsDBNull(ordinal))
                    {
                        uom.Comment = null;
                    }
                    else
                    {
                        uom.Comment = reader.GetString(ordinal);
                    }

                    ordinal = reader.GetOrdinal("Status");
                    uom.Status = (EntityStatus)reader.GetInt32(ordinal);

                    ordinal = reader.GetOrdinal("UpdateUser");
                    uom.UpdateUser = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("UpdateTime");
                    uom.UpdateTime = reader.GetDateTime(ordinal);

                    ordinal = reader.GetOrdinal("Version");
                    uom.Version = reader.GetInt64(ordinal);

                    uomList.Add(uom);
                }

                reader.Close();
            }

            uomList = ConvertDataSourceByLanguage(uomList, "Uom", "Code", "Comment");

            return uomList;
        }


    


   

        #endregion
    }
}