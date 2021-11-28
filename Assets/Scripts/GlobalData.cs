using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public static class GlobalData
{
    public static int testInt = new int();
    public static int[] charactersToWinArray;
    public static int[] ratingTierArray;

    public static Color32 blueTextColor = new Color32(59, 88, 153, 255);
    public static Color32 greenTextColor = new Color32(69, 255, 86, 255);
    public static Color32 redTextColor = new Color32(143, 80, 83, 255);

    /*    public static Color32 blueTextColor = new Color32(0, 0, 255, 255);
        public static Color32 greenTextColor = new Color32(0, 255, 0, 255);
        public static Color32 redTextColor = new Color32(255, 0, 0, 255);*/

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadGlobalData()
    {
        // Misc game data
        testInt = 1;
        charactersToWinArray = new int[3] { 100, 350, 500 };
        ratingTierArray = new int[3] { 1000, 1500, 2000 };
    }
    


    public struct TestStuct
    {
        public string skillName;

        public TestStuct(string skillName)
        {
            this.skillName = skillName;
        }
    }

    public static class AllSkillsData
    {
        public static TestStuct farming = new TestStuct("farming");
    }

}