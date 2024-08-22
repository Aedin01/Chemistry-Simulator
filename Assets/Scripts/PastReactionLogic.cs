using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
using System.IO;

public class PastReactionLogic
{
    public class Reaction
    {
        public int id;
        public string title;
        public string equation;
        public DateTime date;
    }

    public static List<Reaction> reactions = new List<Reaction>();
    public static void SaveReactions()
    {
        string json = JsonConvert.SerializeObject(reactions, Formatting.Indented);
        File.WriteAllText(Application.persistentDataPath + "/reactions.json", json);
    }
    public static void LoadReactions()
    {
        string path = Application.persistentDataPath + "/reactions.json";
        string json = File.ReadAllText(path);
        reactions = JsonConvert.DeserializeObject<List<Reaction>>(json);
    }

    [Serializable]
    private class ReactionList
    {
        public List<Reaction> reactions;

        public ReactionList(List<Reaction> reactions)
        {
            this.reactions = reactions;
        }
    }
}
