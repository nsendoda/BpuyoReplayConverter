using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tommy;

namespace BpuyoReplayConverter
{
    // 一手ごとのフィールドと現在手とネクストとどこに置くかを示すクラス
    class RecordOnePut
    {
        public int[] my_field;
        private int[] pre_my_field;
        //        private int[] enemy_field = new int[BpuyoParameter.FieldSize];
        private int[] now_kumipuyo;
        private int[] next_kumipuyo;
        private PutPattern put_pattern;
        private int _hand_count;
        private bool _set_mode_beforeput;
        private bool _set_mode_wait;


        public RecordOnePut()
        {
            this.my_field = new int[BpuyoParameter.FieldSize];
            this.pre_my_field = new int[BpuyoParameter.FieldSize];
            now_kumipuyo = new int[BpuyoParameter.KumipuyoSize];
        next_kumipuyo = new int[BpuyoParameter.KumipuyoSize];
        put_pattern = new PutPattern();
            this._hand_count = 0;
            this._set_mode_beforeput = false;
            this._set_mode_wait = false;
        }

        // そのターンで初めて呼ばれるMode3は必ずぷよが消える前で、設置が確定している。
        public void SetModeBeforePut(in FrameStream frame)
        {
            _set_mode_beforeput = true;
            frame.now_puyos.CopyTo(now_kumipuyo, 0);
            frame.next_puyos.CopyTo(next_kumipuyo, 0);
            put_pattern.Set(frame.x, frame.rotate);
            frame.field.CopyTo(my_field, 0);
        }

        // Mode6の時だけターンが正しい保証があるので、ここでターンをセット
        public void SetModeWait(in FrameStream frame)
        {
            _set_mode_wait = true;
            _hand_count = frame.hand_count;
        }

        // 引数のオブジェクトのmy_fieldをこのオブジェクトのpre_fieldに入れる
        public void SetPreMyField(in RecordOnePut rec)
        {
            rec.my_field.CopyTo(pre_my_field, 0);
        }

        public bool IsSetWhenBeforePut()
        {
            return _set_mode_beforeput;
        }

        public bool IsSetWhenWait()
        {
            return _set_mode_wait;
        }

        public bool Invalid()
        {
            if (_hand_count == 0 ||
                !_set_mode_beforeput || !_set_mode_wait) return true;
            return false;
        }



        override public String ToString()
        {
            String output = String.Empty;

            output += _hand_count.ToString();
            output += '\n';

            output += put_pattern.PatternIndex;
            output += '\n';

            output += now_kumipuyo[0].ToString() + ' ' + now_kumipuyo[1].ToString();
            output += '\n';

            output += next_kumipuyo[0].ToString()    + ' ' + next_kumipuyo[1].ToString();
            output += '\n';

            output += FieldToString(my_field);

            return output;
        }

        // tomltableにして返す
        public TomlTable ToTomlTable()
        {
            TomlTable toml = new TomlTable
            {
                [RecordTomlHelper.NUMBER] = _hand_count,
                [RecordTomlHelper.FIELD] = new TomlString
                {
                    IsMultiline = true,
                    Value = FieldToString(pre_my_field)
                },
                [RecordTomlHelper.PUT_FIELD] = new TomlString
                {
                    IsMultiline = true,
                    Value = FieldToString(my_field)
                },
                [RecordTomlHelper.NOW_KUMIPUYO] = new TomlNode[] { now_kumipuyo[0], now_kumipuyo[1] },
                [RecordTomlHelper.NEXT_KUMIPUYO] = new TomlNode[] { next_kumipuyo[0], next_kumipuyo[1] },
                [RecordTomlHelper.PUT_X] = put_pattern.X,
                [RecordTomlHelper.PUT_ROTATE] = put_pattern.Rotate,
                [RecordTomlHelper.PUT_INDEX] = put_pattern.PatternIndex
            };
            return toml;
        }

        // Fieldのぷよを
        // \n000000\n00..の形式の文字列で返す
        // @note 左上から右へ向かうindex
        private String FieldToString(in int[] field)
        {
            String res = String.Empty;
            for (int i = 0; i < field.Length; i++)
            {
                if (i % BpuyoParameter.ColumnNumber == 0) res += '\n';
                res += field[i].ToString();
            }
            return res;
        }

    }
}
