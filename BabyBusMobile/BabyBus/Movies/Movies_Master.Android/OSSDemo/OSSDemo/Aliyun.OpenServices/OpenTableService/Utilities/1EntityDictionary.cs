// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.PrimaryKeyDictionary
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  internal class PrimaryKeyDictionary : EntityDictionary<PrimaryKeyValue>
  {
    public PrimaryKeyDictionary()
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

    protected override void OnAdding(string key, PrimaryKeyValue value)
    {
      base.OnAdding(key, value);
      if (string.IsNullOrEmpty(value.Value))
        throw new ArgumentException(OtsExceptions.PrimaryKeyValueIsNullOrEmpty);
      if (value.IsInf)
        throw new ArgumentException(OtsExceptions.PKInfNotAllowed);
    }
  }
}
