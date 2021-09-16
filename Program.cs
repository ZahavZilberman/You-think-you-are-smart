using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;

namespace standup
{
    class Program
    {
        public static void Main()
        {
            Console.Title = "You think you are smart - The edition of The abnormal thinker";
            Console.SetWindowSize(110, 58);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Green;

            for (int repeat = 0; repeat < double.MaxValue; repeat++)
            {
                #region Pre game start..

                Console.WriteLine("We'll soon start this nonesense..");
                bool IsTwiceGame = false;

                string[] openedText = (File.ReadAllLines(@"E:\standup\standup\standup\HowManyTimesyYouPlayedToday.txt"));
                DateTime currentDate = DateTime.Now;
                string day = currentDate.DayOfYear.ToString();
                if (openedText.Length == 0)
                {
                    string[] otherText = new string[1];
                    otherText[0] = day;
                    File.WriteAllLines(@"E:\standup\standup\standup\HowManyTimesyYouPlayedToday.txt", otherText);
                }
                else
                {
                    for (int line = 0; line < openedText.Length; line++)
                    {
                        string ThatDay = openedText[line];
                        int dayTime = int.Parse(day);
                        int ThatDayTime = int.Parse(ThatDay);
                        if (dayTime == ThatDayTime)
                        {
                            IsTwiceGame = true;
                            SoundPlayer playTwiceToday = new SoundPlayer(@"E:\standup\standup\standup\OtherSounds\PlayTodayAgain.wav");
                            playTwiceToday.Play();
                            Thread.Sleep(5000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            break;
                        }
                    }

                    string[] newText = new string[openedText.Length + 1];
                    for (int line = 0; line < openedText.Length; line++)
                    {
                        newText[line] = openedText[line];
                    }
                    newText[openedText.Length] = day;
                    File.WriteAllLines(@"E:\standup\standup\standup\HowManyTimesyYouPlayedToday.txt", newText);
                }
                    if (!IsTwiceGame)
                    {
                        DayOfWeek week = new DayOfWeek();
                        week = currentDate.DayOfWeek;
                        if(week.ToString() == "Saturday")
                        {
                            SoundPlayer playTwiceToday = new SoundPlayer(@"E:\standup\standup\standup\OtherSounds\Saturday.wav");
                            playTwiceToday.Play();
                            Thread.Sleep(4800);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                        }

                        else if (currentDate.DayOfYear == 1)
                        {
                            SoundPlayer NewYear = new SoundPlayer(@"E:\standup\standup\standup\OtherSounds\NewYear.wav");
                            NewYear.Play();
                            Thread.Sleep(5200);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                        }

                        else
                        {
                            SoundPlayer Default = new SoundPlayer(@"E:\standup\standup\standup\OtherSounds\DefaultOpening.wav");
                            Default.Play();
                            Thread.Sleep(4500);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                        }
                }
                
                #endregion

                #region Game settings

                Console.Clear();
                Console.WriteLine("Welcome to the edition of inbar and zahav to the dumbest game ever!");
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

                    Console.Clear();
                    string ThePlayerNumKey = playersNum.Key.ToString();
                    numOfPlayers = int.Parse(ThePlayerNumKey.Substring(1));
                    string playerName = null;
                    List<string> playerNames = new List<string>(numOfPlayers);

                    Console.WriteLine("So you are " + numOfPlayers + " players.");
                    Console.WriteLine();

                    List<Player> players = new List<Player>();

                    for (int countPlayerNames = 1; countPlayerNames <= numOfPlayers; countPlayerNames++)
                    {
                        Console.WriteLine("Enter the name of player " + countPlayerNames.ToString() + ":");
                        playerName = Console.ReadLine();
                        playerNames.Add(playerName);
                        Player player = new Player(countPlayerNames, playerName);
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

                    string TheQuestionNumKey = questionsNum.Key.ToString();
                    ChoosenQuestions = int.Parse(TheQuestionNumKey.Substring(1));

                    Console.WriteLine("Ok, so you've choosen " + ChoosenQuestions.ToString() + " questions.");

                    Game game = new Game(players, ChoosenQuestions, numOfPlayers);

                    game.Instructions();

                #endregion

                //game.GameStart();
            }            
        }
    }
}
