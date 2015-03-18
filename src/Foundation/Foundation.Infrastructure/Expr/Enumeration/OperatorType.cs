/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: OperatorType.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Operator type enum for formula
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Expr
{
    /// <summary>
    /// Indicate the type of an operator
    /// </summary>
    public enum OperatorType
    {
        /// <summary>
        /// Plus
        /// </summary>
        Plus,
        /// <summary>
        /// Minus
        /// </summary>
        Minus,
        /// <summary>
        /// Multiply
        /// </summary>
        Multiply,
        /// <summary>
        /// Divide
        /// </summary>
        Divide
    }
}