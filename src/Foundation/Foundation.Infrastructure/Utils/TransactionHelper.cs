/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: TransactionHelper.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Helper for transaction
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using System.Transactions;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    public static class TransactionHelper
    {
        public static TransactionScope CreateReadCommitted()
        {
            return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = GetIsolationLevel(IsolationLevel.ReadCommitted) });
        }

        public static TransactionScope CreateRepeatableRead()
        {
            return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = GetIsolationLevel(IsolationLevel.RepeatableRead) });
        }

        public static TransactionScope CreateSerializable()
        {
            return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = GetIsolationLevel(IsolationLevel.Serializable) });
        }

        private static IsolationLevel GetIsolationLevel(IsolationLevel isolationLevel)
        {
            return Transaction.Current == null ? isolationLevel : Transaction.Current.IsolationLevel;
        }
    }
}
