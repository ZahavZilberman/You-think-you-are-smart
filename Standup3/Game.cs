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
            IsOnNormalMode = false;
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
            /*
            string currentPlayerturnName = playerNames.ElementAt(0);
            XDocument questions = new XDocument("standup/Questions.xml");
            XElement questionsXMLstart = questions.Root;
            List<XElement> questionsXML = new List<XElement>(questionsXMLstart.Elements("question"));

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
                QuestionText = File.ReadAllLines(textPath).To;
                
                SystemSound sound = new SystemSound
                sound.Play();
            }
            */


            ChooseCategory QuestionCategoryDetails = ChooseCategoryFunction(game, game.GamePlayers.ElementAt(0));

            foreach (Player player in GamePlayers)
            {
                if (player.PlayerNumber == QuestionCategoryDetails.PlayerNumber)
                {
                    game.PlayerToChooseCategory = player;
                }
            }

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
                IsOnNormalMode = true; // just to make sure this is why the console app sometimes shuts off
            }
            else if (!WillFactGameHappen && WillWhoAmIHappen)
            {
                IsOnWhoAmIMode = true;
                WhoAmI whoamigame = new WhoAmI(game);
                IsOnNormalMode = true; // just to make sure this is why the console app sometimes shuts off
            }
            else if (WillFactGameHappen && WillWhoAmIHappen)
            {
                IsOnNormalMode = true;
            }
            else if (!WillWhoAmIHappen && !WillFactGameHappen)
            {
                IsOnNormalMode = true;
            }

            if (IsOnNormalMode)
            {
                NormalQuestion(game, QuestionCategoryDetails);
                while(game.CountOfQuestion < game.NumOfGameQuestions)
                {
                    NextTurn(game, game.PlayerToChooseCategory);
                }
            }
        }

        #endregion

        #region Next question..

        public void NextTurn(Game game, Player playerToChooseCategory)
        {

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
            int chanceOfDoubleRound = doubleM.Next(1, 5);
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
                    while (ChoosenCategory.Key.ToString() != "D1" && ChoosenCategory.Key.ToString() != "D2")
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
                        Console.WriteLine($@"1. {choosingthis.AllCategoriesNames.ElementAt(FirstCategoryDisplayed)} (${Money1stCategory}");
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
                        Console.WriteLine($@"2. {choosingthis.AllCategoriesNames.ElementAt(SecondCategoryDisplayed)} ${Money2ndCategory}");
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
            Thread.Sleep(2000);
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
            game.CountOfQuestion = game.CountOfQuestion + 1;
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
                    Console.Write($@" {10 - i}");
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

            for (int i = 0; i <= 30; i++)
            {
                Console.WriteLine(" i");
                Thread.Sleep(1000); // Wait for one second
            }
        }

        #endregion

        #region Normal question happening

        public void NormalQuestion(Game game, ChooseCategory QuestionDetails)
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo UnTimedKey = Console.ReadKey(true);
            }

            NormalQuestion TheQuestion = new NormalQuestion(game, QuestionDetails.MoneyForThisCategoryQuestion, "", game.CountOfQuestion, 0, 0, "", QuestionDetails.choosenCategory);

            #region Opening object data

            DirectoryInfo SubjectTextDirectory = new DirectoryInfo($@"You-think-you-are-smart\QuestionsText\{QuestionDetails.choosenCategory}");
            Random rnd = new Random();
            //SubjectTextDirectory.
            DirectoryInfo[] AllQuestionsFolders = SubjectTextDirectory.GetDirectories();
            int AmountOfQuestions = AllQuestionsFolders.Count();
            int ChoosenQuestion = rnd.Next(0, AmountOfQuestions);
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

                //Thread timerThread = new Thread(new ThreadStart(RunTimerForNormalModeEveryone));

                double SecondsThatPassed = 0;
                SoundPlayer QuestionMusic = new SoundPlayer($@"You-think-you-are-smart\QuestionSound\NormalQuestionMusic.wav");
                QuestionMusic.Play();
                string TheKeyPressed = "";
                Console.WriteLine("");
                ConsoleKeyInfo buzzer = new ConsoleKeyInfo();
                //timerThread.Start();
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
                    return;
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


                        if (consoleKeyInfo.Key.ToString().ToUpper() == "M" || consoleKeyInfo.Key.ToString().ToUpper() == "A" || consoleKeyInfo.Key.ToString().ToUpper() == "OEMPLUS")
                        {
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
                            else if(game.GamePlayers.Count == 2)
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
                                if(Player1AnsweredAlready || Player2AnsweredAlready)
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
                        

                                if(ActivatedBuzzer.ToUpper() == "M")
                                {
                                    playerAnswering = game.GamePlayers.ElementAt(0);
                                    playerAnswering.PlayerName = game.GamePlayers.ElementAt(0).PlayerName;
                                }
                                else if(ActivatedBuzzer.ToUpper() == "OEMPLUS")
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
                                    NormalQuestion(game, QuestionDetails);
                                    return;
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
                                    return;
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
                                    return;
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
                                        NormalQuestion(game, QuestionDetails);
                                        return;
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
                                        return;
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

        #region Blank spaces

        public string BlankSpaces(int numberOfSpaces)
        {
            return new string(' ', numberOfSpaces);
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
