//AchievementLogic.cs
//Aiden Furey, 15/7/2024
//Logic for the achievement system

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
using System.IO;

//Class for achievement logic, not derived from MonoBehaviour, as no Unity runtime methods are required
public class AchievementLogic
{
    //Struct that represents an achievement, 
    public struct Achievement
    {
        public int id;
        public string title;
        public string description;
        public string date;
        public bool complete;
        //Progress from 0 to 100, representing a percentage
        public int progress;
    }
    //A static, universal list containing achievement data at runtime
    public static List<Achievement> achievements = new List<Achievement>();
    //Specifies the path for the achievement json file, appended to the application's peristent data path
    public static string jsonPath = Application.persistentDataPath + "/achievements.json";
    //A method to save the current achievements in the chosen save file
    private static void SaveAchievements()
    {
        //Determines how to write the set of achievement data for a JSON file
        string json = JsonConvert.SerializeObject(achievements, Formatting.Indented);
        //Writes the achievement data to the specified json path
        File.WriteAllText(jsonPath, json);
    }
    //A method for loading all saved achievements to the hard-coded sachievements list at runtime
    public static void LoadAchievements()
    {
        //Saves the serialized json data to a string
        string json = File.ReadAllText(jsonPath);
        //Populates the achievements list with the deserialized json data
        achievements = JsonConvert.DeserializeObject<List<Achievement>>(json);
    }
    [Serializable]
    private class AchievementList
    {
        public List<Achievement> achievements;

        public AchievementList(List<Achievement> achievements)
        {
            this.achievements = achievements;
        }
    }
}
