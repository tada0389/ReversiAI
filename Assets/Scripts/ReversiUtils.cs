using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Reversi
{
    public class ReversiUtils
    {
        private static int[] dx = { 1, 0, -1, 0, 1, 1, -1, -1 };
        private static int[] dy = { 0, 1, 0, -1, 1, -1, 1, -1 };

        // 座標から盤面に対応するインデックスを得る
        public static int GetChipIndex(int x, int y)
        {
            return y * 8 + x;
        }


        // 盤面から置ける手を探す
        public static List<int> GetEnableHands(List<eStoneType> boards, eStoneType next_type)
        {
            List<int> ret = new List<int>();

            for(int y = 0; y < 8; ++y)
            {
                for(int x = 0; x < 8; ++x)
                {
                    if (boards[GetChipIndex(x, y)] != eStoneType.None) continue;

                    bool can = false;

                    for (int i = 0; i < 8; ++i)
                    {
                        if (can) break;

                        int ty = y + dy[i];
                        int tx = x + dx[i];

                        if (ty < 0 || ty >= 8) continue;
                        if (tx < 0 || tx >= 8) continue;

                        if (boards[GetChipIndex(tx, ty)] == eStoneType.None || boards[GetChipIndex(tx, ty)] == next_type) continue;

                        int j = 2;
                        while (!can)
                        {
                            int tty = y + j * dy[i];
                            int ttx = x + j * dx[i];

                            if (tty < 0 || tty >= 8) break;
                            if (ttx < 0 || ttx >= 8) break;
                            if (boards[GetChipIndex(ttx, tty)] == eStoneType.None) break;
                            if (boards[GetChipIndex(ttx, tty)] == next_type) can = true;
                            ++j;
                        }
                    }

                    if (can) ret.Add(GetChipIndex(x, y)); // 置ける
                }
            }

            return ret;
        }

        // 盤面に石を置いて得られる石の座標を取得する
        public static List<int> GetObtainStones(List<eStoneType> boards, int pos, eStoneType type)
        {
            int x = pos % 8;
            int y = pos / 8;

            List<int> get_stones_ = new List<int>();

            for (int i = 0; i < 8; ++i)
            {

                bool can = false;
                List<int> new_pos = new List<int>();

                int j = 1;
                while (!can)
                {
                    int tx = x + j * dx[i];
                    int ty = y + j * dy[i];

                    if (tx < 0 || tx >= 8) break;
                    if (ty < 0 || ty >= 8) break;
                    int index = GetChipIndex(tx, ty);
                    if (boards[index] == eStoneType.None) break;
                    if (boards[index] == type)
                    {
                        can = true;
                        break;
                    }
                    // 置く
                    new_pos.Add(index);
                    ++j;
                }

                if (can)
                {
                    foreach (var p in new_pos)
                    {
                        get_stones_.Add(p);
                    }
                }
            }

            // 自分自身
            get_stones_.Add(pos);

            return get_stones_;
        }
    }
}