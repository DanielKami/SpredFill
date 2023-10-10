using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpredFill
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D texture;

        readonly int SizeX = 340;
        readonly int SizeY = 260;

        int BoxX = 2;
        int BoxY = 2;

        //Spave with blops
        int[,] space;
        //To generate random blops
        double[] rnd;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            space = new int[SizeX, SizeY];



            //Generate blobs
            rnd = new double[1000];
            Random r = new Random();
            for (int n = 0; n < 1000; n++)
                rnd[n] = r.Next(0, 30);

            for (int n = 0; n < 20; n++)
            {
                //Position
                int x_p = (int)(rnd[n] / 30 * SizeX);
                int y_p = (int)(rnd[n + 1] / 30 * SizeY);
                int r1 = (int)rnd[n + 3];
                int r2 = (int)rnd[n + 4];

                for (int i = 0; i < 360; i++)
                {
                    double w = Math.PI * i / 180;

                    for (int r_j = 0; r_j < 100; r_j++)
                    {
                        int x = (int)(x_p + 1.0 * r1 * r_j / 100 * Math.Sin(w));
                        int y = (int)(y_p + 1.0 * r2 * r_j / 100 * Math.Cos(w));
                        if (x > 0 && y > 0 && x < SizeX && y < SizeY)
                            space[x, y] = 1;
                    }
                }
            }

            //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

            //Find all blops
            Finder.FindAllBlops(space, SizeX, SizeY, 1);
        }


        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            texture = new Texture2D(graphics.GraphicsDevice, BoxX, BoxY);


            Color[] data = new Color[BoxX * BoxY];
            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;
            texture.SetData(data);

        }

        protected override void UnloadContent()
        {

        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    Vector2 coordinate = new Vector2(i * (BoxX), j * (BoxX));
                    if (space[i, j] >= 1)
                    {
                        spriteBatch.Draw(texture, coordinate, Color.Yellow);
                    }
                }
            }

            for (int i = 0; i < Finder.blop_points.Count; i++)
            {
                Color col = Color.FromNonPremultiplied(10, (int)rnd[i + 5] * 7, (int)rnd[i + 10] * 7, 255);
                for (int j = 0; j < Finder.blop_points[i].Count; j++)
                {
                    Point point = Finder.blop_points[i][j];
                    Vector2 coor = new Vector2(point.X * (BoxX), point.Y * (BoxX));
                    spriteBatch.Draw(texture, coor, col);
                }
            }

            for (int i = 0; i < Finder.center.Count; i++)
            {
                Vector2 coordinate = new Vector2(Finder.center[i].X * (BoxX), Finder.center[i].Y * (BoxX));

                //just a cross
                spriteBatch.Draw(texture, coordinate, Color.Red);
                Vector2 coor2 = coordinate + new Vector2(0, 1);
                spriteBatch.Draw(texture, coor2, Color.Red);
                coor2 = coordinate + new Vector2(0, -1);
                spriteBatch.Draw(texture, coor2, Color.Red);
                coor2 = coordinate + new Vector2(1, 0);
                spriteBatch.Draw(texture, coor2, Color.Red);
                coor2 = coordinate + new Vector2(-1, 0);
                spriteBatch.Draw(texture, coor2, Color.Red);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}