using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// VOBank
/// Description: An audio bank that holds a collection of voice-over clips and its properties.
/// </summary>

// contains information for each audio set in the vo library
[System.Serializable]
public class ClipProperties
{
    public int id;                      // keeps track of the clip's current id 
    public AudioClip audClp;    // the current audio clip
    public string strName;              // the name of the current clip
    public bool blnExpandClipPnl;       // boolean to keep track of custom editor panel
    public string subtitle;             // the current clip's subtitle
}

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(VOManager))]
public class VOBank : MonoBehaviour
{
    // a list of audio clip properties
    public List<ClipProperties> bank = new List<ClipProperties>();

    // add a new clip to the bank
    public void AddClip(ClipProperties _clipProperties)
    {
        bank.Add(new ClipProperties());
    }

    // add a new empty clip to the bank
    public void AddEmptyClip()
    {
        ClipProperties clipProp = new ClipProperties();
        clipProp.id = 0;
        clipProp.strName = "Clip Name";
        clipProp.subtitle = "";
        bank.Add(clipProp);
    }

    // remove a clip from the bank
    public void RemoveClip(int _id)
    {
        bank.RemoveAt(_id);
    }
}