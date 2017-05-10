// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.EntityDictionary`1
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  /// <summary>
  /// A custom dictionary which store items, names of which need to follow the rules
  ///             for entity names in Aliyun Open Table Service.
  /// 
  /// </summary>
  /// <typeparam name="TValue">Type of values.</typeparam>
  internal class EntityDictionary<TValue> : IDictionary<string, TValue>, ICollection<KeyValuePair<string, TValue>>, IEnumerable<KeyValuePair<string, TValue>>, IEnumerable
  {
    private Dictionary<string, TValue> _innerDictionary;

    public ICollection<string> Keys
    {
      get
      {
        return (ICollection<string>) this._innerDictionary.Keys;
      }
    }

    public ICollection<TValue> Values
    {
      get
      {
        return (ICollection<TValue>) this._innerDictionary.Values;
      }
    }

    public TValue this[string key]
    {
      get
      {
        return this._innerDictionary[key];
      }
      set
      {
        this.OnAdding(key, value);
        this._innerDictionary[key] = value;
      }
    }

    public int Count
    {
      get
      {
        return this._innerDictionary.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return ((IDictionary) this._innerDictionary).IsReadOnly;
      }
    }

    public EntityDictionary()
    {
      this._innerDictionary = new Dictionary<string, TValue>();
    }

    public EntityDictionary(IDictionary<string, TValue> dictionary)
    {
      this._innerDictionary = new Dictionary<string, TValue>(dictionary);
    }

    public EntityDictionary(IEqualityComparer<string> comparer)
    {
      this._innerDictionary = new Dictionary<string, TValue>(comparer);
    }

    public EntityDictionary(int capacity)
    {
      this._innerDictionary = new Dictionary<string, TValue>(capacity);
    }

    public EntityDictionary(IDictionary<string, TValue> dictionary, IEqualityComparer<string> comparer)
    {
      this._innerDictionary = new Dictionary<string, TValue>(dictionary, comparer);
    }

    public EntityDictionary(int capacity, IEqualityComparer<string> comparer)
    {
      this._innerDictionary = new Dictionary<string, TValue>(capacity, comparer);
    }

    public void Add(string key, TValue value)
    {
      if (key == null)
        throw new ArgumentNullException("key");
      this.OnAdding(key, value);
      this._innerDictionary.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
      return this._innerDictionary.ContainsKey(key);
    }

    public bool Remove(string key)
    {
      return this._innerDictionary.Remove(key);
    }

    public bool TryGetValue(string key, out TValue value)
    {
      return this._innerDictionary.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<string, TValue> item)
    {
      if (item.Key == null)
        throw new ArgumentNullException("item");
      this.OnAdding(item.Key, item.Value);
      ((ICollection<KeyValuePair<string, TValue>>) this._innerDictionary).Add(item);
    }

    public void Clear()
    {
      this._innerDictionary.Clear();
    }

    public bool Contains(KeyValuePair<string, TValue> item)
    {
      return Enumerable.Contains<KeyValuePair<string, TValue>>((IEnumerable<KeyValuePair<string, TValue>>) this._innerDictionary, item);
    }

    public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
    {
      ((ICollection<KeyValuePair<string, TValue>>) this._innerDictionary).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<string, TValue> item)
    {
      return ((ICollection<KeyValuePair<string, TValue>>) this._innerDictionary).Remove(item);
    }

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
      return ((IEnumerable<KeyValuePair<string, TValue>>) this._innerDictionary).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable) this._innerDictionary).GetEnumerator();
    }

    protected virtual void OnAdding(string key, TValue value)
    {
      if (string.IsNullOrEmpty(key) || !OtsUtility.IsEntityNameValid(key))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "key");
    }
  }
}
