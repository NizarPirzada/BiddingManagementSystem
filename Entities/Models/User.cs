using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PanacealogicsSales.Entities.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [MaxLength(255), Required]
        public string username { get; set; }
        [MaxLength(255), Required]
        public string password { get; set; }
        [MaxLength(500)]
        public string desc { get; set; }
        public bool is_active { get; set; }
        public string social_id { get; set; }
        public DateTime? date { get; set; }
        public virtual int role_id { get; set; }

        [ForeignKey("role_id")]
        public virtual Role roleID { get; set; }
    }
}
