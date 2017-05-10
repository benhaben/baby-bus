// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.UploadPartResultDeserializer
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using System.Diagnostics;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  /// <summary>
  /// Description of UploadPartResultDeserializer.
  /// 
  /// </summary>
  internal class UploadPartResultDeserializer : ResponseDeserializer<UploadPartResult, UploadPartResult>
  {
    private int _partNumber;

    public UploadPartResultDeserializer(int partNumber)
      : base((IDeserializer<Stream, UploadPartResult>) null)
    {
      this._partNumber = partNumber;
    }

    public override UploadPartResult Deserialize(ServiceResponse response)
    {
      Debug.Assert(response != null);
      UploadPartResult uploadPartResult = new UploadPartResult();
      if (response.Headers.ContainsKey("ETag"))
        uploadPartResult.ETag = OssUtils.TrimETag(response.Headers["ETag"]);
      uploadPartResult.PartNumber = this._partNumber;
      return uploadPartResult;
    }
  }
}
