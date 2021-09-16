using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;
using System.Text;


namespace standup
{
    public class Game
    {
        #region ctor

        public Game(List<Player> players, int NumOfQuestions, int NumOfPlayers)
        {
            CountOfQuestion = 1;
            NumOfGameQuestions = NumOfQuestions;
            PlayersNum = NumOfPlayers;
            GamePlayers = new List<Player>();
            GamePlayers = players;
            IsOnMultiplyMode = false;
            IsOnChooseCatgoryMode = false;
            IsOnCorrectOrFalseMode = false;
            IsOnWhoAmIMode = false;
            IsOnTrapMode = false;

            PathToInstructions = @"standup/game.wav";
        }

        #endregion

        #region Instructions part

        public void Instructions()
        {
            #region The buttons

            Console.Clear();


            #endregion

            #region The general instructions

            Console.Clear();
            Console.WriteLine("The general instructions.. you can hear them..");
            Console.WriteLine();
            Console.WriteLine("Press any key to skip this repetive shit.");

            SoundPlayer InstructionsAudio = new SoundPlayer(@"E:\standup\standup\standup\OtherSounds\Instructions.wav");
            int startSecond = DateTime.Now.Second;
            int startMinute = DateTime.Now.Minute;
            int endSecond;
            int endMinute;
            if (startSecond <= 22)
            {
                endMinute = startMinute;
                endSecond = startSecond + 38;
            }
            else
            {
                endMinute = startMinute + 1;
                endSecond = startSecond - 22;
            }
            InstructionsAudio.Play();
            while (!Console.KeyAvailable || (DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute))
            {
                Thread.Sleep(100);
            }
            SoundPlayer InstructionsOtherAudio = new SoundPlayer(@"E:\standup\standup\standup\OtherSounds\SkippedInstructions.wav");
            if (Console.KeyAvailable == true)
            {
                InstructionsAudio.Stop();
                InstructionsOtherAudio.Play();
                Thread.Sleep(3750);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                }

                Console.Clear();
                Console.ReadLine();
            }
            if (DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute)
            {
                Console.Clear();
                Console.ReadLine();
            }
        }
            #endregion

        #endregion

        #region Game start

        public void GameStart()
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

        #endregion

        #region Properties

        public int CountOfQuestion { get; set; }

        public List<Player> GamePlayers { get; set; }

        public int PlayersNum { get; set; }

        public int NumOfGameQuestions { get; set; }

        public bool IsOnMultiplyMode { get; set; }

        public bool IsOnCorrectOrFalseMode { get; set; }

        public bool IsOnTrapMode { get; set; }

        public bool IsOnWhoAmIMode { get; set; }

        public bool IsOnChooseCatgoryMode { get; set; }

        public string PathToInstructions { get; set; }

        public bool IsGameOver { get; set; }

        #endregion
    }
}
