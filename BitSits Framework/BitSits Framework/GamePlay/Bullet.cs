using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    class Bullet
    {
        Rank rank;

        public float totalDistance;
        float rotation = 0, speed = 3, direction;
        Vector2 position;

        Texture2D texture;

        public Bullet(Texture2D texture, Rank rank, Vector2 position, float direction)
        {
            this.position = position;
            this.direction = direction;
            this.texture = texture;
            this.rank = rank;
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                int halfSize = 10 / 2;
                return new Rectangle((int)position.X - halfSize, (int)position.Y - halfSize,
                    2 * halfSize, 2 * halfSize);
            }
        }

        public void Update(GameTime gameTime)
        {
            position += new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction)) * speed
                * (float)gameTime.ElapsedGameTime.TotalSeconds * 50;
            totalDistance += speed;

            if (rank == Rank.ninja) rotation += 10f / 180 * (float)Math.PI;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, direction + rotation,
                new Vector2(Tile.Width) / 2, 1,
                Math.Abs(direction) > (float)Math.PI / 2 ? SpriteEffects.FlipVertically : SpriteEffects.None, 1);
        }
    }
}
