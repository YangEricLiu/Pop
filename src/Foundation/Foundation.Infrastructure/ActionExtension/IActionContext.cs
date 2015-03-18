using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.ActionExtension
{
    public interface IActionContext
    {

        /// <summary>
        /// Get Host value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetHostValue(string key);

        /// <summary>
        /// Set or Get action handle status
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string key] { set; get; }


        bool IsInTransaction { set; get; }


        PositionType PositionType { set; get; } 
    }
}
