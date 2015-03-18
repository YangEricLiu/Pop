using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace SE.DSP.Foundation.DA.Service
{
    using SE.DSP.Foundation.DA.Interface;
    using SE.DSP.Foundation.Infrastructure.BaseClass;
    using SE.DSP.Foundation.Infrastructure.BE.Entities;
    public class UomGroupDA : DABase, IUomGroupDA
    {
        #region Retrieve
        public UomGroupEntity RetrieveUomGroupById(long uomGroupId)
        {
            string sql = @"SELECT [Id],[Code],[Comment],[UpdateUser],[UpdateTime],CAST([Version] as bigint) as Version,[CommodityId],[IsEnergyConsumptionGroup] FROM [UomGroup] WHERE [Id]=@Id";

            DbCommand command = Database.GetSqlStringCommand(sql);

            Database.AddInParameter(command, "Id", DbType.Int64, uomGroupId);

            List<UomGroupEntity> entityList = this.ReadEntityList(Database.ExecuteReader(command));

            return entityList.Count == 0 ? null : entityList[0];
        }

        public UomGroupEntity[] RetrieveUomGroupByCommodity(long commodityId)
        {
            string sql = @"SELECT [Id],[Code],[Comment],[UpdateUser],[UpdateTime],CAST([Version] as bigint) as Version,[CommodityId],[IsEnergyConsumptionGroup] FROM [UomGroup] WHERE [CommodityId]=@CommodityId";

            DbCommand command = Database.GetSqlStringCommand(sql);

            Database.AddInParameter(command, "CommodityId", DbType.Int64, commodityId);

            List<UomGroupEntity> entityList = this.ReadEntityList(Database.ExecuteReader(command));

            return entityList.ToArray();
        }

        public UomGroupEntity RetrieveUomGroupByCommodityAndUom(long commodityId, long uomId)
        {
            string sql = @" SELECT [uomgroup].[Id],[uomgroup].[Code],[uomgroup].[Comment],[uomgroup].[UpdateUser],[uomgroup].[UpdateTime],CAST([uomgroup].[Version] as bigint) as Version,[uomgroup].[CommodityId],[uomgroup].[IsEnergyConsumptionGroup] FROM [uom]
	                            INNER JOIN [uomgrouprelation] ON [uom].id = [uomgrouprelation].uomid
	                            INNER JOIN [uomgroup] ON [uomgrouprelation].groupid = [uomgroup].id
                            WHERE [uomgroup].commodityid=@commodityid AND [uom].id=@uomid";

            DbCommand command = Database.GetSqlStringCommand(sql);

            Database.AddInParameter(command, "commodityid", DbType.Int64, commodityId);
            Database.AddInParameter(command, "uomid", DbType.Int64, uomId);

            List<UomGroupEntity> entityList = this.ReadEntityList(Database.ExecuteReader(command));

            return entityList.Count == 0 ? null : entityList[0];
        }
        #endregion

        private List<UomGroupEntity> ReadEntityList(IDataReader reader)
        {
            List<UomGroupEntity> UomGroupEntityList = new List<UomGroupEntity>();

            using (reader)
            {
                while (reader.Read())
                {
                    UomGroupEntityList.Add(new UomGroupEntity()
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("Id")),
                        Code = reader.GetString(reader.GetOrdinal("Code")),
                        Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? String.Empty : reader.GetString(reader.GetOrdinal("Comment")),
                        CommodityId = reader.GetInt64(reader.GetOrdinal("CommodityId")),
                        IsEnergyConsumption = reader.GetBoolean(reader.GetOrdinal("IsEnergyConsumptionGroup")),
                        UpdateUser = reader.GetString(reader.GetOrdinal("UpdateUser")),
                        UpdateTime = reader.GetDateTime(reader.GetOrdinal("UpdateTime")),
                        Version = reader.GetInt64(reader.GetOrdinal("Version")),
                    });
                }

                reader.Close();
            }

            return UomGroupEntityList;
        }
    }
}
