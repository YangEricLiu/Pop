/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: Constant.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Constant for formular
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using System;

namespace SE.DSP.Foundation.Infrastructure.Expr
{
    /// <summary>
    /// Represent an element of constant type in formula.
    /// </summary>
    public class Constant : Element
    {
        public Constant()
        {
            this.ElementType = ElementType.Constant;
        }

        public double Value { get; set; }

        public static Constant operator +(Constant a, Constant b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Operand is null.");

            Constant retConstant = new Constant
            {
                Value = a.Value + b.Value
            };

            return retConstant;
        }

        public static Constant operator -(Constant a, Constant b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Operand is null.");

            Constant retConstant = new Constant
            {
                Value = a.Value - b.Value
            };

            return retConstant;
        }

        public static Constant operator *(Constant a, Constant b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Operand is null.");

            Constant retConstant = new Constant
            {
                Value = a.Value * b.Value
            };

            return retConstant;
        }

        public static Constant operator /(Constant a, Constant b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Operand is null.");

            Constant retConstant = new Constant
            {
                Value = a.Value / b.Value
            };

            return retConstant;
        }
    }
}
