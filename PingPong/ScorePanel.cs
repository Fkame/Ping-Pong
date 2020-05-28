using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPong
{
    /// <summary>
    /// Класс, который просто фиксирует счёт и может привести его в форму для вывода на Label
    /// </summary>
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
