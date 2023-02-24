using Raylib_CsLo;

namespace Trinkit.Graphics
{
    public class Sprite : Object
    {
        public Texture SourceTexture;
        public Rectangle SourceRect;

        public Sprite(Texture sourceTexture, Rectangle sourceRect)
        {
            SourceTexture = sourceTexture;
            SourceRect = sourceRect;
        }

        public static void DrawSprite(Texture texture, Vector3 position, float rotation, Color tint, Rectangle region = default, float pixelsPerUnit = 100.0f)
        {
            if (region.width == 0) region.width = texture.Width;
            if (region.height == 0) region.height = texture.Height;

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

            Debug.Counters.SpritesRendered++;
        }

        private static void DrawTexturePro3D(Texture texture, Rectangle sourceRec, Rectangle destRec, Vector3 origin, float rotation, float posZ, Color tint)
        {
            // Check if texture is valid
            if (texture.ID > 0)
            {
                float width = -(float)texture.Width;
                float height = -(float)texture.Height;

                bool flipX = false;

                if (sourceRec.width < 0) { flipX = true; sourceRec.width *= -1; }
                if (sourceRec.height < 0) sourceRec.y -= sourceRec.height;

                RlGl.rlSetTexture(texture.ID);
                RlGl.rlDisableDepthTest();
                RlGl.rlEnableDepthMask();
                RlGl.rlPushMatrix();
                {
                    RlGl.rlTranslatef(destRec.x, destRec.y, 0.0f);
                    RlGl.rlRotatef(rotation, 0.0f, 0.0f, 1.0f);
                    RlGl.rlTranslatef(-origin.x, -origin.y, -origin.z);

                    RlGl.rlBegin(RlGl.RL_QUADS);
                    {
                        RlGl.rlColor4f(tint.r, tint.g, tint.b, tint.a);
                        // Normal vector pointing towards viewer
                        RlGl.rlNormal3f(0.0f, 0.0f, 1.0f);

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
                RlGl.rlEnableDepthTest();
                RlGl.rlDisableDepthMask();
                RlGl.rlSetTexture(0);
            }
        }
    }
}
