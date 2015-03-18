/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: VariableType.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Variable type for formular
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Expr
{
    /// <summary>
    /// Indicate the type of a variable element in formula
    /// </summary>
    public enum VariableType
    {
        /// <summary>
        /// PTag
        /// </summary>
        PTag,
        /// <summary>
        /// VTag
        /// </summary>
        VTag,
        /// <summary>
        /// AdvancedProperty
        /// </summary>
        AdvancedProperty,
    }
}