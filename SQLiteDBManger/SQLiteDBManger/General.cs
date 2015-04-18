using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDBManger
{
    public static class General
    {
        public static bool IsValidKeyValuePair(object[] tovalidate)
        {
            if (tovalidate.Length == 0) return false;

            if (tovalidate.Length % 2 == 0) return true;
            
            return false;
        }
    }
}
