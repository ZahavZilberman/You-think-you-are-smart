using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;


namespace standup
{
    public class Game
    {
        #region ctor

        public Game(List<Player> players, int NumOfQuestions, int NumOfPlayers)
        {
            CountOfQuestion = 0;
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
