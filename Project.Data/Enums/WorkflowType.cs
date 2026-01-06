using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Data.Enums
{
    public enum WorkflowType
    {
        NormalOrder = 1,
        OrderFromAllowance = 2,
        Return = 3,
        Discard = 4,
        NormalOrderForTrainingPurpose = 5,
        NormalOrder_Weapon = 6,
        OrderFromAllowance_Weapon = 7,
        NormalOrderForTrainingPurpose_Weapon = 8
    }
}
