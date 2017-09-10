/*using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// VOManager - Description:
/// Manages and handles how the clips 
/// from the VOBank are played.
/// </summary>

// automatically assign the VOBank.cs script and 
// audio source component to the object if none
// is present
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(VOBank))]
public class VOManager : MonoBehaviour
{
    // global variables
    public static VOManager Instance;       // make VOManager a global instance so we can use for reference

    // public variables
    public Text txtSubtitle;                // the text component that will dispaly the subtitles

    // private variables
    private AudioSource m_audSrc;           // the object's audio source component
    private VOBank voBank;                  // the VOBank reference
    private bool m_blnDialogueTriggered;    // boolean to check if a dialogue has been triggered

    // on enabled
    void OnEnable()
    {
        m_audSrc = this.GetComponent<AudioSource>();    // get the object's audio source component
        voBank = this.GetComponent<VOBank>();           // get the VOBank reference
    }

    // on loaded
    void Awake()
    {
        Instance = this;    // set the Instance to this object reference

        // try and run the following
        try
        {
            txtSubtitle.text = "";              // clear out any subtitle text
        }
        catch
        {
            // throw a warning
            Debug.LogError("No UI Text component is found. Please set the subtitle Text component.");
        }

        m_blnDialogueTriggered = false;     // set dialogue to not triggered
    }

    // on initialization
    void Start ()
    {
        m_audSrc.playOnAwake = false;   // disable play on awake
        m_audSrc.spatialBlend = 0;      // set sound settings to 2D
        m_audSrc.loop = false;          // turn off sound looping
    }

    // once per frame
    void Update()
    {
        // if a dialogue was triggered
        if (m_blnDialogueTriggered)
            // if the current audio clip is done playing
            if (!m_audSrc.isPlaying)
            {
                Stop();                             // call the stop function
                m_blnDialogueTriggered = false;     // set the dialogue trigger to false
            }
    }

    // play the desired clip only if no other clips are playing
    public void Play(int _id)
    {
        // if there is an audio clip playing
        if (m_audSrc.isPlaying)
            return;
        else
        {
            // try and run the following
            try
            {
                txtSubtitle.text = voBank.bank[_id].strSubtitle;   // set the subtitle text on the screen (display)
                m_audSrc.clip = voBank.bank[_id].audClpDialogue;   // set the audio source's clip to the new clip
                m_audSrc.Play();                            // play the new audio clip
                m_blnDialogueTriggered = true;              // set the dialogue trigger to true
            }
            catch
            {
                // throw a warning
                Debug.LogError("Index is out of range. Please check if the audio bank is the correct length.");
            }
        }
    }

    // play the desired clip and stop other audio clips that are playing
    public void PlayInterrupt(int _id)
    {
        // if there is an audio clip playing
        if (m_audSrc.isPlaying)
            // then stop playing the current clip
            m_audSrc.Stop();

        // try and run the following
        try
        {
            txtSubtitle.text = voBank.bank[_id].strSubtitle;   // set the subtitle text on the screen (display)
            m_audSrc.clip = voBank.bank[_id].audClpDialogue;   // set the audio source's clip to the new clip
            m_audSrc.Play();                                   // play the new audio clip
            m_blnDialogueTriggered = true;                     // set the dialogue trigger to true
        }
        catch
        {
            // throw a warning
            Debug.LogError("Index is out of range. Please check if the audio bank is the correct length.");
        }
    }

}
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// VOManager
/// Description: Manages and handles how the clips from the VOBank are played.
/// </summary>

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(VOBank))]
public class VOManager : MonoBehaviour
{
    // global variables
    public static VOManager Instance;   // instance to access our script globally
    public Text uiTextObject;  // get a reference to our ui text object

    // private variables
    private VOBank m_voBank;  // our vo bank reference
    private AudioSource m_audSrc;   // get a reference to our default audio source
    private AudioSource m_externalAudSrc;   // get a reference to our external audio source if there is any
    private bool m_blnAudioTriggered;   // check if our audio is playing or not
    private float m_fltDialogueTimer;   // keep track of how long a dialogue should be displayed

    // on loaded
    void Awake()
    {
        Instance = this;    // set the instance to this object reference
        m_voBank = this.GetComponent<VOBank>();   // get the vo bank reference
        m_audSrc = this.GetComponent<AudioSource>();    // get the audio source on this object
        m_externalAudSrc = null;    // reset the external audio source
    }

    // on start
    private void Start()
    {
        try
        {
            uiTextObject.text = "";   // reset the ui text object
        }
        catch
        {
            Debug.LogError("No UI Text Object has been set.");
        }

        m_audSrc.clip = null;   // empty the current clip
        m_audSrc.loop = false;  // turn off looping
        m_audSrc.playOnAwake = false;   // disable play on awake
        m_blnAudioTriggered = false;    // set audio trigger to false
    }

    // on each frame
    private void Update()
    {
        // if the audio is triggered
        if (m_blnAudioTriggered)
        {
            // if there isn't an external source
            if (m_externalAudSrc == null)
            {
                // if the audio is not playing 
                if (!m_audSrc.isPlaying)
                {
                    // reset the audio source
                    m_audSrc.clip = null;

                    // reset the dialogue
                    uiTextObject.text = "";

                    // reset the trigger
                    m_blnAudioTriggered = false;

                }
            }
            else
            {
                // if the external audio is not playing 
                if (!m_externalAudSrc.isPlaying)
                {
                    // clear the audio source
                    m_externalAudSrc.clip = null;
                    m_externalAudSrc = null;

                    // reset the dialogue
                    uiTextObject.text = "";

                    // reset the trigger
                    m_blnAudioTriggered = false;
                }
            }
        }
    }

    // find and return an audio with the corresponding id
    private ClipProperties FindAudioByID(int _id)
    {
        // sort through the list
        foreach (ClipProperties clip in m_voBank.bank)
        {
            // find the matching id
            if (clip.id == _id)
                // return the matching clip
                return clip;
        }

        // if nothing return null
        return null;
    }

    // find and return an audio with the corresponding name
    private ClipProperties FindAudioByName(string _name)
    {
        // sort through the list
        foreach (ClipProperties clip in m_voBank.bank)
        {
            // find the matching id
            if (clip.strName == _name)
                // return the matching clip
                return clip;
        }

        // if nothing return null
        return null;
    }

    // play an audio through the default source via an id
    public void Play(int _id)
    {
        // if an audio clip is already playing
        if (m_blnAudioTriggered)
            // return and don't run anything else
            return;
        // else
        else
        {
            // try and run the following
            try
            {
                // find and return the current audio we are playing
                ClipProperties clip = FindAudioByID(_id);

                // set the source and play the audio
                m_audSrc.clip = clip.audClpDialogue;
                m_audSrc.Play();
                m_blnAudioTriggered = true;

                // display the dialogue
                uiTextObject.text = clip.dialogue;
            }
            catch
            {
                // throw a warning
                Debug.LogError("There has been an error playing your dialogue. Please double check and make sure the VO Bank and Manager is configured correctly.");
            }
        }
    }

    // play an audio to a source via an id
    public void Play(AudioSource _audSrc, int _id)
    {
        // if an audio clip is already playing
        if (m_blnAudioTriggered)
            // return and don't run anything else
            return;
        // else
        else
        {
            // try and run the following
            try
            {
                // find and return the current audio we are playing
                ClipProperties clip = FindAudioByID(_id);

                // set the source and play the audio
                m_externalAudSrc = _audSrc;
                m_externalAudSrc.clip = clip.audClpDialogue;
                m_externalAudSrc.Play();
                m_blnAudioTriggered = true;

                // display the dialogue
                uiTextObject.text = clip.dialogue;
            }
            catch
            {
                // throw a warning
                Debug.LogError("There has been an error playing your dialogue. Please double check and make sure the VO Bank and Manager is configured correctly.");
            }
        }
    }

    // play an audio through the default source via a name
    public void Play(string _name)
    {
        // if an audio clip is already playing
        if (m_blnAudioTriggered)
            // return and don't run anything else
            return;
        // else
        else
        {
            // try and run the following
            try
            {
                // find and return the current audio we are playing
                ClipProperties clip = FindAudioByName(_name);

                // set the source and play the audio
                m_audSrc.clip = clip.audClpDialogue;
                m_audSrc.Play();
                m_blnAudioTriggered = true;

                // display the dialogue
                uiTextObject.text = clip.dialogue;
            }
            catch
            {
                // throw a warning
                Debug.LogError("There has been an error playing your dialogue. Please double check and make sure the VO Bank and Manager is configured correctly.");
            }
        }
    }

    // play an audio to a source via a name
    public void Play(AudioSource _audSrc, string _name)
    {
        // if an audio clip is already playing
        if (m_blnAudioTriggered)
            // return and don't run anything else
            return;
        // else
        else
        {
            // try and run the following
            try
            {
                // find and return the current audio we are playing
                ClipProperties clip = FindAudioByName(_name);

                // set the source and play the audio
                m_externalAudSrc = _audSrc;
                m_externalAudSrc.clip = clip.audClpDialogue;
                m_externalAudSrc.Play();
                m_blnAudioTriggered = true;

                // display the dialogue
                uiTextObject.text = clip.dialogue;
            }
            catch
            {
                // throw a warning
                Debug.LogError("There has been an error playing your dialogue. Please double check and make sure the VO Bank and Manager is configured correctly.");
            }
        }
    }

    // force play an audio to the default source via an id
    public void ForcePlay(int _id)
    {
        // if there is an audio clip playing stop it
        Stop();

        // try and run the following
        try
        {
            // find and return the current audio we are playing
            ClipProperties clip = FindAudioByID(_id);

            // set the source and play the audio
            m_audSrc.clip = clip.audClpDialogue;
            m_audSrc.Play();
            m_blnAudioTriggered = true;

            // display the dialogue
            uiTextObject.text = clip.dialogue;
        }
        catch
        {
            // throw a warning
            Debug.LogError("There has been an error playing your dialogue. Please double check and make sure the VO Bank and Manager is configured correctly.");
        }
    }

    // force play an audio to a source via an id
    public void ForcePlay(AudioSource _audSrc, int _id)
    {
        // if there is an audio clip playing stop it
        Stop();

        // try and run the following
        try
        {
            // find and return the current audio we are playing
            ClipProperties clip = FindAudioByID(_id);

            // set the source and play the audio
            m_externalAudSrc = _audSrc;
            m_externalAudSrc.clip = clip.audClpDialogue;
            m_externalAudSrc.Play();
            m_blnAudioTriggered = true;

            // display the dialogue
            uiTextObject.text = clip.dialogue;
        }
        catch
        {
            // throw a warning
            Debug.LogError("There has been an error playing your dialogue. Please double check and make sure the VO Bank and Manager is configured correctly.");
        }
    }

    // force play an audio to the default source via a name
    public void ForcePlay(string _name)
    {
        // if there is an audio clip playing stop it
        Stop();

        // try and run the following
        try
        {
            // find and return the current audio we are playing
            ClipProperties clip = FindAudioByName(_name);

            // set the source and play the audio
            m_audSrc.clip = clip.audClpDialogue;
            m_audSrc.Play();
            m_blnAudioTriggered = true;

            // display the dialogue
            uiTextObject.text = clip.dialogue;
        }
        catch
        {
            // throw a warning
            Debug.LogError("There has been an error playing your dialogue. Please double check and make sure the VO Bank and Manager is configured correctly.");
        }
    }

    // force play an audio to a source via a name
    public void ForcePlay(AudioSource _audSrc, string _name)
    {
        // if there is an audio clip playing stop it
        Stop();

        // try and run the following
        try
        {
            // find and return the current audio we are playing
            ClipProperties clip = FindAudioByName(_name);

            // set the source and play the audio
            m_externalAudSrc = _audSrc;
            m_externalAudSrc.clip = clip.audClpDialogue;
            m_externalAudSrc.Play();
            m_blnAudioTriggered = true;

            // display the dialogue
            uiTextObject.text = clip.dialogue;
        }
        catch
        {
            // throw a warning
            Debug.LogError("There has been an error playing your dialogue. Please double check and make sure the VO Bank and Manager is configured correctly.");
        }
    }

    // stop an audio file from playing
    public void Stop()
    {
        // if there is an external audio source
        if (m_externalAudSrc)
        {
            // stop the enteral audio source and remove the clip
            m_externalAudSrc.Stop();
            m_externalAudSrc.clip = null;
        }
        else
        {
            // stop audio source and remove the clip
            m_audSrc.Stop();
            m_audSrc.clip = null;
        }

        // clear the text object
        uiTextObject.text = "";
    }
}