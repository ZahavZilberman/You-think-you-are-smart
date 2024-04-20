using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Media;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.Numerics;
using static System.Formats.Asn1.AsnWriter;
using NAudio.Utils;


namespace standup
{
    public class Game
    {
        public static volatile bool _stopRequestedForEveryoneAnswering = false;
        public static volatile bool _stopRequestedForSomeoneAnswering = false;


        #region ctor

        public Game(List<Player> players, int NumOfQuestions, int NumOfPlayers)
        {
            PlayerToChooseCategory = players.ElementAt(0);
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
            IsOnNormalMode = false;

            string currentPlayerturnName = GamePlayers.ElementAt(0).PlayerName;
            /*
            XDocument questions = new XDocument($@"You think you are smart xml\Questions.xml");
            XElement questionsXMLstart = questions.Root;
            List<XElement> questionsXML = new List<XElement>(questionsXMLstart.Elements("Question"));

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
                QuestionText = File.ReadAllLines(textPath).ToList();                
            }
            */

            /*
            ChooseCategory QuestionCategoryDetails = ChooseCategoryFunction(game, game.GamePlayers.ElementAt(0));

            foreach (Player player in GamePlayers)
            {
                if (player.PlayerNumber == QuestionCategoryDetails.PlayerNumber)
                {
                    PlayerToChooseCategory = player;
                }
            }
            */
        }

        #endregion

        #region Instructions part

        public void Instructions()
        {
            #region The buttons

            Console.Clear();

            #endregion

            #region The general instructions and buzzers

            #region Instructions

            Console.Clear();
            Console.WriteLine($@"So you've choosen {NumOfGameQuestions} questions..");
            Console.WriteLine();

            Console.WriteLine("Now, The general instructions.. you can hear them..");
            Console.WriteLine();
            Console.WriteLine("Press any key to skip this repetitive shit.");

            SoundPlayer InstructionsAudio = new SoundPlayer(@"You-think-you-are-smart\OtherSounds\Instructions.wav");
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

            double SecondsThatPassed = 0;

            InstructionsAudio.Play();
            while (!Console.KeyAvailable && SecondsThatPassed < 38)
            {
                Thread.Sleep(100);
                SecondsThatPassed = SecondsThatPassed + 0.1;
            }
            SoundPlayer InstructionsOtherAudio = new SoundPlayer(@"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav");
            if (Console.KeyAvailable == true)
            {
                InstructionsAudio.Stop();
                InstructionsOtherAudio.Play();
                Thread.Sleep(3750);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                Console.Clear();
            }
            if (SecondsThatPassed >= 38)/*DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute*/
            {
                Console.Clear();
                Thread.Sleep(2000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }

            #endregion

            #region Buzzers

            Console.WriteLine("Player 1 buzzer button is: M");
            Console.WriteLine("Player 2 buzzer is: +");
            Console.WriteLine("Player 3 buzzer is: A");
            Console.WriteLine("Press on enter to confirm you will remember this.");
            Console.WriteLine();
            Console.ReadLine();
            Console.Clear();

            #endregion

        }
        #endregion

        #endregion

        #region Game start

        public void GameStart(Game game)
        {

            #region Choosing category and determating if the next question is going to be normal or else            
            /*
            for (int CurrentQuestionNum = 0; CurrentQuestionNum == 0NumOfQuestions; CurrentQuestionNum++)
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

            
            

            Random FactGameChance = new Random();
            int WillFactGameHappenRange = FactGameChance.Next(1, 10);
            bool WillFactGameHappen = WillFactGameHappenRange == 1 || WillFactGameHappenRange == 10;

            Random WhoAmIGameChance = new Random();
            int WillWhoAmIHappenRange = FactGameChance.Next(11, 20);
            bool WillWhoAmIHappen = WillFactGameHappenRange == 11 || WillFactGameHappenRange == 20;

            if (WillFactGameHappen && !WillWhoAmIHappen)
            {
                IsOnCorrectOrFalseMode = true;
                CorrectOrFalse FactGame = new CorrectOrFalse(game, game.GamePlayers.ElementAt(0).PlayerNumber);
                IsOnNormalMode = true;
                string note = "just to make sure this is why the console app sometimes shuts off";
            }
            else if (!WillFactGameHappen && WillWhoAmIHappen)
            {
                IsOnWhoAmIMode = true;
                WhoAmI whoamigame = new WhoAmI(game);
                IsOnNormalMode = true;
                string note2 = "just to make sure this is why the console app sometimes shuts off";
            }
            else if (WillFactGameHappen && WillWhoAmIHappen)
            {
                IsOnNormalMode = true;
            }
            else if (!WillWhoAmIHappen && !WillFactGameHappen)
            {
                IsOnNormalMode = true;
            }            
            
            #endregion

            IsOnNormalMode = true;

            if (IsOnNormalMode)
            {
                //WhoAmIGame(game, false);
                //CorrectOrFalseGame(game);
                //TrapMode(game);
                //ChooseCategory QuestionCategoryDetails = ChooseCategoryFunction(game, game.GamePlayers.ElementAt(0));
                //game = NormalQuestion(game, QuestionCategoryDetails, false);               
                return;
                /*
                while(game.CountOfQuestion < game.NumOfGameQuestions)
                {
                    NextTurn(game, game.PlayerToChooseCategory);
                }
                */
            }
        }

        #endregion

        #region Next turn

        public void NextTurn(Game game, Player playerToChooseCategory)
        {
            for(int i = 0; i < game.NumOfGameQuestions; i++)
            {                
                #region Each question..

                game.CountOfQuestion = i + 1;

                #region determating if the next question is going to be normal or else            
                /*
                for (int CurrentQuestionNum = 0; CurrentQuestionNum == 0NumOfQuestions; CurrentQuestionNum++)
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

                Random FactGameChance = new Random();
                int WillFactGameHappenRange = FactGameChance.Next(1, 10);
                bool WillFactGameHappen = WillFactGameHappenRange == 9 || WillFactGameHappenRange == 2;

                Random WhoAmIGameChance = new Random();
                int WillWhoAmIHappenRange = WhoAmIGameChance.Next(1, 10);
                bool WillWhoAmIHappen = WillWhoAmIHappenRange == 8 || WillWhoAmIHappenRange == 1;

                if (WillFactGameHappen && !WillWhoAmIHappen)
                {
                    IsOnCorrectOrFalseMode = true;
                    IsOnWhoAmIMode = false;
                    IsOnNormalMode = false;
                    string note = "just to make sure this is why the console app sometimes shuts off";
                }
                else if (!WillFactGameHappen && WillWhoAmIHappen)
                {
                    IsOnWhoAmIMode = true;
                    IsOnNormalMode = false;
                    IsOnCorrectOrFalseMode = false;
                    string note2 = "just to make sure this is why the console app sometimes shuts off";
                }
                else if (WillFactGameHappen && WillWhoAmIHappen)
                {
                    IsOnWhoAmIMode = false;
                    IsOnNormalMode = true;
                    IsOnCorrectOrFalseMode = false;
                }
                else if (!WillWhoAmIHappen && !WillFactGameHappen)
                {
                    IsOnWhoAmIMode = false;
                    IsOnNormalMode = true;
                    IsOnCorrectOrFalseMode = false;
                }

                #endregion

                if (IsOnNormalMode)
                {
                    //
                    //CorrectOrFalseGame(game);
                    //TrapMode(game);
                    ChooseCategory QuestionCategoryDetails = ChooseCategoryFunction(game, game.GamePlayers.ElementAt(0));
                    game = NormalQuestion(game, QuestionCategoryDetails, false, true, 10000);               
                    /*
                    while(game.CountOfQuestion < game.NumOfGameQuestions)
                    {
                        NextTurn(game, game.PlayerToChooseCategory);
                    }
                    */
                }
                if(IsOnWhoAmIMode)
                {
                    game = WhoAmIGame(game, false);
                }
                if(IsOnCorrectOrFalseMode)
                {
                    game = CorrectOrFalseGame(game);
                }

                #endregion
            }
            game = TrapMode(game);
            GameOver(game);
            return;
        }

        #endregion

        #region Choose Category

        public ChooseCategory ChooseCategoryFunction(Game game, Player PlayerToChoose)
        {
            ChooseCategory choosingthis = new ChooseCategory(game, PlayerToChoose.PlayerNumber);

            Console.Clear();
            Console.WriteLine($@"{PlayerToChoose.PlayerName} (${PlayerToChoose.PreTrapSum}), choose category:");
            Console.WriteLine();

            Random DisplayingCategories1 = new Random();
            int FirstCategoryDisplayed = DisplayingCategories1.Next(0, choosingthis.AllCategoriesNames.Count);
            Random DisplayingCategories2 = new Random();
            int SecondCategoryDisplayed = DisplayingCategories1.Next(0, choosingthis.AllCategoriesNames.Count);
            while (SecondCategoryDisplayed == FirstCategoryDisplayed)
            {
                SecondCategoryDisplayed = DisplayingCategories1.Next(0, choosingthis.AllCategoriesNames.Count);
            }

            Random MoneyFirstCategory = new Random();
            int Money1stCategory = (MoneyFirstCategory.Next(1, 4)) * 1000;
            Random MoneySecondCategory = new Random();
            int Money2ndCategory = (MoneyFirstCategory.Next(1, 4)) * 1000;

            Console.WriteLine($@"1. {choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed)} ({Money1stCategory} dollars)");
            Console.WriteLine();
            Console.WriteLine($@"2. {choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed)} ({Money2ndCategory} dollars)");
            Console.WriteLine();


            SoundPlayer PlayerSound = new SoundPlayer($@"You-think-you-are-smart\NameSounds\Player{PlayerToChoose.PlayerNumber}.wav");
            PlayerSound.Play();
            Thread.Sleep(1800);
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            SoundPlayer ChooseCategory = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\choosecategory.wav");
            ChooseCategory.Play();
            Thread.Sleep(2000);
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            int ChoosenCategoryNumber = 0;

            Random doubleM = new Random();
            int chanceOfDoubleRound = doubleM.Next(1, 7);
            bool IsDoubleRound = chanceOfDoubleRound == 3;

            #region The nightmare of the automatic respond after 13 seconds

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            int startSecond = DateTime.Now.Second;
            int startMinute = DateTime.Now.Minute;
            int endSecond;
            int endMinute;
            if (startSecond <= 47)
            {
                endMinute = startMinute;
                endSecond = startSecond + 13;
            }
            else
            {
                endMinute = startMinute + 1;
                endSecond = startSecond - 47;
            }
            double SecondsThatPassed = 0;
            SoundPlayer CategoryMusic = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\CategoryMusic.wav");
            CategoryMusic.Play();
            string TheKeyPressed = "";

            while (!Console.KeyAvailable || (DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute))
            {
                Thread.Sleep(100);
                SecondsThatPassed = SecondsThatPassed + 0.1;
                if (SecondsThatPassed >= 13)
                {
                    break;
                }
            }

            if (SecondsThatPassed >= 13)
            {
                CategoryMusic.Stop();
                choosingthis.IsRandomCategoryHasBeenChoose = true;

                if (choosingthis.RandomChoiceIfNeeded == 0)
                {
                    choosingthis.choosenCategory = choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed);
                    choosingthis.MoneyForThisCategoryQuestion = Money1stCategory;
                }
                if (choosingthis.RandomChoiceIfNeeded == 1)
                {
                    choosingthis.choosenCategory = choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed);
                    choosingthis.MoneyForThisCategoryQuestion = Money2ndCategory;
                }

                SoundPlayer ZahavChoosesCategory = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\CategoryGameChooses.wav");
                ZahavChoosesCategory.Play();
                Thread.Sleep(5000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                Console.Clear();
                Console.Beep();
                if (choosingthis.RandomChoiceIfNeeded == 0)
                {
                    Console.WriteLine($@"1. {choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed)} ({Money1stCategory} dollars)");
                    Thread.Sleep(1000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
                if (choosingthis.RandomChoiceIfNeeded == 1)
                {
                    Console.WriteLine($@"2. {choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed)} ({Money2ndCategory} dollars)");
                    Thread.Sleep(1000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
            }

            else
            {
                choosingthis.IsRandomCategoryHasBeenChoose = false;
                ConsoleKeyInfo ChoosenCategory;

                //ChooseCategoryMusic.Play(); 
                if (Console.KeyAvailable == true)
                {
                    ChoosenCategory = Console.ReadKey(true);
                    while (ChoosenCategory.Key.ToString() != "D1" && ChoosenCategory.Key.ToString() != "D2" && ChoosenCategory.Key.ToString() != "Escape")
                    {
                        Console.WriteLine("Press on a number between 1-2 only!");
                        ChoosenCategory = Console.ReadKey(true);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                    }


                    CategoryMusic.Stop();
                    TheKeyPressed = ChoosenCategory.Key.ToString();
                    Console.Beep();
                    Console.Clear();

                    if (TheKeyPressed == "D1")
                    {
                        Console.Beep();
                        Console.WriteLine($@"1. {choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed)} (${Money1stCategory})");
                        Thread.Sleep(1000);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        choosingthis.choosenCategory = choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed);
                        /*
                        if(IsDoubleRound)
                        {
                          
                        choosingthis.MoneyForThisCategoryQuestion = Money1stCategory * 2;
                        */
                        /*
                        else
                        {
                          */
                        choosingthis.MoneyForThisCategoryQuestion = Money1stCategory;
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        //}

                    }
                    if (TheKeyPressed == "D2")
                    {
                        Console.Beep();
                        Console.WriteLine($@"2. {choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed)} $({Money2ndCategory})");
                        Thread.Sleep(1000);
                        choosingthis.choosenCategory = choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed);
                        /*
                        if (IsDoubleRound)
                        {
                            choosingthis.MoneyForThisCategoryQuestion = Money2ndCategory * 2;
                        }
                        else
                        {
                         */
                        choosingthis.MoneyForThisCategoryQuestion = Money2ndCategory;
                        //}
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                    }
                    if (TheKeyPressed == "Escape")
                    {
                        Console.Beep();
                        Console.Clear();
                        Console.WriteLine("The game has been paused. Press enter to return to choosing category.");
                        Console.ReadLine();
                        choosingthis = ChooseCategoryFunction(game, PlayerToChoose);
                        return choosingthis;
                    }
                }
            }

            if (DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute)
            {
                // I think this is what happens if the user doesn't skip the instructions
                if (choosingthis.RandomChoiceIfNeeded == 0)
                {
                    choosingthis.choosenCategory = choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed);
                }
                if (choosingthis.RandomChoiceIfNeeded == 1)
                {
                    choosingthis.choosenCategory = choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed);
                }
            }

            #endregion


            SoundPlayer QuestionAbout = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\QuestionIsAbout...wav");
            QuestionAbout.Play();
            Thread.Sleep(2000);
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }
            QuestionAbout.Stop();
            SoundPlayer Topic = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\{choosingthis.choosenCategory}.wav");
            Topic.Play();
            Thread.Sleep(3000);
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }
            Topic.Stop();
            SoundPlayer money1000 = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\1000dollars.wav");
            SoundPlayer money2000 = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\2000dollars.wav");
            SoundPlayer money3000 = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\3000dollars.wav");
            SoundPlayer money4000 = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\4000dollars.wav");
            SoundPlayer money6000 = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\6000dollars.wav");

            if (choosingthis.MoneyForThisCategoryQuestion == 1000)
            {
                money1000.Play();
                Thread.Sleep(2000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }
            else if (choosingthis.MoneyForThisCategoryQuestion == 2000)
            {
                money2000.Play();
                Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }
            else if (choosingthis.MoneyForThisCategoryQuestion == 3000)
            {
                money3000.Play();
                Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }
            else if (choosingthis.MoneyForThisCategoryQuestion == 4000)
            {
                money4000.Play();
                Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }
            else if (choosingthis.MoneyForThisCategoryQuestion == 6000)
            {
                money6000.Play();
                Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }

            Console.Clear();
            string BlankSpace = "";
            for (int i = 0; i < 25; i++)
            {
                BlankSpace = BlankSpaces(i + 1);
                Console.Write($@"{BlankSpace}Question {game.CountOfQuestion}");
                Thread.Sleep(40);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                Console.Clear();
            }

            Console.Clear();


            if (IsDoubleRound)
            {

                Console.WriteLine("The double round!");
                if (!choosingthis.IsRandomCategoryHasBeenChoose)
                {
                    if (TheKeyPressed == "D1")
                    {
                        choosingthis.MoneyForThisCategoryQuestion = Money1stCategory * 2;
                    }
                    if (TheKeyPressed == "D2")
                    {
                        choosingthis.MoneyForThisCategoryQuestion = Money2ndCategory * 2;
                    }
                }
                if (choosingthis.IsRandomCategoryHasBeenChoose)
                {
                    if (choosingthis.RandomChoiceIfNeeded == 0)
                    {
                        choosingthis.MoneyForThisCategoryQuestion = Money1stCategory * 2;
                    }
                    if (choosingthis.RandomChoiceIfNeeded == 1)
                    {
                        choosingthis.MoneyForThisCategoryQuestion = Money2ndCategory * 2;
                    }
                }

                SoundPlayer DoubleRound = new SoundPlayer($@"You-think-you-are-smart\CategorySounds\DoubleRound.wav");
                DoubleRound.Play();
                Thread.Sleep(3500);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                if (choosingthis.MoneyForThisCategoryQuestion == 1000)
                {
                    money1000.Play();
                    Thread.Sleep(2000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
                else if (choosingthis.MoneyForThisCategoryQuestion == 2000)
                {
                    money2000.Play();
                    Thread.Sleep(3000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
                else if (choosingthis.MoneyForThisCategoryQuestion == 3000)
                {
                    money3000.Play();
                    Thread.Sleep(3000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
                else if (choosingthis.MoneyForThisCategoryQuestion == 4000)
                {
                    money4000.Play();
                    Thread.Sleep(3000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
                else if (choosingthis.MoneyForThisCategoryQuestion == 6000)
                {
                    money6000.Play();
                    Thread.Sleep(3000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }
            }

            //choosingthis.PlayerNumber;

            int PlayerChoosingNext = 0;
            Console.Clear();

            if (PlayerToChoose.PlayerNumber < 3)
            {
                if (game.GamePlayers.Count == 1)
                {
                    choosingthis.PlayerNumber = 1;
                }
                if (GamePlayers.Count == 2)
                {
                    if (choosingthis.playerTurn.PlayerName == game.GamePlayers.ElementAt(1).PlayerName)
                    {
                        choosingthis.PlayerNumber = 1;
                    }
                    if (choosingthis.playerTurn.PlayerName == game.GamePlayers.ElementAt(0).PlayerName)
                    {
                        choosingthis.PlayerNumber = 2;
                    }
                }
                if (GamePlayers.Count == 3)
                {
                    if (choosingthis.playerTurn.PlayerName == game.GamePlayers.ElementAt(0).PlayerName)
                    {
                        choosingthis.PlayerNumber = 2;
                    }
                    if (choosingthis.playerTurn.PlayerName == game.GamePlayers.ElementAt(1).PlayerName)
                    {
                        choosingthis.PlayerNumber = 3;
                    }
                    if (choosingthis.playerTurn.PlayerName == game.GamePlayers.ElementAt(2).PlayerName)
                    {
                        choosingthis.PlayerNumber = 1;
                    }
                }
            }

            if (PlayerToChoose.PlayerNumber == 3)
            {
                choosingthis.PlayerNumber = 1;
            }

            return choosingthis;

            // Now call the "Normal game questioning"..

            //SoundPlayer Money = new SoundPlayer($@"");

            /*
            ConsoleKeyInfo ChoosenCategory = Console.ReadKey(true);
            while (ChoosenCategory.Key.ToString() != "D1" && ChoosenCategory.Key.ToString() != "D2")
            {
                Console.WriteLine("Press on a number between 1-2 only!");
                ChoosenCategory = Console.ReadKey(true);
            }
            */


            //Console.Clear();
            //Console.WriteLine($@"The question is about: {}");

        }

        #endregion

        #region Timer for all modes but trap mode

        public static void RunTimerForNormalModeEveryone()
        {
            int count = 0;
            while (!_stopRequestedForEveryoneAnswering && count < 10)
            {
                for (int i = 0; i <= 10; i++)
                {
                    count = i;
                    Thread.Sleep(1000); // Wait for one second
                }
            }

        }

        public static void RunTimerForNormalModeSomeone()
        {
            int count = 0;

            while (!_stopRequestedForSomeoneAnswering && count < 10)
            {
                for (int i = 0; i <= 10; i++)
                {
                    Console.Write($@" {10 - i}");
                    count = i;
                    Thread.Sleep(1000); // Wait for one second
                }
            }

        }

        public static void RunTimerForWhoAmIAndCorrectOrFalseModes()
        {
            for (int i = 0; i <= 10; i++)
            {
                Console.Write($@" {10 - i}");
                Thread.Sleep(1000); // Wait for one second
            }
        }

        #endregion

        #region Thread for playing game over music

        public static void RunGameOverMusic()
        {

        }

        #endregion

        #region Normal question happening

        public Game NormalQuestion(Game game, ChooseCategory QuestionDetails, bool HasTheQuestionBeenReadItself, bool IsANewQuestionLoaded, int TheChoosenQuestion)
        {
            if(IsANewQuestionLoaded)
            {
                for(int i = 0; i < game.GamePlayers.Count; i++)
                {
                    game.GamePlayers.ElementAt(i).HasThePlayerChoosenAnswer = false;
                    game.GamePlayers.ElementAt(i).WrongAnswerChoosen = 0;
                }
                /*
                foreach(Player player in game.GamePlayers)
                {
                    player.HasThePlayerChoosenAnswer = false;
                    player.WrongAnswerChoosen = 0;
                }
                */
            }

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            NormalQuestion TheQuestion = new NormalQuestion(game, QuestionDetails.MoneyForThisCategoryQuestion, "", game.CountOfQuestion, 0, 0, "", QuestionDetails.choosenCategory);

            #region Opening object data            

            DirectoryInfo SubjectTextDirectory = new DirectoryInfo($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}");
            //SubjectTextDirectory.
            DirectoryInfo[] AllQuestionsFolders = SubjectTextDirectory.GetDirectories();

            int ChoosenQuestion = 0;
            int AmountOfQuestions = AllQuestionsFolders.Count();

            if (!HasTheQuestionBeenReadItself)
            {
                Random rnd = new Random();
                ChoosenQuestion = rnd.Next(0, AmountOfQuestions);
                TheChoosenQuestion = ChoosenQuestion;
            }
            else
            {
                ChoosenQuestion = TheChoosenQuestion;
            }

            List<DirectoryInfo> QuestionsFoldersList = AllQuestionsFolders.ToList();
            string Answer1Text = "";
            string Answer2Text = "";
            string Answer3Text = "";
            string Answer4Text = "";
            List<string> AllAnswers = new List<string>();
            DirectoryInfo CorrectFolder = new DirectoryInfo($@"You-think-you-are-smart");

            foreach (DirectoryInfo Folder in AllQuestionsFolders)
            {
                if (ChoosenQuestion == QuestionsFoldersList.IndexOf(Folder))
                {

                    DirectoryInfo ChoosenFolder = new DirectoryInfo($@"{QuestionsFoldersList.Find(x => x == Folder)}");
                    CorrectFolder = Folder;
                    FileInfo[] files2 = ChoosenFolder.GetFiles();
                    List<FileInfo> ListOfFiles2 = files2.ToList();

                    foreach (FileInfo file in ListOfFiles2)
                    {
                        if (file.Name != "Answer1.txt" && file.Name != "Answer2.txt" && file.Name != "Answer3.txt" && file.Name != "Answer4.txt")
                        {
                            //$@"C:\Users\zahav\OneDrive\Desktop\my own software\Standup3\Standup3\bin\Debug\net8.0\You-think-you-are-smart\QuestionsText\Subject3\Question5\Answer1.txt"
                            {
                                #region Openining its properties from the XML

                                XDocument QuestionXML = new XDocument(XDocument.Load($@"You think you are smart xml\Questions.xml"));
                                IEnumerable<XElement> AllQuestions = QuestionXML.Root.Elements("Question");
                                List<XElement> AllQuestionsList = AllQuestions.ToList();
                                List<XElement> TheSubjectQuestions = new List<XElement>();

                                foreach (XElement AnyQuestion in AllQuestionsList)
                                {
                                    XElement QuestionSubject = AnyQuestion.Element("Subject");
                                    if (QuestionSubject.Value == QuestionDetails.choosenCategory)
                                    {
                                        TheSubjectQuestions.Add(AnyQuestion);
                                    }
                                }


                                foreach (XElement question in TheSubjectQuestions)
                                {
                                    XElement QuestionNameElement = question.Element("Name");
                                    string QuestionName = QuestionNameElement.Value;

                                    XElement textPath = question.Element("Text");
                                    string XMLTextPath = textPath.Value;

                                    //if(XMLTextPath == $@"")
                                    string CheckedPath = $@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{QuestionsFoldersList.Find(x => x == Folder).Name}\{file.Name}";
                                    if (XMLTextPath == CheckedPath)
                                    {
                                        TheQuestion.QuestionNumber = int.Parse(question.Element("Number").Value);
                                        TheQuestion.SoundPath = question.Element("Sound").Value;
                                        TheQuestion.CorrectAnswer = int.Parse(question.Element("Answer").Value);
                                        TheQuestion.QuestionNumber = int.Parse(question.Element("Number").Value);
                                        TheQuestion.Subject = question.Element("Subject").Value;
                                        TheQuestion.QuestionName = QuestionName;
                                        TheQuestion.QuestionText = XMLTextPath;
                                    }
                                }


                                #endregion

                                Answer1Text = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == Folder).FullName}\Answer1.txt");
                                Answer2Text = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == Folder).FullName}\Answer2.txt");
                                Answer3Text = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == Folder).FullName}\Answer3.txt");
                                Answer4Text = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == Folder).FullName}\Answer4.txt");

                                AllAnswers.Add(Answer1Text);
                                AllAnswers.Add(Answer2Text);
                                AllAnswers.Add(Answer3Text);
                                AllAnswers.Add(Answer4Text);
                                break;
                            }
                        }


                    }
                }
            }

            /*
            FileInfo[] files = SubjectTextDirectory.GetFiles();

            List<FileInfo> ListOfFiles = files.ToList();
            foreach (FileInfo file in ListOfFiles)
            {
                if (ChoosenQuestion == ListOfFiles.IndexOf(file))
                {
                    TheQuestion.QuestionText = ($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{file.Name}");

                    #region Openining its properties from the XML

                    XDocument QuestionXML = new XDocument(XDocument.Load($@"You think you are smart xml\Questions.xml"));
                    IEnumerable<XElement> AllQuestions = QuestionXML.Root.Elements("Question");
                    List<XElement> AllQuestionsList = AllQuestions.ToList();
                    foreach (XElement question in AllQuestionsList)
                    {
                        XElement textPath = question.Element("Text");
                        string XMLTextPath = textPath.Value;
                        if (XMLTextPath == TheQuestion.QuestionText)
                        {
                            TheQuestion.QuestionNumber = int.Parse(question.Element("Number").Value);
                            TheQuestion.SoundPath = question.Element("Sound").Value;
                            TheQuestion.CorrectAnswer = int.Parse(question.Element("Answer").Value);
                            TheQuestion.QuestionNumber = int.Parse(question.Element("Number").Value);
                            TheQuestion.Subject = question.Element("Subject").Value;
                        }
                    }


                    #endregion
                }
            }
            */

            Console.Clear();
            Console.WriteLine($@"{TheQuestion.Subject}                                                             ${TheQuestion.MoneyOnTable}");
            Console.WriteLine("");
            Console.WriteLine("");
            string[] QuestionActualTextArray = File.ReadAllLines(TheQuestion.QuestionText);
            List<string> QuestionActualTextList = new List<string>();
            QuestionActualTextList = QuestionActualTextArray.ToList();

            #endregion

            #region Writing the question

            List<int> WrongAnswersGiven = new List<int>();
            foreach (Player player in game.GamePlayers)
            {
                if (player.HasThePlayerChoosenAnswer && player.WrongAnswerChoosen != 0)
                {
                    if (player.WrongAnswerChoosen == 1)
                    {
                        AllAnswers.Remove(Answer1Text);
                    }
                    if (player.WrongAnswerChoosen == 2)
                    {
                        AllAnswers.Remove(Answer2Text);
                    }
                    if (player.WrongAnswerChoosen == 3)
                    {
                        AllAnswers.Remove(Answer3Text);
                    }
                    if (player.WrongAnswerChoosen == 4)
                    {
                        AllAnswers.Remove(Answer4Text);
                    }
                }

            }

            foreach (string QuestionActualContent in QuestionActualTextList)
            {
                Console.WriteLine($@"{QuestionActualContent}");
            }
            Console.WriteLine("");

            foreach (string answer in AllAnswers)
            {
                Console.WriteLine($"{answer}");
            }

            bool Player1AnsweredOrNot = false;
            bool Player2AnsweredOrNot = false;
            bool Player3AnsweredOrNot = true;

            if (game.GamePlayers.Count == 1)
            {
                Player1AnsweredOrNot = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
            }
            else if (game.GamePlayers.Count == 2)
            {
                Player1AnsweredOrNot = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                Player2AnsweredOrNot = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
            }
            else if (game.GamePlayers.Count == 3)
            {
                Player1AnsweredOrNot = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                Player2AnsweredOrNot = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                Player3AnsweredOrNot = game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer;
            }

            Console.WriteLine("");
            if (game.GamePlayers.Count == 1)
            {
                if (!Player1AnsweredOrNot)
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (Wrong..)");
                    Console.WriteLine("");
                }
            }

            if (game.GamePlayers.Count == 2)
            {
                if (!Player1AnsweredOrNot)
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) (Wrong..)");
                    Console.WriteLine("");
                }

                if (!Player2AnsweredOrNot)
                {
                    Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) ({game.GamePlayers.ElementAt(1).PlayerBuzzer})");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) (Wrong..)");
                    Console.WriteLine("");
                }
            }

            if (game.GamePlayers.Count == 3)
            {
                if (!Player1AnsweredOrNot)
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) (Wrong..)");
                    Console.WriteLine("");
                }

                if (!Player2AnsweredOrNot)
                {
                    Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) ({game.GamePlayers.ElementAt(1).PlayerBuzzer})");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) (Wrong..)");
                    Console.WriteLine("");
                }

                if (!Player3AnsweredOrNot)
                {
                    Console.WriteLine($@"3. {game.GamePlayers.ElementAt(2).PlayerName} (${game.GamePlayers.ElementAt(2).PreTrapSum}) ({game.GamePlayers.ElementAt(2).PlayerBuzzer})");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine($@"3. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) (Wrong..)");
                }
            }

            #endregion

            // yuval wants hint for the normal questions as well
            // yuval wants a chaser or something
            Console.WriteLine();

            #region The nightmare of the automatic respond after 10 seconds

            #region Opening the basic time

            int startSecond = DateTime.Now.Second;
            int startMinute = DateTime.Now.Minute;
            int endSecond;
            int endMinute;
            if (startSecond <= 50)
            {
                endMinute = startMinute;
                endSecond = startSecond + 10;
            }
            else
            {
                endMinute = startMinute + 1;
                endSecond = startSecond - 50;
            }

            #endregion

            #region The time for the question itself to be read

            if (!HasTheQuestionBeenReadItself)
            {
                Console.WriteLine("5 seconds to read the question itself..");
                Console.WriteLine("Next 10 seconds will be to actually answer.");
                SoundPlayer QuestionSaying = new SoundPlayer($@"{TheQuestion.SoundPath}");
                QuestionSaying.Play();
                Thread.Sleep(5000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }

            #endregion

            //Thread timerThread = new Thread(new ThreadStart(RunTimerForNormalModeEveryone));
            //timerThread.Start();

            double SecondsThatPassed = 0;
            SoundPlayer QuestionMusic = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\NormalQuestionMusic.wav");
            QuestionMusic.Play();
            string TheKeyPressed = "";
            Console.WriteLine("");
            ConsoleKeyInfo buzzer = new ConsoleKeyInfo();
            while (!Console.KeyAvailable && SecondsThatPassed < 10)
            {

                Thread.Sleep(1000);
                SecondsThatPassed = SecondsThatPassed + 1;
                Console.Write($@" {10 - SecondsThatPassed}");
                if (SecondsThatPassed == 10)
                {
                    break;
                }
            }

            #region In case nobody answers to begin with

            if (SecondsThatPassed == 10)
            {
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                }

                QuestionMusic.Stop();

                SoundPlayer NoAnswer = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav");
                NoAnswer.Play();
                Thread.Sleep(6500);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                Console.Clear();
                Console.Beep();
                Console.WriteLine($@"The correct answer (in my opinion) is:");
                Console.WriteLine($@"");
                string CorrectAnswer = "";
                string CorrectAnswerContent = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == CorrectFolder).FullName}\Answer{TheQuestion.CorrectAnswer}.txt");
                // ($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{QuestionsFoldersList.Find(x => x == CorrectFolder).Name}\Answer{TheQuestion.CorrectAnswer}.txt");

                Console.WriteLine($@"{CorrectAnswerContent}");

                Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                Console.Clear();
                return game;
            }

            #endregion

            else
            {
                /*
                if(TheQuestion.AreEveryoneWrong)
                {
                    QuestionMusic.Stop();

                    SoundPlayer EveryoneWrong = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\EveryoneAreWrong.wav");
                    EveryoneWrong.Play();
                    Thread.Sleep(4000);

                    Console.Clear();
                    Console.Beep();
                }*/

                //ChooseCategoryMusic.Play(); 

                if (Console.KeyAvailable == true)
                {
                    ConsoleKeyInfo consoleKeyInfo = new ConsoleKeyInfo();
                    consoleKeyInfo = Console.ReadKey(true);

                    #region Throwing wrong key press                       

                    if (consoleKeyInfo.Key.ToString().ToUpper() != "A" && consoleKeyInfo.Key.ToString().ToUpper() != "M" && consoleKeyInfo.Key.ToString().ToUpper() != "OEMPLUS")
                    {
                        SoundPlayer buzzerexpection = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav");
                        buzzerexpection.Play();
                        Thread.Sleep(5000);
                        throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                        /*
                         * 
                            //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                            // a sound can be put in here
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }
                            consoleKeyInfo = Console.ReadKey(true);
                            Console.Write($@"{10 - SecondsThatPassed}");
                            Thread.Sleep(1000);
                            SecondsThatPassed = SecondsThatPassed + 1;

                            //consoleKeyInfo = Console.ReadKey(true);
                        */
                    }


                    #endregion

                    #region Ensuring relevant buzzer has been pressed


                    if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "A" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS")
                    {
                        bool IsTheBuzzerCorrect = false;

                        if (game.GamePlayers.Count == 1)
                        {
                            if (consoleKeyInfo.Key.ToString().ToUpper() == "M")
                            {
                                IsTheBuzzerCorrect = true;
                            }
                        }
                        if (game.GamePlayers.Count == 2)
                        {
                            if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS")
                            {
                                IsTheBuzzerCorrect = true;
                            }
                        }
                        if (game.GamePlayers.Count == 3)
                        {
                            if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS" || consoleKeyInfo.Key.ToString().ToUpper() == "A")
                            {
                                IsTheBuzzerCorrect = true;
                            }
                        }

                        if (!IsTheBuzzerCorrect)
                        {
                            #region Throwing wrong key press                       

                            SoundPlayer buzzerexpection = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav");
                            buzzerexpection.Play();
                            Thread.Sleep(5000);
                            throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                            /*
                             * 
                                //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                                // a sound can be put in here
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }
                                consoleKeyInfo = Console.ReadKey(true);
                                Console.Write($@"{10 - SecondsThatPassed}");
                                Thread.Sleep(1000);
                                SecondsThatPassed = SecondsThatPassed + 1;

                                //consoleKeyInfo = Console.ReadKey(true);
                            */

                            #endregion
                        }

                        #endregion

                        #region What to do when a player buzzers, before he answers..

                        string ActivatedBuzzer = consoleKeyInfo.Key.ToString().ToUpper();

                        #region If it's a player who already answered..

                        bool Player1AnsweredAlready = false;
                        bool Player2AnsweredAlready = false;
                        bool Player3AnsweredAlready = false;

                        if (game.GamePlayers.Count == 1)
                        {
                            Player1AnsweredAlready = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && ActivatedBuzzer == "M";
                        }
                        else if (game.GamePlayers.Count == 2)
                        {
                            Player1AnsweredAlready = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && ActivatedBuzzer == "M";
                            Player2AnsweredAlready = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && ActivatedBuzzer == "OEMPLUX";
                        }
                        else if (game.GamePlayers.Count == 3)
                        {
                            Player1AnsweredAlready = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && ActivatedBuzzer == "M";
                            Player2AnsweredAlready = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && ActivatedBuzzer == "OEMPLUX";
                            Player3AnsweredAlready = game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer && ActivatedBuzzer == "A";
                        }

                        if (game.GamePlayers.Count == 2)
                        {
                            if (Player1AnsweredAlready || Player2AnsweredAlready)
                            {
                                SoundPlayer ReBuzzerException = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\PressingBuzzerAgainException.wav");
                                ReBuzzerException.Play();
                                Thread.Sleep(5000);
                                throw new Exception(" The sound you just heard explains this.. Restart the game. ");
                            }
                        }
                        if (game.GamePlayers.Count == 3)
                        {
                            if (Player1AnsweredAlready || Player2AnsweredAlready || Player3AnsweredAlready)
                            {
                                SoundPlayer ReBuzzerException = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\PressingBuzzerAgainException.wav");
                                ReBuzzerException.Play();
                                Thread.Sleep(5000);
                                throw new Exception(" The sound you just heard explains this.. Restart the game. ");
                            }
                        }

                        #endregion


                        double SecondsForAnswer = 0;

                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        Player playerAnswering = new Player(1, "");

                        ConsoleKeyInfo ChoosenAnswer = new ConsoleKeyInfo();
                        if (ActivatedBuzzer.ToUpper() == "M")
                        {
                            playerAnswering = new Player(game.GamePlayers.ElementAt(0).PlayerNumber, game.GamePlayers.ElementAt(0).PlayerName);
                            playerAnswering = game.GamePlayers.ElementAt(0);
                            playerAnswering.PlayerNumber = 1;
                        }
                        if (ActivatedBuzzer.ToUpper() == "A")
                        {
                            playerAnswering = new Player(game.GamePlayers.ElementAt(2).PlayerNumber, game.GamePlayers.ElementAt(2).PlayerName);
                            playerAnswering = game.GamePlayers.ElementAt(2);
                            playerAnswering.PlayerNumber = 3;
                        }
                        if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                        {
                            playerAnswering = new Player(game.GamePlayers.ElementAt(1).PlayerNumber, game.GamePlayers.ElementAt(1).PlayerName);
                            playerAnswering = game.GamePlayers.ElementAt(1);
                            playerAnswering.PlayerNumber = 2;
                        }

                        Console.Beep();
                        Console.WriteLine("");
                        Console.WriteLine($@"{game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).PlayerName} is Answering..");
                        Console.WriteLine("");

                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }

                        //Thread timerThread2 = new Thread(new ThreadStart(RunTimerForNormalModeSomeone));
                        //double SecondsForAnswer = 10;
                        //timerThread2.Start();
                        ConsoleKeyInfo anotherpress = new ConsoleKeyInfo();
                        while (!Console.KeyAvailable && SecondsForAnswer < 10)
                        {
                            Console.Write($@" {10 - SecondsForAnswer}");
                            SecondsForAnswer = SecondsForAnswer + 1;
                            Thread.Sleep(1000);
                            // do nothing and remain stuck at this part of the code
                        }

                        #endregion


                        if (Console.KeyAvailable)
                        {
                            ChoosenAnswer = Console.ReadKey(true);

                            #region Waiting for proper input

                            #region Throwing wrong key press                       

                            if (ChoosenAnswer.Key.ToString().ToUpper() != "D1" && ChoosenAnswer.Key.ToString().ToUpper() != "D2" && ChoosenAnswer.Key.ToString() != "D3" && ChoosenAnswer.Key.ToString() != "D4")
                            {
                                SoundPlayer buzzerexpection = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav");
                                buzzerexpection.Play();
                                Thread.Sleep(5000);
                                throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                                /*
                                 * 
                                    //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                                    // a sound can be put in here
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    consoleKeyInfo = Console.ReadKey(true);
                                    Console.Write($@"{10 - SecondsThatPassed}");
                                    Thread.Sleep(1000);
                                    SecondsThatPassed = SecondsThatPassed + 1;

                                    //consoleKeyInfo = Console.ReadKey(true);
                                */
                            }


                            #endregion

                            /*
                             * 
                            while (ChoosenAnswer.Key.ToString() != "D1" && ChoosenAnswer.Key.ToString() != "D2" && ChoosenAnswer.Key.ToString() != "D3" && ChoosenAnswer.Key.ToString() != "D4" && SecondsForAnswer < 10)
                            {
                                ChoosenAnswer = Console.ReadKey(true);
                                while (ChoosenAnswer.Key.ToString() != "D1" && ChoosenAnswer.Key.ToString() != "D2" && ChoosenAnswer.Key.ToString() != "D3" && ChoosenAnswer.Key.ToString() != "D4")
                                {
                                    //Console.WriteLine("Press only on the answer numbers (1-4)!");
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    Thread.Sleep(1000);
                                    Console.Write($@"{10 - SecondsForAnswer}");
                                    SecondsForAnswer = SecondsForAnswer + 1;
                                }
                            }
                            */
                            #endregion

                        }

                        if (SecondsForAnswer == 10)
                        {
                            #region what happens if the player buzzers but doesn't answer

                            QuestionMusic.Stop();

                            SoundPlayer NoAnswer = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav");
                            NoAnswer.Play();
                            Thread.Sleep(6500);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }


                            if (ActivatedBuzzer.ToUpper() == "M")
                            {
                                playerAnswering = game.GamePlayers.ElementAt(0);
                                playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                            }
                            else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                            {
                                playerAnswering = game.GamePlayers.ElementAt(1);
                                playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                                playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                            }
                            else if (ActivatedBuzzer.ToUpper() == "A")
                            {
                                playerAnswering = game.GamePlayers.ElementAt(2);
                                playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                                playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                            }

                            playerAnswering.PreTrapSum = playerAnswering.PreTrapSum - TheQuestion.MoneyOnTable;

                            Console.Beep();
                            Console.WriteLine();
                            Console.WriteLine("What, you thought you could get away with it?");
                            Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.PreTrapSum}");
                            Console.WriteLine();
                            playerAnswering.HasThePlayerChoosenAnswer = true;
                            game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).HasThePlayerChoosenAnswer = true;
                            bool HasEveryoneAnswered = false;

                            if (game.GamePlayers.Count == 1)
                            {
                                HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                            }
                            if (game.GamePlayers.Count == 2)
                            {
                                HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                            }
                            if (game.GamePlayers.Count == 3)
                            {
                                HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer;
                            }
                            if (!HasEveryoneAnswered)
                            {
                                game = NormalQuestion(game, QuestionDetails, true, false, ChoosenQuestion);
                                return game;
                            }
                            else
                            {
                                Thread.Sleep(1000);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                                }
                                SoundPlayer EveryoneAreWrong = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\EveryoneAreWrong.wav");
                                EveryoneAreWrong.Play();
                                Thread.Sleep(3500);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                                }
                                Console.Clear();
                                Console.Beep();
                                Console.WriteLine($@"The correct answer (in my opinion) is:");
                                Console.WriteLine($@"");
                                string CorrectAnswer = "";
                                string CorrectAnswerContent = File.ReadAllText($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{QuestionsFoldersList.Find(x => x == CorrectFolder).Name}\Answer{TheQuestion.CorrectAnswer}.txt");


                                Console.WriteLine($@"{CorrectAnswerContent}");

                                Thread.Sleep(3000);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }
                                Console.Clear();
                                return game;
                            }

                            #endregion
                        }

                        if ((ChoosenAnswer.Key.ToString() == "D1" || ChoosenAnswer.Key.ToString() == "D2" || ChoosenAnswer.Key.ToString() == "D3" || ChoosenAnswer.Key.ToString() == "D4"))
                        {
                            #region When he does answer..

                            _stopRequestedForSomeoneAnswering = true;

                            playerAnswering.HasThePlayerChoosenAnswer = true;

                            if (ChoosenAnswer.Key.ToString() == $@"D{TheQuestion.CorrectAnswer.ToString()}")
                            {
                                #region if the answer is correct..

                                Console.WriteLine("");
                                Console.WriteLine($@"{TheQuestion.CorrectAnswer}");

                                if (ActivatedBuzzer.ToUpper() == "M")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(0);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(1);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "A")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(2);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                                }

                                playerAnswering.PreTrapSum = playerAnswering.PreTrapSum + TheQuestion.MoneyOnTable;

                                SoundPlayer correct = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\CorrectAnswer.wav");
                                QuestionMusic.Stop();
                                correct.Play();
                                Thread.Sleep(600);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }
                                Console.WriteLine("");
                                Console.WriteLine("Oh yeah! Correct Answer!");
                                Console.WriteLine("");
                                Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.PreTrapSum}.");
                                /* stopping by to explain sound file
                                 * SoundPlayer explanation = new SoundPlayer($@"path for explanation .wav file")
                                 * explanation.Play();
                                 * Thread.Sleep(sound file length);
                                 * than, no additional user kep press is required
                                 */
                                Console.WriteLine("Press enter to continue..");
                                Console.ReadLine();
                                Console.Clear();
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }
                                return game;

                                #endregion
                            }


                            if (ChoosenAnswer.Key.ToString() != $@"D{TheQuestion.CorrectAnswer.ToString()}")
                            {
                                #region If the answer is wrong..

                                _stopRequestedForSomeoneAnswering = true;

                                if (ActivatedBuzzer.ToUpper() == "M")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(0);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(1);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "A")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(2);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                                }

                                playerAnswering.PreTrapSum = playerAnswering.PreTrapSum - TheQuestion.MoneyOnTable;


                                Console.WriteLine("");
                                Console.WriteLine($@"{ChoosenAnswer.Key.ToString().Substring(1)}");

                                SoundPlayer Incorrect = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\IncorrectAnswer.wav");
                                QuestionMusic.Stop();
                                Incorrect.Play();
                                Thread.Sleep(1200);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }

                                Console.Beep();
                                Console.WriteLine();
                                Console.WriteLine("Wrong!");
                                Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.PreTrapSum}");
                                Console.WriteLine();
                                playerAnswering.HasThePlayerChoosenAnswer = true;
                                playerAnswering.WrongAnswerChoosen = int.Parse($@"{ChoosenAnswer.Key.ToString().Substring(1)}");

                                game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).HasThePlayerChoosenAnswer = true;
                                bool HasEveryoneAnswered = false;

                                if (game.GamePlayers.Count == 1)
                                {
                                    HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                                }
                                if (game.GamePlayers.Count == 2)
                                {
                                    HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                                }
                                if (game.GamePlayers.Count == 3)
                                {
                                    HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer;
                                }
                                if (!HasEveryoneAnswered)
                                {
                                    game = NormalQuestion(game, QuestionDetails, true, false, ChoosenQuestion);
                                    return game;
                                }
                                else
                                {
                                    Thread.Sleep(1000);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                                    }
                                    SoundPlayer EveryoneAreWrong = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\EveryoneAreWrong.wav");
                                    EveryoneAreWrong.Play();
                                    Thread.Sleep(3500);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                                    }
                                    Console.Clear();
                                    Console.Beep();
                                    Console.WriteLine($@"The correct answer (in my opinion) is:");
                                    Console.WriteLine($@"");
                                    string CorrectAnswer = "";
                                    string CorrectAnswerContent = File.ReadAllText($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{QuestionsFoldersList.Find(x => x == CorrectFolder).Name}\Answer{TheQuestion.CorrectAnswer}.txt");


                                    Console.WriteLine($@"{CorrectAnswerContent}");

                                    Thread.Sleep(3000);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    Console.Clear();
                                    return game;
                                }




                                // Now to continue to the other buzzers..

                                #endregion
                            }
                        }
                    }
                }
                //Thread timerThread2 = new Thread(new ThreadStart(RunTimerForNormalMode));
                //ChoosenCategory = Console.ReadKey(true);


                //TheKeyPressed = consoleKeyInfo.Key.ToString();


                Console.Beep();
                return game;

                //Thread.Sleep(1000);
                /*
                if(IsDoubleRound)
                {

                choosingthis.MoneyForThisCategoryQuestion = Money1stCategory * 2;
                */
                /*
                else
                {
                  */
                //QuestionDetails.MoneyForThisCategoryQuestion = Money1stCategory;
                //}

            }

            /*
            if (TheKeyPressed == "D2")
            {
                Console.Beep();
                //Console.WriteLine($@"2. {QuestionDetails.AllCategoriesNames.ElementAt(SecondCategoryDisplayed)} ({Money2ndCategory} dollars)");
                Thread.Sleep(1000);
                //QuestionDetails.choosenCategory = QuestionDetails.AllCategoriesNames.ElementAt(SecondCategoryDisplayed);
                /*
                if (IsDoubleRound)
                {
                    choosingthis.MoneyForThisCategoryQuestion = Money2ndCategory * 2;
                }
                else
                {
                 */
            //QuestionDetails.MoneyForThisCategoryQuestion = Money2ndCategory;
            //}

            //}



            /*while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
            }*/



        }
        /*
        if (DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute)
        {
            // I think this is what happens if the user doesn't skip the instructions
            if (QuestionDetails.RandomChoiceIfNeeded == 0)
            {
                QuestionDetails.choosenCategory = QuestionDetails.AllCategoriesNames.ElementAt(FirstCategoryDisplayed);
            }
            if (QuestionDetails.RandomChoiceIfNeeded == 1)
            {
                QuestionDetails.choosenCategory = QuestionDetails.AllCategoriesNames.ElementAt(SecondCategoryDisplayed);
            }
        }
        */
        #endregion




        #endregion

        #endregion

        #region Trap mode

        public Game TrapMode(Game game)
        {
            Console.Clear();

            Trap trap = new Trap(game);

            #region Instructions

            #region Instructions part



            #region The general instructions and buzzers

            #region Instructions

            Console.Clear();
            Console.WriteLine("We are now going into trap mode. ");


            Console.WriteLine("Now, The general instructions.. you can hear them..");
            Console.WriteLine();
            Console.WriteLine("Press any key to skip this repetitive shit.");

            SoundPlayer InstructionsAudio = new SoundPlayer(trap.FilePathToInstructions);
            int startSecond = DateTime.Now.Second;
            int startMinute = DateTime.Now.Minute;
            int endSecond;
            int endMinute;
            if (startSecond <= 43)
            {
                endMinute = startMinute;
                endSecond = startSecond + 17;
            }
            else
            {
                endMinute = startMinute + 1;
                endSecond = startSecond - 43;
            }
            InstructionsAudio.Play();
            while (!Console.KeyAvailable || (DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute))
            {
                Thread.Sleep(100);
            }
            SoundPlayer InstructionsOtherAudio = new SoundPlayer(@"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav");
            if (Console.KeyAvailable == true)
            {
                InstructionsAudio.Stop();
                InstructionsOtherAudio.Play();
                Thread.Sleep(3750);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                Console.Clear();
            }
            if (DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute)
            {
                Console.Clear();
            }

            #endregion


            #endregion

            #endregion

            #endregion

            #region Loop of questions

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
            }

            Random rnd = new Random();

            for (int i = 0; i < 4; i++)
            {
                Console.Clear();

                int ChoosenQuestion = rnd.Next(0, trap.Questions.Count);
                TrapQuestion question = trap.AllQuestions.ElementAt(ChoosenQuestion);

                #region The time for the question itself to be read

                Console.WriteLine("");
                //Console.WriteLine("4 seconds to read the question itself..");
                //Console.WriteLine("Next 5 seconds will be to actually answer.");
                Console.WriteLine($@"{question.QuestionText}");
                Thread.Sleep(4000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }


                #endregion

                #region Writing the answers and responses

                List<int> WrongAnswersGiven = new List<int>();

                Console.WriteLine("");

                for (int PossibleAnswer = 0; PossibleAnswer < 4; PossibleAnswer++)
                {
                    Console.Clear();

                    Console.WriteLine($@"{question.QuestionText}");

                    Console.WriteLine("");

                    Console.WriteLine($@"{question.PossibleAnswers.ElementAt(PossibleAnswer)}");
                    Console.WriteLine("");
                    Console.WriteLine("");

                    if (game.GamePlayers.Count == 1)
                    {

                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).TrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                        Console.WriteLine("");
                    }

                    if (game.GamePlayers.Count == 2)
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).TrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                        Console.WriteLine("");
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).TrapSum}) ({game.GamePlayers.ElementAt(1).PlayerBuzzer})");
                        Console.WriteLine("");
                    }

                    if (game.GamePlayers.Count == 3)
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).TrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                        Console.WriteLine("");
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).TrapSum}) ({game.GamePlayers.ElementAt(1).PlayerBuzzer})");
                        Console.WriteLine("");
                        Console.WriteLine($@"3. {game.GamePlayers.ElementAt(2).PlayerName} (${game.GamePlayers.ElementAt(2).TrapSum}) ({game.GamePlayers.ElementAt(2).PlayerBuzzer})");
                    }

                    #endregion

                    #region The nightmare of the timer

                    double SecondsThatPassed = 0;
                    string TheKeyPressed = "";
                    Console.WriteLine("");
                    ConsoleKeyInfo buzzer = new ConsoleKeyInfo();
                    //timerThread.Start();

                    while (!Console.KeyAvailable && SecondsThatPassed < 3)
                    {

                        Thread.Sleep(500);
                        SecondsThatPassed = SecondsThatPassed + 0.5;
                        if (SecondsThatPassed == 3)
                        {
                            break;
                        }
                    }

                    if (SecondsThatPassed >= 3)
                    {

                    }

                    else if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                        TheKeyPressed = consoleKeyInfo.Key.ToString();

                        #region Throwing wrong key press

                        if (consoleKeyInfo.Key.ToString().ToUpper() != "A" && consoleKeyInfo.Key.ToString().ToUpper() != "M" && consoleKeyInfo.Key.ToString().ToUpper() != "OEMPLUS")
                        {
                            SoundPlayer buzzerexpection = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav");
                            buzzerexpection.Play();
                            Thread.Sleep(5000);
                            throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                        }

                        #endregion

                        else
                        {
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }

                            #region What to do when a player buzzers

                            string ActivatedBuzzer = consoleKeyInfo.Key.ToString().ToUpper();

                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }

                            Player playerAnswering = new Player(1, "");

                            int ChoosenAnswer = 0;
                            ChoosenAnswer = PossibleAnswer;

                            if (ActivatedBuzzer.ToUpper() == "M")
                            {
                                playerAnswering = new Player(game.GamePlayers.ElementAt(0).PlayerNumber, game.GamePlayers.ElementAt(0).PlayerName);
                                playerAnswering = game.GamePlayers.ElementAt(0);
                                playerAnswering.PlayerNumber = 1;
                            }
                            if (ActivatedBuzzer.ToUpper() == "A")
                            {
                                playerAnswering = new Player(game.GamePlayers.ElementAt(2).PlayerNumber, game.GamePlayers.ElementAt(2).PlayerName);
                                playerAnswering = game.GamePlayers.ElementAt(2);
                                playerAnswering.PlayerNumber = 3;
                            }
                            if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                            {
                                playerAnswering = new Player(game.GamePlayers.ElementAt(1).PlayerNumber, game.GamePlayers.ElementAt(1).PlayerName);
                                playerAnswering = game.GamePlayers.ElementAt(1);
                                playerAnswering.PlayerNumber = 2;
                            }

                            if (playerAnswering.TrapSum == 0)
                            {
                                question.MoneyOnTable = 1000;
                            }
                            if (playerAnswering.TrapSum == 1000)
                            {
                                question.MoneyOnTable = 500;
                            }
                            if (playerAnswering.TrapSum != 1000 && playerAnswering.TrapSum != 0)
                            {
                                question.MoneyOnTable = playerAnswering.TrapSum;
                            }

                            Console.Beep();
                            Console.WriteLine("");

                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }

                            //Thread timerThread2 = new Thread(new ThreadStart(RunTimerForNormalModeSomeone));
                            //double SecondsForAnswer = 10;
                            //timerThread2.Start();

                            #endregion

                            playerAnswering.HasThePlayerChoosenAnswer = true;
                            int ChoosenAnswerNumber = ChoosenAnswer + 1;
                            if (ChoosenAnswerNumber == question.CorrectAnswer)
                            {
                                #region if the answer is correct..

                                Console.WriteLine("");
                                Console.WriteLine($@"{question.CorrectAnswer}");

                                if (ActivatedBuzzer.ToUpper() == "M")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(0);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(1);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "A")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(2);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                                }

                                playerAnswering.TrapSum = playerAnswering.TrapSum + question.MoneyOnTable;

                                SoundPlayer correct = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\CorrectAnswer.wav");
                                correct.Play();
                                Thread.Sleep(600);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }
                                Console.WriteLine("");
                                Console.WriteLine("Oh yeah! Correct Answer!");
                                Console.WriteLine("");
                                Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.TrapSum}.");
                                /* stopping by to explain sound file
                                 * SoundPlayer explanation = new SoundPlayer($@"path for explanation .wav file")
                                 * explanation.Play();
                                 * Thread.Sleep(sound file length);
                                 * than, no additional user kep press is required
                                 */

                                Thread.Sleep(3000);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }

                                break;

                                #endregion
                            }


                            if (ChoosenAnswerNumber != question.CorrectAnswer)
                            {
                                #region If the answer is wrong..

                                _stopRequestedForSomeoneAnswering = true;

                                if (ActivatedBuzzer.ToUpper() == "M")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(0);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(1);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                                }
                                else if (ActivatedBuzzer.ToUpper() == "A")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(2);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                                    playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                                }

                                if (playerAnswering.TrapSum - question.MoneyOnTable < 0)
                                {
                                    playerAnswering.TrapSum = 0;
                                }
                                else
                                {
                                    playerAnswering.TrapSum = 0;
                                }

                                SoundPlayer Incorrect = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\IncorrectAnswer.wav");
                                Incorrect.Play();
                                Thread.Sleep(1200);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }

                                Console.Beep();
                                Console.WriteLine("");
                                Console.WriteLine("Wrong!");
                                Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.TrapSum}");
                                Console.WriteLine("");
                                playerAnswering.HasThePlayerChoosenAnswer = true;
                                //playerAnswering.WrongAnswerChoosen = int.Parse($@"{ChoosenAnswer.Key.ToString().Substring(1)}");

                                game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).HasThePlayerChoosenAnswer = true;

                                Thread.Sleep(2000);
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }

                                // Now to continue to the other answers..

                                #endregion
                            }
                        }
                    }



                    //bool Player1AnsweredOrNot = false;
                    //bool Player2AnsweredOrNot = false;
                    //bool Player3AnsweredOrNot = true;


                }

                #endregion

            }

            #endregion

            Console.Clear();

            return game;
        }

        #endregion

        #region Blank spaces

        public string BlankSpaces(int numberOfSpaces)
        {
            return new string(' ', numberOfSpaces);
        }


        #endregion

        #region Game over part

        public void GameOver(Game game)
        {
            Console.Clear();

            #region Final tital screen

            SoundPlayer GameOverSoundEffect = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\GameOverSoundEffect.wav");
            GameOverSoundEffect.Play();

            string BlankSpace = "";
            for (int i = 0; i < 30; i++)
            {
                BlankSpace = BlankSpaces(i + 1);
                Console.Write($@"{BlankSpace}Final results!!111");
                Thread.Sleep(40);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                if (i == 29)
                {
                    break;
                }
                Console.Clear();
            }

            SoundPlayer GameOverSpeech = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\GameOverSpeech.wav");
            GameOverSpeech.Play();
            Thread.Sleep(5000);
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            Console.Clear();

            #endregion

            #region Showing game results

            SoundPlayer HighScoreSoundEffect = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\HighScoreSoundEffect.wav");
            HighScoreSoundEffect.Play();
            Thread.Sleep(2000);
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            List<int> FindingTheMiddleSum = new List<int>();

            List<int> AllPlayersSum = new List<int>();

            foreach (Player player in game.GamePlayers)
            {
                int total = player.PreTrapSum + player.TrapSum;
                AllPlayersSum.Add(total);
                FindingTheMiddleSum.Add(total);
            }

            Console.Beep();

            int WorstSum = 1000000;
            int MiddleSum = 1000000;
            int BestSum = 1000000;

            Player WinnerPlayer = new Player(10, "");
            Player MiddlePlayer = new Player(10, "");
            Player WorstPlayer = new Player(10, "");
            bool EveryoneDraw = false;
            bool Draw = false;

            #region If there are 3 players..

            if (game.GamePlayers.Count == 3)
            {
                BestSum = AllPlayersSum.Max();
                FindingTheMiddleSum.Remove(BestSum);
                WorstSum = AllPlayersSum.Min();
                FindingTheMiddleSum.Remove(WorstSum);
                MiddleSum = FindingTheMiddleSum.ElementAt(0);

                #region in the case of draws..

                EveryoneDraw = BestSum == FindingTheMiddleSum.ElementAt(0) && FindingTheMiddleSum.ElementAt(0) == WorstSum;

                if (BestSum == FindingTheMiddleSum.ElementAt(0) || FindingTheMiddleSum.ElementAt(0) == WorstSum)
                {
                    if(!EveryoneDraw)
                    {
                        Draw = true;
                        if (BestSum == FindingTheMiddleSum.ElementAt(0))
                        {
                            WinnerPlayer.PlayerPosition = 1;
                            MiddlePlayer.PlayerPosition = 1;
                            WorstPlayer.PlayerPosition = 2;
                        }
                        if (FindingTheMiddleSum.ElementAt(0) == WorstSum)
                        {
                            WinnerPlayer.PlayerPosition = 1;
                            MiddlePlayer.PlayerPosition = 2;
                            WorstPlayer.PlayerPosition = 2;
                        }
                    }
                              
                }
                if (EveryoneDraw)
                {
                    WinnerPlayer.PlayerPosition = 1;
                    MiddlePlayer.PlayerPosition = 1;
                    WorstPlayer.PlayerPosition = 1;
                }

                if (EveryoneDraw)
                {
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${BestSum})");
                    Console.WriteLine("");
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(1).PlayerName} (${MiddleSum})");
                    Console.WriteLine("");
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(2).PlayerName} (${WorstSum})");
                }
                if(Draw && !EveryoneDraw)
                {
                    if (BestSum == FindingTheMiddleSum.ElementAt(0))
                    {
                        #region If the two players win together..

                        if ((game.GamePlayers.ElementAt(0).PreTrapSum + game.GamePlayers.ElementAt(0).TrapSum == BestSum) && game.GamePlayers.ElementAt(1).PreTrapSum + game.GamePlayers.ElementAt(1).TrapSum == BestSum)
                        {
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${BestSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(1).PlayerName} (${MiddleSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(2).PlayerName} (${WorstSum})");
                        }
                        if ((game.GamePlayers.ElementAt(0).PreTrapSum + game.GamePlayers.ElementAt(0).TrapSum == BestSum) && game.GamePlayers.ElementAt(2).PreTrapSum + game.GamePlayers.ElementAt(2).TrapSum == BestSum)
                        {
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${BestSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(2).PlayerName} (${MiddleSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${WorstSum})");
                        }
                        if ((game.GamePlayers.ElementAt(1).PreTrapSum + game.GamePlayers.ElementAt(1).TrapSum == BestSum) && game.GamePlayers.ElementAt(2).PreTrapSum + game.GamePlayers.ElementAt(2).TrapSum == BestSum)
                        {
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(1).PlayerName} (${BestSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(2).PlayerName} (${MiddleSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(0).PlayerName} (${WorstSum})");
                        }

                        #endregion

                        /*
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerPosition} (${BestSum})");
                        Console.WriteLine("");
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(1).PlayerPosition} (${MiddleSum})");
                        Console.WriteLine("");
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(2).PlayerPosition} (${WorstSum})");
                        */
                    }
                    #region If a player wins and the others are equal..

                    if (BestSum > FindingTheMiddleSum.ElementAt(0) && FindingTheMiddleSum.ElementAt(0) == WorstSum)
                    {
                        if (game.GamePlayers.ElementAt(0).PreTrapSum + game.GamePlayers.ElementAt(0).TrapSum == BestSum)
                        {
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${BestSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${MiddleSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(2).PlayerName} (${WorstSum})");
                        }
                        else if (game.GamePlayers.ElementAt(1).PreTrapSum + game.GamePlayers.ElementAt(1).TrapSum == BestSum)
                        {
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(1).PlayerName} (${BestSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(0).PlayerName} (${MiddleSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(2).PlayerName} (${WorstSum})");
                        }
                        else if (game.GamePlayers.ElementAt(2).PreTrapSum + game.GamePlayers.ElementAt(2).TrapSum == BestSum)
                        {
                            Console.WriteLine($@"1. {game.GamePlayers.ElementAt(2).PlayerName} (${BestSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(0).PlayerName} (${MiddleSum})");
                            Console.WriteLine("");
                            Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${WorstSum})");
                        }
                    }

                    #endregion

                }

                #endregion

                if (!Draw && !EveryoneDraw)
                {
                    WinnerPlayer = game.GamePlayers.Find(x => x.PreTrapSum + x.TrapSum == BestSum);
                    MiddlePlayer = game.GamePlayers.Find(x => x.PreTrapSum + x.TrapSum == FindingTheMiddleSum.ElementAt(0));
                    WorstPlayer = game.GamePlayers.Find(x => x.PreTrapSum + x.TrapSum == WorstSum);

                    Console.WriteLine($@"1. {WinnerPlayer.PlayerName} (${BestSum})");
                    Console.WriteLine("");
                    Console.WriteLine($@"2. {MiddlePlayer.PlayerName} (${MiddleSum})");
                    Console.WriteLine("");
                    Console.WriteLine($@"3. {WorstPlayer.PlayerName} (${WorstSum})");
                }                
            }

            #endregion

            #region If there are 2 players..

            else if (game.GamePlayers.Count == 2)
            {
                BestSum = AllPlayersSum.Max();
                WorstSum = AllPlayersSum.Min();

                if(BestSum == WorstSum)
                {
                    Draw = true;
                    EveryoneDraw = true;

                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${BestSum})");
                    Console.WriteLine("");
                    Console.WriteLine($@"1. {game.GamePlayers.ElementAt(1).PlayerName} (${WorstSum})");
                }

                if(!Draw && !EveryoneDraw)
                {
                    WinnerPlayer = game.GamePlayers.Find(x => x.PreTrapSum + x.TrapSum == BestSum);
                    WorstPlayer = game.GamePlayers.Find(x => x.PreTrapSum + x.TrapSum == WorstSum);

                    Console.WriteLine($@"1. {WinnerPlayer.PlayerName} (${BestSum})");
                    Console.WriteLine("");
                    Console.WriteLine($@"2. {WorstPlayer.PlayerName} (${WorstSum})");
                }
            }

            #endregion

            else if (game.GamePlayers.Count == 1)
            {
                Draw = false;
                EveryoneDraw = false;
                WinnerPlayer = game.GamePlayers.ElementAt(0);
                BestSum = AllPlayersSum.ElementAt(0);
                Console.WriteLine($@"1. {WinnerPlayer.PlayerName} (${BestSum})");
            }

            //AllPlayersSum.Sort();

            #endregion

            #region Results

            SoundPlayer result = new SoundPlayer();

            if(EveryoneDraw || Draw)
            {
                if (BestSum == FindingTheMiddleSum.ElementAt(0))
                {
                    result.SoundLocation = $@"You-think-you-are-smart\OtherSounds\Draw.wav";
                }
                else
                {
                    result.SoundLocation = $@"You-think-you-are-smart\OtherSounds\WinnerIs.wav";
                }
            }
            else
            {
                result.SoundLocation = $@"You-think-you-are-smart\OtherSounds\WinnerIs.wav";
            }            

            result.Play();
            Thread.Sleep(2000);
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }            

            SoundPlayer WinnerPlayerSound = new SoundPlayer();

            bool DoWeHaveAWinner = BestSum > FindingTheMiddleSum.ElementAt(0) || game.GamePlayers.Count == 1;

            #region Getting the winner's sound file

            if (DoWeHaveAWinner)
            {
                if (game.GamePlayers.Count == 1)
                {
                    WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player1.wav";
                }
                if (game.GamePlayers.Count == 2)
                {
                    if (game.GamePlayers.ElementAt(0).TrapSum + game.GamePlayers.ElementAt(0).PreTrapSum == BestSum)
                    {
                        WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player1.wav";
                    }
                    if (game.GamePlayers.ElementAt(1).TrapSum + game.GamePlayers.ElementAt(1).PreTrapSum == BestSum)
                    {
                        WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player2.wav";
                    }
                }
                if (game.GamePlayers.Count == 3)
                {
                    if (game.GamePlayers.ElementAt(0).TrapSum + game.GamePlayers.ElementAt(0).PreTrapSum == BestSum)
                    {
                        WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player1.wav";
                    }
                    if (game.GamePlayers.ElementAt(1).TrapSum + game.GamePlayers.ElementAt(1).PreTrapSum == BestSum)
                    {
                        WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player2.wav";
                    }
                    if (game.GamePlayers.ElementAt(2).TrapSum + game.GamePlayers.ElementAt(2).PreTrapSum == BestSum)
                    {
                        WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player3.wav";
                    }
                }
            }

            #endregion

            /*
            if (WinnerPlayer.PlayerName == game.GamePlayers.ElementAt(0).PlayerName)
            {
                WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player1.wav";
            }
            if (WinnerPlayer.PlayerName == game.GamePlayers.ElementAt(1).PlayerName)
            {
                WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player2.wav";
            }
            if (WinnerPlayer.PlayerName == game.GamePlayers.ElementAt(2).PlayerName)
            {
                WinnerPlayerSound.SoundLocation = $@"You-think-you-are-smart\NameSounds\Player3.wav";
            }
            */
            if(DoWeHaveAWinner)
            {
                WinnerPlayerSound.Play();
                Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }
            
            if(DoWeHaveAWinner)
            {
                SoundPlayer ResultGoodOrBad = new SoundPlayer();

                if (BestSum > 0)
                {
                    ResultGoodOrBad.SoundLocation = $@"You-think-you-are-smart\OtherSounds\PositiveScore.wav";
                }
                if (BestSum <= 0)
                {
                    ResultGoodOrBad.SoundLocation = $@"You-think-you-are-smart\OtherSounds\NegativeScore.wav";
                }

                ResultGoodOrBad.Play();
                Thread.Sleep(4000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }            

            #endregion

            #region Writing All results To XML

            bool IsThereANewWR = false;

            List<XElement> AllScoreElements = new List<XElement>();
            List<int> AllScores = new List<int>();
            List<int> WorstToBestScores = new List<int>();
            List<int> NoCurrentRecordsOnly = new List<int>();

            HighScore highScore = new HighScore(game);
            List<string> WhatToWriteForXML = new List<string>();
            WhatToWriteForXML.Add("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            WhatToWriteForXML.Add("<HighScore>");

            #region Redone

            foreach (XElement score in highScore.scores)
            {
                WhatToWriteForXML.Add($@"<Score>");
                WhatToWriteForXML.Add($@"<PlayerName>{score.Element("PlayerName").Value}</PlayerName>");
                WhatToWriteForXML.Add($@"<Money>{score.Element("Money").Value}</Money>");
                WhatToWriteForXML.Add($@"<Position>{score.Element("Position").Value}</Position>");
                WhatToWriteForXML.Add($@"</Score>");
            }
            foreach(Player player in game.GamePlayers)
            {
                WhatToWriteForXML.Add($@"<Score>");
                WhatToWriteForXML.Add($@"<PlayerName>{player.PlayerName}</PlayerName>");
                WhatToWriteForXML.Add($@"<Money>{player.PreTrapSum + player.TrapSum}</Money>");
                WhatToWriteForXML.Add($@"<Position>{0}</Position>");
                WhatToWriteForXML.Add($@"</Score>");
            }

            WhatToWriteForXML.Add($@"</HighScore>");

            highScore.HighScoreDocument.RemoveNodes();
            File.Delete($@"You think you are smart xml\HighScore.xml");
            File.WriteAllLines($@"You think you are smart xml\HighScore.xml", WhatToWriteForXML);

            #endregion


            #region Old way
            /*
            foreach (XElement score in highScore.scores)
            {
                AllScores.Add(int.Parse(score.Element("Money").Value));
                WorstToBestScores.Add(int.Parse(score.Element("Money").Value));
                NoCurrentRecordsOnly.Add(int.Parse(score.Element("Money").Value));
                
                //WhatToWriteForXML.Add($@"<Score>");
                //WhatToWriteForXML.Add($@"<PlayerName>{score.Element("PlayerName").Value}</PlayerName>");
                //WhatToWriteForXML.Add($@"<Money>{score.Element("Money").Value}</Money>");
                //WhatToWriteForXML.Add($@"<Position>{score.Element("Position").Value}</Position>");
                //WhatToWriteForXML.Add($@"</Score>");
                
            }
            */

            /*
            foreach (Player player in game.GamePlayers)
            {
                AllScores.Add(player.PreTrapSum + player.TrapSum);
                WorstToBestScores.Add(player.PreTrapSum + player.TrapSum);
                
                //WhatToWriteForXML.Add("<Score>");
                //WhatToWriteForXML.Add($@"<PlayerName>{player.PlayerName}</PlayerName>");
                //WhatToWriteForXML.Add($@"<Money>{player.PreTrapSum + player.TrapSum}</Money>");
                //WhatToWriteForXML.Add($@"<Position>{player.PlayerPosition}</Position>");
                //WhatToWriteForXML.Add($@"</Score>");
                
            }

            WorstToBestScores.Sort();
            

            List<int> CalculatedPositions = new List<int>(new int[AllScores.Count]);

            int position = 1;
            CalculatedPositions[AllScores.Count - 1] = position;


            for (int i = AllScores.Count - 2; i >= 0; i--)
            {
                // any xelement needs to be edited its position value.
                // You can use the setvalue function, but it requires making the xml file the normal way
                // you can re-create the string list, since everything but the position int value is identical
                // to calculate the position value, here's how you do it:
                // you go throught the score list (for loop),
                // and for each element, you find its position (index of) in the sorted list
                // the position value is the (total elements count) - (the score index in the sorted list)
                // than you simply change the order

                if (AllScores[i] == AllScores[i + 1])
                {
                    CalculatedPositions[i] = position;
                }
                else
                {
                    // If scores are different, increment the rank
                    // and assign the new rank
                    position += 1;
                    CalculatedPositions[i] = position;
                }

                //position = AllScores.Count - WorstToBestScores.IndexOf(AllScores[i]);
                //CalculatedPositions.Add(position);
            }

            NoCurrentRecordsOnly.Sort();

            int BestExistingScore = 100;

            BestExistingScore = NoCurrentRecordsOnly.ElementAt(NoCurrentRecordsOnly.Count - 1);

            foreach (Player player in game.GamePlayers)
            {
                int score = player.PreTrapSum + player.TrapSum;
            */
                /*
                int PositionIn = AllScores.Count - WorstToBestScores.IndexOf(score);
                player.PositionInHighScore = PositionIn;
                if (score == CalculatedPositions.ElementAt(0) || PositionIn == 1)
                {
                    IsThereANewWR = true;
                }
                */
                /*
                if(WorstToBestScores.ElementAt(WorstToBestScores.Count - 1) == score)
                {
                    IsThereANewWR = true;
                }
                */
                /*
                if(score > BestExistingScore)
                {
                    IsThereANewWR = true;
                }
            }
                */
            /*
            for (int i = CalculatedPositions.Count; i > 0; i--)
            {
                int CurrentMoney = WorstToBestScores.ElementAt(i - 1);

                bool IsExistInDataBase = true;

                Player CurrentPlayerForThis = new Player(1, "");

                foreach(Player player in game.GamePlayers)
                {
                    if(CurrentMoney == player.PreTrapSum + player.TrapSum)
                    {
                        IsExistInDataBase = false;
                        CurrentPlayerForThis = player;
                    }
                }

                if(IsExistInDataBase)
                {
                    XElement score = highScore.scores.Find(x => int.Parse(x.Element("Money").Value) == CurrentMoney);
                    WhatToWriteForXML.Add($@"<Score>");
                    WhatToWriteForXML.Add($@"<PlayerName>{score.Element("PlayerName").Value}</PlayerName>");
                    WhatToWriteForXML.Add($@"<Money>{score.Element("Money").Value}</Money>");
                    if(i == CalculatedPositions.Count)
                    {
                        WhatToWriteForXML.Add($@"<Position>{CalculatedPositions.ElementAt(i - 1).ToString()}</Position>");
                    }
                    else if (i < CalculatedPositions.Count)
                    {
                        if(WorstToBestScores.ElementAt(i - 1) == WorstToBestScores.ElementAt(i))
                        {
                            WhatToWriteForXML.Add($@"<Position>{CalculatedPositions.ElementAt(i).ToString()}</Position>");
                        }
                        else
                        {
                            WhatToWriteForXML.Add($@"<Position>{CalculatedPositions.ElementAt(i - 1).ToString()}</Position>");
                        }
                    }
                    WhatToWriteForXML.Add($@"</Score>");
                }
                else
                {
                    WhatToWriteForXML.Add($@"<Score>");
                    WhatToWriteForXML.Add($@"<PlayerName>{CurrentPlayerForThis.PlayerName}</PlayerName>");
                    WhatToWriteForXML.Add($@"<Money>{CurrentPlayerForThis.PreTrapSum + CurrentPlayerForThis.TrapSum}</Money>");

                    if (i == CalculatedPositions.Count)
                    {
                        WhatToWriteForXML.Add($@"<Position>{CalculatedPositions.ElementAt(i - 1).ToString()}</Position>");
                    }
                    else if (i < CalculatedPositions.Count)
                    {
                        if (WorstToBestScores.ElementAt(i - 1) == WorstToBestScores.ElementAt(i))
                        {
                            WhatToWriteForXML.Add($@"<Position>{CalculatedPositions.ElementAt(i).ToString()}</Position>");
                        }
                        else
                        {
                            WhatToWriteForXML.Add($@"<Position>{CalculatedPositions.ElementAt(i - 1).ToString()}</Position>");
                        }
                    }

                    WhatToWriteForXML.Add($@"</Score>");
                }                
            }

            WhatToWriteForXML.Add($@"</HighScore>");

            highScore.HighScoreDocument.RemoveNodes();
            File.Delete($@"You think you are smart xml\HighScore.xml");
            File.WriteAllLines($@"You think you are smart xml\HighScore.xml", WhatToWriteForXML);

            */

            #endregion

            #endregion

            #region re-opening the new XML file and displaying the records

            XDocument HighScoreNewDocument = new XDocument(XDocument.Load($@"You think you are smart xml\HighScore.xml"));
            XElement root = HighScoreNewDocument.Root;
            IEnumerable<XElement> ScoresElements = root.Elements("Score");
            List<XElement> scores = ScoresElements.ToList();

            #region Redone

            #region The top 5 records

            Console.Clear();

            IsThereANewWR = false;
            
            List<int> MoneyWorstToBestAfter = new List<int>();

            foreach(XElement score in scores)
            {
                MoneyWorstToBestAfter.Add(int.Parse(score.Element("Money").Value));
            }

            MoneyWorstToBestAfter.Sort();
            int ActualPosition = 1;

            for (int ScoreNumber = 0; ScoreNumber < 5; ScoreNumber++)
            {                
                int NumberToReduce = ScoreNumber + 1;
                XElement score = scores.Find(x => int.Parse(x.Element("Money").Value) == MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - NumberToReduce));
                if(ScoreNumber > 0)
                {
                    if (MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - NumberToReduce) == MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - ScoreNumber))
                    {
                        Console.WriteLine($@"{ActualPosition}. {score.Element("PlayerName").Value} (${score.Element("Money").Value})");
                    }
                    else if(MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - NumberToReduce) < MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - ScoreNumber))
                    {
                        ActualPosition = ActualPosition + 1;
                        Console.WriteLine($@"{ActualPosition}. {score.Element("PlayerName").Value} (${score.Element("Money").Value})");
                    }
                }
                else
                {
                    Console.WriteLine($@"{ActualPosition}. {score.Element("PlayerName").Value} (${score.Element("Money").Value})");
                }

                bool ShouldWeFinish = ScoreNumber + 1 == scores.Count;
                if (ShouldWeFinish)
                {
                    ScoreNumber = 4;
                    break;
                }
            }

            int BestScore = MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - 1);
            foreach(Player player in game.GamePlayers)
            {
                if(player.PreTrapSum + player.TrapSum == BestScore)
                {
                    if(MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - 1) > MoneyWorstToBestAfter.ElementAt(MoneyWorstToBestAfter.Count - 2))
                    {
                        IsThereANewWR = true;
                    }
                }
            }

            if (IsThereANewWR)
            {
                SoundPlayer newWRSound = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\newWR.wav");
                newWRSound.Play();
                Thread.Sleep(2700);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }

            #endregion

            #region The worse record

            int WorstMoney = MoneyWorstToBestAfter.ElementAt(0);
            XElement WorstScoreElement = scores.Find(x => int.Parse(x.Element("Money").Value) == WorstMoney);
            Console.WriteLine("");
            Console.WriteLine($@"Smartass.. {WorstScoreElement.Element("PlayerName").Value} (${WorstScoreElement.Element("Money").Value})");
            Console.WriteLine("");

            #endregion

            #region Old way

            /*

            Console.Clear();

            for (int ScoreNumber = 0; ScoreNumber < 5; ScoreNumber++)
            {                
                XElement score = scores.ElementAt(ScoreNumber);
                Console.WriteLine($@"{score.Element("Position").Value}. {score.Element("PlayerName").Value} (${score.Element("Money").Value})");
                Console.WriteLine("");
                bool ShouldWeFinish = ScoreNumber + 1 == scores.Count;
                if(ShouldWeFinish)
                {
                    ScoreNumber = 4;
                    break;
                }
                
            }

            XElement WorstScoreElement = scores.ElementAt(scores.Count - 1);
            Console.WriteLine($@"Smartass.. {WorstScoreElement.Element("PlayerName").Value} (${WorstScoreElement.Element("Money").Value})");

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            
            foreach (XElement score in scores)
            {
                Console.WriteLine($@"{score.Element("Position").Value}. {score.Element("PlayerName").Value} (${score.Element("Money").Value})");
                Console.WriteLine("");
                
                if(int.Parse(score.Element("Position").Value) == scores.Count)
                {
                    Console.WriteLine($@"Smartass.. {score.Element("PlayerName").Value} (${score.Element("Money").Value})");
                }
            }            
            

            if (IsThereANewWR)
            {
                SoundPlayer newWRSound = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\newWR.wav");
                newWRSound.Play();
                Thread.Sleep(2700);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
            }

            */

            #endregion

            Console.WriteLine("Press enter for a new game with new players,");
            Console.WriteLine("Press space for a new game with the same players.");

            ConsoleKeyInfo choice = Console.ReadKey(true);
            while (choice.Key.ToString().ToUpper() != "ENTER" && choice.Key.ToString().ToUpper() != "SPACEBAR")
            {
                Console.WriteLine("Press only on space or enter!");
                choice = Console.ReadKey(true);
            }
            string ChoiceIs = choice.Key.ToString();

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
            }

            if (ChoiceIs.ToUpper() == "ENTER")
            {
                return;
            }
            if (ChoiceIs.ToUpper() == "SPACEBAR")
            {
                List<Player> players = new List<Player>();
                for (int PlayerNum = 0; PlayerNum < game.GamePlayers.Count; PlayerNum++)
                {
                    Player newPlayer = new Player(PlayerNum + 1, game.GamePlayers.ElementAt(PlayerNum).PlayerName);
                    players.Add(newPlayer);
                }
                Game NewGame = new Game(players, game.NumOfGameQuestions, players.Count);
                NewGame.Instructions();
                Console.Clear();
                NewGame.NextTurn(NewGame, NewGame.GamePlayers.ElementAt(0));
            }

            

            return;

            #endregion

            #endregion
        }

        #endregion

        #region Correct Or false

        public Game CorrectOrFalseGame(Game game)
        {
            #region Choosing player to answer

            Random number = new Random();
            int ChoosenNumber = 0;

            if (game.GamePlayers.Count == 3)
            {
                ChoosenNumber = number.Next(0, 3);
            }
            if (game.GamePlayers.Count == 2)
            {
                ChoosenNumber = number.Next(0, 2);
            }
            if (game.GamePlayers.Count == 1)
            {
                ChoosenNumber = 0;
            }

            #endregion

            #region Intrudction and start game

            Console.Clear();
            SoundPlayer state = new SoundPlayer($@"You-think-you-are-smart\OtherSounds\CorrectOrFalseStatement.wav");
            state.Play();

            string BlankSpace = "";
            for (int i = 0; i < 45; i++)
            {
                BlankSpace = BlankSpaces(i + 1);
                Console.Write($@"{BlankSpace}Correct or incorrect..");
                Thread.Sleep(10);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                Console.Clear();
            }

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }
            Console.WriteLine("");
            Console.WriteLine("Try this or not?");
            Console.WriteLine("Y/y for yes, N/n for no");
            Console.WriteLine("");

            SoundPlayer playerToChoose = new SoundPlayer($@"You-think-you-are-smart\NameSounds\Player{ChoosenNumber + 1}.wav");
            playerToChoose.Play();
            Thread.Sleep(2000);
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            SoundPlayer instructions = new SoundPlayer($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\Instructions.wav");
            instructions.Play();
            double SecondsThatPassed = 0;
            ConsoleKeyInfo Choice = new ConsoleKeyInfo();

            while (!Console.KeyAvailable)
            {

            }

            if (Console.KeyAvailable)
            {
                Choice = Console.ReadKey(true);
                while (Choice.Key.ToString().ToUpper() != "Y" && Choice.Key.ToString().ToUpper() != "N")
                {
                    Choice = Console.ReadKey(true);
                }

                instructions.Stop();

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                if (Choice.Key.ToString().ToUpper() == "N")
                {
                    Console.WriteLine("");
                    Console.WriteLine("No!");
                    SoundPlayer DisAgree = new SoundPlayer($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\IfDisAgrees.wav");
                    DisAgree.Play();
                    Thread.Sleep(1000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                    Thread.Sleep(2000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                    Console.Clear();
                    return game;
                }
                if (Choice.Key.ToString().ToUpper() == "Y")
                {
                    Console.WriteLine("");
                    Console.WriteLine("Yes!");
                    SoundPlayer Agree = new SoundPlayer($@"You-think-you-are-smart\CorrectOrIncorrectQuestions\IfAgrees.wav");
                    Agree.Play();
                    Thread.Sleep(5000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                    #region the actual game

                    Console.Clear();
                    game.IsOnCorrectOrFalseMode = true;
                    CorrectOrFalse TrueFalseGame = new CorrectOrFalse(game, ChoosenNumber);

                    Console.WriteLine($@"{TrueFalseGame.ChoosenQuestionRandom.QuestionContent}");
                    Console.WriteLine("");
                    Console.WriteLine("Yes");
                    Console.WriteLine("No");

                    #region Opening the basic time

                    int startSecond = DateTime.Now.Second;
                    int startMinute = DateTime.Now.Minute;
                    int endSecond;
                    int endMinute;
                    if (startSecond <= 30)
                    {
                        endMinute = startMinute;
                        endSecond = startSecond + 30;
                    }
                    else
                    {
                        endMinute = startMinute + 1;
                        endSecond = startSecond - 30;
                    }

                    #endregion

                    double SecondsThatPassedInGame = 0;

                    string TheKeyPressed = "";
                    Console.WriteLine("");
                    ConsoleKeyInfo buzzer = new ConsoleKeyInfo();
                    while (!Console.KeyAvailable && SecondsThatPassedInGame < 30)
                    {

                        Thread.Sleep(1000);
                        SecondsThatPassedInGame = SecondsThatPassedInGame + 1;
                        Console.Write($@" {30 - SecondsThatPassedInGame}");
                        if (SecondsThatPassed >= 30)
                        {
                            break;
                        }
                    }

                    if (SecondsThatPassedInGame >= 30)
                    {
                        #region If the player doesn't answer..

                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }

                        SoundPlayer NoAnswer = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav");
                        NoAnswer.Play();
                        Thread.Sleep(6500);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }

                        Console.Beep();
                        Console.WriteLine("");
                        Console.WriteLine("Time's up! What, you thought you could get away with it?");
                        Console.WriteLine($@"{TrueFalseGame.playerForChellenge.PlayerName} now has $0");
                        Console.WriteLine("");
                        //string CorrectAnswerContent = File.ReadAllText($@"{QuestionsFoldersList.Find(x => x == CorrectFolder).FullName}\Answer{TheQuestion.CorrectAnswer}.txt");
                        // ($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{QuestionsFoldersList.Find(x => x == CorrectFolder).Name}\Answer{TheQuestion.CorrectAnswer}.txt");

                        //Console.WriteLine($@"{CorrectAnswerContent}");

                        Thread.Sleep(3000);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }
                        Console.Clear();
                        return game;

                        #endregion
                    }
                    if (Console.KeyAvailable)
                    {
                        if (Console.KeyAvailable == true)
                        {
                            ConsoleKeyInfo consoleKeyInfo = new ConsoleKeyInfo();
                            consoleKeyInfo = Console.ReadKey(true);

                            #region Throwing wrong key press                       

                            if (consoleKeyInfo.Key.ToString().ToUpper() != "Y" && consoleKeyInfo.Key.ToString().ToUpper() != "N")
                            {
                                SoundPlayer buzzerexpection = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav");
                                buzzerexpection.Play();
                                Thread.Sleep(5000);
                                throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                                /*
                                 * 
                                    //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                                    // a sound can be put in here
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    consoleKeyInfo = Console.ReadKey(true);
                                    Console.Write($@"{10 - SecondsThatPassed}");
                                    Thread.Sleep(1000);
                                    SecondsThatPassed = SecondsThatPassed + 1;

                                    //consoleKeyInfo = Console.ReadKey(true);
                                */
                            }


                            #endregion

                            if (consoleKeyInfo.Key.ToString().ToUpper() == "Y" || consoleKeyInfo.Key.ToString().ToUpper() == "N")
                            {
                                Console.Clear();

                                if (consoleKeyInfo.Key.ToString().ToUpper() == "Y")
                                {
                                    TrueFalseGame.ChoosenQuestionRandom.AnswerChoice = true;
                                    Console.WriteLine($@"{TrueFalseGame.ChoosenQuestionRandom.QuestionContent}");
                                    Console.WriteLine("");
                                    Console.WriteLine("Yes");
                                    Console.WriteLine("");
                                }
                                if (consoleKeyInfo.Key.ToString().ToUpper() == "N")
                                {
                                    TrueFalseGame.ChoosenQuestionRandom.AnswerChoice = false;
                                    Console.WriteLine($@"{TrueFalseGame.ChoosenQuestionRandom.QuestionContent}");
                                    Console.WriteLine("");
                                    Console.WriteLine("No");
                                    Console.WriteLine("");
                                }
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }

                                if (TrueFalseGame.ChoosenQuestionRandom.answer == TrueFalseGame.ChoosenQuestionRandom.AnswerChoice)
                                {
                                    #region If the player is correct

                                    int NewSum = game.GamePlayers.Find(x => x == TrueFalseGame.playerForChellenge).PreTrapSum * 2;
                                    game.GamePlayers.Find(x => x == TrueFalseGame.playerForChellenge).PreTrapSum = NewSum;

                                    SoundPlayer CorrectAnswerResponse = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\CorrectAnswer.wav");
                                    CorrectAnswerResponse.Play();
                                    Thread.Sleep(600);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }

                                    Console.WriteLine($@"{TrueFalseGame.playerForChellenge.PlayerName} Now has ${NewSum}.");
                                    Thread.Sleep(2000);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    game.IsOnCorrectOrFalseMode = false;
                                    Console.Clear();
                                    return game;

                                    #endregion
                                }
                                if (TrueFalseGame.ChoosenQuestionRandom.answer != TrueFalseGame.ChoosenQuestionRandom.AnswerChoice)
                                {
                                    #region If the player is wrong

                                    game.GamePlayers.Find(x => x == TrueFalseGame.playerForChellenge).PreTrapSum = 0;
                                    TrueFalseGame.playerForChellenge.PreTrapSum = 0;

                                    SoundPlayer InCorrectAnswerResponse = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\IncorrectAnswer.wav");
                                    InCorrectAnswerResponse.Play();
                                    Thread.Sleep(1000);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }

                                    Console.WriteLine($@"{TrueFalseGame.playerForChellenge.PlayerName} Now has ${TrueFalseGame.playerForChellenge.PreTrapSum}.");
                                    Thread.Sleep(2000);
                                    while (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                    }
                                    game.IsOnCorrectOrFalseMode = false;
                                    Console.Clear();
                                    return game;

                                    #endregion                                    
                                }

                                return game;
                            }
                        }

                    }

                    #endregion
                }
            }

            #endregion

            return game;

            //Player Player
        }

        #endregion

        #region Who am I game

        public Game WhoAmIGame(Game game, bool IntrductionHasHappend)
        {
            #region Intrudction part

            if (!IntrductionHasHappend)
            {
                Console.Clear();
                SoundPlayer WhoAmIIntrudction = new SoundPlayer($@"You-think-you-are-smart\WhoAmIQuestions\Instructions.wav");
                WhoAmIIntrudction.Play();

                string BlankSpace = "";
                for (int i = 0; i < 30; i++)
                {
                    BlankSpace = BlankSpaces(i + 1);
                    Console.Write($@"{BlankSpace}Who am I..");
                    Thread.Sleep(10);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                    Console.Clear();
                }

                BlankSpace = BlankSpaces(31);
                Console.Write($@"{BlankSpace}Who am I..");

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                int startSecond = DateTime.Now.Second;
                int startMinute = DateTime.Now.Minute;
                int endSecond;
                int endMinute;
                double SecondsThatPassed = 0;
                if (startSecond <= 43)
                {
                    endMinute = startMinute;
                    endSecond = startSecond + 17;
                }
                else
                {
                    endMinute = startMinute + 1;
                    endSecond = startSecond - 43;
                }

                while (!Console.KeyAvailable/* && (DateTime.Now.Second == endSecond && DateTime.Now.Minute != endMinute)*/ && SecondsThatPassed < 17)
                {
                    Thread.Sleep(100);
                    SecondsThatPassed = SecondsThatPassed + 0.1;
                    if (SecondsThatPassed >= 17)
                    {
                        break;
                    }
                }
                SoundPlayer InstructionsOtherAudio = new SoundPlayer(@"You-think-you-are-smart\OtherSounds\SkippedInstructions.wav");

                if (Console.KeyAvailable == true)
                {
                    WhoAmIIntrudction.Stop();
                    InstructionsOtherAudio.Play();
                    Thread.Sleep(3750);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }

                    Console.Clear();
                }

                if (SecondsThatPassed >= 17)
                {
                    Console.Clear();
                    Thread.Sleep(2000);
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                }

                if (DateTime.Now.Second == endSecond && DateTime.Now.Minute == endMinute)
                {
                    Console.Clear();
                }
            }



            #endregion

            #region The game itself

            foreach(Player player in game.GamePlayers)
            {
                player.HasThePlayerChoosenAnswer = false;
                player.WrongAnswerChoosen = 0;
            }

            Console.Clear();

            // the clues can appear from above
            // but this also means withhin every clue, the console will have to be cleared
            // maybe it will be easier to put the incomplete above, so we won't have to clear the console very time
            // timer will have to appear along with the clues
            // a reminder, for the writing itself, console.ReadLine() will do the trick
            // the sum of money also has to be written and changed accordingly.
            // bottom line: very complicated, take the weekend for this

            WhoAmI WhoAmIObject = new WhoAmI(game);
            bool HasNoOneAnswered = true;
            bool IsAfterAPlayerWrong = false;
            double ExtraSecondsUntilNextClue = 0;

            for (int i = 0; i < 4; i++)
            {
                if (IsAfterAPlayerWrong)
                {
                    i = i - 1;
                }
                Console.Clear();

                Console.WriteLine(WhoAmIObject.choosenQuestion.InCompleteAnswer);
                Console.WriteLine("");
                bool IsCluesDone = false;
                for (int y = 0; !IsCluesDone; y++)
                {
                    Console.WriteLine($@"{y + 1}. {WhoAmIObject.choosenQuestion.ClueText.ElementAt(y)}");
                    if (y == i)
                    {
                        IsCluesDone = true;
                    }
                }

                #region Writing players details

                List<int> WrongAnswersGiven = new List<int>();

                Console.WriteLine("");

                bool Player1AnsweredOrNot = false;
                bool Player2AnsweredOrNot = false;
                bool Player3AnsweredOrNot = true;

                if (game.GamePlayers.Count == 1)
                {
                    Player1AnsweredOrNot = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                }
                else if (game.GamePlayers.Count == 2)
                {
                    Player1AnsweredOrNot = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                    Player2AnsweredOrNot = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                }
                else if (game.GamePlayers.Count == 3)
                {
                    Player1AnsweredOrNot = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                    Player2AnsweredOrNot = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                    Player3AnsweredOrNot = game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer;
                }

                Console.WriteLine("");
                if (game.GamePlayers.Count == 1)
                {
                    if (!Player1AnsweredOrNot)
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (Wrong..)");
                        Console.WriteLine("");
                    }
                }

                if (game.GamePlayers.Count == 2)
                {
                    if (!Player1AnsweredOrNot)
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) (Wrong..)");
                        Console.WriteLine("");
                    }

                    if (!Player2AnsweredOrNot)
                    {
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) ({game.GamePlayers.ElementAt(1).PlayerBuzzer})");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) (Wrong..)");
                        Console.WriteLine("");
                    }
                }

                if (game.GamePlayers.Count == 3)
                {
                    if (!Player1AnsweredOrNot)
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) ({game.GamePlayers.ElementAt(0).PlayerBuzzer})");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($@"1. {game.GamePlayers.ElementAt(0).PlayerName} (${game.GamePlayers.ElementAt(0).PreTrapSum}) (Wrong..)");
                        Console.WriteLine("");
                    }

                    if (!Player2AnsweredOrNot)
                    {
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) ({game.GamePlayers.ElementAt(1).PlayerBuzzer})");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($@"2. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) (Wrong..)");
                        Console.WriteLine("");
                    }

                    if (!Player3AnsweredOrNot)
                    {
                        Console.WriteLine($@"3. {game.GamePlayers.ElementAt(2).PlayerName} (${game.GamePlayers.ElementAt(2).PreTrapSum}) ({game.GamePlayers.ElementAt(2).PlayerBuzzer})");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine($@"3. {game.GamePlayers.ElementAt(1).PlayerName} (${game.GamePlayers.ElementAt(1).PreTrapSum}) (Wrong..)");
                    }
                }

                #endregion


                WhoAmIObject.choosenQuestion.CluesSoundPlayers.ElementAt(i).Play();

                double SecondsThatPassed3 = 0;
                if (ExtraSecondsUntilNextClue == 0)
                {
                    SecondsThatPassed3 = 0;
                }
                else
                {
                    SecondsThatPassed3 = 0 + ExtraSecondsUntilNextClue;
                }

                string TheKeyPressed = "";
                Console.WriteLine("");
                ConsoleKeyInfo buzzer = new ConsoleKeyInfo();

                while (!Console.KeyAvailable && SecondsThatPassed3 < 5)
                {
                    Console.Write($@" ${WhoAmIObject.choosenQuestion.Money}");
                    WhoAmIObject.choosenQuestion.Money = WhoAmIObject.choosenQuestion.Money - 500;
                    if (SecondsThatPassed3 >= 5)
                    {
                        WhoAmIObject.choosenQuestion.Money = WhoAmIObject.choosenQuestion.Money + 500;
                        break;
                    }
                    Thread.Sleep(1000);
                    SecondsThatPassed3 = SecondsThatPassed3 + 1;
                    WhoAmIObject.choosenQuestion.TimeLeft = WhoAmIObject.choosenQuestion.TimeLeft - 1;
                }
                if (SecondsThatPassed3 >= 5)
                {
                    ExtraSecondsUntilNextClue = 0;
                    // than continue to the next clue
                    if (i == 3)
                    {
                        Console.Write($@" $0");
                    }
                    IsAfterAPlayerWrong = false;
                }
                if (Console.KeyAvailable)
                {
                    WhoAmIObject.choosenQuestion.CluesSoundPlayers.ElementAt(i).Stop();

                    Console.Beep();

                    ConsoleKeyInfo consoleKeyInfo = new ConsoleKeyInfo();
                    consoleKeyInfo = Console.ReadKey(true);

                    #region Throwing wrong key press                       

                    if (consoleKeyInfo.Key.ToString().ToUpper() != "A" && consoleKeyInfo.Key.ToString().ToUpper() != "M" && consoleKeyInfo.Key.ToString().ToUpper() != "OEMPLUS")
                    {
                        SoundPlayer buzzerexpection = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav");
                        buzzerexpection.Play();
                        Thread.Sleep(5000);
                        throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                        /*
                         * 
                            //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                            // a sound can be put in here
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }
                            consoleKeyInfo = Console.ReadKey(true);
                            Console.Write($@"{10 - SecondsThatPassed}");
                            Thread.Sleep(1000);
                            SecondsThatPassed = SecondsThatPassed + 1;

                            //consoleKeyInfo = Console.ReadKey(true);
                        */
                    }


                    #endregion

                    #region Ensuring relevant buzzer has been pressed

                    if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "A" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS")
                    {
                        bool IsTheBuzzerCorrect = false;

                        if (game.GamePlayers.Count == 1)
                        {
                            if (consoleKeyInfo.Key.ToString().ToUpper() == "M")
                            {
                                IsTheBuzzerCorrect = true;
                            }
                        }
                        if (game.GamePlayers.Count == 2)
                        {
                            if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS")
                            {
                                IsTheBuzzerCorrect = true;
                            }
                        }
                        if (game.GamePlayers.Count == 3)
                        {
                            if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS" || consoleKeyInfo.Key.ToString().ToUpper() == "A")
                            {
                                IsTheBuzzerCorrect = true;
                            }
                        }

                        if (!IsTheBuzzerCorrect)
                        {
                            #region Throwing wrong key press                       

                            SoundPlayer buzzerexpection = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\HandlingIncorrectBuzzerPressing.wav");
                            buzzerexpection.Play();
                            Thread.Sleep(5000);
                            throw new Exception($@"Sorry, can't handle incorrect key press at this mode. I tried for over 20 hours to find a solution for this and failed. Restart the game");
                            /*
                             * 
                                //Console.WriteLine("Press only on one of your buzzers - A/M/+");
                                // a sound can be put in here
                                while (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                                }
                                consoleKeyInfo = Console.ReadKey(true);
                                Console.Write($@"{10 - SecondsThatPassed}");
                                Thread.Sleep(1000);
                                SecondsThatPassed = SecondsThatPassed + 1;

                                //consoleKeyInfo = Console.ReadKey(true);
                            */

                            #endregion
                        }
                    }

                    #endregion

                    string ActivatedBuzzer = consoleKeyInfo.Key.ToString();

                    #region What to do when a player buzzers, before he answers

                    #region If it's a player who already answered..

                    bool Player1AnsweredAlready = false;
                    bool Player2AnsweredAlready = false;
                    bool Player3AnsweredAlready = false;

                    if (game.GamePlayers.Count == 1)
                    {
                        Player1AnsweredAlready = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && ActivatedBuzzer == "M";
                    }
                    else if (game.GamePlayers.Count == 2)
                    {
                        Player1AnsweredAlready = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && ActivatedBuzzer == "M";
                        Player2AnsweredAlready = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && ActivatedBuzzer == "OEMPLUX";
                    }
                    else if (game.GamePlayers.Count == 3)
                    {
                        Player1AnsweredAlready = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && ActivatedBuzzer == "M";
                        Player2AnsweredAlready = game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && ActivatedBuzzer == "OEMPLUX";
                        Player3AnsweredAlready = game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer && ActivatedBuzzer == "A";
                    }

                    if (game.GamePlayers.Count == 2)
                    {
                        if (Player1AnsweredAlready || Player2AnsweredAlready)
                        {
                            SoundPlayer ReBuzzerException = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\PressingBuzzerAgainException.wav");
                            ReBuzzerException.Play();
                            Thread.Sleep(5000);
                            throw new Exception(" The sound you just heard explains this.. Restart the game. ");
                        }
                    }
                    if (game.GamePlayers.Count == 3)
                    {
                        if (Player1AnsweredAlready || Player2AnsweredAlready || Player3AnsweredAlready)
                        {
                            SoundPlayer ReBuzzerException = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\PressingBuzzerAgainException.wav");
                            ReBuzzerException.Play();
                            Thread.Sleep(5000);
                            throw new Exception(" The sound you just heard explains this.. Restart the game. ");
                        }
                    }

                    #endregion

                    #region Waiting for answer from him..

                    double SecondsForAnswer = 0;

                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }
                    Player playerAnswering = new Player(1, "");

                    ConsoleKeyInfo ChoosenAnswer = new ConsoleKeyInfo();
                    if (ActivatedBuzzer.ToUpper() == "M")
                    {
                        playerAnswering = new Player(game.GamePlayers.ElementAt(0).PlayerNumber, game.GamePlayers.ElementAt(0).PlayerName);
                        playerAnswering = game.GamePlayers.ElementAt(0);
                        playerAnswering.PlayerNumber = 1;
                    }
                    if (ActivatedBuzzer.ToUpper() == "A")
                    {
                        playerAnswering = new Player(game.GamePlayers.ElementAt(2).PlayerNumber, game.GamePlayers.ElementAt(2).PlayerName);
                        playerAnswering = game.GamePlayers.ElementAt(2);
                        playerAnswering.PlayerNumber = 3;
                    }
                    if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                    {
                        playerAnswering = new Player(game.GamePlayers.ElementAt(1).PlayerNumber, game.GamePlayers.ElementAt(1).PlayerName);
                        playerAnswering = game.GamePlayers.ElementAt(1);
                        playerAnswering.PlayerNumber = 2;
                    }

                    ExtraSecondsUntilNextClue = SecondsThatPassed3;

                    Console.Beep();
                    Console.WriteLine("");
                    Console.WriteLine($@"{game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).PlayerName} is Answering..");
                    Console.WriteLine("You have 30 seconds.");
                    Console.WriteLine("");

                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                    }

                    //Thread timerThread2 = new Thread(new ThreadStart(RunTimerForNormalModeSomeone));
                    //double SecondsForAnswer = 10;
                    //timerThread2.Start();

                    //Thread count = new Thread(new ThreadStart(RunTimerForWhoAmIAndCorrectOrFalseModes));
                    //count.Start();

                    DateTime now = DateTime.Now;
                    string PlayerAnswer = Console.ReadLine();
                    DateTime after = DateTime.Now;
                    bool AfterMinute = after.Minute > now.Minute && after.Second >= now.Second - 30;
                    bool SameMinute = after.Minute == now.Minute && after.Second >= now.Second + 30;

                    if (AfterMinute || SameMinute)
                    {
                        #region the same as the player not answering

                        IsAfterAPlayerWrong = true;

                        SoundPlayer NoAnswer = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav");
                        NoAnswer.Play();
                        Thread.Sleep(6500);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                        }


                        if (ActivatedBuzzer.ToUpper() == "M")
                        {
                            playerAnswering = game.GamePlayers.ElementAt(0);
                            playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                        }
                        else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                        {
                            playerAnswering = game.GamePlayers.ElementAt(1);
                            playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                            playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                        }
                        else if (ActivatedBuzzer.ToUpper() == "A")
                        {
                            playerAnswering = game.GamePlayers.ElementAt(2);
                            playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                            playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                        }

                        playerAnswering.PreTrapSum = playerAnswering.PreTrapSum - WhoAmIObject.choosenQuestion.Money;

                        Console.Beep();
                        Console.WriteLine();
                        Console.WriteLine("It's been over 30 seconds..");
                        Console.WriteLine("What, you thought you could get away with it?");
                        Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.PreTrapSum}");
                        Console.WriteLine();
                        playerAnswering.HasThePlayerChoosenAnswer = true;
                        game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).HasThePlayerChoosenAnswer = true;
                        bool HasEveryoneAnswered = false;
                        HasNoOneAnswered = false;

                        if (game.GamePlayers.Count == 1)
                        {
                            HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                        }
                        if (game.GamePlayers.Count == 2)
                        {
                            HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                        }
                        if (game.GamePlayers.Count == 3)
                        {
                            HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer;
                        }
                        if (!HasEveryoneAnswered)
                        {
                            Thread.Sleep(2000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            //game = WhoAmIGame(game, true);
                            //return game;
                        }
                        else
                        {
                            Thread.Sleep(1000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            SoundPlayer EveryoneAreWrong = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\EveryoneAreWrong.wav");
                            EveryoneAreWrong.Play();
                            Thread.Sleep(3500);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            Console.Clear();
                            Console.Beep();
                            Console.WriteLine($@"The correct answer (in my opinion) is:");
                            Console.WriteLine($@"");
                            string CorrectAnswer = "";
                            string CorrectAnswerContent = $@"{WhoAmIObject.choosenQuestion.CorrectAnswer}";


                            Console.WriteLine($@"{CorrectAnswerContent}");

                            Thread.Sleep(3000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }
                            Console.Clear();
                            return game;
                        }

                        #endregion
                    }
                    else
                    {
                        #region If the player is right

                        if (PlayerAnswer.ToUpper() == WhoAmIObject.choosenQuestion.CorrectAnswer.ToUpper())
                        {
                            #region if the answer is correct..     

                            if (ActivatedBuzzer.ToUpper() == "M")
                            {
                                playerAnswering = game.GamePlayers.ElementAt(0);
                                playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                            }
                            else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                            {
                                playerAnswering = game.GamePlayers.ElementAt(1);
                                playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                                playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                            }
                            else if (ActivatedBuzzer.ToUpper() == "A")
                            {
                                playerAnswering = game.GamePlayers.ElementAt(2);
                                playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                                playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                            }

                            HasNoOneAnswered = false;

                            playerAnswering.PreTrapSum = playerAnswering.PreTrapSum + WhoAmIObject.choosenQuestion.Money;

                            SoundPlayer correct = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\CorrectAnswer.wav");
                            correct.Stop();
                            correct.Play();
                            Thread.Sleep(600);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }
                            Console.WriteLine("");
                            Console.WriteLine("Oh yeah! Correct Answer!");
                            Console.WriteLine("");
                            Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.PreTrapSum}.");
                            /* stopping by to explain sound file
                             * SoundPlayer explanation = new SoundPlayer($@"path for explanation .wav file")
                             * explanation.Play();
                             * Thread.Sleep(sound file length);
                             * than, no additional user kep press is required
                             */
                            Console.WriteLine("");
                            Console.WriteLine("Press enter to continue..");
                            Console.ReadLine();
                            Console.Clear();
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }
                            return game;

                            #endregion
                        }

                        #endregion

                        #region If the answer is wrong..

                        _stopRequestedForSomeoneAnswering = true;

                        if (ActivatedBuzzer.ToUpper() == "M")
                        {
                            playerAnswering = game.GamePlayers.ElementAt(0);
                            playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                        }
                        else if (ActivatedBuzzer.ToUpper() == "OEMPLUS")
                        {
                            playerAnswering = game.GamePlayers.ElementAt(1);
                            playerAnswering.PlayerName = game.GamePlayers.ElementAt(1).PlayerName;
                            playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(1).PlayerNumber;
                        }
                        else if (ActivatedBuzzer.ToUpper() == "A")
                        {
                            playerAnswering = game.GamePlayers.ElementAt(2);
                            playerAnswering.PlayerName = game.GamePlayers.ElementAt(2).PlayerName;
                            playerAnswering.PlayerNumber = game.GamePlayers.ElementAt(2).PlayerNumber;
                        }

                        HasNoOneAnswered = false;

                        playerAnswering.PreTrapSum = playerAnswering.PreTrapSum - WhoAmIObject.choosenQuestion.Money;

                        IsAfterAPlayerWrong = true;

                        Console.WriteLine("");

                        SoundPlayer Incorrect = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\IncorrectAnswer.wav");
                        Incorrect.Play();
                        Thread.Sleep(1200);
                        while (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                        }

                        Console.Beep();
                        Console.WriteLine();
                        Console.WriteLine("Wrong!");
                        Console.WriteLine($@"{playerAnswering.PlayerName} now has ${playerAnswering.PreTrapSum}");
                        Console.WriteLine();
                        playerAnswering.HasThePlayerChoosenAnswer = true;

                        game.GamePlayers.ElementAt(playerAnswering.PlayerNumber - 1).HasThePlayerChoosenAnswer = true;
                        bool HasEveryoneAnswered = false;

                        if (game.GamePlayers.Count == 1)
                        {
                            HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer;
                        }
                        if (game.GamePlayers.Count == 2)
                        {
                            HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer;
                        }
                        if (game.GamePlayers.Count == 3)
                        {
                            HasEveryoneAnswered = game.GamePlayers.ElementAt(0).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(1).HasThePlayerChoosenAnswer && game.GamePlayers.ElementAt(2).HasThePlayerChoosenAnswer;
                        }
                        if (!HasEveryoneAnswered)
                        {
                            Thread.Sleep(2000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            //game = WhoAmIGame(game, true);
                            //return game;
                        }
                        else
                        {
                            Thread.Sleep(1000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            SoundPlayer EveryoneAreWrong = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\EveryoneAreWrong.wav");
                            EveryoneAreWrong.Play();
                            Thread.Sleep(3500);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                            }
                            Console.Clear();
                            Console.Beep();
                            Console.WriteLine($@"The correct answer (in my opinion) is:");
                            Console.WriteLine($@"");
                            string CorrectAnswer = "";
                            string CorrectAnswerContent = WhoAmIObject.choosenQuestion.CorrectAnswer;

                            Console.WriteLine($@"{CorrectAnswerContent}");

                            Thread.Sleep(3000);
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                            }
                            Console.Clear();
                            return game;
                        }

                        // Now to continue to the other buzzers..

                        #endregion
                    }

                    #endregion

                    #endregion
                }
            }

            #region If nobody bothered to answer or time out..

            if (HasNoOneAnswered || WhoAmIObject.choosenQuestion.Money == 0)
            {
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey();
                }

                SoundPlayer NoAnswer = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\NoOneAnswered.wav");
                NoAnswer.Play();
                Thread.Sleep(6500);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }

                Console.Clear();
                Console.Beep();
                Console.WriteLine($@"The correct answer (in my opinion) is:");
                Console.WriteLine($@"");
                string CorrectAnswer = WhoAmIObject.choosenQuestion.CorrectAnswer;
                
                //string CorrectAnswerContent = File.ReadAllText($@"
                // ($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}\{QuestionsFoldersList.Find(x => x == CorrectFolder).Name}\Answer{TheQuestion.CorrectAnswer}.txt");

                Console.WriteLine($@"{CorrectAnswer}");

                Thread.Sleep(3000);
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
                }
                Console.Clear();
                return game;
            }

            #endregion

            #endregion

            Console.Beep();
            return game;
        }

        #endregion

        #region Properties

        public int CountOfQuestion { get; set; }
        public Player PlayerToChooseCategory { get; set; }

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
        public bool IsOnNormalMode { get; set; }


        #endregion
    }
}
