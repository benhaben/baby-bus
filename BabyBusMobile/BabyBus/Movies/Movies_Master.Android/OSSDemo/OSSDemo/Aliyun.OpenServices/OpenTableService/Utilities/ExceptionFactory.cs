// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.ExceptionFactory
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService;
using Aliyun.OpenServices.OpenTableService.Model;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  /// <summary>
  /// The factory to create an instance of <see cref="T:Aliyun.OpenServices.OpenTableService.OtsException"/>.
  /// 
  /// </summary>
  internal static class ExceptionFactory
  {
    public static OtsException CreateException(string errorCode, string message, string requestId, string hostId)
    {
      return ExceptionFactory.CreateException(errorCode, message, requestId, hostId, (Exception) null);
    }

    public static OtsException CreateException(string errorCode, string message, string requestId, string hostId, Exception innerException)
    {
      OtsException otsException = innerException != null ? new OtsException(message, innerException) : new OtsException(message);
      otsException.ErrorCode = errorCode;
      otsException.RequestId = requestId;
      otsException.HostId = hostId;
      return otsException;
    }

    public static OtsException CreateException(ErrorResult errorResult, Exception innerException)
    {
      Debug.Assert(errorResult != null);
      return ExceptionFactory.CreateException(errorResult.Code, errorResult.Message, errorResult.RequestId, errorResult.HostId, innerException);
    }

    public static Exception CreateInvalidResponseException(ServiceResponse response, string message, Exception innerException)
    {
      string str1 = (string) null;
      string str2 = (string) null;
      if (response != null)
      {
        try
        {
          string input = ExceptionFactory.ReadResponseAsString(response);
          Match match1 = new Regex("\\<RequestID\\>(\\w+)\\</RequestID\\>").Match(input);
          if (match1.Success)
            str1 = match1.Groups[1].Value;
          Match match2 = new Regex("\\<HostID\\>(\\w+)\\</HostID\\>").Match(input);
          if (match2.Success)
            str2 = match2.Groups[1].Value;
        }
        catch (InvalidOperationException ex)
        {
        }
      }
      if (!string.IsNullOrEmpty(str1) || !string.IsNullOrEmpty(str2))
        message = message + string.Format((IFormatProvider) CultureInfo.InvariantCulture, OtsExceptions.InvalidResponseMessage, new object[2]
        {
          (object) str1,
          (object) str2
        });
      return (Exception) new InvalidOperationException(message, innerException);
    }

    private static string ReadResponseAsString(ServiceResponse response)
    {
      using (Stream content = response.Content)
      {
        StringBuilder stringBuilder = new StringBuilder();
        byte[] numArray = new byte[4096];
        while (content.Read(numArray, 0, numArray.Length) > 0)
        {
          string @string = OtsUtility.DataEncoding.GetString(numArray);
          stringBuilder.Append(@string);
        }
        return ((object) stringBuilder).ToString();
      }
    }
  }
}
