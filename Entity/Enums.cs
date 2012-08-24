using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottyApps.ScottyBlogging.Entity
{
    public enum RegisterStatus
    {
        OK = 0,
        UserExisting
    }

    public enum UserValidationStatus
    {
        OK = 0,
        UserNotExist,
        WrongPassword
    }
}
