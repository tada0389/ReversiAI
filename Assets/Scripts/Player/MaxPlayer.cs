using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// 現在の盤面で最も石をとれる場所に置く
/// </summary>

namespace Reversi
{
    public class MaxPlayer : BasePlayer
    {
        public override int Play(eStoneType type, List<int> hands, List<eStoneType> boards, ReadOnlyCollection<int> evalutions)
        {
            int max_num = 0;

            // 最大の評価値からランダムにする
            Dictionary<int, List<int>> dicts = new Dictionary<int, List<int>>();

            foreach (var pos in hands)
            {
                int val = ReversiUtils.GetObtainStones(boards, pos, type).Count;
                if(val > max_num)
                {
                    max_num = val;
                    dicts.Add(val, new List<int>());
                    dicts[val].Add(pos);
                }
                else if(val == max_num)
                {
                    dicts[val].Add(pos);
                }
            }

            int n = dicts[max_num].Count;
            return dicts[max_num][Random.Range(0, n)];
        }

        public override string ToString()
        {
            return "MaxGet";
        }
    }
} // namespace Reversi
