using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrairieKingSkill
{
    class PrairieKingSkill : ISkill
    {
        private string name = "PrairieKing";
        public string Name { get { return name; } }
        private string description = "PrairieKing skill to be good at things.";
        public string Description { get { return description; } }

        //0-4 are base skills, 5 = Luck, 6 = Cooking
        private int index = 7;
        public int Index { get { return index; } }

        private IProfession left;
        public IProfession Left { get { return left; } }

        private IProfession right;
        public IProfession Right { get { return right; } }

        public PrairieKingSkill(int index)
        {
            this.index = Math.Min(this.index, index);
            Util.extendExpLength(index);
            PrairieKingProfession.Lives.Lhs = PrairieKingProfession.PowerUpDuration;
            PrairieKingProfession.Lives.Rhs = PrairieKingProfession.ShootingDelay;
            PrairieKingProfession.ShootingDelay.Parent = PrairieKingProfession.Lives;
            PrairieKingProfession.PowerUpDuration.Parent = PrairieKingProfession.Lives;
            right = PrairieKingProfession.Lives;
            PrairieKingProfession.Coins.Lhs = PrairieKingProfession.CoinChance;
            PrairieKingProfession.Coins.Rhs = PrairieKingProfession.LootChance;
            PrairieKingProfession.CoinChance.Parent = PrairieKingProfession.Coins;
            PrairieKingProfession.LootChance.Parent = PrairieKingProfession.Coins;
            left = PrairieKingProfession.Coins;
        }

        private static readonly int[] expNeededForLevel = new int[] { 200, 400, 800, 1400, 2400, 3200, 5000, 7000, 10000, 16000 };
        public int getSkillLevel()
        {
            Util.extendExpLength(index);
            for (int i = expNeededForLevel.Length - 1; i >= 0; --i)
            {
                if (Game1.player.experiencePoints[index] >= expNeededForLevel[i])
                {
                    return i + 1;
                }
            }

            return 0;
        }

        private bool level5LevelUp;
        public bool Level5LevelUp { get { return level5LevelUp; } }
        private bool level10LevelUp;
        public bool Level10LevelUp { get { return level10LevelUp; } }
        public int addExp(int amt)
        {
            int maxExp = expNeededForLevel[expNeededForLevel.Length - 1];
            if (amt > 0 && !(Game1.player.experiencePoints[index] == maxExp))
            {
                int oldLevel = getSkillLevel();
                int totalExp = Game1.player.experiencePoints[index] + amt;
                Game1.player.experiencePoints[index] = totalExp > maxExp ? maxExp : totalExp;

                int newLevel = getSkillLevel();
                if (oldLevel < 5 && newLevel >= 5)
                {
                    level5LevelUp = true;
                }
                if (oldLevel < 10 && newLevel >= 10)
                {
                    level10LevelUp = true;
                }
            }
            return getSkillLevel();
        }

        public List<IProfession> getLevel5Choices()
        {
            return new List<IProfession>() { left, right };
        }

        public List<IProfession> getLevel10Choices()
        {
            return new List<IProfession>() { level5Profession.Lhs, level5Profession.Rhs };
        }

        private HashSet<IProfession> aquiredProfessions = new HashSet<IProfession>();
        public HashSet<IProfession> AquiredProfessions { get { return aquiredProfessions; } }
        private IProfession level5Profession;
        public IProfession Level5Profession { get { return level5Profession; } }
        private IProfession level10Profession;
        public IProfession Level10Profession { get { return level10Profession; } }
        public void chooseLevel5Profession(IProfession profession)
        {
            if (left.Equals(profession) || right.Equals(profession))
            {
                aquiredProfessions.Add(profession);
                level5Profession = profession;
            } else
            {
                throw new Exception(profession + " is not a valid level 5 profession.");
            }
        }

        public void chooseLevel10Profession(IProfession profession)
        {
            if (level5Profession.Lhs.Equals(profession) || level5Profession.Rhs.Equals(profession))
            {
                aquiredProfessions.Add(profession);
                level10Profession = profession;
            }
            else
            {
                throw new Exception(profession + " is not a valid level 10 profession for " + level5Profession);
            }
        }

        private IProfession getChosenTree(IProfession profession)
        {
            return left.Equals(profession) ? left : right;
        }
    }
}
