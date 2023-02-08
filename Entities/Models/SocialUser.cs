using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace PanacealogicsSales.Entities.Models
{
    [Table("social_user")]
    public class SocialUser
    {
        [Key]
        public int social_user_id { get; set; }
        public string name { get; set; }
        public string social_id { get; set; }
        public virtual int user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual User userID { get; set; }
        public string email { get; set; }
        public string image_url { get; set; }
        [MaxLength(100),AllowNull]
        public string signup_type { get; set; }
    }

    public class respUser
    {
        public int user_id { get; set; }
        public string username { get; set; }
        public int role_id { get; set; }
        public string social_id { get; set; }
      

    }
}
