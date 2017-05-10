// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.ValidationResponseHandler
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Handlers;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenTableService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  /// <summary>
  /// Description of ValidationResponseHandler.
  /// 
  /// </summary>
  internal class ValidationResponseHandler : ResponseHandler
  {
    private const int _responseTimeoutMinutes = 15;
    private ServiceCredentials _credentials;
    private string _action;

    public ValidationResponseHandler(ServiceCredentials credentials, string action)
    {
      Debug.Assert(!string.IsNullOrEmpty(action) && credentials != null);
      this._credentials = credentials;
      this._action = action;
    }

    public override void Handle(ServiceResponse response)
    {
      base.Handle(response);
      IDictionary<string, string> headers = response.Headers;
      if (!headers.Keys.Contains("x-ots-date"))
        throw ExceptionFactory.CreateInvalidResponseException(response, string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.ResponseDoesNotContainHeader, new object[1]
        {
          (object) "x-ots-date"
        }), (Exception) null);
      else if (!headers.Keys.Contains("Content-Md5"))
        throw ExceptionFactory.CreateInvalidResponseException(response, string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.ResponseDoesNotContainHeader, new object[1]
        {
          (object) "Content-Md5"
        }), (Exception) null);
      else if (!headers.Keys.Contains("Content-Type"))
      {
        throw ExceptionFactory.CreateInvalidResponseException(response, string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.ResponseDoesNotContainHeader, new object[1]
        {
          (object) "Content-Type"
        }), (Exception) null);
      }
      else
      {
        if (!headers.Keys.Contains("Authorization"))
          throw ExceptionFactory.CreateInvalidResponseException(response, OtsExceptions.ResponseFailedAuthorization, (Exception) null);
        string dt = headers["x-ots-date"];
        if (string.IsNullOrEmpty(dt))
          throw ExceptionFactory.CreateInvalidResponseException(response, OtsExceptions.ResponseExpired, (Exception) null);
        if (DateTime.UtcNow.Subtract(DateUtils.ParseRfc822Date(dt)).TotalMinutes > 15.0)
          throw ExceptionFactory.CreateInvalidResponseException(response, OtsExceptions.ResponseExpired, (Exception) null);
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string index in (IEnumerable<string>) headers.Keys)
        {
          if (index.StartsWith("x-ots-", StringComparison.OrdinalIgnoreCase))
            stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}:{1}\n", new object[2]
            {
              (object) index,
              (object) headers[index]
            });
        }
        string str1 = headers["Content-Md5"];
        string str2 = headers["Content-Type"];
        string str3 = "/" + this._action;
        string signature = ServiceSignature.Create().ComputeSignature(this._credentials.AccessKey, str1 + "\n" + str2 + "\n" + ((object) stringBuilder).ToString() + str3);
        string str4 = headers["Authorization"];
        bool flag = false;
        if (Enumerable.Contains<char>((IEnumerable<char>) str4, ':'))
          flag = Enumerable.Last<string>((IEnumerable<string>) str4.Split(':')).EndsWith(signature, StringComparison.Ordinal);
        if (!flag)
          throw ExceptionFactory.CreateInvalidResponseException(response, OtsExceptions.ResponseFailedAuthorization, (Exception) null);
      }
    }
  }
}
