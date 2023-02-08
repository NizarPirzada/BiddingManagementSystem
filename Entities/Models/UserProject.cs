using SQLite;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PanacealogicsSales.Entities.Models
{
    public class UserProject
    {
        [Key]
        public int user_project_id { get; set; }

        public virtual int user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual User userID { get; set; }

        [Unique]
        public Guid project_id { get; set; }
  
        public DateTime? date { get; set; }
        public bool is_default_submission { get; set; }
        public bool is_processed { get; set; }
        public int? assign_user_id { get; set; }
    }

    public class ProjectCountDTO
    {
        public int? TotalProject { get; set; }
        public int? MyProject { get; set; }
        public int? MissedProject { get; set; }
        public int? ProposedProject { get; set; }
        public int? userId { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }

      

    }
}
