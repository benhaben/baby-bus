// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.GetObjectResponseDeserializer
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using System.Diagnostics;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  /// <summary>
  /// Description of GetObjectResponseDeserializer.
  /// 
  /// </summary>
  internal class GetObjectResponseDeserializer : ResponseDeserializer<OssObject, OssObject>
  {
    private GetObjectRequest _getObjectRequest;

    public GetObjectResponseDeserializer(GetObjectRequest getObjectRequest)
      : base((IDeserializer<Stream, OssObject>) null)
    {
      Debug.Assert(getObjectRequest != null && !string.IsNullOrEmpty(getObjectRequest.BucketName) && !string.IsNullOrEmpty(getObjectRequest.Key));
      this._getObjectRequest = getObjectRequest;
    }

    public override OssObject Deserialize(ServiceResponse response)
    {
      return new OssObject(this._getObjectRequest.Key)
      {
        BucketName = this._getObjectRequest.BucketName,
        Content = response.Content,
        Metadata = DeserializerFactory.GetFactory().CreateGetObjectMetadataResultDeserializer().Deserialize(response)
      };
    }
  }
}
