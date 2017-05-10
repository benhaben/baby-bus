// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.AccessControlList
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 表示OSS的访问控制列表（Access Control List， ACL），
  ///             包含了一组为指定被授权者<see cref="T:Aliyun.OpenServices.OpenStorageService.IGrantee"/>
  ///             分配特定权限<see cref="T:Aliyun.OpenServices.OpenStorageService.Permission"/>的集合。
  /// 
  /// </summary>
  public class AccessControlList
  {
    private HashSet<Grant> _grants = new HashSet<Grant>();

    /// <summary>
    /// 获取所有<see cref="T:Aliyun.OpenServices.OpenStorageService.Grant"/>实例的枚举。
    /// 
    /// </summary>
    public IEnumerable<Grant> Grants
    {
      get
      {
        return (IEnumerable<Grant>) this._grants;
      }
    }

    /// <summary>
    /// 获取或设置所有者。
    /// 
    /// </summary>
    public Owner Owner { get; set; }

    /// <summary>
    /// 为指定<see cref="T:Aliyun.OpenServices.OpenStorageService.IGrantee"/>授予特定的<see cref="T:Aliyun.OpenServices.OpenStorageService.Permission"/>。
    ///             目前只支持被授权者为<see cref="P:Aliyun.OpenServices.OpenStorageService.GroupGrantee.AllUsers"/>。
    /// 
    /// </summary>
    /// <param name="grantee">被授权者。</param><param name="permission">被授予的权限。</param>
    public void GrantPermission(IGrantee grantee, Permission permission)
    {
      if (grantee == null)
        throw new ArgumentNullException("grantee");
      this._grants.Add(new Grant(grantee, permission));
    }

    /// 取消指定{@link Grantee}已分配的所有权限。
    ///             @param grantee
    ///                       被授权者。目前只支持被授权者为{@link GroupGrantee#AllUsers}。
    /// 
    /// <summary>
    /// 取消指定<see cref="T:Aliyun.OpenServices.OpenStorageService.IGrantee"/>已分配的所有权限。
    /// 
    /// </summary>
    /// <param name="grantee">被授权者。</param>
    public void RevokeAllPermissions(IGrantee grantee)
    {
      if (grantee == null)
        throw new ArgumentNullException("grantee");
      this._grants.RemoveWhere((Predicate<Grant>) (e => e.Grantee == grantee));
    }

    /// 返回该对象的字符串表示。
    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Grant grant in this.Grants)
        stringBuilder.Append(grant.ToString()).Append(",");
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "[AccessControlList: Owner={0}, Grants={1}]", new object[2]
      {
        (object) this.Owner,
        (object) ((object) stringBuilder).ToString()
      });
    }
  }
}
