// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Model.ListBucketResultContents
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenStorageService;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenStorageService.Model
{
  /// <remarks/>
  [GeneratedCode("xsd", "4.0.30319.1")]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true)]
  [DebuggerStepThrough]
  [Serializable]
  public class ListBucketResultContents
  {
    private string keyField;
    private DateTime lastModifiedField;
    private string eTagField;
    private string typeField;
    private long sizeField;
    private string storageClassField;
    private Owner ownerField;

    /// <remarks/>
    public string Key
    {
      get
      {
        return this.keyField;
      }
      set
      {
        this.keyField = value;
      }
    }

    /// <remarks/>
    public DateTime LastModified
    {
      get
      {
        return this.lastModifiedField;
      }
      set
      {
        this.lastModifiedField = value;
      }
    }

    /// <remarks/>
    public string ETag
    {
      get
      {
        return this.eTagField;
      }
      set
      {
        this.eTagField = value;
      }
    }

    /// <remarks/>
    public string Type
    {
      get
      {
        return this.typeField;
      }
      set
      {
        this.typeField = value;
      }
    }

    /// <remarks/>
    public long Size
    {
      get
      {
        return this.sizeField;
      }
      set
      {
        this.sizeField = value;
      }
    }

    /// <remarks/>
    public string StorageClass
    {
      get
      {
        return this.storageClassField;
      }
      set
      {
        this.storageClassField = value;
      }
    }

    /// <remarks/>
    public Owner Owner
    {
      get
      {
        return this.ownerField;
      }
      set
      {
        this.ownerField = value;
      }
    }
  }
}
