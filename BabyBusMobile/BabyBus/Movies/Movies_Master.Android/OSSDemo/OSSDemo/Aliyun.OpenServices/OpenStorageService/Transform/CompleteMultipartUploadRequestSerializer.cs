// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.CompleteMultipartUploadRequestSerializer
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Model;
using System.Collections.Generic;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  /// <summary>
  /// Description of CompleteMultipartUploadRequestSerializer.
  /// 
  /// </summary>
  internal class CompleteMultipartUploadRequestSerializer : RequestSerializer<CompleteMultipartUploadRequest, CompleteMultipartUploadRequestModel>
  {
    public CompleteMultipartUploadRequestSerializer(ISerializer<CompleteMultipartUploadRequestModel, Stream> contentSerializer)
      : base(contentSerializer)
    {
    }

    public override Stream Serialize(CompleteMultipartUploadRequest request)
    {
      CompleteMultipartUploadRequestModel input = new CompleteMultipartUploadRequestModel();
      List<CompleteMultipartUploadRequestModel.CompletePart> list = new List<CompleteMultipartUploadRequestModel.CompletePart>();
      foreach (PartETag partEtag in (IEnumerable<PartETag>) request.PartETags)
        list.Add(new CompleteMultipartUploadRequestModel.CompletePart()
        {
          ETag = "\"" + partEtag.ETag + "\"",
          PartNumber = partEtag.PartNumber
        });
      input.Parts = list.ToArray();
      return this.ContentSerializer.Serialize(input);
    }
  }
}
