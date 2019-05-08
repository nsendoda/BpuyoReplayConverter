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
