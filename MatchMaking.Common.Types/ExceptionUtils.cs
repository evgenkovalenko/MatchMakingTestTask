using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaking.Common.Types
{
    public static class ExceptionExtensions
    {

        public static string GetFullMessage(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            GetFullExceptionMessage(ex, ref sb, true);
            return sb.ToString();
        }

        public static string GetExceptionMessage(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            GetFullExceptionMessage(ex, ref sb, false);
            return sb.ToString();
        }

        private static void GetFullExceptionMessage(Exception ex, ref StringBuilder sb, bool verbose)
        {
            if (ex is AggregateException)
            {
                foreach (Exception innerEx in ((AggregateException)ex).InnerExceptions)
                {
                    GetFullExceptionMessage(innerEx, ref sb, verbose);
                }
            }
            else
            {
                ////delimeter
                if (sb.Length > 0)
                {
                    sb.Append("; ");
                }

                ////message
                sb.Append(ex.Message);

                if (verbose)
                {
                    ////type
                    Type type = ex.GetType();
                    if (type != null)
                    {
                        sb.Append(" [");
                        sb.Append(type.ToString());
                        sb.Append("]");
                    }

                    ////stacktrace
                    if (!string.IsNullOrEmpty(ex.StackTrace))
                    {
                        sb.Append(" {");
                        sb.Append(ex.StackTrace);
                        sb.Append("}");
                    }

                    ////data
                    if (ex.Data != null && ex.Data.Count > 0)
                    {
                        sb.Append(" (");
                        int i = 0;
                        foreach (DictionaryEntry d in ex.Data)
                        {
                            if (i++ > 0)
                            {
                                sb.Append("|");
                            }

                            sb.Append(string.Format("key={0} value={1}",
                                d.Key.ToString(), d.Value.ToString()));
                        }

                        sb.Append(")");
                    }

                }

                ////recursive call for next inner exception
                if (ex.InnerException != null)
                {
                    GetFullExceptionMessage(ex.InnerException, ref sb, verbose);
                }
            }
        }
    }
}
