using UnityEngine;
using UnityEngine.UI;

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
    private float m_fltSubtitleTimer;   // keep track of how long a subtitle should be displayed

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

                    // reset the subtitle
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

                    // reset the subtitle
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
                m_audSrc.clip = clip.audClp;
                m_audSrc.Play();
                m_blnAudioTriggered = true;

                // display the subtitle
                uiTextObject.text = clip.subtitle;
            }
            catch
            {
                // throw a warning
                Debug.LogError("There has been an error playing your audio clip. Please double check and make sure the VO Bank and Manager is configured correctly.");
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
                m_externalAudSrc.clip = clip.audClp;
                m_externalAudSrc.Play();
                m_blnAudioTriggered = true;

                // display the subtitle
                uiTextObject.text = clip.subtitle;
            }
            catch
            {
                // throw a warning
                Debug.LogError("There has been an error playing your audio clip. Please double check and make sure the VO Bank and Manager is configured correctly.");
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
                m_audSrc.clip = clip.audClp;
                m_audSrc.Play();
                m_blnAudioTriggered = true;

                // display the subtitle
                uiTextObject.text = clip.subtitle;
            }
            catch
            {
                // throw a warning
                Debug.LogError("There has been an error playing your audio clip. Please double check and make sure the VO Bank and Manager is configured correctly.");
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
                m_externalAudSrc.clip = clip.audClp;
                m_externalAudSrc.Play();
                m_blnAudioTriggered = true;

                // display the subtitle
                uiTextObject.text = clip.subtitle;
            }
            catch
            {
                // throw a warning
                Debug.LogError("There has been an error playing your audio clip. Please double check and make sure the VO Bank and Manager is configured correctly.");
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
            m_audSrc.clip = clip.audClp;
            m_audSrc.Play();
            m_blnAudioTriggered = true;

            // display the subtitle
            uiTextObject.text = clip.subtitle;
        }
        catch
        {
            // throw a warning
            Debug.LogError("There has been an error playing your audio clip. Please double check and make sure the VO Bank and Manager is configured correctly.");
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
            m_externalAudSrc.clip = clip.audClp;
            m_externalAudSrc.Play();
            m_blnAudioTriggered = true;

            // display the subtitle
            uiTextObject.text = clip.subtitle;
        }
        catch
        {
            // throw a warning
            Debug.LogError("There has been an error playing your audio clip. Please double check and make sure the VO Bank and Manager is configured correctly.");
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
            m_audSrc.clip = clip.audClp;
            m_audSrc.Play();
            m_blnAudioTriggered = true;

            // display the subtitle
            uiTextObject.text = clip.subtitle;
        }
        catch
        {
            // throw a warning
            Debug.LogError("There has been an error playing your audio clip. Please double check and make sure the VO Bank and Manager is configured correctly.");
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
            m_externalAudSrc.clip = clip.audClp;
            m_externalAudSrc.Play();
            m_blnAudioTriggered = true;

            // display the subtitle
            uiTextObject.text = clip.subtitle;
        }
        catch
        {
            // throw a warning
            Debug.LogError("There has been an error playing your audio clip. Please double check and make sure the VO Bank and Manager is configured correctly.");
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
            m_externalAudSrc = null;
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

    // check if the audio source is playing
    public bool IsPlaying()
    {
        // if there is an external audio source
        if (m_externalAudSrc)
        {
            // return if the audio source is playing or not
            return m_externalAudSrc.isPlaying;
        }
        else
        {
            // return if the audio source is playing or not
            return m_audSrc.isPlaying;
        }
    }
}