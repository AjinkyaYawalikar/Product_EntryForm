using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Product_Entry_Demo.Library
{
    public class Constants
    {
        public enum ColorList
        {
            [Description("RED")]
            Red = 1,
            [Description("PINK")]
            Pink = 2,
            [Description("GREEN")]
            Green = 3,
            [Description("YELLOW")]
            Yellow = 4,
            [Description("BLACK")]
            Black = 5
        }
    }
}