using Trinkit.Audio;
using Trinkit.Graphics;

namespace Trinkit
{
    public static class Resources
    {
        public static T Load<T>(string location) where T : Object
        {
            var obj = Load(typeof(T), location) as T;

            if (obj == null)
                throw new Exception("Resource not found!");

            return obj;
        }

        public static Object? Load(Type type, string location)
        {
            var assemblyPath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

            if (assemblyPath == null) throw new Exception("AssemblyPath not found.");

            // var resourcesPath = Path.Combine(assemblyPath, "Resources");
            var resourceLocation = $"resources/{location}";

            if (!File.Exists(resourceLocation)) 
                throw new Exception("File not found.");

            if (type == typeof(AudioClip))
                return new AudioClip(resourceLocation);
            if (type == typeof(Texture))
                return new Texture(resourceLocation);

            return null;
        }
    }
}
