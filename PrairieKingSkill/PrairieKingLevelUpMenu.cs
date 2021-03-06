﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using SFarmer = StardewValley.Farmer;

namespace PrairieKingSkill
{
    class PrairieKingLevelUpMenu : IClickableMenu
    {
        public const int basewidth = 768;

        public const int baseheight = 512;

        public bool informationUp;

        public bool isActive;

        public bool isProfessionChooser;

        private int currentLevel;

        private PrairieKingSkill currentSkill;

        private int timerBeforeStart;

        private float scale;

        private Color leftProfessionColor = Game1.textColor;

        private Color rightProfessionColor = Game1.textColor;

        private MouseState oldMouseState;

        private ClickableTextureComponent starIcon;

        private ClickableTextureComponent okButton;

        private List<CraftingRecipe> newCraftingRecipes = new List<CraftingRecipe>();

        private List<string> extraInfoForLevel = new List<string>();

        //private List<string> leftProfessionDescription = new List<string>();

        private IProfession leftProfession;

        //private List<string> rightProfessionDescription = new List<string>();

        private IProfession rightProfession;

        private Rectangle sourceRectForLevelIcon;

        private string title;

        //private List<int> professionsToChoose = new List<int>();

        private List<TemporaryAnimatedSprite> littleStars = new List<TemporaryAnimatedSprite>();

        public PrairieKingLevelUpMenu()
            : base(Game1.viewport.Width / 2 - 384, Game1.viewport.Height / 2 - 256, 768, 512, false)
        {
            this.width = Game1.tileSize * 12;
            this.height = Game1.tileSize * 8;
            this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
        }

        public PrairieKingLevelUpMenu(PrairieKingSkill skill)
            : base(Game1.viewport.Width / 2 - 384, Game1.viewport.Height / 2 - 256, 768, 512, false)
        {
            this.timerBeforeStart = 250;
            this.isActive = true;
            this.width = Game1.tileSize * 12;
            this.height = Game1.tileSize * 8;
            this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
            this.newCraftingRecipes.Clear();
            this.extraInfoForLevel.Clear();
            Game1.player.completelyStopAnimatingOrDoingAction();
            this.informationUp = true;
            this.isProfessionChooser = false;
            this.currentLevel = skill.getSkillLevel();
            this.currentSkill = skill;
            this.title = string.Concat(new object[]
            {
                "Level ",
                this.currentLevel,
                " ",
                currentSkill.Name
            });
            this.extraInfoForLevel = new List<string>();
            this.extraInfoForLevel.Add(currentSkill.Description);
            Tuple<IProfession, IProfession> professionChoices = null;
            if (currentSkill.Level5LevelUp)
            {
                professionChoices = currentSkill.getLevel5Choices();
            }
            else if (currentSkill.Level10LevelUp)
            {
                professionChoices = currentSkill.getLevel10Choices();
            }
            if(professionChoices != null)
            {
                this.isProfessionChooser = true;
                this.leftProfession = professionChoices.Item1;
                this.rightProfession = professionChoices.Item2;
            }
            int num = 0;
            this.height = num + Game1.tileSize * 4 + this.extraInfoForLevel.Count<string>() * Game1.tileSize * 3 / 4;
            Game1.player.freezePause = 100;
            this.gameWindowSizeChanged(Rectangle.Empty, Rectangle.Empty);
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            this.xPositionOnScreen = Game1.viewport.Width / 2 - this.width / 2;
            this.yPositionOnScreen = Game1.viewport.Height / 2 - this.height / 2;
            this.okButton.bounds = new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
        }

        public override void performHoverAction(int x, int y)
        {
        }

        public override void update(GameTime time)
        {
            if (!this.isActive)
            {
                base.exitThisMenu(true);
                return;
            }
            for (int i = this.littleStars.Count - 1; i >= 0; i--)
            {
                if (this.littleStars[i].update(time))
                {
                    this.littleStars.RemoveAt(i);
                }
            }
            if (Game1.random.NextDouble() < 0.03)
            {
                Vector2 position = new Vector2(0f, (float)(Game1.random.Next(this.yPositionOnScreen - Game1.tileSize * 2, this.yPositionOnScreen - Game1.pixelZoom) / (Game1.pixelZoom * 5) * Game1.pixelZoom * 5 + Game1.tileSize / 2));
                if (Game1.random.NextDouble() < 0.5)
                {
                    position.X = (float)Game1.random.Next(this.xPositionOnScreen + this.width / 2 - 57 * Game1.pixelZoom, this.xPositionOnScreen + this.width / 2 - 33 * Game1.pixelZoom);
                }
                else
                {
                    position.X = (float)Game1.random.Next(this.xPositionOnScreen + this.width / 2 + 29 * Game1.pixelZoom, this.xPositionOnScreen + this.width - 40 * Game1.pixelZoom);
                }
                if (position.Y < (float)(this.yPositionOnScreen - Game1.tileSize - Game1.pixelZoom * 2))
                {
                    position.X = (float)Game1.random.Next(this.xPositionOnScreen + this.width / 2 - 29 * Game1.pixelZoom, this.xPositionOnScreen + this.width / 2 + 29 * Game1.pixelZoom);
                }
                position.X = position.X / (float)(Game1.pixelZoom * 5) * (float)Game1.pixelZoom * 5f;
                this.littleStars.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(364, 79, 5, 5), 80f, 7, 1, position, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
                {
                    local = true
                });
            }
            if (this.timerBeforeStart > 0)
            {
                this.timerBeforeStart -= time.ElapsedGameTime.Milliseconds;
                return;
            }
            if (this.isActive && this.isProfessionChooser)
            {
                this.leftProfessionColor = Game1.textColor;
                this.rightProfessionColor = Game1.textColor;
                Game1.player.completelyStopAnimatingOrDoingAction();
                Game1.player.freezePause = 100;
                if (Game1.getMouseY() > this.yPositionOnScreen + Game1.tileSize * 3 && Game1.getMouseY() < this.yPositionOnScreen + this.height)
                {
                    if (Game1.getMouseX() > this.xPositionOnScreen && Game1.getMouseX() < this.xPositionOnScreen + this.width / 2)
                    {
                        this.leftProfessionColor = Color.Green;
                        if (((Mouse.GetState().LeftButton == ButtonState.Pressed && this.oldMouseState.LeftButton == ButtonState.Released) || (Game1.options.gamepadControls && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && !Game1.oldPadState.IsButtonDown(Buttons.A))) && this.readyToClose())
                        {
                            Game1.player.professions.Add(this.leftProfession.Id);
                            this.isActive = false;
                            this.informationUp = false;
                            this.isProfessionChooser = false;
                        }
                    }
                    else if (Game1.getMouseX() > this.xPositionOnScreen + this.width / 2 && Game1.getMouseX() < this.xPositionOnScreen + this.width)
                    {
                        this.rightProfessionColor = Color.Green;
                        if (((Mouse.GetState().LeftButton == ButtonState.Pressed && this.oldMouseState.LeftButton == ButtonState.Released) || (Game1.options.gamepadControls && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && !Game1.oldPadState.IsButtonDown(Buttons.A))) && this.readyToClose())
                        {
                            Game1.player.professions.Add(this.rightProfession.Id);
                            this.isActive = false;
                            this.informationUp = false;
                            this.isProfessionChooser = false;
                        }
                    }
                }
                this.height = Game1.tileSize * 8;
            }
            this.oldMouseState = Mouse.GetState();
            if (this.isActive && !this.informationUp && this.starIcon != null)
            {
                if (this.starIcon.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
                {
                    this.starIcon.sourceRect.X = 294;
                }
                else
                {
                    this.starIcon.sourceRect.X = 310;
                }
            }
            if (this.isActive && this.starIcon != null && !this.informationUp && (this.oldMouseState.LeftButton == ButtonState.Pressed || (Game1.options.gamepadControls && Game1.oldPadState.IsButtonDown(Buttons.A))) && this.starIcon.containsPoint(this.oldMouseState.X, this.oldMouseState.Y))
            {
                this.newCraftingRecipes.Clear();
                this.extraInfoForLevel.Clear();
                Game1.player.completelyStopAnimatingOrDoingAction();
                Game1.playSound("bigSelect");
                this.informationUp = true;
                this.isProfessionChooser = false;
                //this.currentLevel = Game1.player.newLevels.First<Point>().Y;
                //this.currentSkill = Game1.player.newLevels.First<Point>().X;
                this.title = Game1.content.LoadString("Strings\\UI:LevelUp_Title", new object[]
                {
                this.currentLevel,
                currentSkill.Name
                });
                this.extraInfoForLevel = new List<string>();
                this.extraInfoForLevel.Add(currentSkill.Description);
                Tuple<IProfession, IProfession> professionChoices = null;
                if (currentSkill.Level5LevelUp)
                {
                    professionChoices = currentSkill.getLevel5Choices();
                }
                else if (currentSkill.Level10LevelUp)
                {
                    professionChoices = currentSkill.getLevel10Choices();
                }
                if (professionChoices != null)
                {
                    this.isProfessionChooser = true;
                    this.leftProfession = professionChoices.Item1;
                    this.rightProfession = professionChoices.Item2;
                }
                int num = 0;
                this.height = num + Game1.tileSize * 4 + this.extraInfoForLevel.Count<string>() * Game1.tileSize * 3 / 4;
                Game1.player.freezePause = 100;
            }
            if (this.isActive && this.informationUp)
            {
                Game1.player.completelyStopAnimatingOrDoingAction();
                if (this.okButton.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) && !this.isProfessionChooser)
                {
                    this.okButton.scale = Math.Min(1.1f, this.okButton.scale + 0.05f);
                    if ((this.oldMouseState.LeftButton == ButtonState.Pressed || (Game1.options.gamepadControls && Game1.oldPadState.IsButtonDown(Buttons.A))) && this.readyToClose())
                    {
                        this.isActive = false;
                        this.informationUp = false;
                    }
                }
                else
                {
                    this.okButton.scale = Math.Max(1f, this.okButton.scale - 0.05f);
                }
                Game1.player.freezePause = 100;
            }
        }

        public override void receiveKeyPress(Keys key)
        {
        }

        public override void draw(SpriteBatch b)
        {
            if (this.timerBeforeStart > 0)
            {
                return;
            }
            b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
            foreach (TemporaryAnimatedSprite current in this.littleStars)
            {
                current.draw(b, false, 0, 0);
            }
            b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + this.width / 2 - 58 * Game1.pixelZoom / 2), (float)(this.yPositionOnScreen - Game1.tileSize / 2 + Game1.pixelZoom * 3)), new Rectangle?(new Rectangle(363, 87, 58, 22)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
            if (!this.informationUp && this.isActive && this.starIcon != null)
            {
                this.starIcon.draw(b);
                return;
            }
            if (this.informationUp)
            {
                if (this.isProfessionChooser)
                {
                    Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
                    base.drawHorizontalPartition(b, this.yPositionOnScreen + Game1.tileSize * 3, false);
                    base.drawVerticalIntersectingPartition(b, this.xPositionOnScreen + this.width / 2 - Game1.tileSize / 2, this.yPositionOnScreen + Game1.tileSize * 3);
                    Utility.drawWithShadow(b, Game1.buffsIcons, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), this.sourceRectForLevelIcon, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.88f, -1, -1, 0.35f);
                    b.DrawString(Game1.dialogueFont, this.title, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.dialogueFont.MeasureString(this.title).X / 2f, (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), Game1.textColor);
                    Utility.drawWithShadow(b, Game1.buffsIcons, new Vector2((float)(this.xPositionOnScreen + this.width - IClickableMenu.spaceToClearSideBorder - IClickableMenu.borderWidth - Game1.tileSize), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), this.sourceRectForLevelIcon, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.88f, -1, -1, 0.35f);
                    string text = Game1.content.LoadString("Strings\\UI:LevelUp_ChooseProfession", new object[0]);
                    b.DrawString(Game1.smallFont, text, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.smallFont.MeasureString(text).X / 2f, (float)(this.yPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearTopBorder)), Game1.textColor);
                    b.DrawString(Game1.dialogueFont, this.leftProfession.Name, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2)), this.leftProfessionColor);
                    b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + this.width / 2 - Game1.tileSize * 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2 - Game1.tileSize / 4)), new Rectangle?(new Rectangle(this.leftProfession.Id % 6 * 16, 624 + this.leftProfession.Id / 6 * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
                    b.DrawString(Game1.smallFont, Game1.parseText(this.leftProfession.Description, Game1.smallFont, this.width / 2 - 64), new Vector2((float)(-4 + this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2 + 8 + Game1.tileSize * (2))), this.leftProfessionColor);
                    b.DrawString(Game1.dialogueFont, this.rightProfession.Name, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + this.width / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2)), this.rightProfessionColor);
                    b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + this.width - Game1.tileSize * 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2 - Game1.tileSize / 4)), new Rectangle?(new Rectangle(this.rightProfession.Id % 6 * 16, 624 + this.rightProfession.Id / 6 * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
                    b.DrawString(Game1.smallFont, Game1.parseText(this.rightProfession.Description, Game1.smallFont, this.width / 2 - 48), new Vector2((float)(-4 + this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + this.width / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2 + 8 + Game1.tileSize * (2))), this.rightProfessionColor);
                }
                else
                {
                    Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
                    Utility.drawWithShadow(b, Game1.buffsIcons, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), this.sourceRectForLevelIcon, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.88f, -1, -1, 0.35f);
                    b.DrawString(Game1.dialogueFont, this.title, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.dialogueFont.MeasureString(this.title).X / 2f, (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), Game1.textColor);
                    Utility.drawWithShadow(b, Game1.buffsIcons, new Vector2((float)(this.xPositionOnScreen + this.width - IClickableMenu.spaceToClearSideBorder - IClickableMenu.borderWidth - Game1.tileSize), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), this.sourceRectForLevelIcon, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.88f, -1, -1, 0.35f);
                    int num = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 4;
                    foreach (string current2 in this.extraInfoForLevel)
                    {
                        b.DrawString(Game1.smallFont, current2, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.smallFont.MeasureString(current2).X / 2f, (float)num), Game1.textColor);
                        num += Game1.tileSize * 3 / 4;
                    }
                    foreach (CraftingRecipe current3 in this.newCraftingRecipes)
                    {
                        string text2 = Game1.content.LoadString("Strings\\UI:LearnedRecipe_" + (current3.isCookingRecipe ? "cooking" : "crafting"), new object[0]);
                        string text3 = Game1.content.LoadString("Strings\\UI:LevelUp_NewRecipe", new object[]
                        {
                            text2,
                            current3.name
                        });
                        b.DrawString(Game1.smallFont, text3, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.smallFont.MeasureString(text3).X / 2f - (float)Game1.tileSize, (float)(num + (current3.bigCraftable ? (Game1.tileSize * 3 / 5) : (Game1.tileSize / 5)))), Game1.textColor);
                        current3.drawMenuView(b, (int)((float)(this.xPositionOnScreen + this.width / 2) + Game1.smallFont.MeasureString(text3).X / 2f - (float)(Game1.tileSize * 3 / 4)), num - Game1.tileSize / 4, 0.88f, true);
                        num += (current3.bigCraftable ? (Game1.tileSize * 2) : Game1.tileSize) + Game1.pixelZoom * 2;
                    }
                    this.okButton.draw(b);
                }
                base.drawMouse(b);
            }
        }
    }
}
