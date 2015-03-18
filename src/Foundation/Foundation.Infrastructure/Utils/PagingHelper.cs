using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// modified by mike 20140321 add property WithQuery into Paging Class 
    /// </summary>
    public static class PagingHelper
    {
        public static String GeneratePagingSql(Paging paging)
        {
            if (paging == null || string.IsNullOrEmpty(paging.Columns) || string.IsNullOrEmpty(paging.Table) 
                || string.IsNullOrEmpty(paging.OrderBy) || paging.Limit == 0)
            {
                throw new Exception("Invalid parameter");
            }

            if (string.IsNullOrEmpty(paging.WithQuery))
            {
                return String.Format(
    @"SELECT * FROM (SELECT {1}, ROW_NUMBER() OVER (ORDER BY {3}) rownum FROM {0} {2}) t WHERE rownum > {4} AND rownum <= {5};
SELECT COUNT(1) FROM {0} {2}",
                        paging.Table,
                        paging.Columns,
                        String.IsNullOrWhiteSpace(paging.Where) ? "" : " WHERE " + paging.Where,
                        paging.OrderBy,
                        paging.Start,
                        paging.Start + paging.Limit);
            }
            else
            {
                return String.Format(
                    @"{6} SELECT * FROM (SELECT {1}, ROW_NUMBER() OVER (ORDER BY {3}) rownum FROM {0} {2}) t WHERE rownum > {4} AND rownum <= {5};
SELECT COUNT(1) FROM {0} {2}",
                         paging.Table,
                         paging.Columns,
                         String.IsNullOrWhiteSpace(paging.Where) ? "" : " WHERE " + paging.Where,
                         paging.OrderBy,

                         paging.Start,
                         paging.Start + paging.Limit,
                         paging.WithQuery);

            }
        }
    }

    /// <summary>
    /// modified by mike 20140321 add property WithQuery
    /// </summary>
    public class Paging
    {
        public string WithQuery { set; get; }
        public String Table { get; set; }
        public String Columns { get; set; }
        public String OrderBy { get; set; }
        public String Where { get; set; }
        public int Start { get; set; }
        public int Limit { get; set; }
    }
}
