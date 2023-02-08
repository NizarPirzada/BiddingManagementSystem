using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PanacealogicsSales.Entities.Models
{
   public class Thread
    {  
        [Key]
        public int thread_id { get; set; }
        public string user_1 { get; set; }
        public int user_2 { get; set; }
        public Guid project_id { get; set; }
     
        public DateTime? created_date{ get; set; }
        public bool has_new_messages { get; set; }
        public DateTime? last_updated { get; set; }

    }

    public class ThreadDTO
    {
        public int? thread_id { get; set; }
        public string user_1 { get; set; }
        public int? user_2 { get; set; }
        public Guid? project_id { get; set; }
        public string message { get; set; }
        public DateTime? created_time { get; set; }
        public int user_id { get; set; }
        public string username { get; set; }
        public bool has_new_message { get; set; }
        public string project_name { get; set; }
    }
}
