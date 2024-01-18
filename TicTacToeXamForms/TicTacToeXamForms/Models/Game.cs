using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

namespace TicTacToeXamForms.Models
{
    public class Game
    {
        private readonly Board _board;
        private Player _player1;
        private Player _player2;
        private Player _curPlayer; // текущий игрок
        public BehaviorSubject<List<Tuple<string, int>>> GameStat { get; }
        public string GameText { get; private set; } = "";
        public BehaviorSubject<bool> GameRunning { get; } = new BehaviorSubject<bool>(false);

        // Событие, сообщающее о выигрышной комбинации
        public Subject<List<Tuple<int, int>>> WinningCombination { get; } =
            new Subject<List<Tuple<int, int>>>();

        // Событие, сообщающее об изменении состояния игры
        public Subject<bool> GameStateChanged { get; } = new Subject<bool>();

        // Конструктор класса Game
        public Game(int boardResolution)
        {
            _board = new Board(boardResolution);
            _player1 = new Player('X', "Cross");
            _player2 = new Player('O', "Zero");
            GameStat = new BehaviorSubject<List<Tuple<string, int>>>(GetNewStat());
            _curPlayer = _player1;
        }

        // Получение текущего состояния доски
        public List<List<char?>> GetBoard()
        {
            return _board.GetBoard();
        }

        // Начало новой игры
        public void StartGame()
        {
            _board.EmptyBoard();
            GameText = "Player " + _player1.PlayerName + " turn";
            GameRunning.OnNext(true);
            GameStateChanged.OnNext(true);
        }

        // Получение размерности доски
        public int GetBoardResolution()
        {
            return _board.BoardPermissionn;
        }

        // Остановка игры
        public void StopGame()
        {
            GameRunning.OnNext(false);
        }

        // Проверка условий завершения игры
        private bool CheckStopGame()
        {
            return _board.SearchWinner() != null ||
                   _board.ListAllowedMoves().Count == 0;
        }

        // Ход игрока
        public void MakeMove(Tuple<int, int> coords)
        {
            if (_board.IsPermissibleToMove(coords.Item1, coords.Item2))
                _board.AssignValueToPosition(_curPlayer.Symbol, coords.Item1, coords.Item2);

            if (CheckStopGame())
            {
                StopGame();
                if (_board.SearchWinner() != null)
                {
                    var winner = _curPlayer.PlayerName;
                    _curPlayer.QuantityWins++;
                    GameText = winner + " win!";
                    WinningCombination.OnNext(_board.VictoryComb);
                    GameStat.OnNext(GetNewStat());
                }
                else
                    GameText = "Draw";
            }
            else
                SetNextPlayer();

            GameStateChanged.OnNext(true);
        }

        // Переключение на следующего игрока
        private void SetNextPlayer()
        {
            _curPlayer = _curPlayer.Symbol == _player1.Symbol ? _player2 : _player1;
            GameText = "Player " + _curPlayer.PlayerName + " turn";
        }

        // Получение новой статистики по игрокам
        private List<Tuple<string, int>> GetNewStat()
        {
            return new List<Tuple<String, int>>() {new Tuple<String, int>(_player1.PlayerName, _player1.QuantityWins),
                new Tuple<String, int>(_player2.PlayerName, _player2.QuantityWins)};
        }
    }
}
