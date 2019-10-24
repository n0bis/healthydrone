using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPointsAPI.Domain.Models
{
    public enum EType : byte
    {
        [Description("Hospital")]
        Hospital = 1,

        [Description("Nursery")]
        Nursery = 2

    }
}
