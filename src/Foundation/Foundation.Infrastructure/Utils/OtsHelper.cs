/*------------------------------Summary------------------------------------------------------
 * Product Name : REM
 * File Name	: YouchaoHelper.cs
 * Author	    : Figo
 * Date Created : 2011-09-28
 * Description  : The utility for access Youchao
--------------------------------------------------------------------------------------------*/


using Aliyun.OpenServices;
using Aliyun.OpenServices.OpenTableService;
using Aliyun.OpenServices.OpenTableService.Utilities;
using Aliyun.OpenServices.Ots;
using SE.DSP.Foundation.Infrastructure.Constant;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;


namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// The utility for OTS accessing. 
    /// </summary>
    public class OtsHelper
    {
        /// <summary>
        /// Create OTS client
        /// </summary>
        /// <returns>A instance of <see cref="OtsXmlClient" /></returns>
        /// <remarks>Reads the OTS configuration from the app setting section in the web.config under AppHost.</remarks>
        public static IOts CreateOtsClient()
        {
            IOts otsClient;
            Aliyun.OpenServices.Ots.ClientConfiguration clientConfiguration = null;

            string ostEndPoint = ConfigHelper.Get(DAConfigurationKey.OTSENDPOINT);
            string ostAccessId = ConfigHelper.Get(DAConfigurationKey.OTSACCESSID);
            string ostAccessKey = ConfigHelper.Get(DAConfigurationKey.OTSACCESSKEY);
            string ostApiVersion = ConfigHelper.Get(DAConfigurationKey.OTSAPIVERSION);

            if (!string.IsNullOrEmpty(ConfigHelper.Get(DAConfigurationKey.PROXYHOST)))
            {
                clientConfiguration = new Aliyun.OpenServices.Ots.ClientConfiguration()
                                          {
                                              ProxyHost = ConfigHelper.Get(DAConfigurationKey.PROXYHOST),
                                              ProxyPort = Convert.ToInt32(ConfigHelper.Get(DAConfigurationKey.PROXYPORT))
                                          };

                string[] tokens = ConfigHelper.Get(DAConfigurationKey.PROXYUSERNAME).Split('\\');

                if (tokens.Length == 1)
                {
                    clientConfiguration.ProxyUserName = tokens[0];
                }
                else
                {
                    clientConfiguration.ProxyDomain = tokens[0];
                    clientConfiguration.ProxyUserName = tokens[1];
                }

                clientConfiguration.ProxyPassword = ConfigHelper.Get(DAConfigurationKey.PROXYPASSWORD);

                clientConfiguration.MaxErrorRetry = Convert.ToInt32(ConfigurationManager.AppSettings[DAConfigurationKey.RETRYCOUNT]);
                clientConfiguration.RetryPauseScale = Convert.ToInt32(ConfigurationManager.AppSettings[DAConfigurationKey.RETRYWAITTIME]);
            }

            switch (ostApiVersion)
            {
                case OtsConstants.ApiVersion:
                default:
                    otsClient = new OtsXmlClient(ostEndPoint, ostAccessId, ostAccessKey, clientConfiguration);

                    break;
                case ProtoBufConstant.ProtoBufApiVersion:
                    otsClient = new OtsProtoBufClient(ostEndPoint, ostAccessId, ostAccessKey, clientConfiguration);

                    break;
            }

            return otsClient;
        }

        #region Table
        /// <summary>
        /// Create energy table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        public static void CreateEnergyTable(string tableName)
        {
            TableMeta tableMeta = new TableMeta(tableName);

            tableMeta.PrimaryKeys.Add("TagGuidCode", PrimaryKeyType.Integer);
            tableMeta.PrimaryKeys.Add("TagId", PrimaryKeyType.Integer);
            tableMeta.PrimaryKeys.Add("AggregationStep", PrimaryKeyType.Integer);
            tableMeta.PrimaryKeys.Add("UtcTime", PrimaryKeyType.Integer);

            OtsHelper.CreateOtsClient().CreateTable(tableMeta);

            //workaround
            ReadOneRow(tableName);
        }

        /// <summary>
        /// Drop Energy Table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        public static void DropEnergyTable(string tableName)
        {
            OtsHelper.CreateOtsClient().DeleteTable(tableName);
        }

        public static void ReadOneRow(string tableName)
        {
            SingleRowQueryCriteria criteria = new SingleRowQueryCriteria(tableName);
            criteria.PrimaryKeys["TagGuidCode"] = 0;
            criteria.PrimaryKeys["TagId"] = 0;
            criteria.PrimaryKeys["UtcTime"] = 0;

            for (int i = 0; i < 20; ++i)
            {
                try
                {
                    OtsHelper.CreateOtsClient().GetRow(criteria);
                }
                catch
                { }
            }
        }
        #endregion

        #region Batch Retrieve
        public static IEnumerable<T> RetrieveData<T>(RangeRowQueryCriteria queryCriteria, Func<IEnumerable<Row>,
            IEnumerable<T>> buildEntityList, Func<IEnumerable<T>, PrimaryKeyValue> getNextBatchRangeBegin)
        {
            int maxRetrieveRowCount = Convert.ToInt32(ConfigurationManager.AppSettings[DAConfigurationKey.MAXRETRIEVEROWCOUNT]);

            if (maxRetrieveRowCount <= 0 && maxRetrieveRowCount != -1)
            {
                throw new ParameterException(Layer.DA, Module.Energy, Convert.ToInt32(EnergyErrorCode.MaxRetrieveRowCountIsIllegal));
            }

            //set batch row count
            queryCriteria.Top = maxRetrieveRowCount;

            IEnumerable<T> dataList = RetrieveDataOneBatch(queryCriteria, maxRetrieveRowCount, buildEntityList);

            List<T> allDataList = new List<T>();
            allDataList.AddRange(dataList);

            while (dataList.Count() == maxRetrieveRowCount)
            {
                PrimaryKeyRange range = new PrimaryKeyRange(queryCriteria.Range.PrimaryKeyName, getNextBatchRangeBegin(allDataList), queryCriteria.Range.RangeEnd);
                //queryCriteria.Range.RangeBegin = getNextBatchRangeBegin(allDataList);
                queryCriteria.Range = range;

                dataList = RetrieveDataOneBatch(queryCriteria, maxRetrieveRowCount, buildEntityList);

                allDataList.AddRange(dataList);
            }

            return allDataList.ToArray();
        }

        public static IEnumerable<T> RetrieveTopNData<T>(RangeRowQueryCriteria queryCriteria, Func<IEnumerable<Row>,
          IEnumerable<T>> buildEntityList, int topCount)
        {

            IEnumerable<T> dataList = RetrieveDataOneBatch(queryCriteria, topCount, buildEntityList);

            List<T> allDataList = new List<T>();
            allDataList.AddRange(dataList);

            return allDataList.ToArray();
        }

        public static IEnumerable<T> RetrieveDataOneBatch<T>(RangeRowQueryCriteria queryCriteria,
            int batchRowCount, Func<IEnumerable<Row>, IEnumerable<T>> buildEntityList)
        {
            try
            {
                var rowList = OtsHelper.CreateOtsClient().GetRowsByRange(queryCriteria);

                return buildEntityList(rowList == null ? new List<Row>() : rowList);
            }
            catch (OtsException e)
            {
                if (e.ErrorCode == OtsErrorCode.StorageObjectNotExist)
                {
                    LogHelper.LogError("Get Data from OTS failed because table does not exist:" + queryCriteria.TableName);
                }
                if (e.ErrorCode == OtsErrorCode.StorageParameterInvalid)
                {
                    LogHelper.LogError("Get Data from OTS failed because parameter is not correct");
                }

                throw;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Single Retrieve
        public static T RetrieveSingleData<T>(SingleRowQueryCriteria queryCriteria, Func<Row, T> buildEntityList)
        {
            var row = OtsHelper.CreateOtsClient().GetRow(queryCriteria);
            if (row != null && row.Columns.Count == 0)
                row = null;
            return buildEntityList(row);
        }
        #endregion

        #region Batch Update
        public static void UpdateData(List<RowChange> putRows, string dataTableName, PartitionKeyValue partitionKeyValue)
        {
            var otsClient = OtsHelper.CreateOtsClient();

            try
            {
                string transactionId = otsClient.StartTransaction(dataTableName, partitionKeyValue);

                //loop
                int maxUpdateRowCount = Convert.ToInt32(ConfigurationManager.AppSettings[DAConfigurationKey.MAXMODIFYROWCOUNT]);

                if (maxUpdateRowCount <= 0 && maxUpdateRowCount != -1)
                {
                    throw new ParameterException(Layer.DA, Module.Energy, Convert.ToInt32(EnergyErrorCode.MaxUpdateRowCountIsIllegal));
                }

                int loop = Convert.ToInt32(Math.Ceiling((decimal)putRows.Count / maxUpdateRowCount));

                for (int i = 0; i < loop; i++)
                {
                    var query = putRows.Skip(i * maxUpdateRowCount).Take(maxUpdateRowCount);

                    otsClient.BatchModifyData(dataTableName, query, transactionId);
                }

                otsClient.CommitTransaction(transactionId);

                return;
            }
            catch (OtsException e)
            {
                if (e.ErrorCode == OtsErrorCode.StorageObjectNotExist)
                {
                    LogHelper.LogError("Save to OTS failed because table does not exist:" + dataTableName);
                }
                else if (e.ErrorCode == OtsErrorCode.StorageParameterInvalid)
                {
                    LogHelper.LogError("Save to OTS failed because parameter is not correct");
                }

                throw;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}