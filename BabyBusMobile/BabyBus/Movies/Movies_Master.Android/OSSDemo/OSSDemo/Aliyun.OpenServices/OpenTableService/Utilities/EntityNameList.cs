// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.EntityNameList
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  /// <summary>
  /// A custom list that holds names, which must follows the naming rules
  ///             of Table/TableGroup/PrimaryKey/Column in Aliyun Open Table Service.
  /// 
  /// </summary>
  internal class EntityNameList : IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable
  {
    private List<string> _innerList;

    public int Count
    {
      get
      {
        return this._innerList.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return ((ICollection<string>) this._innerList).IsReadOnly;
      }
    }

    public string this[int index]
    {
      get
      {
        return this._innerList[index];
      }
      set
      {
        EntityNameList.OnAdding(value);
        this._innerList[index] = value;
      }
    }

    public EntityNameList()
    {
      this._innerList = new List<string>();
    }

    public EntityNameList(IEnumerable<string> collection)
    {
      this._innerList = new List<string>(collection);
    }

    public void Add(string item)
    {
      EntityNameList.OnAdding(item);
      this._innerList.Add(item);
    }

    public void Clear()
    {
      this._innerList.Clear();
    }

    public bool Contains(string item)
    {
      return this._innerList.Contains(item);
    }

    public void CopyTo(string[] array, int arrayIndex)
    {
      this._innerList.CopyTo(array, arrayIndex);
    }

    public bool Remove(string item)
    {
      return this._innerList.Remove(item);
    }

    public IEnumerator<string> GetEnumerator()
    {
      return (IEnumerator<string>) this._innerList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable) this._innerList).GetEnumerator();
    }

    public int IndexOf(string item)
    {
      return this._innerList.IndexOf(item);
    }

    public void Insert(int index, string item)
    {
      EntityNameList.OnAdding(item);
      this._innerList.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
      this._innerList.RemoveAt(index);
    }

    private static void OnAdding(string item)
    {
      if (string.IsNullOrEmpty(item) || !OtsUtility.IsEntityNameValid(item))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid);
    }
  }
}
