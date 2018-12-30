using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECommerceGurseller.Helpers
{
    public static class Clear
    {
        public static void textClear(params TextBox[] txts)
        {
            foreach (var txt in txts)
            {
                txt.Clear();
            }
        }

    }
}
