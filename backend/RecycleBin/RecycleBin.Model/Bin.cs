using System;
using System.Collections.Generic;
using System.Text;

namespace RecycleBin.Model
{
    public class Bin : EntityBase
    {
        public GeoLocation Location { get; set; }
    }
}
