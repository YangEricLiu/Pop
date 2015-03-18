/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: FormulaUtil.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Formula utility
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;

namespace SE.DSP.Foundation.Infrastructure.Expr
{
    using SE.DSP.Foundation.Infrastructure.Constant;
    using System.Collections;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Util class for Formula
    /// </summary>
    public static class FormulaUtil
    {
        #region Const
        //public const string CODEREGEX = @"^[0-9a-zA-Z_+.\-_~#&][0-9a-zA-Z_+.\-_~#& ]*$";
        private static string PATTERN_SPLITOR = @"\" + Element.SPLITOR;
        private static string PATTERN_CODE = ConstantValue.CODEREGEX.TrimStart('^').TrimEnd('$'); // @"[0-9a-zA-Z_+.\-_~#&][0-9a-zA-Z_+.\-_~#& ]*";
        private const string PATTERN_IDENTITY = @"\d{1,19}?";
        private const string PATTERN_CONSTANT = @"\d+({1}\d+)?";
        private const string PATTERN_OPERATOR = @"([+\-*/%()]{1})";

        private static string PATTERN_PTAG_RAW = string.Format(@"({{ptag{1}{0}}})", PATTERN_CODE, PATTERN_SPLITOR);
        private static string PATTERN_PTAG_DB = string.Format(@"({{ptag{1}{0}}})", PATTERN_IDENTITY, PATTERN_SPLITOR);
        private static string PATTERN_VTAG_RAW = string.Format(@"({{vtag{1}{0}}})", PATTERN_CODE, PATTERN_SPLITOR);
        private static string PATTERN_VTAG_DB = string.Format(@"({{vtag{1}{0}}})", PATTERN_IDENTITY, PATTERN_SPLITOR);

        private static string PATTERN_TAG_RAW = string.Format("({0})", string.Join("|", new[] { PATTERN_PTAG_RAW, PATTERN_VTAG_RAW, }));
        private static string PATTERN_TAG_DB = string.Format("({0})", string.Join("|", new[] { PATTERN_PTAG_DB, PATTERN_VTAG_DB, }));

        private static string PATTERN_DPROPERTY_RAW = string.Format(@"({{{0}{1}prop{1}{0}}})", PATTERN_CODE, PATTERN_SPLITOR);
        private static string PATTERN_DPROPERTY_DB = string.Format(@"({{prop{1}{0}}})", PATTERN_IDENTITY, PATTERN_SPLITOR);
        //private const string PATTERN_FACTOR_RAW = @"({})";
        //private static string PATTERN_FACTOR_DB = string.Format(@"({{comm{1}{0},uom{1}{0}}})", PATTERN_IDENTITY);

        private static string PATTERN_VARIABLE_RAW = string.Format("({0})", string.Join("|", new[] { PATTERN_PTAG_RAW, PATTERN_VTAG_RAW, PATTERN_DPROPERTY_RAW }));
        private static string PATTERN_VARIABLE_DB = string.Format("({0})", string.Join("|", new[] { PATTERN_PTAG_DB, PATTERN_VTAG_DB, PATTERN_DPROPERTY_DB }));
        private static string PATTERN_CONSTANT_VARIABLE_BLANK_CONCAT = string.Format(@"(({0})|\d)\s*({0})|({0}\s*\d)|(\d\s+\d)", PATTERN_VARIABLE_RAW);

        //const string PATTERN_VARIABLE_RAW_PART = @"\{[0-9a-zA-Z_]+{1}[vp]tag{1}[0-9a-zA-Z_]+}";
        //const string PATTERN_VARIABLE_RPN_PART = @"\{[vp]tag{1}\d+\}";
        //const string PATTERN_CONSTANT_VARIABLE_BLANK_CONCAT = @"(({[0-9a-zA-Z_]+{1}[vp]tag{1}[0-9a-zA-Z_]+})|\d)\s*({[0-9a-zA-Z_]+{1}[vp]tag{1}[0-9a-zA-Z_]+})|({[0-9a-zA-Z_]+{1}[vp]tag{1}[0-9a-zA-Z_]+}\s*\d)|(\d\s+\d)";

        //const string PATTERN_OPERATOR_RPN_Full = @"^([+\-*/%()]{1})$";
        //const string PATTERN_VARIABLE_RPN_Full = @"^\{[vp]tag{1}\d+\}$";
        //const string PATTERN_CONSTANT_RPN_Full = @"^\d+({1}\d+)?$";

        const string PATTERN_FORMULA = @"^(((?<o>\()[-+]?([0-9]+(.[0-9]+)?[-+*/])*)+[0-9]+(.[0-9]+)?((?<-o>\))([-+*/][0-9]+(.[0-9]+)?)*)+($|[-+*/]))*(?(o)(?!))$";

        readonly static string[] allOperators = { "+", "-", "*", "/", "(", ")" };

        #endregion

        /// <summary>
        /// Convert from raw formula string to RPN formula string
        /// </summary>
        /// <param name="rawformula"></param>
        /// <returns></returns>
        public static String RawString2RpnString(String rawformula)
        {
            rawformula = rawformula.Replace(" ", "");

            var strArr = Regex.Split(rawformula, PATTERN_OPERATOR);

            var queueOperand = new Queue<String>();
            var stackOperator = new Stack<String>();

            bool isPreviousLeftBrace = false;
            foreach (var token in strArr)
            {
                if (String.IsNullOrEmpty(token)) continue;

                if (!Regex.IsMatch(token, WrapPattern(PATTERN_OPERATOR)))
                {
                    queueOperand.Enqueue(token);
                    isPreviousLeftBrace = false;
                    continue;
                }
                if (token == "(")
                {
                    stackOperator.Push(token);
                    isPreviousLeftBrace = true;
                }
                else if (token == ")")
                {
                    string top;
                    while ((top = stackOperator.Pop()) != "(")
                    {
                        queueOperand.Enqueue(top);
                    }
                    isPreviousLeftBrace = false;
                }
                else
                {
                    if (queueOperand.Count == 0)
                    {
                        queueOperand.Enqueue("0");
                    }
                    if (stackOperator.Count == 0 || IsHigherPrecOperator(token, stackOperator.Peek()))
                    {
                        stackOperator.Push(token);
                    }
                    else if (stackOperator.Peek() == "(")
                    {
                        if (isPreviousLeftBrace) queueOperand.Enqueue("0");
                        stackOperator.Push(token);
                    }
                    else
                    {
                        while (stackOperator.Count != 0)
                        {
                            if (IsHigherPrecOperator(token, stackOperator.Peek())) break;

                            var top = stackOperator.Peek();
                            if (top == "(") break;

                            top = stackOperator.Pop();
                            queueOperand.Enqueue(top);
                        }
                        stackOperator.Push(token);
                    }
                    isPreviousLeftBrace = false;
                }
            }
            while (stackOperator.Count != 0)
            {
                var szTop = stackOperator.Pop();
                queueOperand.Enqueue(szTop);
            }
            return String.Join(";", queueOperand);
        }

        /// <summary>
        /// Convert RPN string to RPN queue
        /// </summary>
        /// <param name="rpnformula"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        public static Queue<Element> RpnString2RpnQueue(String rpnformula, out bool isValid)
        {
            var strArr = rpnformula.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var queue = new Queue<Element>();
            isValid = true;
            foreach (var str in strArr)
            {
                queue.Enqueue(CreateRpnQueueElement(str, out isValid));
                if (!isValid) break;
            }
            return queue;
        }

        /// <summary>
        /// Validate if the code-styled formula is a valid formula
        /// </summary>
        /// <param name="rawformula"></param>
        /// <returns></returns>
        public static bool Validate(string rawformula)
        {
            if (String.IsNullOrWhiteSpace(rawformula)) return false;
            if (!Regex.IsMatch(rawformula, PATTERN_TAG_RAW)) return false;
            if (Regex.IsMatch(rawformula, PATTERN_CONSTANT_VARIABLE_BLANK_CONCAT)) return false;
            var formulaWithoutVar = Regex.Replace(rawformula, PATTERN_VARIABLE_RAW, "1");
            return Regex.IsMatch("(" + formulaWithoutVar.Replace(" ", "") + ")", PATTERN_FORMULA);
        }

        public static String[] GetVarStringsFromRawString(String rawformula)
        {
            var coll = Regex.Matches(rawformula, PATTERN_VARIABLE_RAW);
            return (from Match match in coll select match.Captures[0].Value).Distinct().ToArray();
        }

        public static String[] GetVarStringsFromRpnString(String rpnformula)
        {
            var coll = Regex.Matches(rpnformula, PATTERN_VARIABLE_DB);
            return (from Match match in coll select match.Captures[0].Value).Distinct().ToArray();
        }

        public static Variable[] GetVarsFromRpnString(String rpnformula)
        {
            var coll = Regex.Matches(rpnformula, PATTERN_VARIABLE_DB);
            var variables = new List<Variable>(coll.Count);

            foreach (Match varStr in coll)
            {
                Variable variable = null;
                if (varStr.Value.IndexOf(PTagVariable.SYMBOL, StringComparison.Ordinal) > 0)
                {
                    long identityId = long.Parse(varStr.Value.Substring(6, varStr.Length - 7));
                    variable = new PTagVariable { Id = identityId };
                }
                else if (varStr.Value.IndexOf(VTagVariable.SYMBOL, StringComparison.Ordinal) > 0)
                {
                    long identityId = long.Parse(varStr.Value.Substring(6, varStr.Length - 7));
                    variable = new VTagVariable { Id = identityId };
                }
                else if (varStr.Value.IndexOf(AdvancedPropertyVariable.SYMBOL, StringComparison.Ordinal) > 0)
                {
                    long identityId = long.Parse(varStr.Value.Substring(6, varStr.Length - 7));
                    variable = new AdvancedPropertyVariable
                    {
                        Id = identityId,
                    };
                }
                if (!variables.Contains(variable)) variables.Add(variable);
            }
            return variables.ToArray();
        }

        private static Element CreateRpnQueueElement(String elementStr, out bool isValid)
        {
            Element obj = null;

            if (Regex.IsMatch(elementStr, WrapPattern(PATTERN_OPERATOR)))
            {
                var oprt = new Operator();
                if (elementStr == "+") oprt.OperatorType = OperatorType.Plus;
                if (elementStr == "-") oprt.OperatorType = OperatorType.Minus;
                if (elementStr == "*") oprt.OperatorType = OperatorType.Multiply;
                if (elementStr == "/") oprt.OperatorType = OperatorType.Divide;

                obj = oprt;
            }
            else if (Regex.IsMatch(elementStr, WrapPattern(PATTERN_CONSTANT)))
            {
                var constant = new Constant();

                double elementValDouble = 0;
                if (double.TryParse(elementStr, out elementValDouble))
                {
                    constant.Value = elementValDouble;
                    obj = constant;
                }
                else
                {
                    isValid = false;
                    return null;
                }
            }
            else if (Regex.IsMatch(elementStr, WrapPattern(PATTERN_PTAG_DB)))
            {
                var idstr = elementStr.Substring(6, elementStr.Length - 7);
                obj = new PTagVariable { Id = long.Parse(idstr) };
            }
            else if (Regex.IsMatch(elementStr, WrapPattern(PATTERN_VTAG_DB)))
            {
                var idstr = elementStr.Substring(6, elementStr.Length - 7);
                obj = new VTagVariable { Id = long.Parse(idstr) };
            }
            else if (Regex.IsMatch(elementStr, WrapPattern(PATTERN_DPROPERTY_DB)))
            {
                var idstr = elementStr.Substring(6, elementStr.Length - 7);
                obj = new AdvancedPropertyVariable { Id = long.Parse(idstr) };
            }

            isValid = obj != null;
            return obj;
        }

        private static bool IsHigherPrecOperator(string current, string previous)
        {
            int nCurrIdx;
            int nPrevIdx;
            GetCurrentAndPreviousIndex(allOperators, current, previous, out nCurrIdx, out nPrevIdx);
            return nCurrIdx > nPrevIdx;
        }

        private static void GetCurrentAndPreviousIndex(string[] allOps, string currentOp, string prevOp,
            out int nCurrIdx, out int nPrevIdx)
        {
            nCurrIdx = -1;
            nPrevIdx = -1;
            for (int nIdx = 0; nIdx < allOps.Length; nIdx++)
            {
                if (allOps[nIdx] == currentOp)
                {
                    nCurrIdx = nIdx;
                }
                if (allOps[nIdx] == prevOp)
                {
                    nPrevIdx = nIdx;
                }
                if (nPrevIdx != -1 && nCurrIdx != -1)
                {
                    break;
                }
            }
        }

        private static String WrapPattern(string pattern)
        {
            return string.Format("^{0}$", pattern);
        }
    }
}

