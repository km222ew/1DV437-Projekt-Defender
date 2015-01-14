using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defender.Controller
{
    class StateChoice : EventArgs
    {
        public int choice;

        public StateChoice(int choice)
        {
            this.choice = choice;
        }
    }
}
