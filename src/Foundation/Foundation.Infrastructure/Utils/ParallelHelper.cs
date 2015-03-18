using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    public static class ParallelHelper
    {
        public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> body)
        {
            if (OperationContext.Current != null)
            {
                var user = (User)OperationContext.Current.IncomingMessageHeaders.GetHeader<User>(ServiceContextConstant.CURRENTUSER, ServiceContextConstant.HEADERNAMESPACE);

                CallContext.LogicalSetData(ServiceContextConstant.CURRENTUSER, user);
            }

            Action<TSource, ParallelLoopState> newBody = (x, y) =>
                                                                {
                                                                    try
                                                                    {
                                                                        body(x);
                                                                    }
                                                                    catch
                                                                    {
                                                                        y.Stop();

                                                                        throw;
                                                                    }
                                                                };

            try
            {
                var result= Parallel.ForEach(source, newBody);

                CallContext.FreeNamedDataSlot(ServiceContextConstant.CURRENTUSER);

                return result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }
    }
}