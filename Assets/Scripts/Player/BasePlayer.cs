using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Reversi
{
    public abstract class BasePlayer : MonoBehaviour
    {
        // 次の手を打つ (返り値は盤面のインデックス)
        // 引数は順に　どの色を打つか，次に打てる手，盤面情報，評価関数
        public abstract int Play(eStoneType type, List<int> hands, List<eStoneType> boards, ReadOnlyCollection<int> evalutions);
    }
} // namespace Reversi