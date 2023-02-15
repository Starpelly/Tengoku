using Raylib_CsLo;

namespace Trinkit.Graphics
{
    public class Sprite
    {
        public static void DrawAnimation(Texture texture, Rectangle source, Vector3 pos)
        {
            DrawSprite(texture, source, pos, Vector2.one, 0, Trinkit.Color.white);
        }

        public static void DrawSprite(Texture texture, Rectangle sourceRec, Vector3 pos, Vector2 scale, float rot, Color color)
        {
            var pixelsPerUnit = 100.0f;

            float newWidth = sourceRec.width;
            float newHeight = sourceRec.height;

            Raylib.DrawTexturePro(
            texture,
            new Rectangle(sourceRec.x, sourceRec.y, -sourceRec.width, -sourceRec.height),
            new Rectangle(
                pos.x - ((newWidth / (float)pixelsPerUnit) * 0.5f), pos.y - ((newHeight / (float)pixelsPerUnit) * 0.5f),
                newWidth / (float)pixelsPerUnit, newHeight / (float)pixelsPerUnit),
            new System.Numerics.Vector2(0, 0),
                rot,
                color);
        }
    }
}
