using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reversi
{
    public enum eStoneType
    {
        None = 0, // 何もなし
        Black = 1, 
        White = 2,
    }

    public class ReversiManager : MonoBehaviour
    {
        [SerializeField]
        private int trial_num_ = 100;
        [SerializeField]
        private float game_interval_time_ = 0.24f;

        // 現在のゲーム木のroot
        private GameTree game_tree_;

        [SerializeField]
        private List<BasePlayer> players_;

        // 盤面の高さ
        private const int kHeight = 8;
        // 盤面の幅
        private const int kWidth = 8;

        [SerializeField]
        private float ai_think_time_ = 0.05f;

        // 盤面を描画要請するクラス
        private BoardRenderer renderer_;

        private void Start()
        {
            renderer_ = GetComponent<BoardRenderer>();

            // ゲーム開始
            StartCoroutine(GameFlow());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StopAllCoroutines();
                StartCoroutine(GameFlow());
            }
        }

        private IEnumerator GameFlow()
        {
            // ゲームを初期化
            InitGame();

            renderer_.Reset();
            renderer_.ProcBoard(game_tree_);
            renderer_.ShowPlayerNames(players_);

            while (true)
            {
                GameTree next_game_tree = null;
                // 入力を待つ
                while (next_game_tree == null)
                {
                    next_game_tree = players_[(int)game_tree_.StoneType - 1].Play(game_tree_);
                    yield return null;
                }

                // 現在のノードの更新
                game_tree_ = next_game_tree;

                // 描画情報の更新
                renderer_.ProcBoard(game_tree_);

                // 次の移行先がないなら終了
                if (game_tree_.GetEnableMoveNodes().Count == 0) break;

                yield return new WaitForSeconds(ai_think_time_);
            }

            // 勝敗処理
            renderer_.ProcResult();

            // 終了処理
            FinalizeGame();
        }

        // ゲームを初期化する
        private void InitGame()
        {
            // 初期の盤面を作る
            Board board = MakeInitialBoard();

            // 先行後攻どっちか
            int player = Random.Range(0, 2);
            eStoneType stone = (player == 0) ? eStoneType.Black : eStoneType.White;

            // ゲーム木を生成する
            game_tree_ = new GameTree(board, stone, -1, false);
        }

        // ゲームを終了する
        private void FinalizeGame()
        {
            // ゲーム木を開放
            game_tree_ = null;

            --trial_num_;
        }

        // 初期の盤面を作成する
        private Board MakeInitialBoard()
        {
            Board board = new Board(kHeight, kWidth);

            int x = kWidth / 2;
            int y = kHeight / 2;
            board[x - 1, y - 1] = eStoneType.Black;
            board[x - 1, y] = eStoneType.White;
            board[x, y - 1] = eStoneType.White;
            board[x, y] = eStoneType.Black;

            return board;
        }

        //[SerializeField]
        //private int trial_num_ = 100;
        //[SerializeField]
        //private float game_interval_time_ = 0.24f;

        //// 現在のゲーム木のroot
        //private GameTree game_tree_;

        //[SerializeField]
        //private List<BasePlayer> players_;

        //private const int kWidth = 8;
        //private const int kHeight = 8;

        //// 評価関数
        //private ReadOnlyCollection<int> kEvaluations = System.Array.AsReadOnly(new int[] {
        // 30, -12,  0, -1, -1,  0, -12,  30,
        //-12, -15, -3, -3, -3, -3, -15, -12,
        //  0,  -3,  0, -1, -1,  0,  -3,   0,
        // -1,  -3, -1, -1, -1, -1,  -3,  -1,
        // -1,  -3, -1, -1, -1, -1,  -3,  -1,
        //  0,  -3,  0, -1, -1,  0,  -3,   0,
        //-12, -15, -3, -3, -3, -3, -15, -12,
        // 30, -12,  0, -1, -1,  0, -12,  30,
        //});

        //// 先手，後手どっちか
        //private int turn_;


        //private List<eStoneType> boards_;

        //private float timer_;
        //[SerializeField]
        //private float turn_interval_ = 0.2f;

        //// 盤面を描画要請するクラス
        //private BoardRenderer renderer_;

        //// 次に置くことのできるマス目たち
        //private List<int> enable_hands_;

        //bool game_end_;

        //private int stone_cnt_;

        //private int trial_cnt_;

        //private void Start()
        //{
        //    UnityEngine.Assertions.Assert.IsTrue(players_.Count == 2, "プレイヤーの人数を２人にしてください");

        //    renderer_ = GetComponent<BoardRenderer>();
        //    renderer_.ShowPlayerNames(players_);

        //    enable_hands_ = new List<int>();

        //    boards_ = new List<eStoneType>(kWidth * kHeight);
        //    // 全て空にする
        //    boards_.AddRange(System.Linq.Enumerable.Repeat(eStoneType.None, kWidth * kHeight));

        //    trial_cnt_ = 0;
        //    Reset();
        //}

        //private void Update()
        //{
        //    if (trial_cnt_ >= trial_num_) return;
        //    timer_ += Time.deltaTime;

        //    // ゲーム終了からのリセット
        //    if (game_end_ && timer_ > game_interval_time_)
        //    {
        //        ++trial_cnt_;
        //        renderer_.ProcResult();
        //        if (trial_cnt_ < trial_num_) Reset();
        //        return;
        //    }
        //    if (game_end_) return;

        //    if (timer_ >= turn_interval_)
        //    {
        //        // 置ける手の色をリセットする
        //        renderer_.DismissSelectedColor(enable_hands_);

        //        Play();
        //        timer_ = 0.0f;

        //        if(stone_cnt_ == kWidth * kHeight)
        //        {
        //            game_end_ = true;
        //            return;
        //        }

        //        // 次に置ける手の色を変える
        //        enable_hands_.Clear();
        //        // もし置き手がないならパス
        //        int cnt = 0;
        //        while (enable_hands_.Count == 0 && cnt++ != 2)
        //        {
        //            turn_ = 1 - turn_;
        //            eStoneType next_type = (turn_ == 0) ? eStoneType.Black : eStoneType.White;
        //            enable_hands_ = ReversiUtils.GetEnableHands(boards_, next_type);
        //        }
        //        if (cnt == 3) game_end_ = true;
        //        renderer_.SetSelectedColor(enable_hands_);
        //    }
        //}

        //// ゲームの状態をリセットする
        //private void Reset()
        //{
        //    renderer_.Reset();
        //    for (int i = 0, n = kWidth * kHeight; i < n; ++i) boards_[i] = eStoneType.None;

        //    // ゲーム開始前の石を置く
        //    SetInitialStones();
        //    stone_cnt_ = 4;

        //    turn_ = Random.Range(0, 2);
        //    Debug.Log(((turn_ == 0) ? "Player1" : "Player2") + "からスタートです");

        //    eStoneType next_type = (turn_ == 0) ? eStoneType.Black : eStoneType.White;
        //    enable_hands_ = ReversiUtils.GetEnableHands(boards_, next_type);

        //    // 次に置ける手の色を変える
        //    renderer_.SetSelectedColor(enable_hands_);

        //    timer_ = 0.0f;
        //    game_end_ = false;
        //}

        //private void Play()
        //{
        //    // 現在選べる手から選ぶ
        //    eStoneType next_type = (turn_ == 0) ? eStoneType.Black : eStoneType.White;
        //    int hand = players_[turn_].Play(next_type, enable_hands_, boards_, kEvaluations);
        //    SetStone(hand, next_type);
        //}

        //// 石を置く
        //private void SetStone(int pos, eStoneType type)
        //{
        //    ++stone_cnt_;

        //    List<int> get_stones_ = ReversiUtils.GetObtainStones(boards_, pos, type);

        //    UnityEngine.Assertions.Assert.IsFalse(get_stones_.Count == 0, "不正な置き場です");

        //    // 自分自身
        //    List<eStoneType> types = new List<eStoneType>(get_stones_.Count);
        //    types.AddRange(System.Linq.Enumerable.Repeat(type, get_stones_.Count));

        //    // 描画命令
        //    renderer_.SetStoneSprite(get_stones_, types);

        //    // 自身の情報も更新
        //    foreach(var index in get_stones_)
        //    {
        //        boards_[index] = type;
        //    }
        //} 

        //// ゲーム開始前の石を置く
        //private void SetInitialStones()
        //{
        //    boards_[3 * kHeight + 3] = eStoneType.Black;
        //    boards_[4 * kHeight + 4] = eStoneType.Black;
        //    boards_[4 * kHeight + 3] = eStoneType.White;
        //    boards_[3 * kHeight + 4] = eStoneType.White;

        //    List<int> stone_pos = new List<int>();
        //    List<eStoneType> type = new List<eStoneType>();
        //    stone_pos.Add(3 * kHeight + 3);
        //    stone_pos.Add(4 * kHeight + 4);
        //    stone_pos.Add(3 * kHeight + 4);
        //    stone_pos.Add(4 * kHeight + 3);
        //    type.Add(eStoneType.Black);
        //    type.Add(eStoneType.Black);
        //    type.Add(eStoneType.White);
        //    type.Add(eStoneType.White);

        //    // 石を表示する
        //    renderer_.SetStoneSprite(stone_pos, type);
        //}
    }
} // namespace Reversi