using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Data.Enums
{
    public enum WorkflowType
    {
        NoramlOrder = 1,
        OrderFromAllowance = 2,
        Return = 3,
        Discard = 4,
        NoramlOrderForTrainingPurpose = 5,
        NoramlOrder_Weapon = 1,
        OrderFromAllowance_Weapon = 2,
        NoramlOrderForTrainingPurpose_Weapon = 5
    }
}
