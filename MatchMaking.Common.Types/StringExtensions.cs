using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaking.Common.Types
{
    public static class StringExtensions
    {
        public static string[] SplitByComma(this string content)
        {
            if (content == null)
            {
                return new string[] { };
            }

            return content.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
