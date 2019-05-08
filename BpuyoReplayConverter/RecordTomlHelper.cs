using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tommy;

namespace BpuyoReplayConverter
{
    class RecordTomlHelper
    {
        public static readonly String TITLE = "title";
        public static readonly String MATCH = "match";
        public static readonly String TURN = "match.turn";
        public static readonly String WIN = "win";

        // TOML用
        public static readonly String NUMBER = "number";
        public static readonly String PUT_FIELD = "put_field";
        public static readonly String FIELD = "field";
        public static readonly String NOW_KUMIPUYO = "now_kumipuyo";
        public static readonly String NEXT_KUMIPUYO = "next_kumipuyo";
        public static readonly String PUT_X = "put_x";
        public static readonly String PUT_ROTATE = "put_rotate";
        public static readonly String PUT_INDEX = "put_index";



        // gameをtomltable形式で返す
        // @note 1手以下で終わる試合は集計しない
        static public TomlTable Toml(in RecordGame game, bool onlywin)
        {
            TomlTable toml = new TomlTable
            {
                [TITLE] = "Bpuyo Replay",
                [MATCH] = new TomlArray
                {
                    IsTableArray = true,
                }
            };
            int match_i = 0;
            foreach (RecordMatch match in game.matches) {

                if (match.puts.Count <= 1) continue;

                if (onlywin && !match.Win()) continue;

                toml[MATCH].Add(new TomlTable 
                    {
                        [WIN]  = new TomlBoolean
                        {
                            Value = match.Win()
                        },
                        [TURN] = new TomlArray
                        {
                            IsTableArray = true
                        }
                    });
                foreach (RecordOnePut oneput in match.puts)
                {
                    toml[MATCH][match_i][TURN].Add(oneput.ToTomlTable());
                }
                match_i++;
            }
            return toml;
        }
    }
}
