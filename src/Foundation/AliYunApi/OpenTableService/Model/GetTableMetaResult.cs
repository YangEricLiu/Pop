/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Collections.Generic;
using System.Xml.Serialization;

using Aliyun.OpenServices.OpenTableService.Utilities;

namespace Aliyun.OpenServices.OpenTableService.Model
{
    public class GetTableMetaResult : OpenTableServiceResult
    {
        public InternalTableMeta TableMeta { get; set; }

        public GetTableMetaResult()
        {
        }
    }

    [XmlRoot(ElementName="TableMeta")]
    public class InternalTableMeta
    {
        public string TableName { get; set; }

        public int PagingKeyLen { get; set; }

        public string TableGroupName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), XmlElement(ElementName = "View")]
        public List<InternalViewMeta> Views { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), XmlElement(ElementName = "PrimaryKey")]
        public List<PrimaryKey> PrimaryKeys { get; set; }


        public InternalTableMeta()
        {
            this.PrimaryKeys = new List<PrimaryKey>();
            this.Views = new List<InternalViewMeta>();
        }

        internal TableMeta ToTableMeta()
        {
            TableMeta tableMeta = new TableMeta(this.TableName);
            tableMeta.PagingKeyLength = this.PagingKeyLen;
            tableMeta.TableGroupName = this.TableGroupName;

            foreach (var pk in PrimaryKeys)
            {
                tableMeta.PrimaryKeys.Add(pk.Name, PrimaryKeyTypeHelper.Parse(pk.PrimaryKeyType));
            }

            foreach(var view in this.Views)
            {
                tableMeta.Views.Add(view.ToOpenTableViewMeta());
            }
            return tableMeta;
        }
    }

    [XmlRoot(ElementName="View")]
    public class InternalViewMeta
    {
        public string Name { get; set; }

        public int PagingKeyLen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), XmlElement(ElementName = "PrimaryKey")]
        public List<PrimaryKey> PrimaryKeys { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), XmlElement(ElementName = "Column")]
        public List<ViewColumn> Columns { get; set; }

        public InternalViewMeta()
        {
            this.PrimaryKeys = new List<PrimaryKey>();
            this.Columns = new List<ViewColumn>();
        }

        internal ViewMeta ToOpenTableViewMeta()
        {
            ViewMeta viewMeta = new ViewMeta(this.Name);

            viewMeta.PagingKeyLength = this.PagingKeyLen;

            foreach (var pk in this.PrimaryKeys)
            {
                viewMeta.PrimaryKeys.Add(pk.Name, PrimaryKeyTypeHelper.Parse(pk.PrimaryKeyType));
            }

            foreach (var col in this.Columns)
            {
                viewMeta.AttributeColumns.Add(col.Name, ColumnTypeHelper.Parse(col.ColumnType));
            }

            return viewMeta;
        }
    }

    public class PrimaryKey
    {
        public string Name { get; set; }

        [XmlElement(ElementName = "Type")]
        public string PrimaryKeyType { get; set; }
    }

    public class ViewColumn
    {
        public string Name { get; set; }

        [XmlElement(ElementName = "Type")]
        public string ColumnType { get; set; }
    }
}
