using System.Data;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace ReplaceStringKeysByDataTable
{
    class Program
    {
        readonly static DataTable dt = new DataTable();
        static List<string> pQuery = new List<string>();
        static List<string> pHtml = new List<string>();

        static void Main(string[] args)
        {
            inicialize();
            pQuery = paramsQuery();
            pHtml = paramsHtml();

            var isMatch = match();

            if (isMatch)
            {
                runReplace();
            }
        }

        static bool match()
        {
            if (pHtml.Count != pQuery.Count)
                return false;

            var t = pHtml.Except(pQuery).ToList();
            if (t.Count > 0)
                return false;

            return true;
        }

        static void runReplace()
        {
            string htmlReplaced = html;
            foreach (var pQ in pQuery)
            {
                htmlReplaced = htmlReplaced.Replace(pQ, dt.Rows[0][pQ].ToString());
            }
        }

        static List<string> paramsQuery()
        {
            return dt.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToList();
        }

        static List<string> paramsHtml()
        {
            Regex regex = new Regex("{(.+?)}");
            var v = regex.Matches(html);

            var u = v.Select(value => value.Value.Replace("{", "").Replace("}", "")).ToList();
            return u;
        }

        static void inicialize()
        {
            dt.Columns.Add("nome");
            dt.Columns.Add("email");
            dt.Columns.Add("url");
            dt.Columns.Add("teste");
        }

        readonly static string html = @"
            <html>
            <head></head>
            <body>
                <div>
                    <span>{nome}</span>
                    <span>{email}</span>
                    <span>{url}</span>
                    <span>{teste1}</span>
    
                </div>

            </body>
            </html>            
        ";
    }
}
