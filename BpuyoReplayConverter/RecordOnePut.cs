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
        public int[] my_field    = new int[BpuyoParameter.FieldSize];
        public int[] enemy_field = new int[BpuyoParameter.FieldSize];
        public int[] current_kumipuyo = new int[BpuyoParameter.KumipuyoSize];
        public int[] next_kumipuyo    = new int[BpuyoParameter.KumipuyoSize];
        public PutPattern put_pattern = new PutPattern();
        public int hand_count = 0;

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

        public bool IsSetRecord()
        {
            if (hand_count == 0) return false;
            return true;
        }

        override public String ToString()
        {
            String output = String.Empty;

            output += hand_count.ToString();
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
