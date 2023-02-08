using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanacealogicsSales.Entities.Models
{
  public class Proposal
    {
        [Key]
        public int proposal_id { get; set; }
        public string generated_proposal { get; set; }
        public string main_proposal { get; set; }
        public DateTime? date { get; set; }
        public DateTime? proposal_date { get; set; }
        public Guid project_id { get; set; }
        public virtual int user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual User userID { get; set; }
        public DateTime? last_updated { get; set; }
    }
}
