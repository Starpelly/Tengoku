using Raylib_CsLo;

namespace Trinkit.Graphics
{
    public class RenderTexture : Component
    {
        private Raylib_CsLo.RenderTexture _internRT { get; set; }

        /// <summary>
        /// The texture data of the RenderTexture.
        /// </summary>
        public Raylib_CsLo.Texture texture => _internRT.texture;

        public RenderTexture(int width, int height)
        {
            _internRT = Raylib.LoadRenderTexture(width, height);
        }

        /// <summary>
        /// Begins drawing to the render texture.
        /// </summary>
        public void Begin()
        {
            Raylib.BeginTextureMode(_internRT);
        }

        /// <summary>
        /// Ends drawing to the render texture.
        /// </summary>
        public void End()
        {
            Raylib.EndTextureMode();
        }

        public override void Dispose()
        {
            Raylib.UnloadRenderTexture(_internRT);
        }

        public static implicit operator Raylib_CsLo.RenderTexture(RenderTexture texture)
        {
            return texture._internRT;
        }
    }
}
