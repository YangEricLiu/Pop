/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: Variable.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Variable for formular
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Expr
{
    /// <summary>
    /// Represent an element of variable type in formula.
    /// </summary>
    public class Variable : Element
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// VariableType
        /// </summary>
        public VariableType VariableType { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        protected string Symbol { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Variable()
        {
            ElementType = ElementType.Variable;
        }
        
        protected long IdFromVarString(string idVarString)
        {
            var idstr = idVarString.Substring(2 + Symbol.Length, idVarString.Length - Symbol.Length - 3);
            return long.Parse(idstr);
        }
    }

    /// <summary>
    /// Represent an element of physical tag in formula.
    /// </summary>
    public class PTagVariable : Variable
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SYMBOL = "ptag";
        /// <summary>
        /// 
        /// </summary>
        public const string INVALID = "{Invalid_PTag}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CodeToVarString(string code)
        {
            return "{" + SYMBOL + SPLITOR + code + "}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string IdToVarString(long id)
        {
            return "{" + SYMBOL + SPLITOR + id + "}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="varString"></param>
        /// <returns></returns>
        public static PTagVariable FromIdVarString(string varString)
        {
            var @var = new PTagVariable();
            @var.Id = @var.IdFromVarString(varString);
            return @var;
        }
        /// <summary>
        /// 
        /// </summary>
        public PTagVariable()
        {
            Symbol = SYMBOL;
            VariableType = VariableType.PTag;
        }

        public override string ToString()
        {
            return IdToVarString(Id);
        }
    }

    /// <summary>
    /// Represent an element of virtual tag in formula.
    /// </summary>
    public class VTagVariable : Variable
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SYMBOL = "vtag";
        /// <summary>
        /// 
        /// </summary>
        public const string INVALID = "{Invalid_VTag}";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CodeToVarString(string code)
        {
            return "{" + SYMBOL + SPLITOR + code + "}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string IdToVarString(long id)
        {
            return "{" + SYMBOL + SPLITOR + id + "}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="varString"></param>
        /// <returns></returns>
        public static VTagVariable FromIdVarString(string varString)
        {
            var @var = new VTagVariable();
            @var.Id = @var.IdFromVarString(varString);
            return @var;
        }
        /// <summary>
        /// 
        /// </summary>
        public VTagVariable()
        {
            VariableType = VariableType.VTag; 
        }

        public override string ToString()
        {
            return IdToVarString(Id);
        }
    }

    /// <summary>
    /// Represent an element of dynamic property in formula.
    /// </summary>
    public class AdvancedPropertyVariable : Variable
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SYMBOL = "prop";

        /// <summary>
        /// 
        /// </summary>
        public const string INVALID = "{Invalid_Prop}";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchyCode"></param>
        /// <param name="propCode"></param>
        /// <returns></returns>
        public static string CodeToVarString(string hierarchyCode, string propCode)
        {
            return "{" + hierarchyCode + SPLITOR + SYMBOL + SPLITOR + propCode + "}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string IdToVarString(long id)
        {
            return "{" + SYMBOL + SPLITOR + id + "}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="varString"></param>
        /// <returns></returns>
        public static AdvancedPropertyVariable FromIdVarString(string varString)
        {
            var @var = new AdvancedPropertyVariable();
            @var.Id = @var.IdFromVarString(varString);
            return @var;
        }
        /// <summary>
        /// 
        /// </summary>
        public AdvancedPropertyVariable()
        {
            VariableType = VariableType.AdvancedProperty;
        }
        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return IdToVarString(Id);
        }
    }
}