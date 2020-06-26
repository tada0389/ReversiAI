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
    }
} // namespace Reversi