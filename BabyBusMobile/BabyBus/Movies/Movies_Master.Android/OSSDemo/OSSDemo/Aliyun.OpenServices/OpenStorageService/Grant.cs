// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Grant
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 访问控制的授权信息。
  /// 
  /// </summary>
  public class Grant
  {
    /// <summary>
    /// 获取被授权者信息。
    /// 
    /// </summary>
    public IGrantee Grantee { get; private set; }

    /// <summary>
    /// 获取被授权的权限。
    /// 
    /// </summary>
    public Permission Permission { get; private set; }

    /// <summary>
    /// 构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.Grant"/>实体。
    /// 
    /// </summary>
    /// <param name="grantee">被授权者。目前只支持<see cref="P:Aliyun.OpenServices.OpenStorageService.GroupGrantee.AllUsers"/>。</param><param name="permission">权限。</param>
    public Grant(IGrantee grantee, Permission permission)
    {
      if (grantee == null)
        throw new ArgumentNullException("grantee");
      this.Grantee = grantee;
      this.Permission = permission;
    }

    public override bool Equals(object obj)
    {
      Grant grant = obj as Grant;
      if (grant == null)
        return false;
      else
        return this.Grantee.Identifier == grant.Grantee.Identifier && this.Permission == grant.Permission;
    }

    public override int GetHashCode()
    {
      return (this.Grantee.Identifier + ":" + ((object) this.Permission).ToString()).GetHashCode();
    }
  }
}
