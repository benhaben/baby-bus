// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.GetAclResponseDeserializer
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Model;
using System;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  /// <summary>
  /// Description of GetAclResponseParser.
  /// 
  /// </summary>
  internal class GetAclResponseDeserializer : ResponseDeserializer<AccessControlList, AccessControlPolicy>
  {
    public GetAclResponseDeserializer(IDeserializer<Stream, AccessControlPolicy> contentDeserializer)
      : base(contentDeserializer)
    {
    }

    public override AccessControlList Deserialize(ServiceResponse response)
    {
      AccessControlPolicy accessControlPolicy = this.ContentDeserializer.Deserialize(response.Content);
      AccessControlList accessControlList = new AccessControlList();
      accessControlList.Owner = new Owner(accessControlPolicy.Owner.Id, accessControlPolicy.Owner.DisplayName);
      foreach (string str in accessControlPolicy.Grants)
      {
        if (str == EnumUtils.GetStringValue((Enum) CannedAccessControlList.PublicRead))
          accessControlList.GrantPermission((IGrantee) GroupGrantee.AllUsers, Permission.Read);
        else if (str == EnumUtils.GetStringValue((Enum) CannedAccessControlList.PublicReadWrite))
          accessControlList.GrantPermission((IGrantee) GroupGrantee.AllUsers, Permission.FullControl);
      }
      return accessControlList;
    }
  }
}
