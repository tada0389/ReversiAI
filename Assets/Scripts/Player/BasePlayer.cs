using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Reversi
{
    public abstract class BasePlayer : MonoBehaviour
    {
        // 次の手を打つ (返り値は次の盤面)
        public abstract GameTree Play(GameTree tree);
    }
} // namespace Reversi