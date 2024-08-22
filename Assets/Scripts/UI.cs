//UI.cs
//Aiden Furey, 15/7/2024
//Implementation of the entire system's user interface

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//UI class derived from MonoBehaviour to allow interactive Unity elements
public class UI : MonoBehaviour
{
    //Reference to the element selection panel
    public GameObject elements;
    //Reference to the compound selection panel
    public GameObject compounds;
    //Reference to the notebook's notes panel
    public GameObject notes;
    //Reference to the notebook's achievements panel
    public GameObject achievements;
    //Reference to the notebook's reactions panel
    public GameObject reactions;
    //Reference to the cover of the notebook
    public GameObject notebook;
    //Reference to the button designed for access to the elements panel
    public GameObject elementsbutton;
    //Reference to the button designed to exit to menu
    public GameObject exitbutton;

    //
    //
    //  REDUNDANT
    //
    //
    public void StartButton()
    {
        SceneManager.LoadScene("Chemistry Simulator");
    }
    public void PeriodicTableButton()
    {
        SceneManager.LoadScene("Periodic Table");
    }
    //Called when the elements button is called, to open the element selection panel
    public void Elements()
    {
        elements.SetActive(true);
        elementsbutton.SetActive(false);
        exitbutton.SetActive(false);
    }
    //Called when the exit button is pressed during element selection, to close the element selection panel
    public void ExitElements()
    {
        elements.SetActive(false);
        elementsbutton.SetActive(true);
        exitbutton.SetActive(true);
    }
    //Called when the exit button is pressed during element selection, to close the compound selection panel
    public void ExitCompounds()
    {
        compounds.SetActive(false);
        elementsbutton.SetActive(true);
        exitbutton.SetActive(true);
    }
    //Called when the compounds button is pressed during element selection, to open the compound selection panel
    public void ElementsToCompounds()
    {
        elements.SetActive(false);
        compounds.SetActive(true);
    }
    //Called when the elements button is pressed during compound selection, to open the element selection panel
    public void CompoundsToElements()
    {
        compounds.SetActive(false);
        elements.SetActive(true);
    }
    //Called when the achievements button is pressed when the notes panel is open, to open the achievements panel
    public void NotesToAchievments()
    {
        notes.SetActive(false);
        achievements.SetActive(true);
    }
    //Called when the reactions button is pressed when the notes panel is open, to open the reactions panel
    public void NotesToReactions()
    {
        notes.SetActive(false);
        reactions.SetActive(true);
    }
    //Called when the notes button is pressed when the achievements panel is open, to open the notes panel
    public void AchievementsToNotes()
    {
        achievements.SetActive(false);
        notes.SetActive(true);
    }
    //Called when the reactions button is pressed when the achievements panel is open, to open the reactions panel
    public void AchievementsToReactions()
    {
        achievements.SetActive(false);
        reactions.SetActive(true);
    }
    //Called when the notes button is pressed when the reactions panel is open, to open the notes panel
    public void ReactionsToNotes()
    {
        reactions.SetActive(false);
        notes.SetActive(true);
    }
    //Called when the achievements button is pressed when the reactions panel is open, to open the achievements panel
    public void ReactionsToAchievements()
    {
        reactions.SetActive(false);
        achievements.SetActive(true);
    }
    //Called when the exit button is pressed when the notebook is open, to close the notebook
    public void ExitNotebook()
    {
        notes.SetActive(false);
        achievements.SetActive(false);
        reactions.SetActive(false);
        //Starts an asynchronous routine that closes the notebook
        StartCoroutine(notebook.GetComponent<Notebook>().CloseNotebook());
    }
}
