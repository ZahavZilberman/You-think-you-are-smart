using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;
using System.Globalization;
using System.Text;
using System.Numerics;
using NAudio;
using NAudio.Wave;
using System.Runtime.InteropServices;

namespace standup
{
    class Program
    {
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        const uint ENABLE_EXTENDED_FLAGS = 0x0080;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        public static void Main()
        {
            DisableQuickEditMode();

            // You may have to add the special command that clears the console
            Console.Title = "You think you are smart - The edition of The abnormal thinker";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Green;

            for (int repeat = 0; repeat < double.MaxValue; repeat++)
            {
                Console.Clear();

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                #region Pre game start..

                Console.WriteLine("We'll soon start this nonesense..");
                bool IsTwiceGame = false;

                string[] openedText = (File.ReadAllLines(@"You-think-you-are-smart\HowManyTimesyYouPlayedToday.txt"));
                DateTime currentDate = DateTime.Now;
                string day = currentDate.DayOfYear.ToString();

                string[] otherText = new string[1];
                otherText[0] = day;


                for (int line = 0; line < openedText.Length; line++)
                {
                    string ThatDay = openedText[line];
                    int dayTime = int.Parse(day);
                    int ThatDayTime = int.Parse(ThatDay);
                    if (dayTime == ThatDayTime)
                    {
                        IsTwiceGame = true;
                        string PlayTwiceTodayPath = (@"You-think-you-are-smart\OtherSounds\PlayTodayAgain.wav");
                        SoundPlayer PlayTwiceToday = new SoundPlayer(PlayTwiceTodayPath);
                        PlayTwiceToday.Play();
                        Thread.Sleep(5000);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }
                        break;
                        /*
                        using (var AudioFile = new AudioFileReader(PlayTwiceTodayPath))

                        using (var outputDevice = new WaveOutEvent())
                        {
                            outputDevice.Init(AudioFile);
                            outputDevice.Play();
                        }
                        */
                        
                    }
                }

                string[] newText = new string[openedText.Length];
                for (int line = 0; line < openedText.Length; line++)
                {
                    newText[line] = openedText[line];
                }
                newText[openedText.Length - 1] = day;
                File.WriteAllLines(@"You-think-you-are-smart\HowManyTimesyYouPlayedToday.txt", newText);

                if (!IsTwiceGame)
                {
                    DayOfWeek week = new DayOfWeek();
                    week = currentDate.DayOfWeek;


                    if (week.ToString() == "Saturday")
                    {

                        string PlaySaturdayPath = (@"You-think-you-are-smart\OtherSounds\Saturday.wav");
                        SoundPlayer PlaySaturday = new SoundPlayer(PlaySaturdayPath);
                        PlaySaturday.Play();

                        /*
                        using (var AudioFile = new AudioFileReader(PlaySaturdayPath))

                        using (var outputDevice = new WaveOutEvent())
                        {
                            outputDevice.Init(AudioFile);
                            outputDevice.Play();
                        }
                        */
                        Thread.Sleep(4800);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }
                    }

                    else if (currentDate.DayOfYear == 1)
                    {
                        string NewYearPath = (@"You-think-you-are-smart\OtherSounds\NewYear.wav");

                        SoundPlayer NewYearSound = new SoundPlayer(NewYearPath);
                        NewYearSound.Play();
                        /*
                        using (var AudioFile = new AudioFileReader(NewYearPath))

                        using (var outputDevice = new WaveOutEvent())
                        {
                            outputDevice.Init(AudioFile);
                            outputDevice.Play();
                        }
                        */
                        Thread.Sleep(5200);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }
                    }

                    else
                    {


                        string DefaultPath = (@"You-think-you-are-smart\OtherSounds\PlayTodayAgain.wav");

                        SoundPlayer DeafultSound = new SoundPlayer(DefaultPath);
                        DeafultSound.Play();
                        /*
                        using (var AudioFile = new AudioFileReader(DefaultPath))

                        using (var outputDevice = new WaveOutEvent())
                        {
                            outputDevice.Init(AudioFile);
                            outputDevice.Play();
                        }
                        */
                        Thread.Sleep(5100);

                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }
                    }
                }

                #endregion

                #region Game settings

                Console.Clear();
                Console.WriteLine("Welcome to the edition of zahav to the dumbest game ever!");
                Console.WriteLine("This game is a game of questions and trivia.");
                Console.WriteLine("You have to choose the right answer for every question. You get and lose points for every answer.");
                Console.WriteLine();
                Console.WriteLine("Done reading? click 'c' to continue and confirm that you understand the rules and start the game!");
                ConsoleKeyInfo okay = Console.ReadKey(true);
                while (okay.Key.ToString().ToUpper() != "C")
                {
                    Console.WriteLine("Press c, not other keys!");
                    okay = Console.ReadKey(true);
                }
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                }
                ButtonRespondSound();
                Console.Clear();
                Console.WriteLine("Let's start this baby's game!");
                Console.WriteLine("Hello! how many players are you, from 1 to 3?");
                int numOfPlayers = 0;
                ConsoleKeyInfo playersNum = Console.ReadKey(true);
                while (playersNum.Key.ToString() != "D1" && playersNum.Key.ToString() != "D2" && playersNum.Key.ToString() != "D3")
                {
                    Console.WriteLine("Press on a number between 1-3 only!");
                    playersNum = Console.ReadKey(true);

                }

                Console.Beep();
                Console.Clear();
                string ThePlayerNumKey = playersNum.Key.ToString();
                numOfPlayers = int.Parse(ThePlayerNumKey.Substring(1));
                string playerName = null;
                List<string> playerNames = new List<string>(numOfPlayers);

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                }

                Console.WriteLine("So you are " + numOfPlayers + " players.");
                Console.WriteLine();
                List<Player> players = new List<Player>();

                for (int countPlayerNames = 1; countPlayerNames <= numOfPlayers; countPlayerNames++)
                {
                    Console.WriteLine("Enter the name of player " + countPlayerNames.ToString() + ":");
                    playerName = Console.ReadLine();
                    while (playerName.Contains(" ") || playerName == "")
                    {
                        Console.WriteLine("Don't include spaces or blanks!");
                        playerName = Console.ReadLine();
                    }
                    playerNames.Add(playerName);
                    Player player = new Player(countPlayerNames, playerName);
                    Console.Beep();
                    players.Add(player);
                }

                Console.WriteLine();

                List<int> NumOfQuestionsInTotal = new List<int>();
                NumOfQuestionsInTotal.Add(3);
                NumOfQuestionsInTotal.Add(6);

                int ChoosenQuestions = 0;
                Console.WriteLine("Do you want 3 o 6 questions?");
                ConsoleKeyInfo questionsNum = Console.ReadKey(true);

                while (questionsNum.Key.ToString() != "D3" && questionsNum.Key.ToString() != "D6")
                {
                    Console.WriteLine("Press on either 3 or 6!");
                    questionsNum = Console.ReadKey(true);
                }
                Console.Beep();
                string TheQuestionNumKey = questionsNum.Key.ToString();
                ChoosenQuestions = int.Parse(TheQuestionNumKey.Substring(1));

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                }

                Console.WriteLine("Ok, so you've choosen " + ChoosenQuestions.ToString() + " questions.");

                Game game = new Game(players, ChoosenQuestions, numOfPlayers);
                //GetAllObjectsAndStart(game);
                //game.GameOver(game);
                game.Instructions();
                Console.Clear();
                game.NextTurn(game, game.GamePlayers.ElementAt(0));

                #endregion

            }
        }

        public static void ButtonRespondSound()
        {
            Console.Beep();
        }

        public static void GetAllObjectsAndStart(Game game)
        {
            Trap trap = new Trap(game);
            WhoAmI whoami = new WhoAmI(game);
            CorrectOrFalse correctorfalse = new CorrectOrFalse(game, 1);
        }

        public static void DisableQuickEditMode()
        {
            IntPtr consoleHandle = GetStdHandle(-10);  // -10 is STD_INPUT_HANDLE
            if (!GetConsoleMode(consoleHandle, out uint consoleMode))
            {
                Console.WriteLine("Failed to get console mode.");
                return;
            }

            // Remove the quick edit mode and enable extended flags
            consoleMode &= ~ENABLE_QUICK_EDIT_MODE;
            consoleMode |= ENABLE_EXTENDED_FLAGS;

            if (!SetConsoleMode(consoleHandle, consoleMode))
            {
                Console.WriteLine("Failed to set console mode.");
            }
        }
    }
}
