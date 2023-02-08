using System;
using System.Collections.Generic;
using System.Text;

namespace MissionControl.DataTransferObject.Application
{
    public class Response
    {
        public StatusType StatusCode { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }



}
