using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrairieKingSkill
{
    interface ISkill
    {
        int Index { get; }
        ProfessionTree Right { get; }
        ProfessionTree Left { get; }
        int getSkillLevel();
    }
}
