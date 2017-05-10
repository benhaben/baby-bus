// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.OtsErrorCode
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 表示来自开放结构化数据服务（Open Table Service，OTS）的错误代码。
  /// 
  /// </summary>
  public static class OtsErrorCode
  {
    /// <summary>
    /// 用户身份验证失败。
    /// 
    /// </summary>
    public const string AuthorizationFailure = "OTSAuthFailed";
    /// <summary>
    /// 服务器内部错误。
    /// 
    /// </summary>
    public const string InternalServerError = "OTSInternalServerError";
    /// <summary>
    /// 用户的配额已经用满。
    /// 
    /// </summary>
    public const string QuotaExhausted = "OTSQuotaExhausted";
    /// <summary>
    /// 数据超过范围。
    /// 
    /// </summary>
    public const string StorageDataOutOfRange = "OTSStorageDataOutOfRange";
    /// <summary>
    /// 服务器内部错误。
    /// 
    /// </summary>
    public const string StorageInternalError = "OTSStorageInternalError";
    /// <summary>
    /// 主键列无效，例如类型错误。
    /// 
    /// </summary>
    public const string StorageInvalidPrimaryKey = "OTSStorageInvalidPK";
    /// <summary>
    /// 要创建的对象（表或视图）已存在。
    /// 
    /// </summary>
    public const string StorageObjectAlreadyExist = "OTSStorageObjectAlreadyExist";
    /// <summary>
    /// 对象（表或视图）不存在。
    /// 
    /// </summary>
    public const string StorageObjectNotExist = "OTSStorageObjectNotExist";
    /// <summary>
    /// 参数无效。
    /// 
    /// </summary>
    public const string StorageParameterInvalid = "OTSStorageParameterInvalid";
    /// <summary>
    /// 数据分片未准备就绪。
    /// 
    /// </summary>
    public const string StoragePartitionNotReady = "OTSStoragePartitionNotReady";
    /// <summary>
    /// 要插入的行已经存在。
    /// 
    /// </summary>
    public const string StoragePrimaryKeyAlreadyExist = "OTSStoragePrimaryKeyAlreadyExist";
    /// <summary>
    /// 要更新的行不存在。
    /// 
    /// </summary>
    public const string StoragePrimaryKeyNotExist = "OTSStoragePrimaryKeyNotExist";
    /// <summary>
    /// 服务器忙，请等待。
    /// 
    /// </summary>
    public const string StorageServerBusy = "OTSStorageServerBusy";
    /// <summary>
    /// 要操作的事务不存在或已经过期。
    /// 
    /// </summary>
    public const string StorageSessionNotExist = "OTSStorageSessionNotExist";
    /// <summary>
    /// 超时错误，操作结果未定义。
    /// 
    /// </summary>
    public const string StorageTimeout = "OTSStorageTimeout";
    /// <summary>
    /// 不能获得指定的事务锁。
    /// 
    /// </summary>
    public const string StorageTransactionLockKeyFail = "OTSStorageTxnLockKeyFail";
    /// <summary>
    /// OTS未知错误。
    /// 
    /// </summary>
    public const string StorageUnknownError = "OTSStorageUnknownError";
    /// <summary>
    /// 视图的主键列不完整。
    /// 
    /// </summary>
    public const string StorageViewIncompletePrimaryKey = "OTSStorageViewIncompletePK";
    /// <summary>
    /// 指定主键列的相关信息与表结构的相关信息不匹配。
    /// 
    /// </summary>
    public const string UnmatchedMeta = "OTSMetaNotMatch";
  }
}
