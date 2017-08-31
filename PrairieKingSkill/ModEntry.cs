using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Reflection;

namespace PrairieKingSkill
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            GameEvents.UpdateTick += update;
            TimeEvents.AfterDayStarted += dayStarted;
            //ControlEvents.KeyPressed += this.ControlEvents_KeyPress;
        }

        /*********
        ** Private methods
        *********/

        private void update(object sender, EventArgs e)
        {
            if (Game1.currentMinigame != null && "AbigailGame".Equals(Game1.currentMinigame.GetType().Name))
            {
                Type minigameType = Game1.currentMinigame.GetType();
                foreach (FieldInfo info in minigameType.GetFields())
                {
                    this.Monitor.Log($"field info {info}.");
                }
            }
        }

        private void dayStarted(object sender, EventArgs e)
        {
            Game1.timeOfDay = 1200;
        }
        /// <summary>The method invoked when the player presses a keyboard button.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void ControlEvents_KeyPress(object sender, EventArgsKeyPressed e)
        {
            if (Context.IsWorldReady) // save is loaded
            {
                this.Monitor.Log($"{Game1.player.name} pressed {e.KeyPressed}.");
            }
        }
    }
}
