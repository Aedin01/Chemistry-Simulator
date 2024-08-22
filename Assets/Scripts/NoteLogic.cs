using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
using System.IO;

public class NoteLogic
{
    public class Note
    {
        public string Title;
        public DateTime date;
        public string contents;
    }

    public static List<Note> notes = new List<Note>();
    private static void SaveNotes()
    {
        string json = JsonConvert.SerializeObject(notes, Formatting.Indented);
        Debug.Log(Application.persistentDataPath + "/notes.json");
        File.WriteAllText(Application.persistentDataPath + "/notes.json", json);
    }

    [Serializable]
    private class NoteList
    {
        public List<Note> notes;

        public NoteList(List<Note> notes)
        {
            this.notes = notes;
        }
    }
}
