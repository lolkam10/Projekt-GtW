using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.Model
{
    public class GameStats
    {
        public GameStats(int gameNR, string haslo, string ifWin)
        {
            GameNR = gameNR;
            Haslo = haslo;
            IfWin = ifWin;
        }

        public int GameNR { get; set; }
        public string Haslo { get; set; }
        public string IfWin { get; set; }
    }
}
