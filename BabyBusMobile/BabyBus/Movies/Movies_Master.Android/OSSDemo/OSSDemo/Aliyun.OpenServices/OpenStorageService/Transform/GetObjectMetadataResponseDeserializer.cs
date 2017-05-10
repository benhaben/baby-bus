// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.GetObjectMetadataResponseDeserializer
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  /// <summary>
  /// Description of GetObjectMetadataResponseDeserializer.
  /// 
  /// </summary>
  internal class GetObjectMetadataResponseDeserializer : ResponseDeserializer<ObjectMetadata, ObjectMetadata>
  {
    public GetObjectMetadataResponseDeserializer()
      : base((IDeserializer<Stream, ObjectMetadata>) null)
    {
    }

    public override ObjectMetadata Deserialize(ServiceResponse response)
    {
      Debug.Assert(response != null && response.Headers != null);
      ObjectMetadata objectMetadata = new ObjectMetadata();
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) response.Headers)
      {
        if (keyValuePair.Key.StartsWith("x-oss-meta-", false, CultureInfo.InvariantCulture))
          objectMetadata.UserMetadata.Add(keyValuePair.Key.Substring("x-oss-meta-".Length), keyValuePair.Value);
        else if (string.Equals(keyValuePair.Key, "Content-Length", StringComparison.InvariantCultureIgnoreCase))
          objectMetadata.ContentLength = long.Parse(keyValuePair.Value, (IFormatProvider) CultureInfo.InvariantCulture);
        else if (string.Equals(keyValuePair.Key, "ETag", StringComparison.InvariantCultureIgnoreCase))
          objectMetadata.ETag = OssUtils.TrimETag(keyValuePair.Value);
        else if (string.Equals(keyValuePair.Key, "Last-Modified", StringComparison.InvariantCultureIgnoreCase))
          objectMetadata.LastModified = DateUtils.ParseRfc822Date(keyValuePair.Value);
        else
          objectMetadata.AddHeader(keyValuePair.Key, (object) keyValuePair.Value);
      }
      return objectMetadata;
    }
  }
}
