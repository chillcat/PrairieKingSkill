using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PrairieKingSkill
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private PrairieKingSkill prairieKingSkill;
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            prairieKingSkill = new PrairieKingSkill(Util.GetNumOtherSkills(helper));
            GameEvents.UpdateTick += update;
            TimeEvents.AfterDayStarted += dayStarted;
            PrairieKingSkillMenuEntry skillMenuEntry = new PrairieKingSkillMenuEntry(prairieKingSkill);
            GraphicsEvents.OnPostRenderGuiEvent += skillMenuEntry.draw;
        }

        /*********
        ** Private methods
        *********/
        Boolean gameOverGlobal;
        int wave;
        private void update(object sender, EventArgs e)
        {
            if (Game1.currentMinigame != null && "AbigailGame".Equals(Game1.currentMinigame.GetType().Name))
            {
                Type minigameType = Game1.currentMinigame.GetType();
                Boolean gameOverLocal = (Boolean)minigameType.GetField("gameOver").GetValue(null);
                wave = (Int32)minigameType.GetField("whichWave").GetValue(Game1.currentMinigame);
                this.Monitor.Log($"wave: {wave}.");
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
            }
        }

        private void dayStarted(object sender, EventArgs e)
        {
            Game1.timeOfDay = 1200;
        }
    }
}
