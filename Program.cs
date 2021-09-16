using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;

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

                    Game(numOfPlayers, playerNames, ChoosenQuestions);
            }            
        }

        public static void Game(int numofplayers, List<string> playerNames, int NumOfQuestions)
        {
            /*
            string currentPlayerturnName = playerNames.ElementAt(0);
            XDocument questions = new XDocument("standup/Questions.xml");
            XElement questionsXMLstart = questions.Root;
            List<XElement> questionsXML = new List<XElement>(questionsXMLstart.Elements("question"));

            for (int CurrentQuestionNum = 0; CurrentQuestionNum == NumOfQuestions; CurrentQuestionNum++)
            {
                XElement text = questionsXML.ElementAt(CurrentQuestionNum).Element("QuestionText");
                XElement soundElement = questionsXML.ElementAt(CurrentQuestionNum).Element("Sound");
                string textFilePath = text.Value;
                string soundPath = soundElement.Value;
                string textPath = Path.GetFullPath(textFilePath);
                IEnumerable<string> textLines = File.ReadAllLines(textPath);
                List<string> actualText = textLines.ToList<string>();
                List<string> QuestionText = new List<string>();
                QuestionText = File.ReadAllLines(textPath).To;
                
                SystemSound sound = new SystemSound
                sound.Play();
            }
            */
        }
    }
}
