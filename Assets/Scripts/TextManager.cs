using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/*public class TextManager : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip[] clips;
    public GameObject solid;
    public Rigidbody solidbody;
    public Text textbox;
    Dictionary<string, float> text = new Dictionary<string, float>()
    {
        {"Placeholder text",1.28f},
        {"a",1.92f},
        {"a", 2.11f},
        {"a",5f},
        {"a", 1.5f},
        {"aa",8f}
    };

    void Start()
    {
        StartCoroutine(DisplayText());
        StartCoroutine(PlayClips());
    }
    IEnumerator DisplayText()
    {
        foreach(KeyValuePair<string, float> s in text)
        {
            textbox.text = s.Key;
            yield return new WaitForSeconds(s.Value);
        }
    }
    IEnumerator PlayClips()
    {
        for(int i = 0; i < clips.Length; i++)
        {
            audio.clip = clips[i];
            audio.Play();
            yield return new WaitWhile(() => audio.isPlaying);
            if(i == 3)
            {
                yield return new WaitForSeconds(1f);
                Destroy(solid);
            }
            if(i == 2)
            {
                yield return new WaitForSeconds(3f);
            }
            yield return new WaitForSeconds(1.5f);
            if(i == 1 )
            {
                yield return new WaitForSeconds(2f);
                solidbody.useGravity = true;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}*/
