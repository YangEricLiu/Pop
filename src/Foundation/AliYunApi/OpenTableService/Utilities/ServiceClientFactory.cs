/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using Aliyun.OpenServices.Common.Communication;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    /// <summary>
    /// Description of ServiceClientFactory.
    /// </summary>
    internal class ServiceClientFactory
    {
        public static IServiceClient CreateServiceClient(Aliyun.OpenServices.Ots.ClientConfiguration configuration)
        {
            Debug.Assert(configuration != null);

            RetryableServiceClient retryableServiceClient =
                new RetryableServiceClient(ServiceClient.Create(configuration));
            retryableServiceClient.MaxErrorRetry = configuration.MaxErrorRetry;
            retryableServiceClient.ShouldRetryCallback = CanRetry;

            return retryableServiceClient;
        }

        private static bool CanRetry(Exception ex)
        {
            OtsException otsException = ex as OtsException;
            if (otsException != null)
            {
                // It could retry on the following errors.
                return otsException.ErrorCode == OtsErrorCode.InternalServerError ||
                    otsException.ErrorCode == OtsErrorCode.StorageServerBusy ||
                    otsException.ErrorCode == OtsErrorCode.StorageTimeout ||
                    otsException.ErrorCode == OtsErrorCode.StorageTransactionLockKeyFail ||
                    otsException.ErrorCode == OtsErrorCode.StoragePartitionNotReady;
            }
            
            return false;
        }
    }
}
