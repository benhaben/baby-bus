// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.GroupGrantee
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 定义了可以被授权的一组OSS用户。
  /// 
  /// </summary>
  public sealed class GroupGrantee : IGrantee
  {
    private static GroupGrantee _allUsers = new GroupGrantee("http://oss.service.aliyun.com/acl/group/ALL_USERS");
    private string _identifier;

    /// <summary>
    /// 获取被授权者的标识。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 不支持set操作，否则会抛出<see cref="T:System.NotSupportedException"/>。
    /// 
    /// </remarks>
    public string Identifier
    {
      get
      {
        return this._identifier;
      }
      set
      {
        throw new NotSupportedException();
      }
    }

    /// <summary>
    /// 表示为OSS的<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>或<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>指定匿名访问的权限。
    ///             任何用户都可以根据被授予的权限进行访问。
    /// 
    /// </summary>
    public static GroupGrantee AllUsers
    {
      get
      {
        return GroupGrantee._allUsers;
      }
    }

    private GroupGrantee(string identifier)
    {
      this._identifier = identifier;
    }

    public override bool Equals(object obj)
    {
      GroupGrantee groupGrantee = obj as GroupGrantee;
      if (groupGrantee == null)
        return false;
      else
        return groupGrantee.Identifier == this.Identifier;
    }

    public override int GetHashCode()
    {
      return ("[GroupGrantee ID=" + this.Identifier + "]").GetHashCode();
    }
  }
}
