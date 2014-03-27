using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    class Castle
    {
        public readonly Shape Shape;
        GameContent gameContent;

        public readonly Vector2 position;
        public readonly Rectangle BoundingRectangle;
        public readonly Rectangle AttackRectangle;

        public bool ChangeFlag = false;

        public Castle(GameContent gameContent, Shape shape, Vector2 tileCenter)
        {
            this.Shape = shape;
            this.gameContent = gameContent;

            this.position = tileCenter;

            int halfSize = Tile.Width / 2;
            BoundingRectangle = new Rectangle((int)position.X - halfSize, (int)position.Y - halfSize,
                2 * halfSize, 2 * halfSize);

            AttackRectangle = BoundingRectangle; AttackRectangle.Inflate(200, 600);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gameContent.castle[(int)Shape],
                position + new Vector2(0, Tile.Height / 2) - gameContent.castleOrigin,
                Color.White * .8f);
        }
    }
}
