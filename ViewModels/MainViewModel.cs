using Projekt.Commands;
using Projekt.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Projekt.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private const int MaxAttempts = 8;
        private ObservableCollection<char> letters;
        private ObservableCollection<GameStats> statistics;
        private string gameStatus = "Kliknij przycisk NEW GAME, aby rozpocząć grę.";
        private Brush backgroudColor = Brushes.White;
        private BitmapSource liczbaBledow;
        private List<string> getWords;
        private readonly Random random = new Random();
        private string gameWord;
        private int faults = 0;
        private bool ifGameOver = false;
        private int gameNr;
        private List<string> categoryList;
        private string choosedCategory = "sportowcy";

        public MainViewModel()
        {
            letters = new ObservableCollection<char>();
            statistics = new ObservableCollection<GameStats>();
            categoryList = new List<string>();
            categoryList.Add("sportowcy");
            categoryList.Add("filmy");
            categoryList.Add("kuchnia");
            categoryList.Add("F1");


            newGameCommand = new RelayCommand(NewGame);
            keyClickedCommand = new RelayCommand(KeyClicked);

            gameNr = 0;

        }

        private void GetWords()
        {
            if(!FileHasla.FileExists(ChoosedCategory))
            {
                FileHasla.CreateNewFile();
            }

            getWords = FileHasla.GetAvailableWords(ChoosedCategory);
        }

        private void NewGame(object keyboardGrid)
        {
            EnableKeyboard(keyboardGrid as Grid);
            GetWords();
            NewRandomWord();
            InitBoard();
            if (!ifGameOver & gameNr!=0)
                Statistics.Add(new GameStats(GameNr, gameWord, "porzucona"));
            ifGameOver = false;
            faults = 0;
            UpdateImage();
            GameStatus = "Kliknij w wybraną literę w celu odgagnięcia hasła";
            BackgroundColor = Brushes.White;
            GameNr++;
            
        }

        private void UpdateImage() //zrobiłem po swojemu bo nie działało
        {
            //HangmanPicture = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory, "Images", $"{_wrongAttempts}.png")));
            LiczbaBledow = new BitmapImage(new Uri($"pack://application:,,,/Projekt;component/Model/Images/{faults}.png"));
        }

        private void InitBoard()
        {
            Letters.Clear();

            foreach (var item in gameWord)
            {
                if(item == ' ')
                {
                    Letters.Add(' ');
                    continue;
                }

                Letters.Add('\0');
            }
        }

        private void NewRandomWord()
        {
            var randomIndex = random.Next(getWords.Count);
            gameWord = getWords[randomIndex].ToUpper();
        }

        private void EnableKeyboard(Grid grid)
        {
            var stackPanelsKeyboard = grid.Children;

            foreach (StackPanel stackPanel in stackPanelsKeyboard)
            {
                foreach(Button button in stackPanel.Children)
                {
                    button.IsEnabled = true;
                }
            }
        }

        private void KeyClicked(object clickedButton)
        {
            if(ifGameOver)
            {
                return;
            }
            (clickedButton as Button).IsEnabled = false;
            var chosenKey = Convert.ToChar((clickedButton as Button).Content);

            var keyExistsInGuessingWord = gameWord.Contains(chosenKey);

            if(!keyExistsInGuessingWord)
            {
                faults++;
                UpdateImage();
                if (faults == MaxAttempts)
                {
                    ifGameOver = true;
                    GameStatus = $"Niestety przegrywasz. Prawidłowym hasłem było {gameWord}.";
                    BackgroundColor = Brushes.OrangeRed;
                    Statistics.Add(new GameStats(GameNr, gameWord, "porażka"));
                }
                return;
            }

            for(int i=0;i<gameWord.Length;i++)
            {
                if(gameWord[i]==chosenKey)
                {
                    Letters[i] = chosenKey;
                }
            }

            if(!Letters.Contains('\0'))
            {
                ifGameOver = true;
                GameStatus = "You win!";
                BackgroundColor = Brushes.Green;
                Statistics.Add(new GameStats(GameNr, gameWord, "zwycięstwo"));
            }
        }

        private void Reset(object commandParameter)
        {
            Statistics = new ObservableCollection<GameStats>();
            if (!ifGameOver)
                GameNr = 1;
            else
                GameNr = 0;
        }
        public ObservableCollection<char> Letters
        {
            get { return letters; }
            set { 
                letters = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GameStats> Statistics
        {
            get { return statistics; }
            set
            {
                statistics = value;
                OnPropertyChanged();
            }
        }

        public string GameStatus
        {
            get { return gameStatus; }
            set
            {
                gameStatus = value;
                OnPropertyChanged();
            }
        }

        public List<string> CategoryList
        {
            get { return categoryList; }
            set
            {
                categoryList = value;
                OnPropertyChanged();
            }
        }

        public string ChoosedCategory
        {
            get { return choosedCategory; }
            set
            {
                choosedCategory = value;
                OnPropertyChanged();
            }
        }

        public Brush BackgroundColor
        {
            get { return backgroudColor; }
            set
            {
                backgroudColor = value;
                OnPropertyChanged();
            }
        }

        public BitmapSource LiczbaBledow
        {
            get { return liczbaBledow; }
            set
            {
                liczbaBledow = value;
                OnPropertyChanged();
            }
        }

        public int GameNr
        {
            get { return gameNr; }
            set
            {
                gameNr = value;
                OnPropertyChanged();
            }
        }

        private ICommand newGameCommand { get; set; }
        public ICommand NewGameCommand
        {
            get
            {
                if (newGameCommand == null)
                {
                    newGameCommand = new RelayCommand(NewGame);
                }

                return newGameCommand;
            }
        }
        private ICommand keyClickedCommand { get; set; }
        public ICommand KeyClickedCommand
        {
            get
            {
                if (keyClickedCommand == null)
                {
                    keyClickedCommand = new RelayCommand(KeyClicked);
                }

                return keyClickedCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand resetCommand;

        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new RelayCommand(Reset);
                }

                return resetCommand;
            }
        }

        
    }
}
