using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Data.Enums
{
    public enum RequestStatus
    {
        New = 1,
        UnderProcess = 2,
        Approved = 3,
        Rejected = 4,
        Cancelled = 5,
        ReturnedForReview = 6
    }
}

