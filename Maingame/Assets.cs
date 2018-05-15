using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Auxiliary;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System;
using Microsoft.Xna.Framework;
using System.Linq;

namespace MainGameSpace
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal static class Assets
    {
        public static SpriteFont FontNormal;
        public static SpriteFont FontNormalBold;
        public static SpriteFont FontBig;
        public static SpriteFont FontBigBold;
        public static SpriteFont FontLittle;
        public static SpriteFont FontLittleItalic;
        public static SpriteFont FontLittleBold;
        public static SpriteFont FontLittleBoldItalic;
        public static Texture2D QuestionMark;
        public static Texture2D Blue;
        public static Texture2D Black;

        private static ContentManager content;

        public static void LoadAll(ContentManager contentParameter)
        {
            Assets.content = contentParameter;
            Assets.LoadSpriteFont(out Assets.FontLittle, "Fonts\\Little");
            Assets.LoadSpriteFont(out Assets.FontLittleBold, "Fonts\\LittleBold");
            Assets.LoadSpriteFont(out Assets.FontLittleBoldItalic, "Fonts\\LittleBoldItalics");
            Assets.LoadSpriteFont(out Assets.FontNormal, "Fonts\\Standard");
            Assets.LoadSpriteFont(out Assets.FontNormalBold, "Fonts\\StandardBold");
            Assets.LoadSpriteFont(out Assets.FontBig, "Fonts\\Big");
            Assets.LoadSpriteFont(out Assets.FontBigBold, "Fonts\\BigBold");
            Assets.LoadSpriteFont(out Assets.FontLittleItalic, "Fonts\\LittleItalics");
            Assets.LoadTexture(out Assets.Blue, "backgroundblue");
            Assets.LoadTexture(out Assets.Black, "backgroundblack");

            //StartFetching();
        }
        

        private static void LoadSpriteFont(out SpriteFont spriteFont, string assetName)
        {
            spriteFont = Assets.content.Load<SpriteFont>(assetName);
        }

        private static void LoadTexture(out Texture2D texture2D, string assetName)
        {
            texture2D = Assets.content.Load<Texture2D>(assetName);
        }
        

        static object mutex = new object();
        static bool fetching = false;
        /*
        private static ConcurrentQueue<CardName> FetchWhat = new ConcurrentQueue<CardName>();
        private static ConcurrentDictionary<CardName, Texture2D> ConcurrentCardTextures = new ConcurrentDictionary<CardName, Texture2D>();
        static void StartFetching()
        {
            bool f;
            lock (mutex)
            {
                f = fetching;
                if (f) return;
                CardName fetchWhat;
                if (FetchWhat.TryDequeue(out fetchWhat))
                {
                    Task.Factory.StartNew(() =>
                    {
                        Texture2D result = content.Load<Texture2D>("Cards\\" + fetchWhat.ToString());
                        ConcurrentCardTextures[fetchWhat] = result;
                        lock (mutex)
                        {
                            fetching = false;
                        }
                        StartFetching();
                    });
                    fetching = true;
                }
            }
        }
        public static Texture2D TextureFromCard(CardName c)
        {
            string idstring = c.ToString();
            Texture2D value;
            if (ConcurrentCardTextures.TryGetValue(c, out value))
            {
                if (value != null)
                {
                    return value;
                }
            }
            else
            {
                ConcurrentCardTextures[c] = null;
                FetchWhat.Enqueue(c);
                StartFetching();
            }
            return Assets.QuestionMark;
            // return Assets.CardTextures[c.Id.ToString()];
            // return (c.GetType() == typeof(Girl) ? TextureFromCreature(c as Girl) : TextureFromAction(c as ActionCard));
        }
        public static Texture2D TextureFromCard(Card c)
        {
            return TextureFromCard(c.ID);
        }*/
        public static Texture2D Crop(Texture2D texture, Rectangle newBounds)
        {
            // Get your texture

            // Calculate the cropped boundary

            // Create a new texture of the desired size
            Texture2D croppedTexture = new Texture2D(Root.GraphicsDevice, newBounds.Width, newBounds.Height);

            // Copy the data from the cropped region into a buffer, then into the new texture
            Color[] data = new Color[newBounds.Width * newBounds.Height];
            texture.GetData(0, newBounds, data, 0, newBounds.Width * newBounds.Height);
            croppedTexture.SetData(data);
            return croppedTexture;
        }

    }
}