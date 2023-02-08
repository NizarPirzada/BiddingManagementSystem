using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanacealogicsSales.Entities.Models
{
  public class ProjectSkill
    {
        [Key]
        public int project_skill_id { get; set; }
        public virtual Guid project_id { get; set; }

        [ForeignKey("project_id")]
        public virtual Project ProjectID { get; set; }
        public int skill_id { get; set; }
        [ForeignKey("skill_id")]
        public virtual Skill SkillID { get; set; }
    }
}
