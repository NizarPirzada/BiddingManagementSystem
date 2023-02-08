using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanacealogicsSales.Entities.Models
{
   public  class UserShift
    {
        [Key]
        public int user_shift_id { get; set; }

        public virtual int user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual User userID { get; set; }
        public DateTime? date { get; set; }
        public TimeSpan shift_start { get; set; }
        public TimeSpan shift_end { get; set; }
        public DateTime? last_updated { get; set; }
        public bool state { get; set; }
    }
}
