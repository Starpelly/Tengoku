using Raylib_CsLo;

namespace Trinkit.Graphics
{
    public class Texture : Component
    {
        private Raylib_CsLo.Texture _internTexture { get; set; }

        public uint ID => _internTexture.id;
        public int Width => _internTexture.width;
        public int Height => _internTexture.height;

        public Texture()
        {
            _internTexture = Raylib.LoadTexture("resources/pixel.png");
        }

        public Texture(string location)
        {
            _internTexture = Raylib.LoadTexture(location);
        }

        public override void Dispose()
        {
            Raylib.UnloadTexture(_internTexture);
        }

        public static implicit operator Raylib_CsLo.Texture(Texture texture)
        {
            return texture._internTexture;
        }
    }
}
