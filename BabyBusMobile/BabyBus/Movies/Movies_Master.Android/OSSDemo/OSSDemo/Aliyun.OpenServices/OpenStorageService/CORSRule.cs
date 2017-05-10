// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.CORSRule
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenStorageService
{
  public class CORSRule
  {
    public IList<string> AllowedOrigins { get; set; }

    public IList<string> AllowedMethods { get; set; }

    public IList<string> AllowedHeaders { get; set; }

    public IList<string> ExposeHeaders { get; set; }

    public int MaxAgeSeconds { get; set; }

    public void AddAllowedOrigin(string allowedOrigin)
    {
      if (this.AllowedOrigins == null)
        this.AllowedOrigins = (IList<string>) new List<string>();
      this.AllowedOrigins.Add(allowedOrigin);
    }

    public void AddAllowedMethod(string allowedMethod)
    {
      if (this.AllowedMethods == null)
        this.AllowedMethods = (IList<string>) new List<string>();
      this.AllowedMethods.Add(allowedMethod);
    }

    public void AddAllowedHeader(string allowedHeader)
    {
      if (this.AllowedHeaders == null)
        this.AllowedHeaders = (IList<string>) new List<string>();
      this.AllowedHeaders.Add(allowedHeader);
    }

    public void AddExposeHeader(string exposedHeader)
    {
      if (this.ExposeHeaders == null)
        this.ExposeHeaders = (IList<string>) new List<string>();
      this.ExposeHeaders.Add(exposedHeader);
    }
  }
}
