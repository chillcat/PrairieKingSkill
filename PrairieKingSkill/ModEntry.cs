using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Linq;

namespace PrairieKingSkill
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private PrairieKingSkill prairieKingSkill;
        private static Texture2D icon;
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {

            if (icon == null)
            {
                try
                {
                    icon = helper.Content.Load<Texture2D>("iconA.png");
                }
                catch (Exception e)
                {
                    this.Monitor.Log("Failed to load icon: " + e);
                    icon = new Texture2D(Game1.graphics.GraphicsDevice, 16, 16);
                    icon.SetData(Enumerable.Range(0, 16 * 16).Select(i => new Color(225, 168, 255)).ToArray());
                }
            }

            prairieKingSkill = new PrairieKingSkill(Util.GetNumOtherSkills(helper));
            GameEvents.UpdateTick += update;
            TimeEvents.AfterDayStarted += dayStarted;
            PrairieKingSkillMenuEntry skillMenuEntry = new PrairieKingSkillMenuEntry(prairieKingSkill);
            GraphicsEvents.OnPostRenderGuiEvent += skillMenuEntry.draw;
            Game1.endOfNightMenus.Push(new PrairieKingLevelUpMenu(prairieKingSkill));

            checkForExperienceBars();
            checkForAllProfessions();
        }

        /*********
        ** Private methods
        *********/
        Boolean gameOverGlobal;
        Boolean gameStart;
        int wave;
        private void update(object sender, EventArgs e)
        {
           
            if (Game1.currentMinigame != null && "AbigailGame".Equals(Game1.currentMinigame.GetType().Name))
            {
                Type minigameType = Game1.currentMinigame.GetType();

                /*
                * Calculates what bonus a character should start with based of their PrairieKingSkill level.
                */
                if (gameStart)
                {
                    if (prairieKingSkill.AquiredProfessions.Contains(PrairieKingProfession.Lives))
                    {
                        minigameType.GetField("lives").SetValue(Game1.currentMinigame, 20);
                    }
                    if (prairieKingSkill.AquiredProfessions.Contains(PrairieKingProfession.Coins))
                    {
                        minigameType.GetField("coins").SetValue(Game1.currentMinigame, 20);
                    }
                    if (prairieKingSkill.AquiredProfessions.Contains(PrairieKingProfession.PowerUpDuration))
                    {
                        minigameType.GetField("powerupDuration").SetValue(Game1.currentMinigame, 100);
                    }
                    if (prairieKingSkill.AquiredProfessions.Contains(PrairieKingProfession.ShootingDelay))
                    {
                        minigameType.GetField("shootingDelay").SetValue(Game1.currentMinigame, 20);
                    }
                    if (prairieKingSkill.AquiredProfessions.Contains(PrairieKingProfession.CoinChance))
                    {
                        minigameType.GetField("coinChance").SetValue(Game1.currentMinigame, 50);
                    }
                    if (prairieKingSkill.AquiredProfessions.Contains(PrairieKingProfession.LootChance))
                    {
                        minigameType.GetField("lootChance").SetValue(Game1.currentMinigame, 75);
                    }
                    gameStart = false;
                }

                /*
                * Calculates the amount of exp to grant a player after a PrairieKing session.
                */
                Boolean gameOverLocal = (Boolean)minigameType.GetField("gameOver").GetValue(null);
                wave = (Int32)minigameType.GetField("whichWave").GetValue(Game1.currentMinigame);
                if (!gameOverGlobal && gameOverLocal)
                {
                    prairieKingSkill.addExp((int)Math.Pow(2, wave) * 100);
                    this.Monitor.Log("This should only happen for restarts");
                }
                gameOverGlobal = gameOverLocal;
            } else
            {
                if (gameOverGlobal)
                {
                    prairieKingSkill.addExp((int)Math.Pow(2, wave) * 100);
                    this.Monitor.Log("This should only happen for real exits");
                }
                gameOverGlobal = false;
                gameStart = true;
            }
        }

        private void dayStarted(object sender, EventArgs e)
        {
            Game1.timeOfDay = 1200;
        }

        private void checkForExperienceBars()
        {
            if (!Helper.ModRegistry.IsLoaded("spacechase0.ExperienceBars"))
            {
                this.Monitor.Log("Experience Bars not found");
                return;
            }

            this.Monitor.Log("Experience Bars found, adding cooking experience bar renderer.");
            GraphicsEvents.OnPostRenderHudEvent += drawExperienceBar;
        }

        private void drawExperienceBar(object sender, EventArgs args_)
        {
            if (Game1.activeClickableMenu != null)
                return;

            try
            {
                if (Game1.player.experiencePoints.Length < prairieKingSkill.Index)
                {
                    return;
                }

                Type t = Type.GetType("ExperienceBars.Mod, ExperienceBars");

                int level = prairieKingSkill.getSkillLevel();
                int exp = Game1.player.experiencePoints[prairieKingSkill.Index];
                int x = 10;
                int y = (int)Util.GetStaticField(t, "expBottom");

                int prevReq = 0, nextReq = 1;
                if (level != 10)
                {
                    nextReq = prairieKingSkill.getExpForLevel(level);
                    if (level != 0)
                    {
                        prevReq = prairieKingSkill.getExpForLevel(level - 1);
                    }
                }

                int haveExp = exp - prevReq;
                int needExp = nextReq - prevReq;
                float progress = (float)haveExp / needExp;
                if (level == 10)
                {
                    progress = -1;
                }

                object[] args = new object[]
                {
                    x, y,
                    icon, new Rectangle( 0, 0, 16, 16 ),
                    level, progress,
                    new Color( 196, 79, 255 ),
                };
                Util.CallStaticMethod(t, "renderSkillBar", args);

                Util.SetStaticField(t, "expBottom", y + 40);
            }
            catch (Exception e)
            {
                this.Monitor.Log("Exception rendering prairie king skill bar: " + e);
                GraphicsEvents.OnPostRenderHudEvent -= drawExperienceBar;
            }
        }

        private void checkForAllProfessions()
        {
            if (!Helper.ModRegistry.IsLoaded("community.AllProfessions"))
            {
                this.Monitor.Log("[PrairieKingSkill] All Professions not found.");
                return;
            }

            this.Monitor.Log("[PrairieKingSkill] All Professions found. You will get every cooking profession for your level.");
        }
    }
}
