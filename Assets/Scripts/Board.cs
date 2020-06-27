using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オセロの盤面の状況
/// </summary>

namespace Reversi
{
    public class Board
    {
        // 盤面の状態
        public List<eStoneType> Values { private set; get; }

        public Board(int height, int width)
        {
            Values = new List<eStoneType>(height * width);
            // 全て空にする
            Values.AddRange(System.Linq.Enumerable.Repeat(eStoneType.None, height * width));
        }

        public Board(Board board)
        {
            Values = new List<eStoneType>(board.Values);
        }

        public eStoneType this[int x, int y]
        {
            get
            {
                return Values[y * 8 + x];
            }
            set
            {
                Values[y * 8 + x] = value;
            }
        }

        public eStoneType this[int xy]
        {
            get
            {
                return Values[xy];
            }
            set
            {
                Values[xy] = value;
            }
        }
    }
}