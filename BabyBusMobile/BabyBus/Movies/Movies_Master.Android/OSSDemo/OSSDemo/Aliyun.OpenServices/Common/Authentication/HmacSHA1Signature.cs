// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Authentication.HmacSHA1Signature
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Aliyun.OpenServices.Common.Authentication
{
  internal class HmacSHA1Signature : ServiceSignature
  {
    private Encoding _encoding = Encoding.UTF8;

    public override string SignatureMethod
    {
      get
      {
        return "HmacSHA1";
      }
    }

    public override string SignatureVersion
    {
      get
      {
        return "1";
      }
    }

    protected override string ComputeSignatureCore(string key, string data)
    {
      Debug.Assert(!string.IsNullOrEmpty(data));
      using (KeyedHashAlgorithm keyedHashAlgorithm = KeyedHashAlgorithm.Create(((object) this.SignatureMethod).ToString().ToUpperInvariant()))
      {
        keyedHashAlgorithm.Key = this._encoding.GetBytes(key.ToCharArray());
        return Convert.ToBase64String(keyedHashAlgorithm.ComputeHash(this._encoding.GetBytes(data.ToCharArray())));
      }
    }
  }
}
