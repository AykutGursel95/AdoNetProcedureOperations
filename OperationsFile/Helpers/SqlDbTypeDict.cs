using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceGurseller.Helpers
{

    public static class SqlDbTypeDict
    {
        public static SqlDbType GetType(object TypeName)
        {
            switch ((string)TypeName)
            {
                case "String":
                    return SqlDbType.NVarChar;
                case "Int32":
                    return SqlDbType.Int;
                case "DateTime":
                    return SqlDbType.DateTime;
                case "Float":
                    return SqlDbType.Float;

                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
        }
    }
}
