// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.SetBucketCorsRequest
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenStorageService
{
  public class SetBucketCorsRequest
  {
    /// <summary>
    /// 获取<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>所在<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。
    /// 
    /// </summary>
    public string BucketName { get; private set; }

    /// <summary>
    /// CORS规则的容器，每个bucket最多允许10条规则。
    /// 
    /// </summary>
    public IList<CORSRule> CORSRules { get; set; }

    public SetBucketCorsRequest(string bucketName)
    {
      this.BucketName = bucketName;
    }

    /// <summary>
    /// 添加一条CORSRule。
    /// 
    /// </summary>
    /// <param name="corsRule"/>
    public void AddCORSRule(CORSRule corsRule)
    {
      if (this.CORSRules == null)
        this.CORSRules = (IList<CORSRule>) new List<CORSRule>();
      this.CORSRules.Add(corsRule);
    }
  }
}
