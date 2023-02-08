using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents;
using SQLite;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;
using Index = Microsoft.Azure.Documents.Index;
using MaxLengthAttribute = System.ComponentModel.DataAnnotations.MaxLengthAttribute;

namespace PanacealogicsSales.Entities.Models
{
   
    public class Project
    {
        [Key]
        
        public Guid project_id { get; set; }
        [MaxLength(255), Required]
        public string name { get; set; }
        [MaxLength(255), Required]
        public string skills { get; set; }
        [MaxLength(5000)]
        public string desc { get; set; }
        public DateTime? date { get; set; }
        public bool? is_deleted { get; set; }
        public virtual int user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual User userID { get; set; }
        public string project_type { get; set; }
        [MaxLength(50),AllowNull]
        public string status { get; set; }
        public bool is_acitve { get; set; }
        public string project_time { get; set; }
        public string budget { get; set; }
        [MaxLength(100)]
        public string client_country { get; set; }
        public int? client_reputation { get; set; }
        public int? project_value { get; set; }
        [MaxLength(255)]
        public string external_project_id { get; set; }
    }


    public class ProjectDTO
    {
        public Guid project_id { get; set; }
        public string name { get; set; }
        public string generated_proposal { get; set; }
        public DateTime? date { get; set; }
        public string username { get; set; }
        public string desc { get; set; }
        public string project_time { get; set; }
        public string budget { get; set; }
        public DateTime? proposal_date { get; set; }
        public string client_country { get; set; }
        public int? client_reputation { get; set; }
        public int? project_value { get; set; }
        public string project_type { get; set; }
        public int? user_project_id { get; set; }
        public string external_project_id { get; set; }
        public bool? is_deleted { get; set; }
        public int? user_id { get; set; }
        public bool is_processed { get; set; }
        public string main_proposal { get; set; }
        public string skills { get; set; }
        public string status { get; set; }
      
    }
    public class ProjectObjDTO
    {
        [MaxLength(255), Required]
        public string name { get; set; }
       
        public string generated_proposal { get; set; }
        [MaxLength(1000), Required]
        public string skills { get; set; }
     
        public string desc { get; set; }
        [MaxLength(255), Required]
        public string external_project_id { get; set; }
      
        public string client_country { get; set; }
        [MaxLength(255), Required]
        public string project_type { get; set; }
        [MaxLength(255), Required]
        public string project_time { get; set; }
        public int? project_value { get; set; }
        public int? client_reputation { get; set; }
        public string budget { get; set; }
    }
  }
