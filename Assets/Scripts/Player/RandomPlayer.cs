using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Reversi
{
    public class RandomPlayer : BasePlayer
    {
        public override int Play(eStoneType type, List<int> hands, List<eStoneType> boards, ReadOnlyCollection<int> evalutions)
        {
            int n = hands.Count;
            return hands[Random.Range(0, n)];
        }

        public override string ToString()
        {
            return "Random";
        }
    }
} // namespace Reversi
