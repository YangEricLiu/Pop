/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: Element.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Element for formular
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Expr
{
    /// <summary>
    /// Represent an element for a formula rpn queue.
    /// </summary>
    public class Element
    {
        public const char SPLITOR = '|';

        /// <summary>
        /// ElementType
        /// </summary>
        public ElementType ElementType { get; set; }
        //public object Value { get; set; }
    }
}

