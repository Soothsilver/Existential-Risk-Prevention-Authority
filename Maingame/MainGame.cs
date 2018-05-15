using System;
using System.Windows.Forms;
using Auxiliary;
using Auxiliary.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace MainGameSpace
{
    public class MainGame : Game
    {
        private bool ImmediatelyFullscreenize = true;
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static MainGame instance;
        public const int DEFAULTWIDTH = 1280;
        public const int DEFAULTHEIGHT = 900;

        public Form form
            ;
        
        public static MainGame Instance => MainGame.instance;

        public MainGame()
        {
            if (MainGame.instance != null)
            {
                throw new Exception("You cannot create more than one instance of the main game class.");
            }
            string[] args = Environment.GetCommandLineArgs();
           
            MainGame.instance = this;
            this.graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
            this.Content.RootDirectory = "Content";
            /*
            IntPtr hWnd = this.Window.Handle;
            var control = Control.FromHandle(hWnd);
            this.form = control.FindForm();
            this.form.FormBorderStyle = FormBorderStyle.None;
            //  form.TopMost = true;
            this.form.Width = DEFAULTWIDTH;
            this.form.Height = DEFAULTHEIGHT;
            this.form.WindowState = FormWindowState.Maximized;*/
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Window.Title = "Existential Risk Prevention Authority";

            Resolution resolution = new Resolution(DEFAULTWIDTH, DEFAULTHEIGHT);
            bool fullscreen = false;


            // Init call mandated by Auxiliary library.
            Root.Init(this, this.spriteBatch, this.graphics, resolution, fullscreen);


            if (this.ImmediatelyFullscreenize)
            {
                Root.SetResolution(Utilities.GetSupportedResolutions().FindLast(m => true));
                Root.IsFullscreen = true;
            }


            Assets.LoadAll(Content);


            Primitives.Fonts.Add(FontFamily.Small,
                new FontGroup(Assets.FontLittle, Assets.FontLittleItalic, Assets.FontLittleBold,
                    Assets.FontLittleBoldItalic));
            Primitives.Fonts.Add(FontFamily.Normal,
                new FontGroup(Assets.FontNormal, Assets.FontNormal, Assets.FontNormalBold, Assets.FontNormalBold));
            Primitives.Fonts.Add(FontFamily.Big,
                new FontGroup(Assets.FontBig, Assets.FontBig, Assets.FontBigBold, Assets.FontBigBold));


            // Make the buttons orange.
            GuiSkin mainSkin = GuiSkin.DefaultSkin;
            mainSkin.Font = Assets.FontNormal;
            mainSkin.InnerBorderThickness = 1;
            mainSkin.OuterBorderThickness = 1;
            mainSkin.GreyBackgroundColor = Color.LightBlue;
            mainSkin.GreyBackgroundColorMouseOver = Color.Azure;
            /*
            mainSkin.InnerBorderColor = Color.Red;
            mainSkin.InnerBorderColorMouseOver = Color.Red;
            mainSkin.InnerBorderColorMousePressed = Color.DarkRed;*/

            // Go to main menu.
            Root.PushPhase(new MainPhase());
        }


        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // We will not accept any input when the game window is not in focus.
            if (!this.IsActive)
            {
                return;
            }

            // Allows the game to exit
            if (Root.WasKeyPressed(Keys.Escape, ModifierKey.Alt) ||
                Root.WasKeyPressed(Keys.Escape, ModifierKey.Ctrl))
            {
                Exit();
            }

            // Toggle fullscreen
            if (Root.WasKeyPressed(Keys.Enter, ModifierKey.Alt))
            {
                if (Root.CurrentPhase is MainPhase)
                {
                    (Root.CurrentPhase as MainPhase).ToggleFullscreen();
                }
                else
                {
                    if (Root.IsFullscreen)
                    {
                        Root.IsFullscreen = false;
                        //   Root.SetResolution(DEFAULTWIDTH, 768);
                    }
                    else
                    {
                        //   Root.SetResolution(Utilities.GetSupportedResolutions().FindLast(m => true));
                        Root.IsFullscreen = true;
                    }
                }
            }
            // Accept input, do actions
            Root.Update(gameTime);
            // Exit when we pop out of the main menu
            /*if (Root.PhaseStack.Count == 0)
            {
                Exit();
            }*/
        }

        protected override void Draw(GameTime gameTime)
        {
            // Auxiliary handles all drawing.
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this.spriteBatch.Begin();
            Root.DrawPhase(gameTime);
            Root.DrawOverlay(gameTime);
            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}