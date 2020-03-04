using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public class ProductData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public Nullable<int> Price { get; set; }
    }
}
