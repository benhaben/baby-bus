// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Bucket
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Globalization;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// Bucket是OSS上的命名空间。
  /// 
  /// </summary>
  /// 
  /// <remarks>
  /// 
  /// <para>
  /// Bucket名在整个 OSS 中具有全局唯一性，且不能修改；存储在OSS上的每个Object必须都包含在某个Bucket中。
  ///             一个应用，例如图片分享网站，可以对应一个或多个 Bucket。一个用户最多可创建 10 个Bucket，
  ///             但每个Bucket 中存放的Object的数量和大小总和没有限制，用户不需要考虑数据的可扩展性。
  /// 
  /// </para>
  /// 
  /// <para>
  /// Bucket 命名规范
  /// 
  /// <list type="">
  /// 
  /// <item>
  /// 只能包括小写字母，数字，下划线（_）和短横线（-）
  /// </item>
  /// 
  /// <item>
  /// 必须以小写字母或者数字开头
  /// </item>
  /// 
  /// <item>
  /// 长度必须在 3-255 字节之间
  /// </item>
  /// 
  /// </list>
  /// 
  /// </para>
  /// 
  /// </remarks>
  public class Bucket
  {
    /// <summary>
    /// 获取Bucket的名称。
    /// 
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// 获取Bucket的<see cref="P:Aliyun.OpenServices.OpenStorageService.Bucket.Owner"/>
    /// </summary>
    public Owner Owner { get; internal set; }

    /// <summary>
    /// 获取Bucket的创建时间。
    /// 
    /// </summary>
    public DateTime CreationDate { get; internal set; }

    /// <summary>
    /// 使用指定的Bucket名称构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>实例。
    /// 
    /// </summary>
    /// <param name="name">Bucket的名称。</param>
    public Bucket(string name)
    {
      this.Name = name;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "OSS Bucket [Name={0}], [Owner={1}], [CreationTime={2}]", (object) this.Name, (object) this.Owner, (object) this.CreationDate);
    }
  }
}
