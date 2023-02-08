using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanacealogicsSales.Entities.Models
{
    public class ThreadHistory
    {
        [Key]
        public int thread_history_id { get; set; }
        public virtual int thread_id { get; set; }
        [ForeignKey("thread_id")]
        public virtual Thread ThreadID { get; set; }
        public int previous_user { get; set; }

        public DateTime? created_date { get; set; }
    }
}
