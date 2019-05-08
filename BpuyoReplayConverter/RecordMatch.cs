using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpuyoReplayConverter
{
    class RecordMatch
    {
        public List<RecordOnePut> puts = new List<RecordOnePut>();
        private bool won;

        public RecordMatch()
        {
            won = false;
        }

        public void SetWon()
        {
            won = true;
        }

        public void SetLost()
        {
            won = false;
        }

        public bool Win()
        {
            return won;
        }

        public bool Lose()
        {
            return !won;
        }
    }
}
