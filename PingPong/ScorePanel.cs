using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPong
{
    class ScorePanel
    {
        public int PlayerAI { get; set; }
        public int Player1 { get; set; }

        public override string ToString()
        {
            return $"{PlayerAI} : {Player1}";
        }



    }
}
