using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace SE.DSP.Pop.MSSQL.Mapper
{
    public class PopMapper : IMapper
    {
        public TableInfo GetTableInfo(Type pocoType)
        {
            return TableInfo.FromPoco(pocoType);
        }

        public ColumnInfo GetColumnInfo(System.Reflection.PropertyInfo pocoProperty)
        {
            return ColumnInfo.FromProperty(pocoProperty);
        }

        public Func<object, object> GetFromDbConverter(System.Reflection.PropertyInfo targetProperty, Type sourceType)
        {
            if (targetProperty.Name == "Path")
            {
                return db => db.ToString();
            }

            if (targetProperty.Name == "Version")
            {
                return db => db == DBNull.Value ? null : new long?(DbVersion2Long((byte[])db)); 
            }

            return null;
        }

        public Func<object, object> GetToDbConverter(System.Reflection.PropertyInfo sourceProperty)
        {
            throw new NotImplementedException();
        }

        private static long DbVersion2Long(byte[] version)
        {
            var sb = new StringBuilder();

            foreach (var b in version)
            {
                if (b <= 0xF)
                {
                    sb.Append("0");
                }

                sb.Append(b.ToString("X"));
            }

            var result = Convert.ToInt64(sb.ToString(), 16);

            return result;
        }
    }
}
