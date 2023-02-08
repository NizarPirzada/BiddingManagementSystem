using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanacealogicsSales.Entities.Models
{
    [Table("product")]
    public class Product
    {
        [Key]
        public int product_id { get; set; }
        public string name { get; set; }

        public DateTime date { get; set; }
        public string amount { get; set; }
        public string desc { get; set; }

    }
}
