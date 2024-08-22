using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    public GameObject notes;
    public GameObject achievements;
    public GameObject reactions;
    public Text title1;
    public Text title2;
    public Text description1;
    public Text description2;
    public GameObject complete1;
    public GameObject complete2;
    public Text completetext1;
    public Text completetext2;
    public Text progress1;
    public Text progress2;
    public static int page;
    public Text pagetext;
    public void Start()
    {
        AchievementLogic.LoadAchievements();
        ResetPage();
        LoadAchievements();
    }
    public void AchievementsToNotes()
    {
        achievements.SetActive(false);
        notes.SetActive(true);
    }
    public void AchievementsToReactions()
    {
        achievements.SetActive(false);
        reactions.SetActive(true);
    }
    public void ResetPage()
    {
        page = 1;
    }
    public void PageLeft()
    {
        if(page != 1)
        {
            page--;
            pagetext.text = "Page " + page;
            LoadAchievements();
        }
    }
    public void PageRight()
    {
        if(page != AchievementLogic.achievements.Count / 2)
        {
            page++;
            pagetext.text = "Page " + page;
            LoadAchievements();
        }
    }
    public void LoadAchievements()
    {
        AchievementLogic.achievements = AchievementLogic.achievements.OrderByDescending(x => x.progress).ToList();


        AchievementLogic.Achievement achievement1 = AchievementLogic.achievements[2*page - 2];
        AchievementLogic.Achievement achievement2 = AchievementLogic.achievements[2*page - 1];
        title1.text = achievement1.title;
        title2.text = achievement2.title;
        description1.text = achievement1.description;
        description2.text = achievement2.description;
        complete1.GetComponent<Image>().color = achievement1.complete ?  Color.green : Color.red;
        complete2.GetComponent<Image>().color = achievement2.complete ?  Color.green : Color.red;
        completetext1.text = achievement1.complete ? "Complete" : "Incomplete";
        completetext2.text = achievement2.complete ? "Complete" : "Incomplete";
        progress1.text = achievement1.progress + "%";
        progress2.text = achievement2.progress + "%";
    }
}
