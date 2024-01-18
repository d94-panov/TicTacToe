using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using TicTacToeXamForms.Views;
using Xamarin.Forms;

namespace TicTacToeXamForms
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            GameView.Children.Add(new GameView(3));
            MainContainer.StyleClass = new List<string>{"background"};
        }

        private void ThemeSwitchHandler(object sender, CheckedChangedEventArgs checkedChangedEventArgs)
        {
            MainContainer.StyleClass = checkedChangedEventArgs.Value ? new List<string>{"background", "_dark"} : new List<string>{"background"};
        }
        
        
        private void NumberPickerChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                // Получить выбранный элемент, используя индекс
                string selectedItem = picker.Items[selectedIndex];
                
                // выбирать 3 или 5
                int selectedNumber = int.Parse(selectedItem);

                // обработка изменений
                NumberChangeHandler(selectedNumber);
            }
        }

        private void NumberChangeHandler(int selectedNumber)
        {
            // Логика обработки изменения
            GameView.Children.Clear();
            GameView.Children.Add(new GameView((int)selectedNumber));
        }
    }
}