using System;
using System.Collections.Generic;
using System.Text;

namespace PanacealogicsSales.Entities.Models
{
  public  class RequestMessage
    {
        public string Message { get; set; }
        public string senderUserId { get; set; }
        public string reciverUserId { get; set; }
    }
}
