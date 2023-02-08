using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PanacealogicsSales.Entities.Models
{
   public class Skill
    {
        [Key]
        public int skill_id { get; set; }
        public string name { get; set; }
    }
}
