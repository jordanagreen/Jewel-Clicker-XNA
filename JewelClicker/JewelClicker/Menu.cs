﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JewelClicker
{
    class Menu : DrawableGameComponent
    {
        private ArrayList buttons;
        private SpriteBatch spriteBatch;

        public Menu(Game game) : base(game)
        {
            buttons = new ArrayList();
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
        }

        public void addButton(String text, Color color, Color clickedColor, int x, int y, int width, int height, ButtonDelegate onClick)
        {
            MenuButton b = new MenuButton(text, color, clickedColor, x, y, width, height, onClick);
            buttons.Add(b);
        }
            
        public void LoadButtons(ContentManager content, GraphicsDeviceManager graphics)
        {
            foreach (MenuButton b in buttons)
            {
                b.LoadContent(content, graphics);
            }
        }

        public void UnloadButtons()
        {
            foreach (MenuButton b in buttons)
            {
                b.UnloadTextures();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (MenuButton b in buttons)
            {
                b.Draw(spriteBatch);
            }
            base.Draw(gameTime);
        }

        public void onMouseUp(MouseState mouseState)
        {
            foreach (MenuButton button in buttons){
                if (buttonClicked(button, mouseState)){
                    button.clicked = true;
                    button.onClick();
                }
            }
        }

        private bool buttonClicked(MenuButton b, MouseState ms)
        {
            Rectangle buttonArea = new Rectangle(b.x, b.y, b.width, b.height);
            return buttonArea.Contains(ms.X, ms.Y);
        }
     
    }

    class MenuButton
    {
        private static Texture2D blackTexture;
        private String text;
        private Color color, clickedColor;
        public readonly int x, y, width, height;
        private Texture2D texture, clickedTexture;
        private static SpriteFont font;
        public ButtonDelegate onClick;
        public bool clicked;

        public MenuButton(String t, Color color, Color clickedColor, int x, int y, int w, int h, ButtonDelegate onClick)
        {
            text = t;
            this.color = color;
            this.clickedColor = clickedColor;
            width = w;
            height = h;
            this.x = x;
            this.y = y;
            this.onClick = onClick;
        }

        public void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new [] { color });
            clickedTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            clickedTexture.SetData<Color>(new Color[] { clickedColor });
            if (blackTexture == null)
            {
                blackTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
                blackTexture.SetData<Color>(new Color[] { Color.Black });
            }
            if (font == null)
            {
                font = content.Load<SpriteFont>("ScoreFont");
            }
        }

        public void UnloadTextures()
        {
            texture.Dispose();
            clickedTexture.Dispose();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(blackTexture, new Rectangle(x - 3, y - 3, width + 6, height + 6), Color.White);
            Vector2 textDims = font.MeasureString(text);
            float textX = x + (width / 2) - (textDims.X / 2);
            float textY = y + (height / 2) - (textDims.Y / 2);
            sb.Draw(clicked ? clickedTexture : texture, new Rectangle(x, y, width, height), Color.White);
            sb.DrawString(font, text, new Vector2(textX, textY), Color.Black);
            sb.End();
        }

    }

}
