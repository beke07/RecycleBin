using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RecycleBin.Model
{
    [Owned]
    public class GeoLocation
    {
        public decimal Lat { get; set; }

        public decimal Long { get; set; }
    }
}
