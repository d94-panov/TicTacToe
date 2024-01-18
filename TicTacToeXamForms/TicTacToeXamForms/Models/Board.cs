using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeXamForms.Models
{
    public class Board
    {
        // размерность доски
        public int BoardPermissionn { get; }
        // Комбинация клеток, образующих победу
        public List<Tuple<int, int>> VictoryComb { get; private set; }
        private readonly List<List<char?>> _board;

        // Конструктор доски
        public Board(int boardResolution)
        {
            BoardPermissionn = boardResolution;
            _board = new List<List<char?>>(BoardPermissionn);
            CompleteBoard(); // Заполнение доски
        }

        // Заполнение доски
        private void CompleteBoard()
        {
            for (var i = 0; i < BoardPermissionn; i++)
            {
                _board.Add(new List<char?>(BoardPermissionn));
                for (var j = 0; j < BoardPermissionn; j++)
                {
                    _board[i].Add(null);
                }
            }
        }

        // Получение текущего состояния доски
        public List<List<char?>> GetBoard()
        {
            return _board;
        }

        // Очистка доски
        public void EmptyBoard()
        {
            for (int i = 0; i < BoardPermissionn; i++)
            {
                for (int j = 0; j < BoardPermissionn; j++)
                {
                    _board[i][j] = null;
                }
            }
        }

        // Присвоение значения клетке
        public void AssignValueToPosition(char val, int x, int y)
        {
            if (_board[x][y] == null)
                _board[x][y] = val;
        }

        // Проверка наличия одинаковых элементов в списке
        private bool EqualityElementsChecklist<T>(IReadOnlyList<T> list)
        {
            return list.All(o => o.Equals(list[0]));
        }

        // Поиск победителя
        public char? SearchWinner()
        {
            for (var i = 0; i < BoardPermissionn; i++)
            {
                for (var j = 0; j <= 0; j++)
                {
                    if (CheckRow(i, j))
                    {
                        AssignWin(i, j, 0, 1); // комбинация для выигрыша в строке
                        return _board[i][j];
                    }

                    if (CheckColumn(j, i))
                    {
                        AssignWin(j, i, 1, 0); // комбинация для выигрыша в столбце
                        return _board[j][i];
                    }
                }
            }

            for (var i = 0; i <= 0; i++)
            {
                for (var j = 0; j <= 0; j++)
                {
                    if (CheckMainDiagonal(i, j))
                    {
                        AssignWin(i, j, 1, 1); // комбинация для выигрыша в главной диагонали
                        return _board[i][j];
                    }
                }
            }

            for (var i = BoardPermissionn - 1; i <= BoardPermissionn - 1; i++)
            {
                for (var j = 0; j <= 0; j++)
                {
                    if (СheckMinorDiagonal(i, j))
                    {
                        AssignWin(i, j, -1, 1); // комбинация для выигрыша в побочной диагонали
                        return _board[i][j];
                    }
                }
            }

            return null;
        }

        // выигрышная комбинация в строке
        private bool CheckRow(int startX, int startY)
        {
            return _board[startX][startY] != null &&
                   EqualityElementsChecklist(_board[startX].GetRange(startY, BoardPermissionn));
        }

        // выигрышная комбинация в столбце
        private bool CheckColumn(int startX, int startY)
        {
            for (var i = startX; i < startX + BoardPermissionn; i++)
            {
                if (_board[startX][startY] == null || _board[startX][startY] != _board[i][startY])
                {
                    return false;
                }
            }

            return true;
        }

        // выигрышная комбинация в главной диагонали
        private bool CheckMainDiagonal(int startX, int startY)
        {
            for (var i = 0; i < BoardPermissionn; i++)
            {
                if (_board[startX][startY] == null ||
                    _board[startX][startY] != _board[startX + i][startY + i])
                {
                    return false;
                }
            }

            return true;
        }

        // выигрышная комбинация в побочной диагонали
        private bool СheckMinorDiagonal(int startX, int startY)
        {
            for (var i = 0; i < BoardPermissionn; i++)
            {
                if (_board[startX][startY] == null ||
                    _board[startX][startY] != _board[startX - i][startY + i])
                {
                    return false;
                }
            }

            return true;
        }

        // Проверка возможности хода в клетку
        public bool IsPermissibleToMove(int x, int y)
        {
            return _board[x][y] == null;
        }

        // Список доступных ходов
        public List<Tuple<int, int>> ListAllowedMoves()
        {
            var allowedMovesList = new List<Tuple<int, int>>();

            for (var i = 0; i < BoardPermissionn; i++)
            {
                for (var j = 0; j < BoardPermissionn; j++)
                {
                    if (_board[i][j] == null)
                        allowedMovesList.Add(Tuple.Create(i, j));
                }
            }

            return allowedMovesList;
        }

        // Присвоение выигрышной комбинации
        private void AssignWin(int x
, int y, int dx, int dy)
        {
            VictoryComb = Enumerable.Range(0, BoardPermissionn).Aggregate(new List<Tuple<int, int>>(), (acc, cur) =>
            {
                acc.Add(new Tuple<int, int>(x + cur * dx, y + cur * dy));
                return acc;
            });
        }
    }
}
