using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpuyoReplayConverter
{
    // 通常設置時: MOVING > BEFORE_PUT(1frame) > PUT_ANIMATION > WAIT ( > OJAMA)
    // 連鎖発生時: MOVING > [BEFORE_PUT(1) > CHAIN](連鎖回数分) > PUT_ANIMATION(1) > WAIT ( > OJAMA )
    enum Mode : int
    {
        MOVING = 1, BEFOREPUT_AND_JUDGE = 3, CHAIN = 4, PUT_ANIMATION = 5, WAIT, OJAMA = 7,
        RESULT_LOSE = 8, RESULT_WIN = 9, SELECT = 10, OPENING = 11
    };

}
