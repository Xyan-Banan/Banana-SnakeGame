using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;

namespace Snake
{
    class Input
    {
        private static Hashtable keyTable = new Hashtable();

        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
        public static bool KeyPressed(Keys key)
        {
            if (keyTable[key] == null)
                return false;

            return (bool)keyTable[key];
        }

    }
}
