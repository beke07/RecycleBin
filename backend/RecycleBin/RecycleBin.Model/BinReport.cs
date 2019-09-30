using System;
using System.Collections.Generic;
using System.Text;

namespace RecycleBin.Model
{
    public class BinReport : EntityBase
    {
        public Bin Bin { get; set; }

        public string Comment { get; set; }
    }
}
