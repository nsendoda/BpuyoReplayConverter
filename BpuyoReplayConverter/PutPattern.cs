using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpuyoReplayConverter
{
    class PutPattern
    {
        private int x; // 0-5
        private int rotate; // 0-3
        private int index; // ぷよの置き方(0-22)

        public int X
        {
            get { return x; }
        }
        public int Rotate
        {
            get { return rotate; }
        }
        public int PatternIndex
        {
            get { return index; }
        }

        public void Set(int x_, int rotate_)
        {
            x = x_;
            rotate = rotate_;
            if(x == 0)
            {
                index = rotate_;
            }else if(x == 5)
            {
                if (rotate_ == 0) index = 19;
                if (rotate_ == 2) index = 20;
                if (rotate_ == 3) index = 21;
            }else
            {
                index = x_ * 4 + rotate_ - 1;
            }
        }
    }
}
