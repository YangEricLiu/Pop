阿里云计算开放服务软件开发工具包.NET版
Aliyun Open Services SDK for .NET

版权所有 （C）阿里云计算有限公司

Copyright (C) Alibaba Cloud Computing
All rights reserved.

http://www.aliyun.com

环境要求：
- .NET Framework 3.5 SP1或以上版
- 必须注册有Aliyun.com用户账户，并开通相应的服务（OTS）。

程序集：Aliyun.OpenServices.dll
版本号：1.0.0.0

包结构：
bin
----Aliyun.OpenServices.dll   .NET程序集文件
----Aliyun.OpenServices.pdb   调试和项目状态信息文件
----Aliyun.OpenServices.xml   程序集注释文档
doc
----Aliyun.OpenServices.chm   帮助文档

更新日志:

2012/03/16
- OTS访问接口，包括对表、表组的创建、修改和删除等操作，对数据的插入、修改、删除和查询等操作。
- 访问的客户端设置，如果代理设置、HTTP连接属性设置等。
- 统一的结构化异常处理。

2012/05/16
- OTSClient.GetRowsByRange支持反向读取。

2012/06/15
- OSS：首次加入对OSS的支持。包含了OSS Bucket、ACL、Object的创建、修改、读取、删除等基本操作。
- OTS： OTSClient.GetRowsByOffset支持反向读取。
- 加入对特定请求错误的自动处理机制。
- 增加HTML格式的帮助文件。

2012/09/05
- OSS: 解决ListObjects时Prefix等参数无效的问题。

2012/10/10
- OSS: 将默认的OSS服务地址更新为：http://oss.aliyuncs.com