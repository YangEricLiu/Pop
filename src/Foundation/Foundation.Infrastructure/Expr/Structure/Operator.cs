/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: Operator.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Operator for formular
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Expr
{
    /// <summary>
    /// Represent an element of operator type in formula.
    /// </summary>
    public class Operator : Element
    {
        public Operator()
        {
            this.ElementType = ElementType.Operator;
        }
        /// <summary>
        /// 
        /// </summary>
        public OperatorType OperatorType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public new string Value { get; set; }
    }
}
