using Raylib_CsLo;

namespace Trinkit.Graphics
{
    public class Sprite
    {
        public static void DrawAnimation(Texture texture, Rectangle source, Vector3 pos)
        {
            // DrawSprite(texture, source, pos, Vector2.one, 0, Trinkit.Color.white);
        }

        public static void DrawSprite(Texture texture, Vector3 position, float rotation, Color tint, Rectangle region = default, float pixelsPerUnit = 100.0f)
        {
            if (region.width == 0) region.width = texture.width;
            if (region.height == 0) region.height = texture.height;

            var widthUnit = region.width / pixelsPerUnit;
            var heightUnit = region.height / pixelsPerUnit;

            DrawTexturePro3D(
                texture,
                new Rectangle(-region.x - region.width, -region.y - region.height, region.width, region.height),
                new Rectangle(-position.x, position.y, widthUnit, heightUnit),
                new Vector3(widthUnit * 0.5f, heightUnit * 0.5f),
                rotation,
                position.z,
                tint);
        }

        private static void DrawTexturePro3D(Texture texture, Rectangle sourceRec, Rectangle destRec, Vector3 origin, float rotation, float posZ, Color tint)
        {
            // Check if texture is valid
            if (texture.id > 0)
            {
                float width = -(float)texture.width;
                float height = -(float)texture.height;

                bool flipX = false;

                if (sourceRec.width < 0) { flipX = true; sourceRec.width *= -1; }
                if (sourceRec.height < 0) sourceRec.y -= sourceRec.height;

                RlGl.rlSetTexture(texture.id);
                RlGl.rlPushMatrix();
                {
                    RlGl.rlTranslatef(destRec.x, destRec.y, 0.0f);
                    RlGl.rlRotatef(rotation, 0.0f, 0.0f, 1.0f);
                    RlGl.rlTranslatef(-origin.x, -origin.y, -origin.z);

                    RlGl.rlBegin(RlGl.RL_QUADS);
                    {
                        RlGl.rlColor4f(tint.r, tint.g, tint.b, tint.a);
                        RlGl.rlNormal3f(0.0f, 0.0f, 1.0f);                          // Normal vector pointing towards viewer

                        // Bottom-left corner for texture and quad
                        if (flipX) RlGl.rlTexCoord2f((sourceRec.x + sourceRec.width) / width, sourceRec.y / height);
                        else RlGl.rlTexCoord2f(sourceRec.x / width, sourceRec.y / height);
                        RlGl.rlVertex3f(0.0f, 0.0f, posZ);

                        // Bottom-right corner for texture and quad
                        if (flipX) RlGl.rlTexCoord2f((sourceRec.x + sourceRec.width) / width, (sourceRec.y + sourceRec.height) / height);
                        else RlGl.rlTexCoord2f(sourceRec.x / width, (sourceRec.y + sourceRec.height) / height);
                        RlGl.rlVertex3f(0.0f, destRec.height, posZ);

                        // Top-right corner for texture and quad
                        if (flipX) RlGl.rlTexCoord2f(sourceRec.x / width, (sourceRec.y + sourceRec.height) / height);
                        else RlGl.rlTexCoord2f((sourceRec.x + sourceRec.width) / width, (sourceRec.y + sourceRec.height) / height);
                        RlGl.rlVertex3f(destRec.width, destRec.height, posZ);

                        // Top-left corner for texture and quad
                        if (flipX) RlGl.rlTexCoord2f(sourceRec.x / width, sourceRec.y / height);
                        else RlGl.rlTexCoord2f((sourceRec.x + sourceRec.width) / width, sourceRec.y / height);
                        RlGl.rlVertex3f(destRec.width, 0.0f, posZ);
                    }
                    RlGl.rlEnd();
                }
                RlGl.rlPopMatrix();
                RlGl.rlSetTexture(0);
            }
        }

        /*public static void DrawSprite(Texture texture, Rectangle sourceRec, Vector3 pos, Vector2 scale, float rot, Color color)
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
        }*/
    }
}
