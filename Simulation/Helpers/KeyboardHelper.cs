using System.Collections.Generic;
using static SFML.Window.Keyboard;

namespace Game.Helpers
{

    public static class KeyboardHelper
    {
        private static HashSet<Key> pressedKeys = new HashSet<Key>();

        public static bool IsKeyJustPressed(Key key)
        {
            bool isKeyPressed = IsKeyPressed(key);

            if (!isKeyPressed)
            {
                pressedKeys.Remove(key);
                return false;
            }
            else
            {
                if (pressedKeys.Contains(key))
                {
                    return false;
                }

                pressedKeys.Add(key);
                return true;
            }
        }

    }
}
