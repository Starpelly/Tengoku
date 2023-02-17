using Trinkit;

namespace Tengoku
{
    public class PlayerInput
    {
        public static bool GetPlayer()
        {
            return Input.GetKey(KeyCode.Space) ||
                Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X) || Input.GetMouseButton(0);
        }

        public static bool GetPlayerDown()
        {
            return Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0);
        }

        public static bool GetPlayerUp()
        {
            return Input.GetKeyUp(KeyCode.Space) ||
                Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.X) || Input.GetMouseButtonUp(0);
        }
    }
}
