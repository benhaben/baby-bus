// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Utilities.OssRequestSigner
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Communication;
using System.Diagnostics;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
  /// <summary>
  /// Description of OssRequestSigner.
  /// 
  /// </summary>
  internal class OssRequestSigner : IRequestSigner
  {
    private string _resourcePath;

    public OssRequestSigner(string resourcePath)
    {
      Debug.Assert(resourcePath != null && resourcePath.StartsWith("/"));
      this._resourcePath = resourcePath;
    }

    public void Sign(ServiceRequest request, ServiceCredentials credentials)
    {
      Debug.Assert(request != null && credentials != null);
      string accessId = credentials.AccessId;
      string accessKey = credentials.AccessKey;
      string method = ((object) request.Method).ToString().ToUpperInvariant();
      string resourcePath = this._resourcePath;
      Debug.Assert(!string.IsNullOrEmpty(accessId));
      if (!string.IsNullOrEmpty(accessKey))
      {
        string data = SignUtils.BuildCanonicalString(method, resourcePath, request);
        string signature = ServiceSignature.Create().ComputeSignature(accessKey, data);
        request.Headers.Add("Authorization", "OSS " + accessId + ":" + signature);
      }
      else
        request.Headers.Add("Authorization", accessId);
    }
  }
}
