// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.OtsRequestSigner
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  /// <summary>
  /// Description of OTSRequestSigner.
  /// 
  /// </summary>
  internal class OtsRequestSigner : IRequestSigner
  {
    public void Sign(ServiceRequest request, ServiceCredentials credentials)
    {
      OtsRequestSigner.AddRequiredParameters(request.Parameters, request.ResourcePath, credentials, ServiceSignature.Create(), DateTime.UtcNow);
    }

    private static void AddRequiredParameters(IDictionary<string, string> parameters, string action, ServiceCredentials credentials, ServiceSignature signer, DateTime timestamp)
    {
      if (parameters.ContainsKey("Signature"))
        parameters.Remove("Signature");
      parameters["Date"] = DateUtils.FormatRfc822Date(timestamp);
      parameters["OTSAccessKeyId"] = credentials.AccessId;
      parameters["APIVersion"] = "1";
      parameters["SignatureMethod"] = signer.SignatureMethod;
      parameters["SignatureVersion"] = signer.SignatureVersion;
      string signature = OtsRequestSigner.GetSignature(credentials, signer, action, parameters);
      parameters["Signature"] = signature;
    }

    private static string GetSignature(ServiceCredentials credentials, ServiceSignature signer, string action, IDictionary<string, string> parameters)
    {
      string data = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "/{0}\n{1}", new object[2]
      {
        (object) action,
        (object) HttpUtils.GetRequestParameterString((IEnumerable<KeyValuePair<string, string>>) Enumerable.OrderBy<KeyValuePair<string, string>, string>((IEnumerable<KeyValuePair<string, string>>) parameters, (Func<KeyValuePair<string, string>, string>) (e => e.Key), (IComparer<string>) StringComparer.Ordinal))
      });
      return signer.ComputeSignature(credentials.AccessKey, data);
    }
  }
}
