/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Utilities;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
    /// <summary>
    /// The command to create a table.
    /// </summary>
    internal class CreateTableCommand : OtsCommand
    {
        private TableMeta _tableMeta;

        public const string ActionName = "CreateTable";

        protected override HttpMethod Method
        {
            get { return HttpMethod.Post; }
        }
        
        protected override string ResourcePath
        {
            get { return ActionName; }
        }

        protected CreateTableCommand(IServiceClient client,
                                     Uri endpoint,
                                     ExecutionContext context,
                                     TableMeta tableMeta)
            : base(client, endpoint, context)
        {
            Debug.Assert(tableMeta != null);
            _tableMeta = tableMeta;
        }

        public static CreateTableCommand Create(IServiceClient client,
                                                Uri endpoint,
                                                ExecutionContext context,
                                                TableMeta tableMeta)
        {
            ValidateTableMeta(tableMeta);
            return new CreateTableCommand(client, endpoint, context, tableMeta);
        }

        private static void ValidateTableMeta(TableMeta tableMeta)
        {
            const string paramName = "tableMeta";

            if (tableMeta == null)
                throw new ArgumentNullException(paramName);

            if (string.IsNullOrEmpty(tableMeta.TableName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, paramName);

            if (tableMeta.PrimaryKeys.Count == 0)
                throw new ArgumentException(OtsExceptions.NoPrimaryKeySpecified, paramName);

            // The primary key names should be verified when they are added to table meta.
            OtsUtility.AssertColumnNames(tableMeta.PrimaryKeys.Keys);

            if (tableMeta.PagingKeyLength < 0 ||
                tableMeta.PagingKeyLength >= tableMeta.PrimaryKeys.Count)
            {
                throw new ArgumentException(OtsExceptions.InvalidPagingKeyLength, paramName);
            }

            foreach (var view in tableMeta.Views)
            {
                Debug.Assert(OtsUtility.IsEntityNameValid(view.ViewName),
                             "ViewName should have been validated when it is added.");

                OtsUtility.AssertColumnNames(view.PrimaryKeys.Keys);
                OtsUtility.AssertColumnNames(view.AttributeColumns.Keys);

                if (view.PagingKeyLength < 0 ||
                    view.PagingKeyLength >= view.PrimaryKeys.Count)
                {
                    throw new ArgumentException(OtsExceptions.InvalidPagingKeyLength, paramName);
                }
            }
        }

        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            parameters.Add("TableName", this._tableMeta.TableName);

            int pkIndex = 1;
            foreach (var pk in this._tableMeta.PrimaryKeys)
            {
                var pre = string.Concat("PK.", pkIndex.ToString(CultureInfo.InvariantCulture));
                parameters.Add(string.Concat(pre, ".Name"), pk.Key);
                parameters.Add(string.Concat(pre, ".Type"), PrimaryKeyTypeHelper.GetString(pk.Value));

                pkIndex++;
            }

            if (this._tableMeta.PagingKeyLength > 0)
            {
                parameters.Add("PagingKeyLen", this._tableMeta.PagingKeyLength.ToString(CultureInfo.InvariantCulture));
            }

            // Views
            int viewIndex = 1;
            foreach (var view in this._tableMeta.Views)
            {
                var viewPre = string.Concat("View.", viewIndex.ToString(CultureInfo.InvariantCulture));

                parameters.Add(string.Concat(viewPre, ".Name"), view.ViewName);

                int viewPkIndex = 1;
                foreach (var pk in view.PrimaryKeys)
                {
                    var pre = viewPre + ".PK." + viewPkIndex.ToString(CultureInfo.InvariantCulture);
                    parameters.Add(string.Concat(pre, ".Name"), pk.Key);
                    parameters.Add(string.Concat(pre, ".Type"), PrimaryKeyTypeHelper.GetString(pk.Value));

                    viewPkIndex++;
                }

                int viewColIndex = 1;
                foreach (var col in view.AttributeColumns)
                {
                    var pre = viewPre + ".Column." + viewColIndex.ToString(CultureInfo.InvariantCulture);
                    parameters.Add(string.Concat(pre, ".Name"), col.Key);
                    parameters.Add(string.Concat(pre, ".Type"), ColumnTypeHelper.GetString(col.Value));

                    viewColIndex++;
                }

                if (view.PagingKeyLength > 0)
                {
                    parameters.Add(string.Concat(viewPre, ".PagingKeyLen"), view.PagingKeyLength.ToString(CultureInfo.InvariantCulture));
                }

                viewIndex++;
            }

            if (!string.IsNullOrEmpty(this._tableMeta.TableGroupName))
            {
                parameters.Add("TableGroupName", this._tableMeta.TableGroupName);
            }
        }
    }
}
