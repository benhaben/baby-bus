// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.CopyObjectResult
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 拷贝Object的请求结果
  /// 
  /// </summary>
  public class CopyObjectResult
  {
    /// <summary>
    /// 获取新Object最后更新时间。
    /// 
    /// </summary>
    public DateTime LastModified { get; internal set; }

    /// <summary>
    /// 获取新Object的ETag值。
    /// 
    /// </summary>
    public string ETag { get; internal set; }
  }
}
