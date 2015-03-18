/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    /// <summary>
    /// A custom list that holds names, which must follows the naming rules
    /// of Table/TableGroup/PrimaryKey/Column in Aliyun Open Table Service.
    /// </summary>
    internal class EntityNameList : ICollection<string>, IList<string>
    {
        private List<string> _innerList;

        public EntityNameList()
        {
            _innerList = new List<string>();
        }

        public EntityNameList(IEnumerable<string> collection)
        {
            _innerList = new List<string>(collection);
        }

        public void Add(string item)
        {
            OnAdding(item);
            _innerList.Add(item);
        }

        public void Clear()
        {
            _innerList.Clear();
        }

        public bool Contains(string item)
        {
            return _innerList.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _innerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<string>)_innerList).IsReadOnly; }
        }

        public bool Remove(string item)
        {
            return _innerList.Remove(item);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_innerList).GetEnumerator();
        }

        public int IndexOf(string item)
        {
            return _innerList.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            OnAdding(item);
            _innerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _innerList.RemoveAt(index);
        }

        public string this[int index]
        {
            get
            {
                return _innerList[index];
            }
            set
            {
                OnAdding(value);

                _innerList[index] = value;
            }
        }

        private static void OnAdding(string item)
        {
            if (string.IsNullOrEmpty(item) || !OtsUtility.IsEntityNameValid(item))
            {
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid);
            }
        }
    }
}
