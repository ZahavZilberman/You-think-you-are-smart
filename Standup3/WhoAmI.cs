using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;

namespace standup
{
    public class WhoAmI
    {
        #region ctor

        public WhoAmI(Game game)
        {
            WhoAmIQuestions = new XDocument(XDocument.Load($@"You think you are smart xml\WhoAmIXML.xml"));
            Questions = new List<XElement>(WhoAmIQuestions.Root.Elements());
            CorrectAnswer = null;
            QuestionsList = new List<XElement>();
            game.IsOnWhoAmIMode = true;
            FilePathToInstructions = $@"You-think-you-are-smart\WhoAmIQuestions\Instructions.wav";
            // MusicPath = @"standup/WhoAmI.mp3";
            Clues = new List<string>();


            #region Opening all XML based objects

            WhoAmIQuestions = new XDocument(XDocument.Load($@"You think you are smart xml\WhoAmIXML.xml"));
            Questions = WhoAmIQuestions.Root.Elements("Question");
            QuestionsList = Questions.ToList();
            whoAmIQuestionsList = new List<WhoAmIQuestion>();
            SoundPathes = new List<string>();

            int NumberOfQuestions = QuestionsList.Count;

            for (int i = 0; i < NumberOfQuestions; i++)
            {
                XElement QuestionNumber = QuestionsList[i].Element("Number");
                int ItsNumber = int.Parse(QuestionNumber.Value);
                string answer = File.ReadAllText($@"{QuestionsList.ElementAt(i).Element("Answer").Value}");
                XElement Clue1 = QuestionsList.ElementAt(i).Element("Clue1");
                XElement Clue2 = QuestionsList.ElementAt(i).Element("Clue2");
                XElement Clue3 = QuestionsList.ElementAt(i).Element("Clue3");
                XElement Clue4 = QuestionsList.ElementAt(i).Element("Clue4");
                string IncompleteAnswer = QuestionsList.ElementAt(i).Element("InCompleteAnswer").Value;

                string Clue1SoundPath = Clue1.Element("Sound").Value;
                SoundPathes.Add(Clue1SoundPath);
                string Clue2SoundPath = Clue2.Element("Sound").Value;
                SoundPathes.Add(Clue2SoundPath);
                string Clue3SoundPath = Clue3.Element("Sound").Value;
                SoundPathes.Add(Clue3SoundPath);
                string Clue4SoundPath = Clue4.Element("Sound").Value;
                SoundPathes.Add(Clue4SoundPath);

                string Clue1TextPath = Clue1.Element("Text").Value;
                string Clue1Text = File.ReadAllText(Clue1TextPath);
                Clues.Add(Clue1Text);

                string Clue2TextPath = Clue2.Element("Text").Value;
                string Clue2Text = File.ReadAllText(Clue2TextPath);
                Clues.Add(Clue2Text);

                string Clue3TextPath = Clue3.Element("Text").Value;
                string Clue3Text = File.ReadAllText(Clue3TextPath);
                Clues.Add(Clue3Text);

                string Clue4Text = File.ReadAllText(Clue1TextPath);
                Clues.Add(Clue4Text);

                string InCompleteAnswer = QuestionsList.ElementAt(i).Element("InCompleteAnswer").Value;

                WhoAmIQuestion AQuestion = new WhoAmIQuestion(ItsNumber, SoundPathes, Clues, answer, InCompleteAnswer);
                whoAmIQuestionsList.Add(AQuestion);
            }


            #endregion

            Random rnd = new Random();
            int randomQuestion = rnd.Next(0, NumberOfQuestions);
            for (int i = 0; i < whoAmIQuestionsList.Count - 1; i++)
            {
                if (i == randomQuestion)
                {
                    choosenQuestion = whoAmIQuestionsList.ElementAt(i);
                }

            }
        }

        #endregion

        #region Properties

        public XDocument WhoAmIQuestions { get; set; }
        public IEnumerable<XElement> Questions { get; set; }
        public List<WhoAmIQuestion> whoAmIQuestionsList { get; set; }
        public XElement CorrectAnswer { get; set; }
        public string FilePathToInstructions { get; set; }
        public string MusicPath { get; set; }
        public List<string> Clues { get; set; }
        public List<XElement> QuestionsList { get; set; }
        List<string> SoundPathes { get; set; }
        public WhoAmIQuestion choosenQuestion { get; set; }
        public string ChoosenAnswer { get; set; }


        #endregion
    }
}
