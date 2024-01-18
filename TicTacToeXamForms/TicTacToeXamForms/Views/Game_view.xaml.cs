using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToeXamForms.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicTacToeXamForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameView : ContentView
    {
        private readonly Game _game;
        private List<List<Button>> _buttons = new List<List<Button>>();

        // Конструктор класса GameView
        public GameView(int difficultyGame)
        {
            InitializeComponent();
            _game = new Game(difficultyGame);
            PrepareView();
        }

        // Сброс игры
        private void Reset(object sender, EventArgs eventArgs)
        {
            _game.StopGame();
            _buttons.ForEach(btnRow => btnRow.ForEach(RemoveSelection));
            _game.StartGame();
        }

        // Визуализация состояния доски
        private void VisualizationBoard()
        {
            var boardSnapshot = _game.GetBoard();
            for (var i = 0; i < _game.GetBoardResolution(); i++)
            {
                for (var j = 0; j < _game.GetBoardResolution(); j++)
                {
                    if (boardSnapshot[i][j] == null)
                    {
                        _buttons[i][j].Text = "";
                    }
                    else
                    {
                        _buttons[i][j].Text = boardSnapshot[i][j].ToString();
                    }
                }
            }

            GameTextView.Text = _game.GameText;
        }

        // Создание игрового поля, кнопок
        private void СreatingField()
        {
            for (var i = 0; i < _game.GetBoardResolution(); i++)
            {
                var rowToAdd = new StackLayout
                {
                    StyleClass = new List<string> {"buttons-wrapper"},
                    Orientation = StackOrientation.Horizontal
                };
                var curBtnsRow = new List<Button>();
                for (var j = 0; j < _game.GetBoardResolution(); j++)
                {
                    var btnToAdd = new Button();
                    var i1 = i;
                    var j1 = j;
                    curBtnsRow.Add(btnToAdd);
                    btnToAdd.Clicked += (o, e) => _game.MakeMove(new Tuple<int, int>(i1, j1));
                    btnToAdd.StyleClass = new List<string> {"button"};

                    // Установка стилей для разных размеров доски
                    if (_game.GetBoardResolution() == 3)
                    {
                        btnToAdd.StyleClass.Add("standart_button");
                    }
                    else
                    {
                        btnToAdd.StyleClass.Add("very_little_button");
                    }

                    rowToAdd.Children.Add(btnToAdd);
                }

                _buttons.Add(curBtnsRow);
                BtnsWrapper.Children.Add(rowToAdd);
            }
        }

        // Включение игрового поля
        private void TurnOnGameField()
        {
            _buttons.ForEach(list => list.ForEach(btn => btn.IsEnabled = true));
        }

        // Отключение игрового поля
        private void OffGameField()
        {
            _buttons.ForEach(list => list.ForEach(btn => btn.IsEnabled = false));
        }

        // Подготовка отображения
        private void PrepareView()
        {
            СreatingField();

            // события игры
            _game.GameRunning.Subscribe(x =>
            {
                if (x)
                {
                    TurnOnGameField();
                }
                else
                {
                    OffGameField();
                }
            });

            _game.GameStat
                .Subscribe(x => GameStat.Text = $"{x[0].Item1} {x[0].Item2} : {x[1].Item2} {x[1].Item1}");
            _game.GameStateChanged.Subscribe((x) => VisualizationBoard());

            // событие выигрыша
            _game.WinningCombination
                .Subscribe(winCoords =>
                {
                    winCoords.ForEach(coord => { AddHighlight(_buttons[coord.Item1][coord.Item2]); });
                });
        }

        // подсветка
        private void AddHighlight(View view)
        {
            view.StyleClass.Add("_highlight");
            view.StyleClass = new List<string>(view.StyleClass);
        }
        
        private void RemoveSelection(View view)
        {
            if (view.StyleClass.Remove("_highlight"))
                view.StyleClass = new List<string>(view.StyleClass);
        }
    }
}
