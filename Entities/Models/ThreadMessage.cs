using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanacealogicsSales.Entities.Models
{
   public class ThreadMessage
    {
        [Key]
        public int thread_message_id { get; set; }
        public virtual int thread_id { get; set; }
        [ForeignKey("thread_id")]
        public virtual Thread ThreadID { get; set; }
        public string message { get; set; }
        public bool is_client_message { get; set; }

        public DateTime? created_date { get; set; }
      
        public bool is_processed { get; set; }
        public bool is_read { get; set; }
        public virtual Guid project_id { get; set; }
        [ForeignKey("project_id")]
        public virtual Project ProjectID { get; set; }
    }

    public class ThreadMessageDTO
    {
      
        public int thread_message_id { get; set; }
        public  int thread_id { get; set; }
      
        public string message { get; set; }
        public bool is_client_message { get; set; }

        public DateTime? created_date { get; set; }

        public bool is_processed { get; set; }
        public bool is_read { get; set; }
        public Guid? project_id { get; set; }
        public string project_name { get; set; }
        public string receiver_id { get; set; }
        public int user_2 { get; set; }
    }
}
