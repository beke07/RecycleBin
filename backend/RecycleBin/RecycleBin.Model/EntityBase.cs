using System;
using System.Collections.Generic;
using System.Text;

namespace RecycleBin.Model
{
    public class EntityBase
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public ApplicationUser CreatedBy { get; set; }
    }
}
