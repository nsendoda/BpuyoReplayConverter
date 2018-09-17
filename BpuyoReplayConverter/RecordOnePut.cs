using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpuyoReplayConverter
{
    // 一手ごとのフィールドと現在手とネクストとどこに置くかを示すクラス
    class RecordOnePut
    {
        private int[] my_field    = new int[BpuyoParameter.FieldSize];
        private int[] enemy_field = new int[BpuyoParameter.FieldSize];
        private int[] current_kumipuyo = new int[BpuyoParameter.KumipuyoSize];
        private int[] next_kumipuyo    = new int[BpuyoParameter.KumipuyoSize];
        private PutPattern put_pattern = new PutPattern();
        private int _hand_count;

        public RecordOnePut()
        {
            this._hand_count = 0;
        }


        // Fieldのぷよを
        // 0 0 0 0 0 0
        // 0 0 0 0 0 0..の形式の文字列で返す
        // @note 左上から右へ向かうindex
        private String FieldToString(int[] field)
        {
            String res = String.Empty;
            for(int i = 0; i < field.Length; i++) 
            {
                res += field[i].ToString();
                if (i % BpuyoParameter.ColumnNumber == 5) res += '\n';
                else res += ' ';
            }
            return res;
        }

        public void SetExcludeField(int hand_ct, int[] current_k, int[] next_k, int x, int rotate)
        {
            _hand_count = hand_ct;
            current_kumipuyo[0] = current_k[0];
            current_kumipuyo[1] = current_k[1];
            next_kumipuyo[0] = next_k[1];
            next_kumipuyo[0] = next_k[1];
            put_pattern.Set(x, rotate);
        }

        public bool IsSetRecord()
        {
            if (_hand_count == 0) return false;
            return true;
        }

        override public String ToString()
        {
            String output = String.Empty;

            output += _hand_count.ToString();
            output += '\n';

            output += put_pattern.PatternIndex;
            output += '\n';

            output += current_kumipuyo[0].ToString() + ' ' + current_kumipuyo[1].ToString();
            output += '\n';

            output += next_kumipuyo[0].ToString()    + ' ' + next_kumipuyo[1].ToString();
            output += '\n';

            return output;
        }
    }
}
