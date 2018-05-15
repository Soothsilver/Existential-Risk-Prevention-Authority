using Auxiliary;
using Auxiliary.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MainGameSpace
{
    internal class VictoryPhase : GamePhase
    {
        private string text;
        private int yearOver;
        private bool victory = false;
        public VictoryPhase()
        {
            victory = true;
        }
        public VictoryPhase(int yearOver, string text)
        {
            this.yearOver = yearOver;
            this.text = text;
        }

        protected override void Initialize(Game game)
        {
            base.Initialize(game);
            Button bStartAgain = new Button("Start again from 2018", new Rectangle(Root.ScreenWidth / 2 - 200, Root.ScreenHeight - 100, 400, 40));
            Button bQuitGame = new Button("Quit game", new Rectangle(Root.ScreenWidth / 2 - 200, Root.ScreenHeight - 50, 400, 40));
            bQuitGame.Click += BQuitGame_Click;
            bStartAgain.Click += BStartAgain_Click;
            AddUIElement(bQuitGame);
            AddUIElement(bStartAgain);
        }

        private void BStartAgain_Click(Button obj)
        {
            Root.PopFromPhase();
            Root.PushPhase(new MainPhase());
        }

        private void BQuitGame_Click(Button obj)
        {
            MainGame.Instance.Exit();
        }

        protected override void Draw(SpriteBatch sb, Game game, float elapsedSeconds)
        {
            Primitives.FillRectangle(Root.Screen, victory ? Color.White : Color.Black);
            if (!victory)
            { 
                Primitives.DrawImage(Assets.Black, Root.Screen);
            }
            Rectangle rText = new Rectangle(Root.ScreenWidth / 2 - 300, Root.ScreenHeight / 2 - 300, 600, 600);
            Rectangle rCaption = new Rectangle(rText.X, rText.Y - 100, rText.Width, 100);
            int x = yearOver - 2018;
            int y = x * 7;
            string winText = @"Congratulations! It is now the year 2040 and the humanity still lives and flourishes, the world's people enjoying a better quality of life than ever before. You are a hero and the agency you lead has been the best defender humankind has ever had. You have faced numerous challenges but you dealt with each of them with professionalism, heroism, kindness and strength of will. The world is forever in your debt.

Thank you for playing the Existential Risk Prevention Authority game. We hope you enjoyed playing it as much as we enjoyed making it. We will be grateful for any feedback - either on the game's GameJolt page, or you may send it to my email at petrhudecek2010@gmail.com";
            string loseText = @"R.I.P.
Homo sapiens sapiens
-200 000 -- " + yearOver + @"

The apocalypse has come.

" + text + @"

Humans have been ended. Perhaps they realized their fragility only too late, perhaps it was an unfortunate accident of low probability, or perhaps they deserved to be destroyed by their own arrogance. You have fought valiantly but it matters little for the world. Either it lives or it dies. It died.

Thanks to you, the world survived for " + x  + @" years since 2018. 
Your efforts have caused an additional " + y + @",000,000,000 human years to be lived by humans around the world.
You can still do better.";

            Primitives.DrawMultiLineText(victory ? winText : loseText, rText, victory? Color.Black : Color.White, FontFamily.Small);
            Primitives.DrawMultiLineText(victory ? "{b}APOCALYPSE AVERTED!{/b}" : "{b}APOCALYPSE!{/b}", rCaption, victory ? Color.Black : Color.White, FontFamily.Big, Primitives.TextAlignment.Left);
            base.Draw(sb, game, elapsedSeconds);
        }
    }
}