using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpuyoReplayConverter
{
    // プレイヤーのインデックスを管理する
    // indexが0なら1P, 1なら2P
    class PlayerState
    {
        private int index;

        public void SetFirstPlayer()
        {
            index = 0;
        }

        public void SetSecondPlayer()
        {
            index = 1;
        }

        public bool IsFirstPlayer()
        {
            return index == 0;
        }
    }
}
