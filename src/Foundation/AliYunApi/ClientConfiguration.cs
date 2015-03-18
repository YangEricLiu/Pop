/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;

namespace Aliyun.OpenServices.Ots
{
    /// <summary>
    /// 表示访问阿里云服务的配置信息。
    /// </summary>
    public class ClientConfiguration : ICloneable
    {
        private static string _defaultUserAgent = "aliyun-openservices-sdk-dotnet_"
            + typeof(ClientConfiguration).Assembly.GetName().Version.ToString();

        private string _userAgent = _defaultUserAgent;

        private int _proxyPort = -1;
        private int _connectionTimeout = 60 * 1000;
        private int _maxErrorRetry = 3;
        private int _retryPauseScale = 300;

        /// <summary>
        /// 获取设置访问请求的User-Agent。
        /// </summary>
        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        /// <summary>
        /// 获取或设置代理服务器的地址。
        /// </summary>
        public string ProxyHost { get; set; }

        /// <summary>
        /// 获取或设置代理服务器的端口。
        /// </summary>
        public int ProxyPort
        {
            get { return _proxyPort; }
            set { _proxyPort = value; }
        }

        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        public string ProxyUserName { get; set; }

        /// <summary>
        /// 获取或设置密码。
        /// </summary>
        public string ProxyPassword { get; set; }

        /// <summary>
        /// 获取或设置代理服务器授权用户所在的域。
        /// </summary>
        public string ProxyDomain { get; set; }

        /// <summary>
        /// 获取或设置连接的超时时间，单位为毫秒。
        /// </summary>
        public int ConnectionTimeout
        {
            get { return _connectionTimeout; }
            set { _connectionTimeout = value; }
        }

        /// <summary>
        /// 获取或设置请求发生错误时最大的重试次数。
        /// </summary>
        public int MaxErrorRetry
        {
            get { return _maxErrorRetry; }
            set { _maxErrorRetry = value; }
        }

        /// <summary>
        /// 获取或设置请求发生错误时重试间隔（毫秒）。
        /// </summary>
        public int RetryPauseScale
        {
            get { return _retryPauseScale; }
            set { _retryPauseScale = value; }
        }

        /// <summary>
        /// 初始化新的<see cref="ClientConfiguration"/>的实例。
        /// </summary>
        public ClientConfiguration()
        {
        }

        /// <summary>
        /// 获取该实例的拷贝。
        /// </summary>
        /// <returns>该实例的拷贝。</returns>
        public object Clone()
        {
            ClientConfiguration config = new ClientConfiguration();
            config.ConnectionTimeout = this.ConnectionTimeout;
            config.MaxErrorRetry = this.MaxErrorRetry;
            config.ProxyDomain = this.ProxyDomain;
            config.ProxyHost = this.ProxyHost;
            config.ProxyPassword = this.ProxyPassword;
            config.ProxyPort = this.ProxyPort;
            config.ProxyUserName = this.ProxyUserName;
            config.UserAgent = this.UserAgent;
            return config;
        }
    }
}
