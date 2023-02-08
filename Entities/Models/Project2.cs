using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace PanacealogicsSales.Entities.Models
{
    public class Project2
    {
       
        public Guid project_id { get; set; }
        public string name { get; set; }
        public DateTime? date { get; set; }
        [AllowNull]
        public string proposal { get; set; }
        public string external_project_id { get; set; }
        public bool is_active { get; set; }
    }
}
