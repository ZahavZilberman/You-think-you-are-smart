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
            CorrectAnswerRewardAndPunishment = 2000;
            TrapQuestions = new XDocument(@"standup/Trap.XML");
            Questions = new List<XElement>(TrapQuestions.Root.Elements("Question"));
            question = null;
            QuestionText = null;
            CorrectAnswer = null;
            FilePathToInstructions = @"standup/Trap.wav";
            MusicPath = @"standup/Trap.mp3";
            game.IsOnTrapMode = true;
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

        #endregion
    }
}
