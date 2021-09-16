using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;

namespace standup
{
    public class Player
    {
        #region ctor

        public Player(int PlayerNum, string Name)
        {
            PreTrapSum = 0;
            TrapSum = 0;
            GameSum = 0;
            PlayerNumber = PlayerNum;
            PlayerName = Name;
            PlayerBuzzer = null;
            PlayerNameAudioPath = null; //will change this one later

            if (PlayerNumber == 1)
            {
                PlayerBuzzer = "M";
            }
            else if(PlayerNumber == 2)
            {
                PlayerBuzzer = "+";
            }
            else if (PlayerNumber == 3)
            {
                PlayerBuzzer = "A";
            }

            PlayerNameAudioPath = null;

            try
            {
                PlayerNameAudioPath = @"standup/" + PlayerName + ".wav";
            }
            catch(FileNotFoundException)
            {
                PlayerNameAudioPath = null;
            }

            PlayerPosition = 1;
        }

        #endregion

        #region Properties

        public int PreTrapSum { get; set; }

        public int TrapSum { get; set; }

        public int GameSum { get; set; }

        public int PlayerNumber { get; set; }

        public string PlayerName { get; set; }

        public string PlayerNameAudioPath { get; set; }

        public string PlayerBuzzer { get; set; }

        public int PlayerPosition { get; set; }

        #endregion
    }
}
