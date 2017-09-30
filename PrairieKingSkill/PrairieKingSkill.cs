using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrairieKingSkill
{
    class PrairieKingSkill : ISkill<PrairieKingProfession>
    {
        private string name = "PrairieKing";
        public string Name { get { return name; } }
        private string description = "PrairieKing skill to be good at things.";
        public string Description { get { return description; } }

        //0-4 are base skills, 5 = Luck, 6 = Cooking
        private int index = 7;
        public int Index { get { return index; } }

        private PrairieKingProfession left;
        public PrairieKingProfession Left { get { return left; } }

        private PrairieKingProfession right;
        public PrairieKingProfession Right { get { return right; } }

        private bool allProfessions;

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
            allProfessions = false;
        }

        private static readonly int[] expNeededForLevel = new int[] { 200, 400, 800, 1400, 2400, 3200, 5000, 7000, 10000, 16000 };
        public int getSkillLevel()
        {
            if (allProfessions)
            {
                return 10;
            }

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

        public int getExpForLevel(int level)
        {
            if (level < 10)
            {
                return expNeededForLevel[level];
            }
            else
            {
                return expNeededForLevel[expNeededForLevel.Length - 1];
            }
        }

        private bool level5LevelUp;
        public bool Level5LevelUp { get { return level5LevelUp; } }
        private bool level10LevelUp;
        public bool Level10LevelUp { get { return level10LevelUp; } }
        public int addExp(int amt)
        {
            int maxExp = expNeededForLevel[expNeededForLevel.Length - 1];

            if (allProfessions)
            {
                return maxExp;
            }

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

        public Tuple<IProfession, IProfession> getLevel5Choices()
        {
            return new Tuple<IProfession, IProfession>(left, right);
        }

        public Tuple<IProfession, IProfession> getLevel10Choices()
        {
            return new Tuple<IProfession, IProfession>(level5Profession.Lhs, level5Profession.Rhs);
        }

        private HashSet<IProfession> aquiredProfessions = new HashSet<IProfession>();
        public HashSet<IProfession> AquiredProfessions { get { return aquiredProfessions; } }
        private IProfession level5Profession;
        public IProfession Level5Profession { get { return level5Profession; } }
        private IProfession level10Profession;
        public IProfession Level10Profession { get { return level10Profession; } }
        public void aquireProfession(PrairieKingProfession profession)
        {
            aquiredProfessions.Add(profession);

            if (left.Equals(profession) || right.Equals(profession))
            {
                level5Profession = profession;
                level5LevelUp = false;
            } else
            {
                level10Profession = profession;
                level10LevelUp = false;
            }
        }

        public void setAllProfessions()
        {
            aquiredProfessions.Add(right);
            aquiredProfessions.Add(right.Lhs);
            aquiredProfessions.Add(right.Rhs);
            aquiredProfessions.Add(left);
            aquiredProfessions.Add(left.Lhs);
            aquiredProfessions.Add(left.Rhs);
            level5Profession = right;
            level10Profession = right.Lhs;
            allProfessions = true;
        }
    }
}
