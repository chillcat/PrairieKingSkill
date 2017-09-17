using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrairieKingSkill
{
    interface IProfession
    {
        string Name { get; }
        int Id { get; }
        string Description { get; }
        IProfession Parent { get; set; }
        IProfession Lhs { get; set; }
        IProfession Rhs { get; set; }
    }
}
