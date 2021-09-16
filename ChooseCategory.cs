using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;

namespace standup
{
    public class ChooseCategory
    {
        #region ctor

        public ChooseCategory(Game game, int PlayerToChooseCategory)
        {
            PlayerNumber = PlayerToChooseCategory;
            playerTurn = game.GamePlayers.ElementAt(PlayerToChooseCategory - 1); // The variable values are 1-3, the lst indexes are 0-2
            categoriesDocument = new XDocument(@"standup/categories.xml");
            categories = new List<XElement>(categoriesDocument.Root.Elements("Category"));
            Random3Categories = new List<string>();
            randomchoice = new Random(0);
            for (int RandomCategoryCount = 0; RandomCategoryCount < 3; RandomCategoryCount++)
            {
                XElement RandomChoosenCategory = categories.ElementAt(randomchoice.Next(9)); // assuming there are 10 categories. should be enough
                Random3Categories.Add(RandomChoosenCategory.Value);
            }
            game.IsOnChooseCatgoryMode = true;
            PathToPlayerNameSound = playerTurn.PlayerNameAudioPath;

            MusicPath = @"standup/choosecatgeory.mp3";
        }

        #endregion

        #region Properties

        public int PlayerNumber { get; set; }
        public Player playerTurn { get; set; }
        public XDocument categoriesDocument { get; set; }
        public List<XElement> categories { get; set; }
        public List<string> Random3Categories { get; set; }
        public Random randomchoice { get; set; }
        public string choosenCategory { get; set; }
        public string PathToPlayerNameSound { get; set; }
        public string MusicPath { get; set; }

        #endregion
    }
}
