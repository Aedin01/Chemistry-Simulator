using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PastReactionUI : MonoBehaviour
{
    public GameObject notes;
    public GameObject achievements;
    public GameObject reactions;
    public GameObject reactionUI1;
    public GameObject reactionUI2;
    public Text title1;
    public Text title2;
    public Text equation1;
    public Text equation2;
    public Text date1;
    public Text date2;
    public static int page;
    public Text pagetext;
    public Text noReactions;
    public void Start()
    {
        PastReactionLogic.LoadReactions();
        ResetPage();
        LoadReactions();
    }
    public void ReactionsToNotes()
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
            LoadReactions();
        }
    }
    public void PageRight()
    {
        if(page != Math.Ceiling(Convert.ToDecimal(PastReactionLogic.reactions.Count)/2))
        {
            page++;
            pagetext.text = "Page " + page;
            LoadReactions();
        }
    }
    public void LoadReactions()
    {
        PastReactionLogic.reactions = PastReactionLogic.reactions.OrderByDescending(x => x.date).ToList();

        if(PastReactionLogic.reactions.Count == 0)
        {
            noReactions.gameObject.SetActive(true);
        }
        else
        {
            noReactions.gameObject.SetActive(false);
        }
    
        if(2*page-2 < PastReactionLogic.reactions.Count())
        {
            reactionUI1.SetActive(true);
            PastReactionLogic.Reaction reaction1 = PastReactionLogic.reactions[2*page - 2];
            title1.text = reaction1.title;
            equation1.text = reaction1.equation;
            date1.text = reaction1.date.Date.ToString();
        }
        else
        {
            reactionUI1.SetActive(false);
        }
        if(2*page-1 < PastReactionLogic.reactions.Count())
        {
            reactionUI2.SetActive(true);
            PastReactionLogic.Reaction reaction2 = PastReactionLogic.reactions[2*page - 1];
            title2.text = reaction2.title;
            equation2.text = reaction2.equation;
            date2.text = reaction2.date.Date.ToString();
        }
        else
        {
            reactionUI2.SetActive(false);
        }
    }
}
