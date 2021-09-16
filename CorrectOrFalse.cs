using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;

namespace standup
{
    public class CorrectOrFalse
    {
        #region ctor

        public CorrectOrFalse(Game game, int player)
        {
            QuestionsDocument = new XDocument(@"standup/CorrectOrFlase.xml");
            questions = QuestionsDocument.Root.Elements().ToList();
            playerForChellenge = game.GamePlayers.ElementAt(player - 1);
            MoneyOnTheTable = playerForChellenge.PreTrapSum * 2;
            game.IsOnCorrectOrFalseMode = true;
            MusicPath = @"standup/CorrectOrFalse.mp3";
            InstrcutionsPath = @"standup/CorrectOrFalse.wav";
        }

        #endregion

        #region Propeties

        public XDocument QuestionsDocument { get; set; }
        public List<XElement> questions { get; set; }
        public Player playerForChellenge { get; set; }
        public int MoneyOnTheTable { get; set; }
        public string InstrcutionsPath { get; set; }
        public string MusicPath { get; set; }

        #endregion
    }
}
