namespace Vitaru.Plot.Story
{
    public class Map
    {
        /// <summary>
        /// Maiden's Cappricio
        /// (Reimu)
        /// </summary>
        public const int Map1 = 1371893;

        public const int Map1Timestamp1 = 5200;
        public const int Map1Timestamp2 = 59100;
        public const int Map1Timestamp3 = 93920;
        public const int Map1Timestamp4 = 149572;
        public const int Map1Timestamp5 = 177398;
        public const int Map1Timestamp6 = 205224;
        public const int Map1Timestamp7 = 233050;
        public const int Map1Timestamp8 = 246963;
        public const int Map1Timestamp9 = 274789;
        public const int Map1Timestamp10 = 302615;

        /// <summary>
        /// Age of reason
        /// (Sakuya)
        /// </summary>
        public const int Map2 = 942060;

        public const int Map2Timestamp1 = 1270;
        public const int Map2Timestamp2 = 17822;
        public const int Map2Timestamp3 = 28856;
        public const int Map2Timestamp4 = 44028;
        public const int Map2Timestamp5 = 61960;
        public const int Map2Timestamp6 = 64718;
        public const int Map2Timestamp7 = 66097;
        public const int Map2Timestamp8 = 67477;
        public const int Map2Timestamp9 = 88166;
        public const int Map2Timestamp10 = 89546;
        public const int Map2Timestamp11 = 110235;
        public const int Map2Timestamp12 = 111615;
        public const int Map2Timestamp13 = 128166;
        public const int Map2Timestamp14 = 130925;
        public const int Map2Timestamp15 = 132304;
        public const int Map2Timestamp16 = 133684;

        /// <summary>
        /// Frightful Flan's song
        /// (Flandre) + (Flandre + Remilia)
        /// </summary>
        public const int Map3 = 114716;

        /*
            //TODO: set
            if (workingBeatmap.Value.BeatmapInfo.OnlineBeatmapID == 0 && CurrentCharacter == Characters.SakuyaIzayoi && !late && playerList.Count == 1)
            {
                if (Time.Current >= 0 && time == 0)
                {
                    Speak("Can't be late. . .");
                    time++;
                }
            }

            if (workingBeatmap.Value.BeatmapInfo.OnlineBeatmapID == Map2 && CurrentCharacter == Characters.FlandreScarlet && late && insane && playerList.Count == 1)
            {
                if (Time.Current >= 760 && insanity == 0)
                {
                    Speak("That piano. . .");
                    insanity++;
                }
                if (Time.Current >= 12340 && insanity == 1)
                {
                    Speak("It is driving me insane!");
                    insanity++;
                }
                if (Time.Current >= 28600 && insanity == 2)
                {
                    Speak("Missy please, I am trying to think.");
                    insanity++;
                    insane.Value = false;
                }
            }

            if (workingBeatmap.Value.BeatmapInfo.OnlineBeatmapID == 114716 && CurrentCharacter == Characters.FlandreScarlet && late && !insane && playerList.Count == 1)
            {
                if (Time.Current >= 760 && insanity == 0)
                {
                    Speak("That piano really needs to stop. . .");
                    insanity++;
                }
            }
            if (false)//workingBeatmap.Value.BeatmapInfo.OnlineBeatmapID == 114716 && !insane && playerList.Count == 2)
            {
                if (CurrentCharacter == Characters.RemiliaScarlet)
                {
                    if (Time.Current >= 760 && awakening == 0)
                    {
                        Speak("Flandre, what happened to you?");
                        awakening++;
                    }
                    if (Time.Current >= 12340 && awakening == 1)
                    {
                        Speak("Flan, are you there?");
                        awakening++;
                    }
                    if (Time.Current >= 28600 && awakening == 2)
                    {
                        Speak("Its me, your sister Remilia,");
                        awakening++;
                    }
                    if (Time.Current >= 0 && awakening == 3)
                    {
                        Speak("Flan? I know you can hear me.");
                        awakening++;
                    }
                    if (Time.Current >= 0 && awakening == 4)
                    {
                        Speak("Please? I need to talk.");
                        awakening++;
                    }
                    if (Time.Current >= 0 && awakening == 5)
                    {
                        Speak("I know you're upset,");
                        awakening++;
                    }
                    if (Time.Current >= 0 && awakening == 6)
                    {
                        Speak("But I need my sister back.");
                        awakening++;
                    }
                    if (Time.Current >= 0 && awakening == 7)
                    {
                        Speak("What would Hong say if she knew you were this lazy?");
                        awakening++;
                    }
                }
                else if (CurrentCharacter == Characters.FlandreScarlet)
                {
                    if (Time.Current >= 0 && awakening == 0)
                    {
                        //Speak("\"Get off your ass\"?");
                        awakening++;
                    }
                }
            }

            if (false)//workingBeatmap.Value.BeatmapInfo.OnlineBeatmapID == 148000 && playerList.Count == 2)
            {
                if (CurrentCharacter == Characters.Kaguya)
                {
                    if (Time.Current >= 1280 && tresspassing == 0)
                    {
                        Speak("What a lovely night it is for a walk.");
                        tresspassing++;
                    }
                    if (Time.Current >= 20860 && tresspassing == 1)
                    {
                        Speak("Oh?");
                        tresspassing++;
                    }
                    if (Time.Current >= 22120 && tresspassing == 2)
                    {
                        Speak("It seems I am not alone. . .");
                        tresspassing++;
                    }
                    if (Time.Current >= 37280 && tresspassing == 3)
                    {
                        Speak("Someone is over there,");
                        tresspassing++;
                    }
                    if (Time.Current >= 41060 && tresspassing == 4)
                    {
                        Speak("Whaaa-");
                        tresspassing++;
                    }
                    if (Time.Current >= 82740 && tresspassing == 5)
                    {
                        Speak("Why are we fighting? What did I do to you?");
                        tresspassing++;
                    }
                }
                else if (CurrentCharacter == Characters.MarisaKirisame)
                {
                    if (Time.Current >= 88740 && tresspassing == 0)
                    {
                        Speak("Its what you'll do soon, I can't allow it.");
                        tresspassing++;
                    }
                }
            }

            if (false)//workingBeatmap.Value.BeatmapInfo.OnlineBeatmapID == 221777 && playerList.Count == 2)
            {
                if (CurrentCharacter == Characters.HongMeiling)
                {
                    if (Time.Current >= 0 && bond == 0)
                    {
                        Speak("");
                        bond++;
                    }
                }
                else if (CurrentCharacter == Characters.SakuyaIzayoi)
                {

                }
            }

            if (false)//workingBeatmap.Value.BeatmapInfo.OnlineBeatmapID == 1548917 && currentCharacter == Characters.KokoroHatano && !lastDance && playerList.Count == 1)
            {
                if (Time.Current >= 1430 && dance == 0)
                {
                    Speak("This is it,");
                    dance++;
                }
                if (Time.Current >= 23760 && dance == 1)
                {
                    Speak("My final act,");
                    dance++;
                }
                if (Time.Current >= 43300 && dance == 2)
                {
                    Speak("My Last Dance.");
                    dance++;
                    lastDance.Value = true;
                }
            }*/
    }
}
