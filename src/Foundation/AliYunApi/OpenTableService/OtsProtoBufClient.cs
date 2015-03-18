/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Commands;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService
{
    // TODO: Make OtsClient immutable.
    /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient"]/*'/>
    /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.CreateTable"]/*'/>
    /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.PutData"]/*'/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
                                                     "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ots")]
    public class OtsProtoBufClient : IOts
    {
        #region Fields

        private readonly Uri _endpoint;
        private readonly ServiceCredentials _credentials;
        private readonly Aliyun.OpenServices.Ots.ClientConfiguration _configuration;

        #endregion

        #region Properties

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.Endpoint"]/*'/>
        public Uri Endpoint { get { return _endpoint; } }

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.AccessId"]/*'/>
        public string AccessId { get { return _credentials.AccessId; } }

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.Configuration"]/*'/>
        [Obsolete("请使用OtsClient的构造函数传入Aliyun.OpenServices.Ots.ClientConfiguration的对象，而不要使用该属性进行配置。")]
        public Aliyun.OpenServices.Ots.ClientConfiguration Configuration
        {
            get
            {
                return _configuration;
            }
        }

        #endregion

        #region Constructors

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.Constructor"]/*'/>
        public OtsProtoBufClient(string accessId, string accessKey)
            : this(OtsConstants.DefaultEndpoint, accessId, accessKey)
        {
        }

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.Constructor"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.Constructor.param.endpoint"]/*'/>
        public OtsProtoBufClient(string endpoint, string accessId, string accessKey)
            : this(endpoint, accessId, accessKey, null)
        {
        }

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.Constructor"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.Constructor.param.endpoint"]/*'/>
        /// <param name="configuration"><see cref="Aliyun.OpenServices.Ots.ClientConfiguration"/>.</param>
        public OtsProtoBufClient(string endpoint, string accessId, string accessKey,
                         Aliyun.OpenServices.Ots.ClientConfiguration configuration)
        {
            if (string.IsNullOrEmpty(accessId))
                throw new ArgumentException(Properties.Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessId");
            if (string.IsNullOrEmpty(accessKey))
                throw new ArgumentException(Properties.Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessKey");

            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentException(Properties.Resources.ExceptionIfArgumentStringIsNullOrEmpty, "endpoint");
            if (!endpoint.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException(OtsExceptions.EndpointNotSupportedProtocal, "endpoint");

            _endpoint = new Uri(endpoint);
            _credentials = new ServiceCredentials(accessId, accessKey);

            // Make a definsive copy to ensure the class is immutable.
            _configuration = configuration != null ?
                (Aliyun.OpenServices.Ots.ClientConfiguration)configuration.Clone() :
                new Aliyun.OpenServices.Ots.ClientConfiguration();
        }

        #endregion

        #region Table Group

        /// <inheritdoc/>
        public void CreateTableGroup(string tableGroupName, PartitionKeyType partitionKeyType)
        {
            CreateTableGroupCommand.Create(GetServiceClient(),
                                           _endpoint,
                                           CreateContext(CreateTableGroupCommand.ActionName),
                                           tableGroupName, partitionKeyType).Execute();
        }

        /// <inheritdoc/>
        public IAsyncResult BeginCreateTableGroup(string tableGroupName,
                                                  PartitionKeyType partitionKeyType,
                                                  AsyncCallback callback,
                                                  object state)
        {
            return CreateTableGroupCommand.Create(GetServiceClient(),
                                                  _endpoint,
                                                  CreateContext(CreateTableGroupCommand.ActionName),
                                                  tableGroupName, partitionKeyType)
                .BeginExecute(callback, state);
        }

        /// <inheritdoc/>
        public void EndCreateTableGroup(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            OtsCommand.EndExecute(GetServiceClient(), asyncResult);
        }

        #endregion

        #region Table Groups

        /// <inheritdoc/>
        public IEnumerable<string> ListTableGroups()
        {
            return ListTableGroupCommand.Create(GetServiceClient(),
                                                _endpoint,
                                                CreateContext(ListTableGroupCommand.ActionName))
                .Execute().TableGroupNames;
        }

        /// <inheritdoc/>
        public IAsyncResult BeginListTableGroups(AsyncCallback callback,
                                                 object state)
        {
            return ListTableGroupCommand.Create(GetServiceClient(),
                                                _endpoint,
                                                CreateContext(ListTableGroupCommand.ActionName))
                .BeginExecute(callback, state);
        }

        /// <inheritdoc/>
        public IEnumerable<string> EndListTableGroups(IAsyncResult asyncResult)
        {
            return ListTableGroupCommand.EndExecute(
                GetServiceClient(), asyncResult).TableGroupNames;
        }

        #endregion

        #region Delete Table Group

        /// <inheritdoc/>
        public void DeleteTableGroup(string tableGroupName)
        {
            DeleteTableGroupCommand.Create(GetServiceClient(),
                                           _endpoint,
                                           CreateContext(DeleteTableGroupCommand.ActionName),
                                           tableGroupName).Execute();
        }

        /// <inheritdoc/>
        public IAsyncResult BeginDeleteTableGroup(string tableGroupName,
                                                  AsyncCallback callback,
                                                  object state)
        {
            return DeleteTableGroupCommand.Create(GetServiceClient(),
                                                  _endpoint,
                                                  CreateContext(DeleteTableGroupCommand.ActionName),
                                                  tableGroupName).BeginExecute(callback, state);
        }

        /// <inheritdoc/>
        public void EndDeleteTableGroup(IAsyncResult asyncResult)
        {
            OtsCommand.EndExecute(GetServiceClient(), asyncResult);
        }

        #endregion

        #region Create Table

        /// <inheritdoc/>
        public void CreateTable(TableMeta tableMeta)
        {
            Commands.ProtoBuf.CreateTableCommand.Create(GetServiceClient(),
                                      _endpoint,
                                      CreateContext(Commands.ProtoBuf.CreateTableCommand.ActionName),
                                      tableMeta).Execute();
        }

        /// <inheritdoc/>
        public IAsyncResult BeginCreateTable(TableMeta tableMeta,
                                             AsyncCallback callback,
                                             object state)
        {
            return Commands.ProtoBuf.CreateTableCommand.Create(GetServiceClient(),
                                             _endpoint,
                                             CreateContext(Commands.ProtoBuf.CreateTableCommand.ActionName),
                                             tableMeta).BeginExecute(callback, state);
        }

        /// <inheritdoc/>
        public void EndCreateTable(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            OtsCommand.EndExecute(GetServiceClient(), asyncResult);
        }

        #endregion

        //#region Get Table Meta

        ///// <inheritdoc/>
        //public TableMeta GetTableMeta(string tableName)
        //{
        //    return GetTableMetaCommand.Create(GetServiceClient(),
        //                                      _endpoint,
        //                                      CreateContext(GetTableMetaCommand.ActionName),
        //                                      tableName).Execute().TableMeta.ToTableMeta();
        //}

        ///// <inheritdoc/>
        //public IAsyncResult BeginGetTableMeta(string tableName, AsyncCallback callback, object state)
        //{
        //    if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
        //        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");

        //    return GetTableMetaCommand.Create(GetServiceClient(),
        //                                      _endpoint,
        //                                      CreateContext(GetTableMetaCommand.ActionName),
        //                                      tableName).BeginExecute(callback, state);
        //}

        ///// <inheritdoc/>
        //public TableMeta EndGetTableMeta(IAsyncResult asyncResult)
        //{
        //    return GetTableMetaCommand.EndExecute(
        //        GetServiceClient(), asyncResult).TableMeta.ToTableMeta();
        //}

        //#endregion

        //#region List Tables

        ///// <inheritdoc/>
        //public IEnumerable<string> ListTables()
        //{
        //    return ListTableCommand.Create(GetServiceClient(),
        //                                   _endpoint,
        //                                   CreateContext(ListTableCommand.ActionName))
        //        .Execute().TableNames;
        //}

        ///// <inheritdoc/>
        //public IAsyncResult BeginListTables(AsyncCallback callback, object state)
        //{
        //    return ListTableCommand.Create(GetServiceClient(),
        //                                   _endpoint,
        //                                   CreateContext(ListTableCommand.ActionName))
        //        .BeginExecute(callback, state);
        //}

        ///// <inheritdoc/>
        //public IEnumerable<string> EndListTables(IAsyncResult asyncResult)
        //{
        //    return ListTableCommand.EndExecute(GetServiceClient(), asyncResult).TableNames;
        //}

        //#endregion

        #region Delete Table

        /// <inheritdoc/>
        public void DeleteTable(string tableName)
        {
            Commands.ProtoBuf.DeleteTableCommand.Create(GetServiceClient(),
                                      _endpoint,
                                      CreateContext(Commands.ProtoBuf.DeleteTableCommand.ActionName),
                                      tableName).Execute();
        }

        /// <inheritdoc/>
        public IAsyncResult BeginDeleteTable(string tableName, AsyncCallback callback, object state)
        {
            return Commands.ProtoBuf.DeleteTableCommand.Create(GetServiceClient(),
                                             _endpoint,
                                             CreateContext(Commands.ProtoBuf.DeleteTableCommand.ActionName),
                                             tableName).BeginExecute(callback, state);
        }

        /// <inheritdoc/>
        public void EndDeleteTable(IAsyncResult asyncResult)
        {
            OtsCommand.EndExecute(GetServiceClient(), asyncResult);
        }

        #endregion

        #region Transaction Operations

        /// <inheritdoc/>
        public string StartTransaction(string entityName,
                                       PartitionKeyValue partitionKeyValue)
        {
            return Commands.ProtoBuf.StartTransactionCommand.Create(GetServiceClient(), _endpoint,
                                                  CreateContext(Commands.ProtoBuf.StartTransactionCommand.ActionName),
                                                  entityName, partitionKeyValue).Execute().transaction_id;
        }

        /// <inheritdoc/>
        public IAsyncResult BeginStartTransaction(string entityName,
                                                  PartitionKeyValue partitionKeyValue,
                                                  AsyncCallback callback,
                                                  object state)
        {
            return Commands.ProtoBuf.StartTransactionCommand.Create(GetServiceClient(), _endpoint,
                                                  CreateContext(Commands.ProtoBuf.StartTransactionCommand.ActionName),
                                                  entityName, partitionKeyValue).BeginExecute(callback, state);
        }

        /// <inheritdoc/>
        public string EndStartTransaction(IAsyncResult asyncResult)
        {
            return Commands.ProtoBuf.StartTransactionCommand.EndExecute(GetServiceClient(), asyncResult).transaction_id;
        }

        /// <inheritdoc/>
        public void CommitTransaction(string transactionId)
        {
            Commands.ProtoBuf.CommitTransactionCommand.Create(GetServiceClient(), _endpoint,
                                            CreateContext(Commands.ProtoBuf.CommitTransactionCommand.ActionName),
                                            transactionId).Execute();
        }

        /// <inheritdoc/>
        public IAsyncResult BeginCommitTransaction(string transactionId, AsyncCallback callback, object state)
        {
            return Commands.ProtoBuf.CommitTransactionCommand.Create(GetServiceClient(), _endpoint,
                                                   CreateContext(Commands.ProtoBuf.CommitTransactionCommand.ActionName),
                                                   transactionId).BeginExecute(callback, state);
        }

        /// <inheritdoc/>
        public void EndCommitTransaction(IAsyncResult asyncResult)
        {
            OtsCommand.EndExecute(GetServiceClient(), asyncResult);
        }

        /// <inheritdoc/>
        public void AbortTransaction(string transactionId)
        {
            Commands.ProtoBuf.AbortTransactionCommand.Create(GetServiceClient(), _endpoint,
                                           CreateContext(Commands.ProtoBuf.AbortTransactionCommand.ActionName),
                                           transactionId).Execute();
        }

        /// <inheritdoc/>
        public IAsyncResult BeginAbortTransaction(string transactionId, AsyncCallback callback, object state)
        {
            return Commands.ProtoBuf.AbortTransactionCommand.Create(GetServiceClient(), _endpoint,
                                                  CreateContext(Commands.ProtoBuf.AbortTransactionCommand.ActionName),
                                                  transactionId).BeginExecute(callback, state);
        }

        /// <inheritdoc/>
        public void EndAbortTransaction(IAsyncResult asyncResult)
        {
            OtsCommand.EndExecute(GetServiceClient(), asyncResult);
        }

        #endregion

        #region Get Row

        /// <inheritdoc/>
        public Row GetRow(SingleRowQueryCriteria criteria)
        {
            return GetRow(criteria, null);
        }

        /// <inheritdoc/>
        public Row GetRow(SingleRowQueryCriteria criteria, string transactionId)
        {
            var pbResult = Commands.ProtoBuf.GetRowCommand.Create(GetServiceClient(),
                                                                         _endpoint,
                                                                         CreateContext(GetRowCommand.ActionName),
                                                                         criteria, transactionId).Execute();

            return ProtoBufUtility.ProtoBufRow2XmlRow(pbResult.row);
        }

        /// <inheritdoc/>
        public IAsyncResult BeginGetRow(SingleRowQueryCriteria criteria,
                                        AsyncCallback callback,
                                        object state)
        {
            return BeginGetRow(criteria, null, callback, state);
        }

        /// <inheritdoc/>
        public IAsyncResult BeginGetRow(SingleRowQueryCriteria criteria,
                                        string transactionId,
                                        AsyncCallback callback,
                                        object state)
        {
            return Commands.ProtoBuf.GetRowCommand.Create(GetServiceClient(), _endpoint,
                                CreateContext(Commands.ProtoBuf.GetRowCommand.ActionName),
                                criteria, transactionId).BeginExecute(callback, state);
        }

        /// <inheritdoc/>
        public Row EndGetRow(IAsyncResult asyncResult)
        {
            var pbResult = Commands.ProtoBuf.GetRowCommand.EndExecute(GetServiceClient(), asyncResult);

            return ProtoBufUtility.ProtoBufRow2XmlRow(pbResult.row);
        }

        #endregion

        #region GetRowsByRange

        /// <inheritdoc/>
        public IEnumerable<Row> GetRowsByRange(RangeRowQueryCriteria criteria)
        {
            return GetRowsByRange(criteria, null);
        }

        /// <inheritdoc/>
        public IEnumerable<Row> GetRowsByRange(RangeRowQueryCriteria criteria,
                                               string transactionId)
        {
            var pbResult = Commands.ProtoBuf.GetRowsByRangeCommand.Create(GetServiceClient(),
                                                                         _endpoint,
                                                                         CreateContext(GetRowsByRangeCommand.ActionName),
                                                                         criteria, transactionId).Execute();

            return ProtoBufUtility.ProtoBufRows2XmlRows(pbResult.rows);
        }

        /// <inheritdoc/>
        public IAsyncResult BeginGetRowsByRange(RangeRowQueryCriteria criteria,
                                                AsyncCallback callback,
                                                object state)
        {
            return BeginGetRowsByRange(criteria, null, callback, state);
        }

        /// <inheritdoc/>
        public IAsyncResult BeginGetRowsByRange(RangeRowQueryCriteria criteria,
                                                string transactionId,
                                                AsyncCallback callback,
                                                object state)
        {
            return Commands.ProtoBuf.GetRowsByRangeCommand.Create(GetServiceClient(), _endpoint,
                             CreateContext(Commands.ProtoBuf.GetRowsByRangeCommand.ActionName),
                             criteria, transactionId).BeginExecute(callback, state);
        }

        /// <inheritdoc/>
        public IEnumerable<Row> EndGetRowsByRange(IAsyncResult asyncResult)
        {
            var pbResult = Commands.ProtoBuf.GetRowsByRangeCommand.EndExecute(GetServiceClient(), asyncResult);

            return ProtoBufUtility.ProtoBufRows2XmlRows(pbResult.rows);
        }

        #endregion

        #region Batch Modify Data

        /// <inheritdoc/>
        public void BatchModifyData(string tableName,
                                    IEnumerable<RowChange> rowChanges,
                                    string transactionId)
        {
            Commands.ProtoBuf.BatchModifyDataCommand.Create(GetServiceClient(), _endpoint,
                                          CreateContext(Commands.ProtoBuf.BatchModifyDataCommand.ActionName),
                                          tableName, rowChanges, transactionId).Execute();
        }

        /// <inheritdoc/>
        public IAsyncResult BeginBatchModifyData(string tableName,
                                                 IEnumerable<RowChange> rowChanges,
                                                 string transactionId,
                                                 AsyncCallback callback,
                                                 object state)
        {
            return Commands.ProtoBuf.BatchModifyDataCommand.Create(GetServiceClient(), _endpoint,
                                                 CreateContext(Commands.ProtoBuf.BatchModifyDataCommand.ActionName),
                                                 tableName, rowChanges, transactionId).BeginExecute(callback, state);
        }

        /// <inheritdoc/>
        public void EndBatchModifyData(IAsyncResult asyncResult)
        {
            OtsCommand.EndExecute(GetServiceClient(), asyncResult);
        }

        #endregion

        #region Private Methods

        private IServiceClient GetServiceClient()
        {
            // Don't cache the ServiceClient instance as the ServiceClient would
            // be change to immutable.
            // If we have to tune perf, consider make OtsClient immutable as well
            // and cache the instance.
            return ServiceClientFactory.CreateServiceClient(_configuration);
        }

        private ExecutionContext CreateContext(String action)
        {
            ExecutionContext context = new ExecutionContext();

            context.Signer = new ProtoBufRequestSigner();
            context.Credentials = _credentials;
            context.ResponseHandlers.Add(new OtsProtoBufExceptionHandler());
            context.ResponseHandlers.Add(new ValidationProtoBufResponseHandler(_credentials, action));


            return context;
        }
        #endregion
    }
}