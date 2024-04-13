using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;

namespace standup
{
    public class Trap
    {
        #region ctor

        public Trap(Game game)
        {
            CorrectAnswerRewardAndPunishment = 1000;
            TrapQuestions = new XDocument(XDocument.Load(@"You think you are smart xml\TrapXML.xml"));
            QuestionsElements = TrapQuestions.Root.Elements("Question");
            Questions = QuestionsElements.ToList();
            Answers = new List<string>();
            AllQuestions = new List<TrapQuestion>();
            for (int i = 0; i < 4; i++)
            {
                string AnswerPath = Questions.ElementAt(i).Element($@"Answer{i + 1}Text").Value;
                string ActualAnswer = File.ReadAllText(AnswerPath);
                Answers.Add(ActualAnswer);
                int QuestionPositionInDatabase = int.Parse(Questions.ElementAt(i).Element("Number").Value);
                int correctAnswer = int.Parse(Questions.ElementAt(i).Element("CorrectAnswer").Value);
                string QuestionItselfPath = Questions.ElementAt(i).Element("Text").Value;
                string QuestionContent = File.ReadAllText(QuestionItselfPath);
                TrapQuestion question = new TrapQuestion(QuestionContent, Answers, correctAnswer, i + 1, QuestionPositionInDatabase);
                AllQuestions.Add(question);
            }

            question = null;
            QuestionText = null;
            CorrectAnswer = null;
            FilePathToInstructions = @"You-think-you-are-smart\OtherSounds\TrapInstructions.wav";
            //MusicPath = @"standup/Trap.mp3";
            game.IsOnTrapMode = true;
            MoneyOfPlayersInTheTrap = new List<int>();

            for (int i = 0; i < game.GamePlayers.Count; i++)
            {
                game.GamePlayers.ElementAt(i).TrapSum = 0;
                MoneyOfPlayersInTheTrap.Add(game.GamePlayers.ElementAt(i).TrapSum);
            }
        }

        #endregion

        #region Properties

        public XDocument TrapQuestions { get; set; }
        public List<XElement> Questions { get; set; }
        public XElement question { get; set; }
        public XElement QuestionText { get; set; }
        public XElement CorrectAnswer { get; set; }
        public int CorrectAnswerRewardAndPunishment { get; set; }
        public string FilePathToInstructions { get; set; }
        public string MusicPath { get; set; }
        List<string> Answers { get; set; }
        public IEnumerable<XElement> QuestionsElements { get; set; }
        public List<TrapQuestion> AllQuestions { get; set; }
        public List<int> MoneyOfPlayersInTheTrap { get; set; }


        #endregion
    }
}
