using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;
using GameDataLibrary;

namespace BitSits_Framework
{
    class Level : IDisposable
    {
        #region Fields


        public int Score { get; private set; }

        public bool IsLevelUp { get; private set; }
        public bool ReloadLevel { get; private set; }
        int levelIndex;

        GameContent gameContent;

        List<Army> armies = new List<Army>();
        List<Vector2> armyPosition = new List<Vector2>();
        Army mouseOverArmy, selectedArmy;

        Castle[] castles = new Castle[2];
        float MaxEnemyWaitTime = 2f; float enemyWaitTime;


        #endregion

        #region Initialization


        public Level(ScreenManager screenManager, int levelIndex)
        {
            this.gameContent = screenManager.GameContent;
            this.levelIndex = levelIndex;

            LevelDetails ld = gameContent.content.Load<LevelDetails>("Levels/level" + levelIndex.ToString("00"));

            Vector2 castlePos = new Vector2(1.5f, 6.5f) * Tile.Width;
            armyPosition.Add(castlePos);
            castles[(int)Shape.square] = new Castle(gameContent, Shape.square, castlePos);
            castles[(int)Shape.triangle] = new Castle(gameContent, Shape.triangle, new Vector2(800, 600)
                - castlePos);

            for (int i = 0; i < ld.ArmyNumber.Count; i++)
                for (int j = 0; j < ld.ArmyNumber[i]; j++)
                {
                    Vector2 v = GetPosition();
                    armies.Add(new Army(gameContent, Shape.square, (Rank)i, v));
                    armies.Add(new Army(gameContent, Shape.triangle, (Rank)i, new Vector2(800, 600) - v));
                }

            enemyWaitTime = 0;
            MaxEnemyWaitTime = MaxEnemyWaitTime / (float)Math.Pow(10, ld.GameMode);
        }


        Vector2 GetPosition()
        {
            while (true)
            {
                int i = gameContent.random.Next(5);
                int j = gameContent.random.Next(11);

                Point p = Tile.GetBounds(new Vector2(i, j) * Tile.Width).Center;
                Vector2 v = new Vector2(p.X, p.Y);

                if (!armyPosition.Contains(v))
                {
                    armyPosition.Add(v); return v;
                }
            }
        }


        public void Dispose() { }


        #endregion

        #region Update and HandleInput


        public void Update(GameTime gameTime)
        {
            enemyWaitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            int enemyAIindex = gameContent.random.Next(armies.Count / 2) * 2 + 1;

            bool noneAlive = true;
            for (int i = armies.Count - 1; i >= 0; i--)
            {
                armies[i].Update(gameTime);

                if (armies[i].IsAlive)
                {
                    armies[i].HandleEnemy(armies);
                    if (armies[i].Shape == Shape.triangle && enemyWaitTime > MaxEnemyWaitTime
                        && i == enemyAIindex) // probability it will be moved or not
                    {
                        enemyWaitTime = 0;
                        armies[i].EnemyAI(castles[(int)Shape.square].AttackRectangle);
                    }
                }

                if (armies[i].Shape == Shape.square
                    && castles[(int)Shape.triangle].position == armies[i].position)
                    IsLevelUp = true;
                if (armies[i].Shape == Shape.triangle
                    && castles[(int)Shape.square].position == armies[i].position)
                    ReloadLevel = true;

                if (armies[i].IsAlive && armies[i].Shape == Shape.square) noneAlive = false;
            }

            if (noneAlive) ReloadLevel = true;
        }


        public void HandleInput(InputState input, int playerIndex)
        {
            Vector2 mousePosition = new Vector2(input.CurrentMouseState.X, input.CurrentMouseState.Y)
                / Camera2D.ResolutionScale;
            Point mouseP = new Point((int)mousePosition.X, (int)mousePosition.Y);

            mouseOverArmy = null;
            for (int i = 0; i < armies.Count; i++)
            {
                if (armies[i].MouseBounds.Contains(mouseP) &&armies[i].IsAlive && selectedArmy == null
                    && armies[i].Shape == Shape.square
#if WINDOWS_PHONE
 && input.TouchState.Count > 0 && input.TouchState[0].State == TouchLocationState.Pressed
#endif
)
                {
                    mouseOverArmy = armies[i]; break;
                }
            }

#if WINDOWS
            if (input.CurrentMouseState.LeftButton == ButtonState.Pressed)
#endif
#if WINDOWS_PHONE
            if(input.TouchState.Count > 0 && (input.TouchState[0].State == TouchLocationState.Pressed
                || input.TouchState[0].State == TouchLocationState.Moved))
#endif
            {
                if (input.IsMouseLeftButtonClick() && mouseOverArmy != null)
                {
                    selectedArmy = mouseOverArmy; mouseOverArmy = null;
                    selectedArmy.Reset();
                }

                if (selectedArmy != null) selectedArmy.AddPosition(mousePosition);
            }

#if WINDOWS
            if (input.CurrentMouseState.LeftButton == ButtonState.Released
#endif
#if WINDOWS_PHONE
            if(input.TouchState.Count > 0 && input.TouchState[0].State == TouchLocationState.Released
#endif
                || (selectedArmy != null && !selectedArmy.IsAlive))
                selectedArmy = null;
        }


        #endregion

        #region Draw


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gameContent.background, Vector2.Zero, Color.White);

            if (mouseOverArmy != null) mouseOverArmy.DrawMouseOver(spriteBatch);
            if (selectedArmy != null) selectedArmy.DrawMouseOver(spriteBatch);

            for (int i = 0; i < armies.Count; i++) armies[i].DrawPath(gameTime, spriteBatch);

            for (int i = 0; i < armies.Count; i++) armies[i].Draw(gameTime, spriteBatch);

            foreach (Castle c in castles) c.Draw(spriteBatch);
        }


        #endregion
    }
}
