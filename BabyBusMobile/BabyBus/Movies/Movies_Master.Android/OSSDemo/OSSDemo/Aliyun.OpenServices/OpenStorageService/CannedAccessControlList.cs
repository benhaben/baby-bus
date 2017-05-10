// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.CannedAccessControlList
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Utilities;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 表示一组常用的用户访问权限。
  /// 
  /// <para>
  /// 这一组常用权限相当于给所有用户指定权限的快捷方法。
  /// 
  /// </para>
  /// 
  /// </summary>
  public enum CannedAccessControlList
  {
    [StringValue("private")] Private,
    [StringValue("public-read")] PublicRead,
    [StringValue("public-read-write")] PublicReadWrite,
  }
}
