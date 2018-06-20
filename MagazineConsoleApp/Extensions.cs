using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineConsoleApp
{
    public static class Extensions
    {
        public static bool IsJSON(this string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (JsonReaderException ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: {0},\nStackTrace: {1}", ex.Message, ex.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: {0},\nStackTrace: {1}", ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
