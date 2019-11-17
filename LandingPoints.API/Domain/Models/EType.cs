using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPoints.API.Domain
{
    public enum EType : byte
    {
        [Description("Hospital")]
        Hospital = 1,

        [Description("Nursery")]
        Nursery = 2

    }
}
