
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Entities;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.DA.Interface;
using System.Collections.Generic;
using System.Data;

namespace SE.DSP.Foundation.DA.Service
{
    public class ZoneDA : DABase, IZoneDA
    {
        public ZoneDA() { }

        #region Retrieve
        public ZoneEntity RetrieveZoneById(long zoneId)
        {
            var sql = @"SELECT Id,Code,ParentId,Comment,Status,UpdateUser,UpdateTime,CAST(Version AS bigint) AS Version FROM Zone WHERE Id=@Id";

            var command = Database.GetSqlStringCommand(sql);

            Database.AddInParameter(command, "Id", DbType.Int64, zoneId);

            var zoneList = this.ReadZoneList(Database.ExecuteReader(command));

            return zoneList.Count == 0 ? null : zoneList[0];
        }

        public ZoneEntity[] RetrieveAllZones()
        {
            var sql = @"SELECT Id,Code,ParentId,Comment,Status,UpdateUser,UpdateTime,CAST(Version AS bigint) AS Version FROM Zone";

            var command = Database.GetSqlStringCommand(sql);

            var zoneList = this.ReadZoneList(Database.ExecuteReader(command));

            return zoneList.ToArray();
        }
        #endregion

        private List<ZoneEntity> ReadZoneList(IDataReader reader)
        {
            var zoneList = new List<ZoneEntity>();

            using (reader)
            {
                while (reader.Read())
                {
                    var zone = new ZoneEntity();

                    int ordinal = reader.GetOrdinal("Id");
                    zone.Id = reader.GetInt64(ordinal);

                    ordinal = reader.GetOrdinal("Code");
                    zone.Code = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("ParentId");

                    if (reader.IsDBNull(ordinal))
                    {
                        zone.ParentId = null;
                    }
                    else
                    {
                        zone.ParentId = reader.GetInt64(ordinal);
                    }

                    ordinal = reader.GetOrdinal("Comment");

                    if (reader.IsDBNull(ordinal))
                    {
                        zone.Comment = null;
                    }
                    else
                    {
                        zone.Comment = reader.GetString(ordinal);
                    }

                    ordinal = reader.GetOrdinal("Status");
                    zone.Status = (EntityStatus)reader.GetInt32(ordinal);

                    ordinal = reader.GetOrdinal("UpdateUser");
                    zone.UpdateUser = reader.GetString(ordinal);

                    ordinal = reader.GetOrdinal("UpdateTime");
                    zone.UpdateTime = reader.GetDateTime(ordinal);

                    ordinal = reader.GetOrdinal("Version");
                    zone.Version = reader.GetInt64(ordinal);

                    zoneList.Add(zone);
                }

                zoneList = ConvertDataSourceByLanguage(zoneList, "Zone", "Code", "Comment");

                reader.Close();
            }

            return zoneList;
        }
    }
}