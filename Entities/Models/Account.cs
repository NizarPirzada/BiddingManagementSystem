using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("account")] 
    public class Account 
    {
        [Key]
        public Guid account_id { get; set; }
        public DateTime created_date { get; set; }

        public string account_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }

    }
}
