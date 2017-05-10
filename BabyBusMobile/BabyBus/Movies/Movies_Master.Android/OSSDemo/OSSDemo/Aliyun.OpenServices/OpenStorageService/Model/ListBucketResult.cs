// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Model.ListBucketResult
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenStorageService.Model
{
  /// <remarks/>
  [DesignerCategory("code")]
  [DebuggerStepThrough]
  [XmlType(AnonymousType = true)]
  [XmlRoot(IsNullable = false, Namespace = "")]
  [GeneratedCode("xsd", "4.0.30319.1")]
  [Serializable]
  public class ListBucketResult
  {
    private string nameField;
    private string prefixField;
    private string markerField;
    private int maxKeysField;
    private string delimiterField;
    private bool isTruncatedField;
    private string nextMarkerField;
    private ListBucketResultContents[] contentsField;
    private ListBucketResultCommonPrefixes[] commonPrefixesField;

    /// <remarks/>
    public string Name
    {
      get
      {
        return this.nameField;
      }
      set
      {
        this.nameField = value;
      }
    }

    /// <remarks/>
    public string Prefix
    {
      get
      {
        return this.prefixField;
      }
      set
      {
        this.prefixField = value;
      }
    }

    /// <remarks/>
    public string Marker
    {
      get
      {
        return this.markerField;
      }
      set
      {
        this.markerField = value;
      }
    }

    /// <remarks/>
    public int MaxKeys
    {
      get
      {
        return this.maxKeysField;
      }
      set
      {
        this.maxKeysField = value;
      }
    }

    /// <remarks/>
    public string Delimiter
    {
      get
      {
        return this.delimiterField;
      }
      set
      {
        this.delimiterField = value;
      }
    }

    /// <remarks/>
    public bool IsTruncated
    {
      get
      {
        return this.isTruncatedField;
      }
      set
      {
        this.isTruncatedField = value;
      }
    }

    /// <remarks/>
    public string NextMarker
    {
      get
      {
        return this.nextMarkerField;
      }
      set
      {
        this.nextMarkerField = value;
      }
    }

    /// <remarks/>
    [XmlElement("Contents")]
    public ListBucketResultContents[] Contents
    {
      get
      {
        return this.contentsField;
      }
      set
      {
        this.contentsField = value;
      }
    }

    /// <remarks/>
    [XmlElement("CommonPrefixes")]
    public ListBucketResultCommonPrefixes[] CommonPrefixes
    {
      get
      {
        return this.commonPrefixesField;
      }
      set
      {
        this.commonPrefixesField = value;
      }
    }
  }
}
