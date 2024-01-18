namespace TicTacToeXamForms.Models
{
    public class Player
    {
        public int QuantityWins; // Количество побед игрока
        public readonly char Symbol; // Символ (X или O), которым играет игрок
        public string PlayerName;  // Имя игрока

        // Конструктор класса Player
        public Player(char symbol, string playerName)
        {
            // Инициализация символа и имени игрока
            Symbol = symbol;
            PlayerName = playerName;
            // Начальное количество побед установлено в 0
            QuantityWins = 0;
        }
    }
}