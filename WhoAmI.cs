using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;

namespace standup
{
    public class WhoAmI
    {
        #region ctor

        public WhoAmI(Game game)
        {
            WhoAmIQuestions = new XDocument(@"standup/whoami.xml");
            Questions = new List<XElement>(WhoAmIQuestions.Root.Elements());
            Money = 10000;
            CorrectAnswer = null;
            CluesFilePath = null;
            game.IsOnWhoAmIMode = true;
            FilePathToInstructions = @"standup/WhoAmI.wav";
            MusicPath = @"standup/WhoAmI.mp3";
        }

        #endregion

        #region Properties

        public XDocument WhoAmIQuestions { get; set; }
        public List<XElement> Questions { get; set; }
        public XElement CorrectAnswer { get; set; }
        public XElement CluesFilePath { get; set; }
        public int Money { get; set; }
        public string FilePathToInstructions { get; set; }
        public string MusicPath { get; set; }

        #endregion
    }
}
