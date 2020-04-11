using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Symphonica
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        string[] notes = new string[] {"c", "cs", "d","ds", "e", "f","fs", "g","gs", "a","as", "b", "c2"};

        class keys
        {
            public Texture2D keyTexture;
            public Rectangle keyRect;
            public SoundEffect sound;
        }

        List<keys> keynotes = new List<keys>();

        Song song;
        float displayWidth;
        float displayHeight;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);

            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            displayWidth = GraphicsDevice.Viewport.Width;
            displayHeight = GraphicsDevice.Viewport.Height;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            float keyHeight = displayHeight / 8;
            float keyTop = displayHeight - keyHeight;

            foreach (string noteName in notes)
            {
                keys newKey = new keys();
                newKey.sound = Content.Load<SoundEffect>(noteName);

                if (noteName.Contains("s"))
                {
                    // Draw the black keys across half of the screen offset from 
                    // the white keys
                    newKey.keyRect = new Rectangle(0, (int)((keyTop + (keyHeight / 2)) + 0.5f), (int)(displayWidth / 2), (int)(keyHeight + 0.5f));
                    newKey.keyTexture = Content.Load<Texture2D>("BlackKey");

                    // Insert black keys at the head of the key list
                    keynotes.Insert(0, newKey);
                }
                else
                {
                    // Draw the white key across the whole screen
                    // using the white key texture
                    newKey.keyRect = new Rectangle(0, (int)(keyTop + 0.5f), (int)displayWidth, (int)(keyHeight + 0.5f));
                    newKey.keyTexture = Content.Load<Texture2D>("WhiteKey");

                    // Move down to the next key on the screen
                    keyTop = keyTop - keyHeight;

                    // Insert the white keys into the end of the key list
                    // This is because black keys are "on top" of the white
                    // ones and we need to find and play them first

                    if (keynotes.Count > 0)
                    {
                        keynotes.Insert(keynotes.Count - 1, newKey);
                    }
                    else
                    {
                        keynotes.Add(newKey);
                    }
                }
            }
            SoundEffect sound = Content.Load<SoundEffect>("song");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            TouchCollection touchState = TouchPanel.GetState();

            foreach (TouchLocation touch in touchState)
            {
                Point touchPoint = new Point((int)touch.Position.X, (int)touch.Position.Y);
                foreach (keys key in keynotes)
                {
                    if (key.keyRect.Contains(touchPoint) && touch.State == TouchLocationState.Pressed)
                    {
                        key.sound.Play();
                        break;
                    }
                }

                // If we touch the top right hand corner we play the cheat song
                if (touchPoint.X < 50 && touchPoint.Y < 50)
                {
                    MediaPlayer.Play(song);
                }
                if (touchPoint.X < 50 && touchPoint.Y < 50)
                {
                    MediaPlayer.Play(song);
                }

                if (touchPoint.X < 50 && touchPoint.Y > displayHeight - 50)
                {
                    Exit();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            for (int i = keynotes.Count - 1; i >= 0; i--)
            {
                keys key = keynotes[i];
                spriteBatch.Draw(key.keyTexture, key.keyRect, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
