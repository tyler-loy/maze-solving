using System;
using System.Collections.Generic;
using System.Text;

namespace mazesolvinglib.Extentions
{
    public static class ColorExtentions
    {
        public static string GetHexCode(this System.Drawing.Color c)
        {
            var str = "#" + c.A.ToString("X2") + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
            return str.ToUpper();
        }
    }
}
