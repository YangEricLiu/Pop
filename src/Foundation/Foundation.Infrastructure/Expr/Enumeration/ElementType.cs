/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: ElementType.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Element type enum for formula
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Expr
{
    /// <summary>
    /// Indicate the type of a queue element.
    /// </summary>
    public enum ElementType
    {
        /// <summary>
        /// Operator
        /// </summary>
        Operator,
        /// <summary>
        /// Constant
        /// </summary>
        Constant,
        /// <summary>
        /// Variable
        /// </summary>
        Variable,
    }
}