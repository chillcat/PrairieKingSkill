using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrairieKingSkill
{
    class PrairieKingSkillMenuEntry
    {
        private PrairieKingSkill prairieKingSkill;
        public PrairieKingSkillMenuEntry(PrairieKingSkill prairieKingSkill)
        {
            this.prairieKingSkill = prairieKingSkill;
        }

        private bool didInitSkills = false;
        public void draw(object sender, EventArgs args)
        {
            if (Game1.activeClickableMenu is GameMenu)
            {
                GameMenu menu = Game1.activeClickableMenu as GameMenu;
                if (menu.currentTab == GameMenu.skillsTab)
                {
                    var tabs = (List<IClickableMenu>)Util.GetInstanceField(typeof(GameMenu), menu, "pages");
                    var skills = (SkillsPage)tabs[GameMenu.skillsTab];

                    if (!didInitSkills)
                    {
                        initPrairieKingSkill(skills);
                        didInitSkills = true;
                    }
                    drawPrairieKingSkill(skills);
                }
            }
            else didInitSkills = false;
        }

        private void initPrairieKingProfession(SkillsPage skills, int num2, int horizontalPosition, int verticalPosition, int skillLevel, Profession profession)
        {
            var skillBars = (List<ClickableTextureComponent>)Util.GetInstanceField(typeof(SkillsPage), skills, "skillBars");
            skillBars.Add(new ClickableTextureComponent(
                string.Concat(profession.Id),
                new Rectangle(
                    num2 + horizontalPosition - Game1.pixelZoom + (skillLevel - 1) * (Game1.tileSize / 2 + Game1.pixelZoom),
                    verticalPosition + prairieKingSkill.Index * (Game1.tileSize / 2 + Game1.pixelZoom * 6),
                    14 * Game1.pixelZoom,
                    9 * Game1.pixelZoom),
                null,
                profession.Description,
                Game1.mouseCursors,
                new Rectangle(159, 338, 14, 9),
                (float)Game1.pixelZoom, true)
                );
        }

        private void initPrairieKingSkill(SkillsPage skills)
        {
            // Bunch of stuff from the constructor
            int horizontalPosition = skills.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize - Game1.pixelZoom;
            int verticalPosition = skills.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth - Game1.pixelZoom * 3;
            if (prairieKingSkill.Level5Profession != null)
            {
                initPrairieKingProfession(skills, 0, horizontalPosition, verticalPosition, 5, prairieKingSkill.Level5Profession);
            }
            if (prairieKingSkill.Level10Profession != null)
            {
                initPrairieKingProfession(skills, Game1.pixelZoom * 6, horizontalPosition, verticalPosition, 10, prairieKingSkill.Level10Profession);
            }
            int skillIndex = prairieKingSkill.Index;
            string name = prairieKingSkill.getSkillLevel() > 0 ? prairieKingSkill.Name : "";
            var skillAreas = (List<ClickableTextureComponent>)Util.GetInstanceField(typeof(SkillsPage), skills, "skillAreas");
            skillAreas.Add(new ClickableTextureComponent(
                string.Concat(skillIndex),
                new Rectangle(horizontalPosition - Game1.tileSize * 2 - Game1.tileSize * 3 / 4, verticalPosition + skillIndex * (Game1.tileSize / 2 + Game1.pixelZoom * 6),
                Game1.tileSize * 2 + Game1.pixelZoom * 5, 9 * Game1.pixelZoom),
                string.Concat(skillIndex),
                name,
                null,
                Rectangle.Empty,
                1f,
                false));
        }

        private int getZoom(int skillLevel)
        {
            if (skillLevel < 5)
            {
                return 0;
            }
            else if (skillLevel < 10)
            {
                return Game1.pixelZoom * 6;
            }
            else
            {
                return Game1.pixelZoom * 12;
            }
        }

        private void drawPrairieKingSkill(SkillsPage skills)
        {
            SpriteBatch b = Game1.spriteBatch;
            int prairieSkillIndex = prairieKingSkill.Index;
            int currentLevel = prairieKingSkill.getSkillLevel();
            Rectangle empty = new Rectangle(50, 428, 10, 10);
            int horizontalPosition = skills.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize - 8;
            int verticalPosition = skills.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth - Game1.pixelZoom * 2;

            //Base skill drawing
            b.DrawString(
                Game1.smallFont,
                prairieKingSkill.Name,
                new Vector2((float)horizontalPosition - Game1.smallFont.MeasureString(prairieKingSkill.Name).X - (float)(Game1.pixelZoom * 4) - (float)Game1.tileSize,
                (float)(verticalPosition + Game1.pixelZoom + prairieSkillIndex * (Game1.tileSize / 2 + Game1.pixelZoom * 6))),
                Game1.textColor);
            b.Draw(
                Game1.mouseCursors,
                new Vector2((float)(horizontalPosition - Game1.pixelZoom * 16),
                (float)(verticalPosition + prairieSkillIndex * (Game1.tileSize / 2 + Game1.pixelZoom * 6))),
                new Rectangle?(empty),
                Color.Black * 0.3f,
                0f,
                Vector2.Zero,
                (float)Game1.pixelZoom,
                SpriteEffects.None, 0.85f);
            b.Draw(
                Game1.mouseCursors,
                new Vector2((float)(horizontalPosition - Game1.pixelZoom * 15),
                (float)(verticalPosition - Game1.pixelZoom + prairieSkillIndex * (Game1.tileSize / 2 + Game1.pixelZoom * 6))),
                new Rectangle?(empty),
                Color.White,
                0f,
                Vector2.Zero,
                (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);

            //For skill level 9
            NumberSprite.draw(currentLevel, b, new Vector2((float)(getZoom(9) + horizontalPosition + (11) * (Game1.tileSize / 2 + Game1.pixelZoom) + Game1.pixelZoom * 3 + ((currentLevel >= 10) ? (Game1.pixelZoom * 3) : 0)), (float)(verticalPosition + Game1.pixelZoom * 4 + prairieSkillIndex * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), Color.Black * 0.35f, 1f, 0.85f, 1f, 0, 0);
            NumberSprite.draw(currentLevel, b, new Vector2((float)(getZoom(9) + horizontalPosition + (11) * (Game1.tileSize / 2 + Game1.pixelZoom) + Game1.pixelZoom * 4 + ((currentLevel >= 10) ? (Game1.pixelZoom * 3) : 0)), (float)(verticalPosition + Game1.pixelZoom * 3 + prairieSkillIndex * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), Color.SandyBrown * ((currentLevel == 0) ? 0.75f : 1f), 1f, 0.87f, 1f, 0, 0);

            int skillIndex = 0;
            while (prairieKingSkill.getSkillLevel() > skillIndex)
            {
                b.Draw(Game1.mouseCursors, new Vector2((float)(getZoom(skillIndex + 1) + horizontalPosition - Game1.pixelZoom + skillIndex * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(verticalPosition + prairieSkillIndex * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(129, 338, 8, 9)), Color.Black * 0.35f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.85f);
                b.Draw(Game1.mouseCursors, new Vector2((float)(getZoom(skillIndex + 1) + horizontalPosition + skillIndex * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(verticalPosition - Game1.pixelZoom + prairieSkillIndex * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(137, 338, 8, 9)), Color.White * 1f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
                ++skillIndex;
            }
            for (; skillIndex < 10; skillIndex++)
            {
                int skillLevel = skillIndex + 1;
                if (skillLevel % 5 == 0)
                {
                    b.Draw(Game1.mouseCursors, new Vector2((float)(getZoom(skillIndex) + horizontalPosition - Game1.pixelZoom + skillIndex * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(verticalPosition + prairieSkillIndex * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(145, 338, 14, 9)), Color.Black * 0.35f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
                    b.Draw(Game1.mouseCursors, new Vector2((float)(getZoom(skillIndex) + horizontalPosition + skillIndex * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(verticalPosition - Game1.pixelZoom + prairieSkillIndex * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(145, 338, 14, 9)), Color.White * 0.65f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
                }
                else if (skillLevel % 5 != 0)
                {
                    b.Draw(Game1.mouseCursors, new Vector2((float)(getZoom(skillLevel) + horizontalPosition - Game1.pixelZoom + skillIndex * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(verticalPosition + prairieSkillIndex * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(129, 338, 8, 9)), Color.Black * 0.35f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.85f);
                    b.Draw(Game1.mouseCursors, new Vector2((float)(getZoom(skillLevel) + horizontalPosition + skillIndex * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(verticalPosition - Game1.pixelZoom + prairieSkillIndex * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(129, 338, 8, 9)), Color.White * 0.65f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
                }
            }

            skills.drawMouse(b);
        }
    }
}
