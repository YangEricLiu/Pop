using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using SE.DSP.Foundation.DA.Interface;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Enumerations;

namespace SE.DSP.Foundation.DA.Service
{
    public class UomGroupRelationDA : DABase, IUomGroupRelationDA
    {
        #region Retrieve
        public UomGroupRelationEntity RetrieveCommodityUom(long commodityId, long uomId)
        {
            string sql = @" select 
                                UomGroupRelation.Precision,UomGroupRelation.GroupId,UomGroupRelation.UomId,UomGroupRelation.IsBase,UomGroupRelation.IsCommon,UomGroupRelation.IsStandardCoal,UomGroupRelation.Factor,
                                Commodity.Id as CommodityId,Commodity.Code as CommodityCode,Commodity.Comment as CommodityComment,Commodity.Status as CommodityStatus,Commodity.UpdateUser as CommodityUpdateUser,Commodity.UpdateTime as CommodityUpdateTime,Cast(Commodity.Version as bigint) as CommodityVersion,
                                Uom.Code as UomCode,Uom.Comment as UomComment,Uom.Status as UomStatus,Uom.UpdateUser as UomUpdateUser,Uom.UpdateTime as UomUpdateTime,Cast(Uom.Version as bigint) as UomVersion
                            from Commodity
                                inner join UomGroup on Commodity.Id = UomGroup.CommodityId
                                inner join UomGroupRelation on UomGroup.Id = UomGroupRelation.GroupId
                                inner join Uom on UomGroupRelation.UomId = Uom.Id
                            where Commodity.Id=@commodityId and Uom.Id=@uomId";

            DbCommand command = Database.GetSqlStringCommand(sql);

            Database.AddInParameter(command, "CommodityId", DbType.Int64, commodityId);
            Database.AddInParameter(command, "UomId", DbType.Int64, uomId);

            List<UomGroupRelationEntity> commodityUOMList = this.ReadEntityList(Database.ExecuteReader(command));

            return commodityUOMList.Count == 0 ? null : commodityUOMList[0];
        }

        public UomGroupRelationEntity[] RetrieveUomByGroup(long groupId)
        {
            string sql = @" select 
                                UomGroupRelation.Precision,UomGroupRelation.GroupId,UomGroupRelation.UomId,UomGroupRelation.IsBase,UomGroupRelation.IsCommon,UomGroupRelation.IsStandardCoal,UomGroupRelation.Factor,
                                Commodity.Id as CommodityId,Commodity.Code as CommodityCode,Commodity.Comment as CommodityComment,Commodity.Status as CommodityStatus,Commodity.UpdateUser as CommodityUpdateUser,Commodity.UpdateTime as CommodityUpdateTime,Cast(Commodity.Version as bigint) as CommodityVersion,
                                Uom.Code as UomCode,Uom.Comment as UomComment,Uom.Status as UomStatus,Uom.UpdateUser as UomUpdateUser,Uom.UpdateTime as UomUpdateTime,Cast(Uom.Version as bigint) as UomVersion
                            from UomGroup
                                inner join Commodity on UomGroup.CommodityId = Commodity.Id
                                inner join UomGroupRelation on UomGroup.Id = UomGroupRelation.GroupId
                                inner join Uom on UomGroupRelation.UomId = Uom.Id
                            where UomGroup.Id=@groupId";

            DbCommand command = Database.GetSqlStringCommand(sql);

            Database.AddInParameter(command, "groupId", DbType.Int64, groupId);

            List<UomGroupRelationEntity> entityList = this.ReadEntityList(Database.ExecuteReader(command));

            return entityList.ToArray();
        }

        public UomGroupRelationEntity[] RetrieveUomRelationByCommodityId(long commodityId)
        {
            string sql = @" select 
                                UomGroupRelation.Precision,UomGroupRelation.GroupId,UomGroupRelation.UomId,UomGroupRelation.IsBase,UomGroupRelation.IsCommon,UomGroupRelation.IsStandardCoal,UomGroupRelation.Factor,
                                Commodity.Id as CommodityId,Commodity.Code as CommodityCode,Commodity.Comment as CommodityComment,Commodity.Status as CommodityStatus,Commodity.UpdateUser as CommodityUpdateUser,Commodity.UpdateTime as CommodityUpdateTime,Cast(Commodity.Version as bigint) as CommodityVersion,
                                Uom.Code as UomCode,Uom.Comment as UomComment,Uom.Status as UomStatus,Uom.UpdateUser as UomUpdateUser,Uom.UpdateTime as UomUpdateTime,Cast(Uom.Version as bigint) as UomVersion
                            from Commodity
                                inner join UomGroup on Commodity.Id = UomGroup.CommodityId
                                inner join UomGroupRelation on UomGroup.Id = UomGroupRelation.GroupId
                                inner join Uom on UomGroupRelation.UomId = Uom.Id
                            where Commodity.Id=@commodityId";

            DbCommand command = Database.GetSqlStringCommand(sql);

            Database.AddInParameter(command, "CommodityId", DbType.Int64, commodityId); 

            List<UomGroupRelationEntity> entityList = this.ReadEntityList(Database.ExecuteReader(command));

            return entityList.ToArray();
        }

        public UomGroupRelationEntity[] RetrieveCarbonConvertableCommodity()
        {
            string sql = @" SELECT
                                UomGroupRelation.Precision,UomGroupRelation.GroupId,UomGroupRelation.UomId,UomGroupRelation.IsBase,UomGroupRelation.IsCommon,UomGroupRelation.IsStandardCoal,UomGroupRelation.Factor,
                                Commodity.Id as CommodityId,Commodity.Code as CommodityCode,Commodity.Comment as CommodityComment,Commodity.Status as CommodityStatus,Commodity.UpdateUser as CommodityUpdateUser,Commodity.UpdateTime as CommodityUpdateTime,Cast(Commodity.Version as bigint) as CommodityVersion,
                                Uom.Code as UomCode,Uom.Comment as UomComment,Uom.Status as UomStatus,Uom.UpdateUser as UomUpdateUser,Uom.UpdateTime as UomUpdateTime,Cast(Uom.Version as bigint) as UomVersion
                            FROM Commodity
	                            INNER JOIN UomGroup ON Commodity.id = UomGroup.commodityid
	                            INNER JOIN UomGroupRelation ON UomGroup.id = UomGroupRelation.groupid
	                            INNER JOIN Uom ON UomGroupRelation.uomid = uom.id
                            WHERE UomGroup.IsEnergyConsumptionGroup = 1 AND UomGroupRelation.IsStandardCoal = 1
                            ORDER BY Commodity.Id";

            DbCommand command = Database.GetSqlStringCommand(sql);

            List<UomGroupRelationEntity> entityList = this.ReadEntityList(Database.ExecuteReader(command));

            return entityList.ToArray();
        }

        public UomGroupRelationEntity[] RetrieveUomRelation()
        {
            string sql = @" select 
                                UomGroupRelation.Precision,UomGroupRelation.GroupId,UomGroupRelation.UomId,UomGroupRelation.IsBase,UomGroupRelation.IsCommon,UomGroupRelation.IsStandardCoal,UomGroupRelation.Factor,
                                Commodity.Id as CommodityId,Commodity.Code as CommodityCode,Commodity.Comment as CommodityComment,Commodity.Status as CommodityStatus,Commodity.UpdateUser as CommodityUpdateUser,Commodity.UpdateTime as CommodityUpdateTime,Cast(Commodity.Version as bigint) as CommodityVersion,
                                Uom.Code as UomCode,Uom.Comment as UomComment,Uom.Status as UomStatus,Uom.UpdateUser as UomUpdateUser,Uom.UpdateTime as UomUpdateTime,Cast(Uom.Version as bigint) as UomVersion
                            from Commodity
                                inner join UomGroup on Commodity.Id = UomGroup.CommodityId
                                inner join UomGroupRelation on UomGroup.Id = UomGroupRelation.GroupId
                                inner join Uom on UomGroupRelation.UomId = Uom.Id";

            DbCommand command = Database.GetSqlStringCommand(sql);

            List<UomGroupRelationEntity> entityList = this.ReadEntityList(Database.ExecuteReader(command));

            return entityList.ToArray();
        }
        #endregion

        private List<UomGroupRelationEntity> ReadEntityList(IDataReader reader)
        {
            List<UomGroupRelationEntity> UomGroupRelationEntityList = new List<UomGroupRelationEntity>();

            using (reader)
            {
                while (reader.Read())
                {
                    var entity = new UomGroupRelationEntity();

                    int ordinal = reader.GetOrdinal("Precision");
                    entity.Precision = reader.IsDBNull(ordinal) ? 1 : reader.GetInt32(ordinal);
                    
                    ordinal = reader.GetOrdinal("GroupId");
                    entity.GroupId = reader.GetInt64(ordinal);
                    
                    ordinal = reader.GetOrdinal("UomId");
                    entity.UomId = reader.GetInt64(ordinal);
                    
                    ordinal = reader.GetOrdinal("IsBase");
                    entity.IsBase = reader.GetBoolean(ordinal);
                    
                    ordinal = reader.GetOrdinal("IsCommon");
                    entity.IsCommon = reader.GetBoolean(ordinal);
                    
                    ordinal = reader.GetOrdinal("IsStandardCoal");
                    entity.IsStandardCoal = reader.GetBoolean(ordinal);
                    
                    ordinal = reader.GetOrdinal("Factor");
                    entity.Factor = reader.GetDecimal(ordinal);


                    entity.Commodity = new CommodityEntity();
                    ordinal = reader.GetOrdinal("CommodityCode");
                    entity.Commodity.Code = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("CommodityId");
                    entity.Commodity.Id = reader.GetInt64(ordinal);

                    ordinal = reader.GetOrdinal("CommodityComment");

                   // entity.Commodity.Comment = reader.GetString(ordinal);
                    entity.Commodity.Comment = I18nHelper.GetValue(
                        DAContext.Language, 
                        I18nResourceType.DB, 
                        string.Format("{0}_{1}", 
                        "Commodity", 
                        entity.Commodity.Code));

                    ordinal = reader.GetOrdinal("CommodityUpdateTime");
                    entity.Commodity.UpdateTime = reader.GetDateTime(ordinal);

                    ordinal = reader.GetOrdinal("CommodityUpdateUser");
                    entity.Commodity.UpdateUser = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("CommodityVersion");
                    entity.Commodity.Version = reader.GetInt64(ordinal);


                    entity.Uom = new UomEntity();
                    ordinal = reader.GetOrdinal("UomCode");
                    entity.Uom.Code = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("UomComment");
                    //entity.Uom.Comment = reader.GetString(ordinal);
                    entity.Uom.Comment = I18nHelper.GetValue(
                        DAContext.Language,
                        I18nResourceType.DB,
                        string.Format("{0}_{1}",
                        "Uom",
                        entity.Uom.Code));
                    entity.Uom.Id = entity.UomId;

                    ordinal = reader.GetOrdinal("UomUpdateTime");
                    entity.Uom.UpdateTime = reader.GetDateTime(ordinal);

                    ordinal = reader.GetOrdinal("UomUpdateUser");
                    entity.Uom.UpdateUser = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("UomVersion");
                    entity.Uom.Version = reader.GetInt64(ordinal);

                    UomGroupRelationEntityList.Add(entity);
                }

                reader.Close();
            }

            return UomGroupRelationEntityList;
        }
    }
}
