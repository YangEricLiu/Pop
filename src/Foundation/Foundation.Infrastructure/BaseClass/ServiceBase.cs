
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace SE.DSP.Foundation.Infrastructure.BaseClass
{
    /// <summary>
    /// The abstract base class for app service api
    /// 
    /// 
    /// </summary>
    /// <remarks>
    /// <para> 
    /// This class inherits from <see cref="ContextBoundObject" /> and applys <see cref="InvokeContextAttribute" /> 
    /// for tracing the methods invoke chain.
    /// </para> 
    /// <para> 
    /// Applys <see cref="ServiceErrorHandlerBehavior" /> for handling the exceptions those are thrown by app service.
    /// </para>
    /// <para> 
    /// Applys <see cref="ServiceBehaviorAttribute" /> for setting <see cref="ServiceBehaviorAttribute.InstanceContextMode" />
    /// to <see cref="InstanceContextMode.Single" /> and <see cref="ServiceBehaviorAttribute.ConcurrencyMode" /> to <see cref="ConcurrencyMode.Multiple" />
    /// </para>
    /// </remarks>
    [ServiceErrorHandlerBehavior]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public abstract class ServiceBase //: ContextBoundObject
    {
        /// <summary>
        /// Instantiates an instance of the BLBase class and invoke the <see cref="BLBase.RegisterType()"/> method.
        /// </summary>
        protected ServiceBase()
        {
            this.RegisterType();
        }

        /// <summary>
        /// Register types those are needed by this BL into IoC container.
        /// </summary>
        protected virtual void RegisterType() { }

        protected void ThrowDataAuthorizationException()
        {
            throw new DataAuthorizationException(Layer.BL, Module.AccessControl, Convert.ToInt32(ErrorCode.NoDataPrivilege));
        }

        /// <summary>
        /// convert data by language
        /// </summary>
        /// <typeparam name="T">DtoBase</typeparam>
        /// <param name="entities">Dto collection</param>
        /// <param name="tableName">db Table name</param>
        /// <param name="resKeyFiled">db key column</param>
        /// <param name="resValueField">db column value</param>
        /// <returns>return converted dto collection</returns>
        protected  List<T> ConvertDataSourceByLanguage<T>(IEnumerable<T> entities, string tableName, string resKeyFiled, string resValueField) where T:class //where T : DtoBase,EntityBase
        {
            if (entities == null ||
                string.IsNullOrEmpty(tableName) ||
                string.IsNullOrEmpty(resKeyFiled) ||
                string.IsNullOrEmpty(resValueField))
                return null;

            var tablePrefix = tableName;

            List<string> keys = new List<string>();

            foreach (var temp in entities)
            {
                string tempKey;
                try
                {
                    tempKey = temp.GetType().GetProperty(resKeyFiled).GetValue(temp).ToString();

                    keys.Add(string.Format("{0}_{1}", tablePrefix, tempKey));
                }
                catch (Exception ex)
                {

                }
            }

            var dict = I18nHelper.GetValues(ServiceContext.Language, I18nResourceType.DB, keys.ToArray());

            foreach (var temp in entities)
            {
                var key = string.Format("{0}_{1}", tablePrefix, temp.GetType().GetProperty(resKeyFiled).GetValue(temp).ToString());

                if (dict.ContainsKey(key))
                {
                    temp.GetType().GetProperty(resValueField).SetValue(temp, dict[key]);
                }

            }
            var iterator = entities.GetEnumerator();

            List<T> list = new List<T>();

            while (iterator.MoveNext())
            {
                list.Add(iterator.Current);
            }

            return list;
        }



     
    }
}