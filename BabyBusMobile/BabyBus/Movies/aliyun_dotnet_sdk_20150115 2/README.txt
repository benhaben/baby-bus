阿里云计算开放服务软件开发工具包.NET版
Aliyun Open Services SDK for .NET

版权所有 （C）阿里云计算有限公司

Copyright (C) Alibaba Cloud Computing
All rights reserved.

http://www.aliyun.com

环境要求：
- .NET Framework 4.0及以上版本
- 必须注册有Aliyun.com用户账户。

程序集：Aliyun.OSS.dll
版本号：1.0.5492.31618

包结构：
bin
----Aliyun.OSS.dll   .NET程序集文件
----Aliyun.OSS.pdb   调试和项目状态信息文件
----Aliyun.OSS.xml   程序集注释文档
doc
----Aliyun.OSS.chm   帮助文档
src
----SDK源代码
sample
----Sample源代码

更新日志:

2015/01/14
- 移除OTS分支，程序集命名更改为Aliyun.OSS.dll
- .NET Framework版本升至4.0及以上
- OSS: 添加Copy Part、Delete Objects、Bucket Referer List等接口。
- OSS: 添加ListBuckets分页功能。
- OSS: 添加CNAME支持。
- OSS: 修复Put/GetObject流中断问题。
- OSS: 添加若干Samples。

2014/06/26
- OSS: 添加对cors、Logging、website等接口的支持。

2013/09/02
- OSS: 修复了某些情况下无法抛出正确的异常的Bug。
- 优化了SDK的性能。

2013/06/04
- OSS: 将默认OSS服务访问方式修改为三级域名方式。

2013/05/20
- OTS: 将默认的OTS服务地址更新为：http://ots.aliyuncs.com
- 新加入对Mono的支持。
- 修复了SDK中的几处Bug，使其运行更稳定。

2013/04/10
- OSS：添加了Object分块上传（Multipart Upload）功能。
- OSS：添加了Copy Object功能。
- OSS：添加了生成预签名URL的功能。
- OSS：分离出IOss接口，并由OssClient继承此接口。

2012/10/10
- OSS: 将默认的OSS服务地址更新为：http://oss.aliyuncs.com

2012/09/05
- OSS: 解决ListObjects时Prefix等参数无效的问题。

2012/06/15
- OSS：首次加入对OSS的支持。包含了OSS Bucket、ACL、Object的创建、修改、读取、删除等基本操作。
- OTS： OTSClient.GetRowsByOffset支持反向读取。
- 加入对特定请求错误的自动处理机制。
- 增加HTML格式的帮助文件。

2012/05/16
- OTSClient.GetRowsByRange支持反向读取。

2012/03/16
- OTS访问接口，包括对表、表组的创建、修改和删除等操作，对数据的插入、修改、删除和查询等操作。
- 访问的客户端设置，如果代理设置、HTTP连接属性设置等。
- 统一的结构化异常处理。



