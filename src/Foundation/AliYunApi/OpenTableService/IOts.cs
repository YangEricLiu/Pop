/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient"]/*'/>
    /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.CreateTable"]/*'/>
    /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.PutData"]/*'/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
                                                     "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ots")]
    public interface IOts
    {
        //#region Table Group Operations

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CreateTableGroup"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //void CreateTableGroup(string tableGroupName, PartitionKeyType partitionKeyType);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginCreateTableGroup"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        //IAsyncResult BeginCreateTableGroup(string tableGroupName,
        //                                   PartitionKeyType partitionKeyType,
        //                                   AsyncCallback callback,
        //                                   object state);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndCreateTableGroup"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //void EndCreateTableGroup(IAsyncResult asyncResult);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.ListTableGroups"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //IEnumerable<string> ListTableGroups();

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginListTableGroups"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        //IAsyncResult BeginListTableGroups(AsyncCallback callback,
        //                                  object state);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndListTableGroups"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //IEnumerable<string> EndListTableGroups(IAsyncResult asyncResult);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.DeleteTableGroup"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //void DeleteTableGroup(string tableGroupName);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginDeleteTableGroup"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        //IAsyncResult BeginDeleteTableGroup(string tableGroupName,
        //                                   AsyncCallback callback,
        //                                   object state);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndDeleteTableGroup"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //void EndDeleteTableGroup(IAsyncResult asyncResult);

        //#endregion

        #region Table Operations

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CreateTable"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.CreateTable"]/*'/>
        void CreateTable(TableMeta tableMeta);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginCreateTable"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        IAsyncResult BeginCreateTable(TableMeta tableMeta,
                                      AsyncCallback callback,
                                      object state);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndCreateTable"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        void EndCreateTable(IAsyncResult asyncResult);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetTableMeta"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //TableMeta GetTableMeta(string tableName);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginGetTableMeta"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        //IAsyncResult BeginGetTableMeta(string tableName, AsyncCallback callback, object state);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndGetTableMeta"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //TableMeta EndGetTableMeta(IAsyncResult asyncResult);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.ListTables"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //IEnumerable<string> ListTables();

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginListTables"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        //IAsyncResult BeginListTables(AsyncCallback callback, object state);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndListTables"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //IEnumerable<string> EndListTables(IAsyncResult asyncResult);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.DeleteTable"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        void DeleteTable(string tableName);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginDeleteTable"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        IAsyncResult BeginDeleteTable(string tableName, AsyncCallback callback, object state);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndDeleteTable"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        void EndDeleteTable(IAsyncResult asyncResult);

        #endregion

        #region Transaction Operations

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.StartTransaction"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.AsyncBatchModifyData"]/*'/>
        string StartTransaction(string entityName,
                                PartitionKeyValue partitionKeyValue);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginStartTransaction"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        IAsyncResult BeginStartTransaction(string entityName,
                                           PartitionKeyValue partitionKeyValue,
                                           AsyncCallback callback,
                                           object state);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndStartTransaction"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        string EndStartTransaction(IAsyncResult asyncResult);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommitTransaction"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.AsyncBatchModifyData"]/*'/>
        void CommitTransaction(string transactionId);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginCommitTransaction"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        IAsyncResult BeginCommitTransaction(string transactionId, AsyncCallback callback, object state);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndCommitTransaction"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        void EndCommitTransaction(IAsyncResult asyncResult);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.AbortTransaction"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        void AbortTransaction(string transactionId);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginAbortTransaction"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        IAsyncResult BeginAbortTransaction(string transactionId, AsyncCallback callback, object state);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndAbortTransaction"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        void EndAbortTransaction(IAsyncResult asyncResult);

        #endregion

        #region Data Query

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRow"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.ValueConversion"]/*'/>
        Row GetRow(SingleRowQueryCriteria criteria);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRow"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowTransactionIdParam"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        Row GetRow(SingleRowQueryCriteria criteria, string transactionId);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginGetRow"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        IAsyncResult BeginGetRow(SingleRowQueryCriteria criteria,
                                 AsyncCallback callback,
                                 object state);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginGetRow"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowTransactionIdParam"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        IAsyncResult BeginGetRow(SingleRowQueryCriteria criteria,
                                 string transactionId,
                                 AsyncCallback callback,
                                 object state);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndGetRow"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        Row EndGetRow(IAsyncResult asyncResult);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowsByRange"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.GetRowsByRange"]/*'/>
        IEnumerable<Row> GetRowsByRange(RangeRowQueryCriteria criteria);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowsByRange"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowTransactionIdParam"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        IEnumerable<Row> GetRowsByRange(RangeRowQueryCriteria criteria,
                                        string transactionId);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginGetRowsByRange"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        IAsyncResult BeginGetRowsByRange(RangeRowQueryCriteria criteria,
                                         AsyncCallback callback,
                                         object state);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginGetRowsByRange"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowTransactionIdParam"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        IAsyncResult BeginGetRowsByRange(RangeRowQueryCriteria criteria,
                                         string transactionId,
                                         AsyncCallback callback,
                                         object state);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndGetRowsByRange"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        IEnumerable<Row> EndGetRowsByRange(IAsyncResult asyncResult);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowsByOffset"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //IEnumerable<Row> GetRowsByOffset(OffsetRowQueryCriteria criteria);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowsByOffset"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowTransactionIdParam"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //IEnumerable<Row> GetRowsByOffset(OffsetRowQueryCriteria criteria,
        //                                 string transactionId);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginGetRowsByOffset"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        //IAsyncResult BeginGetRowsByOffset(OffsetRowQueryCriteria criteria,
        //                                  AsyncCallback callback,
        //                                  object state);

        //IAsyncResult BeginGetRowsByOffset(OffsetRowQueryCriteria criteria,
        //                                  string transactionId,
        //                                  AsyncCallback callback,
        //                                  object state);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndGetRowsByOffset"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //IEnumerable<Row> EndGetRowsByOffset(IAsyncResult asyncResult);

        //IEnumerable<Row> SqlQuery(string selectExpression, string transactionId);

        //IAsyncResult BeginSqlQuery(string selectExpression, string transactionId,
        //                                  AsyncCallback callback, object state);

        //IEnumerable<Row> EndSqlQuery(IAsyncResult asyncResult);

        #endregion

        #region Data Manipulation

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.PutData"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.PutData"]/*'/>
        //void PutData(string tableName, RowPutChange rowChange);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.PutData"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowTransactionIdParam"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.CreateTable"]/*'/>
        //void PutData(string tableName, RowPutChange rowChange, string transactionId);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginPutData"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        //IAsyncResult BeginPutData(string tableName,
        //                          RowPutChange rowChange,
        //                          AsyncCallback callback,
        //                          object state);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginPutData"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowTransactionIdParam"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        //IAsyncResult BeginPutData(string tableName,
        //                          RowPutChange rowChange,
        //                          string transactionId,
        //                          AsyncCallback callback,
        //                          object state);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndPutData"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //void EndPutData(IAsyncResult asyncResult);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.DeleteData"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //void DeleteData(string tableName, RowDeleteChange rowChange);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.DeleteData"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowTransactionIdParam"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //void DeleteData(string tableName, RowDeleteChange rowChange, string transactionId);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginDeleteData"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        //IAsyncResult BeginDeleteData(string tableName,
        //                             RowDeleteChange rowChange,
        //                             AsyncCallback callback,
        //                             object state);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginDeleteData"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.GetRowTransactionIdParam"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        //IAsyncResult BeginDeleteData(string tableName,
        //                             RowDeleteChange rowChange,
        //                             string transactionId,
        //                             AsyncCallback callback,
        //                             object state);

        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndDeleteData"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        ///// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        //void EndDeleteData(IAsyncResult asyncResult);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BatchModifyData"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        void BatchModifyData(string tableName,
                             IEnumerable<RowChange> rowChanges,
                             string transactionId);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginBatchModifyData"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.BeginOperationCommon"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.AsyncBatchModifyData"]/*'/>
        IAsyncResult BeginBatchModifyData(string tableName,
                                          IEnumerable<RowChange> rowChanges,
                                          string transactionId,
                                          AsyncCallback callback,
                                          object state);

        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndBatchModifyData"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.EndOperationCommon"]/*'/>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableServiceClient.CommonException"]/*'/>
        void EndBatchModifyData(IAsyncResult asyncResult);

        #endregion
    }
}
