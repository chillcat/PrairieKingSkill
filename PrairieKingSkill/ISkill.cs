using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrairieKingSkill
{
    interface ISkill<Profession> where Profession : IProfession
    {
        int Index { get; }
        string Name { get; }
        string Description { get; }
        Profession Right { get; }
        Profession Left { get; }
        int getSkillLevel();
        bool Level5LevelUp { get; }
        Tuple<IProfession, IProfession> getLevel5Choices();
        bool Level10LevelUp { get; }
        Tuple<IProfession, IProfession> getLevel10Choices();
        void aquireProfession(Profession profession);
    }
}
