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
        private int index;
        public int Index { get { return index; } }

        private ProfessionTree left = ProfessionTree.builder()
            .Root(PrairieKingProfession.Lives)
            .Right(PrairieKingProfession.PowerUpDuration)
            .Left(PrairieKingProfession.ShootingDelay)
            .build();
        public ProfessionTree Left { get { return left; } }

        private ProfessionTree right = ProfessionTree.builder()
            .Root(PrairieKingProfession.Coins)
            .Right(PrairieKingProfession.CoinChance)
            .Left(PrairieKingProfession.LootChance)
            .build();
        public ProfessionTree Right { get { return right; } }

        public PrairieKingSkill(int index)
        {
            this.index = index;
            Util.extendExpLength(index);
        }

        public static readonly int[] expNeededForLevel = new int[] { 1, 2, 3, 1300, 2150, 3300, 4800, 6900, 10000, 15000 };
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

        public List<Profession> getLevel5Choices()
        {
            return new List<Profession>() { left.Root, right.Root };
        }

        public List<Profession> getLevel10Choices()
        {
            ProfessionTree chosenTree = getChosenTree(level5Profession);
            return new List<Profession>() { chosenTree.Root, chosenTree.Root };
        }

        private List<Profession> aquiredProfessions = new List<Profession>();
        public List<Profession> AquiredProfessions { get { return aquiredProfessions; } }
        private Profession level5Profession;
        public Profession Level5Profession { get { return level5Profession; } }
        private Profession level10Profession;
        public Profession Level10Profession { get { return level10Profession; } }
        public void chooseLevel5Profession(Profession profession)
        {
            if (left.Root.Equals(profession) || right.Root.Equals(profession))
            {
                aquiredProfessions.Add(profession);
                level5Profession = profession;
            } else
            {
                throw new Exception(profession + " is not a valid level 5 profession.");
            }
        }

        public void chooseLevel10Profession(Profession profession)
        {
            ProfessionTree chosenTree = getChosenTree(level5Profession);
            if (chosenTree.Left.Equals(profession) || chosenTree.Right.Equals(profession))
            {
                aquiredProfessions.Add(profession);
                level10Profession = profession;
            }
            else
            {
                throw new Exception(profession + " is not a valid level 10 profession for " + level5Profession);
            }
        }

        private ProfessionTree getChosenTree(Profession profession)
        {
            return left.Root.Equals(profession) ? left : right;
        }
    }
}
