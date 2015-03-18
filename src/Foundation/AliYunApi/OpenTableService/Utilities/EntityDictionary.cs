/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    /// <summary>
    /// A custom dictionary which store items, names of which need to follow the rules
    /// for entity names in Aliyun Open Table Service.
    /// </summary>
    /// <typeparam name="TValue">Type of values.</typeparam>
    internal class EntityDictionary<TValue> : IDictionary<string, TValue>
    {
        private Dictionary<string, TValue> _innerDictionary;

        #region Constructors

        public EntityDictionary()
        {
            _innerDictionary = new Dictionary<string, TValue>();
        }

        public EntityDictionary(IDictionary<string, TValue> dictionary)
        {
            _innerDictionary = new Dictionary<string, TValue>(dictionary);
        }

        public EntityDictionary(IEqualityComparer<string> comparer)
        {
            _innerDictionary = new Dictionary<string, TValue>(comparer);
        }

        public EntityDictionary(int capacity)
        {
            _innerDictionary = new Dictionary<string, TValue>(capacity);
        }

        public EntityDictionary(IDictionary<string, TValue> dictionary, IEqualityComparer<string> comparer)
        {
            _innerDictionary = new Dictionary<string, TValue>(dictionary, comparer);
        }

        public EntityDictionary(int capacity, IEqualityComparer<string> comparer)
        {
            _innerDictionary = new Dictionary<string, TValue>(capacity, comparer);
        }

        #endregion

        #region IDictionary Members

        public void Add(string key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            OnAdding(key, value);
            _innerDictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _innerDictionary.Keys; }
        }

        public bool Remove(string key)
        {
            return _innerDictionary.Remove(key);
        }

        public bool TryGetValue(string key, out TValue value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return _innerDictionary.Values; }
        }

        public TValue this[string key]
        {
            get
            {
                return _innerDictionary[key];
            }
            set
            {
                OnAdding(key, value);
                _innerDictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<string, TValue> item)
        {
            if (item.Key == null)
                throw new ArgumentNullException("item");

            OnAdding(item.Key, item.Value);
            ((IDictionary<string, TValue>)_innerDictionary).Add(item);
        }

        public void Clear()
        {
            _innerDictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            return _innerDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, TValue>>)_innerDictionary).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _innerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IDictionary)_innerDictionary).IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            return ((IDictionary<string, TValue>)_innerDictionary).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, TValue>>)_innerDictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_innerDictionary).GetEnumerator();
        }

        #endregion

        #region Private Methods

        protected virtual void OnAdding(string key, TValue value)
        {
            if (string.IsNullOrEmpty(key) || !OtsUtility.IsEntityNameValid(key))
            {
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "key");
            }
        }

        #endregion
    }

    internal class PrimaryKeyDictionary : EntityDictionary<PrimaryKeyValue>
    {
        #region Constructors

        public PrimaryKeyDictionary()
            : base()
        {
        }

        public PrimaryKeyDictionary(IDictionary<string, PrimaryKeyValue> dictionary)
            : base(dictionary)
        {
        }

        public PrimaryKeyDictionary(IEqualityComparer<string> comparer)
            : base(comparer)
        {
        }

        public PrimaryKeyDictionary(int capacity)
            : base(capacity)
        {
        }

        public PrimaryKeyDictionary(IDictionary<string, PrimaryKeyValue> dictionary, IEqualityComparer<string> comparer)
            : base(dictionary, comparer)
        {
        }

        public PrimaryKeyDictionary(int capacity, IEqualityComparer<string> comparer)
            : base(capacity, comparer)
        {
        }

        #endregion

        protected override void OnAdding(string key, PrimaryKeyValue value)
        {
            base.OnAdding(key, value);

            // Value of primary key cannot be null or empty.
            if (string.IsNullOrEmpty(value.Value))
            {
                throw new ArgumentException(OtsExceptions.PrimaryKeyValueIsNullOrEmpty);
            }

            if (value.IsInf)
            {
                throw new ArgumentException(OtsExceptions.PKInfNotAllowed);
            }
        }
    }

}
