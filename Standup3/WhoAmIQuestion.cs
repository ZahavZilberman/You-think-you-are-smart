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
    public class WhoAmIQuestion
    {
        #region ctr

        public WhoAmIQuestion(int questionNumber, List<string> SoundPathes, List<string> Clues, string answer, string inCompleteAnswer)
        {
            QuestionNumber = questionNumber;
            CluesSoundPathes = SoundPathes;
            ClueText = Clues;
            CorrectAnswer = answer;
            InCompleteAnswer = inCompleteAnswer;
            Money = 10000;
            TotalSecondsForAnswer = 25;
            TimeLeft = 25;
        }

        #endregion

        #region Properties

        public string CorrectAnswer { get; set; }
        public int Money { get; set; }
        public int TotalSecondsForAnswer { get; set; }
        public int TimeLeft { get; set; }
        public string PathToAnswer { get; set; }
        public List<string> ClueText { get; set; }
        public List<string> CluesSoundPathes { get; set; }
        public int QuestionNumber { get; set; }
        public string InCompleteAnswer { get; set; }


        #endregion
    }
}
