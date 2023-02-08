using System;
using System.Collections.Generic;
using System.Text;

namespace MissionControl.DataTransferObject.Application
{
    public enum StatusType
    {
        Success=200,
        NotFound=404,
        BadRequest=400,
        ServerError=500,
        Conflict = 409,
        Forbidden = 403,
        ValidationError = 422
    }
}
