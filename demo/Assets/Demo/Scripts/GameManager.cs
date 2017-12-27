using UnityEngine;

/// <summary>
/// Game Manager
/// Description: Manages everthing in the game environment and triggers the VO Manager.
/// </summary>

public class GameManager : MonoBehaviour
{
    // global variables
    public static GameManager Instance; // this instance  

    // public variables
    [Header("Settings")]
    public bool blnForcePlay;   // check if the current mode is force play mode

    [Header("Door")]
    public AudioSource audSrcDoor;   // the audio source for the door conversation
    public bool blnTriggerDoor; // check if the door conversation is triggerable
    public bool blnDoorConvoStart;  // check if the door conversation has started
    public float fltDelayBetweenAudioDoor = 1.0f;   // the delay between audio files
    public int[] col_intHangingPictureConvo;  // an array to store the order for the hanging picture conversation

    [Header("Laptop")]
    public AudioSource audSrcLaptop;   // the audio source for the laptop interview
    public bool blnTriggerLaptop; // check if the laptop interview is triggerable
    public bool blnLaptopInterviewStart;  // check if the laptop interview has started
    public float fltDelayBetweenAudioLaptop = 1.0f;   // the delay between audio files
    public int[] col_intLaptopInterview;  // an array to store the order for the laptop interview

    [Header("Radio")]
    public AudioSource audSrcRadio;   // the audio source for the radio
    public bool blnTriggerRadio; // check if the radio is triggerable
    public bool blnRadioStart;  // check if the radio has started
    public float fltDelayBetweenAudioRadio = 1.0f;   // the delay between audio files
    public int[] col_intRadio;  // an array to store the order for the radio

    // private variables
    private int m_intCurrentIndex;  // keep track of the current audio's index
    private float m_fltDelayBetweenAudio;   // the delay between audio files

    // when the instance is loaded
    private void Awake()
    {
        // set this instance
        Instance = this;
    }

    // update is called once per frame
    void Update()
    {
        // vo options
        VOOptions();
      
        // if the door trigger is on
        if (blnTriggerDoor)
        {
            // if the door conversation has not started
            if (!blnDoorConvoStart)
            {
                // set the text to inform the user they can trigger the conversation
                VOManager.Instance.uiTextObject.text = "Press [E] to listen to the conversation in the hallway.";
            }

            // if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if the door conversation has not started
                if (!blnDoorConvoStart)
                {
                    // start the convo and set index to 0
                    blnDoorConvoStart = true;
                    m_intCurrentIndex = 0;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = 0;
                }

                // if the current mode is force play mode
                if (blnForcePlay)
                {
                    // force play the first audio clip and set the index to 1
                    blnDoorConvoStart = true;
                    VOManager.Instance.ForcePlay(audSrcDoor, col_intHangingPictureConvo[0]);
                    m_intCurrentIndex = 1;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = fltDelayBetweenAudioDoor;
                }
            }
        }

        // if the door conversation has started 
        if (blnDoorConvoStart)
        {
            // if the audio source isn't playing
            if (!VOManager.Instance.IsPlaying())
            {
                // if the current index is less than the list length
                if (m_intCurrentIndex < col_intHangingPictureConvo.Length)
                {
                    // if the delay between audio files is 0
                    if (m_fltDelayBetweenAudio <= 0)
                    {
                        // play the audio at the audio source and increment the index
                        VOManager.Instance.Play(audSrcDoor, col_intHangingPictureConvo[m_intCurrentIndex]);
                        m_intCurrentIndex++;

                        // set the default delay
                        m_fltDelayBetweenAudio = fltDelayBetweenAudioDoor;
                    }
                    else
                    {
                        // decrease the delay based on time
                        m_fltDelayBetweenAudio -= Time.deltaTime;
                    }
                }
                else
                {
                    // set convo to false
                    blnDoorConvoStart = false;
                }
            }
        }

        // if the laptop trigger is on
        if (blnTriggerLaptop)
        {
            // if the laptop conversation has not started
            if (!blnLaptopInterviewStart)
            {
                // set the text to inform the user they can trigger the interview
                VOManager.Instance.uiTextObject.text = "Press [E] to play the interview.";
            }

            // if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if the laptop interview has not started
                if (!blnLaptopInterviewStart)
                {
                    // start the interview and set index to 0
                    blnLaptopInterviewStart = true;
                    m_intCurrentIndex = 0;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = 0;
                }

                // if the current mode is force play mode
                if (blnForcePlay)
                {
                    // force play the first audio clip and set the index to 1
                    blnLaptopInterviewStart = true;
                    VOManager.Instance.ForcePlay(audSrcLaptop, col_intLaptopInterview[0]);
                    m_intCurrentIndex = 1;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = fltDelayBetweenAudioLaptop;
                }
            }
        }

        // if the laptop interview has started 
        if (blnLaptopInterviewStart)
        {
            // if the audio source isn't playing
            if (!VOManager.Instance.IsPlaying())
            {
                // if the current index is less than the list length
                if (m_intCurrentIndex < col_intLaptopInterview.Length)
                {
                    // if the delay between audio files is 0
                    if (m_fltDelayBetweenAudio <= 0)
                    {
                        // play the audio at the audio source and increment the index
                        VOManager.Instance.Play(audSrcLaptop, col_intLaptopInterview[m_intCurrentIndex]);
                        m_intCurrentIndex++;

                        // set the default delay
                        m_fltDelayBetweenAudio = fltDelayBetweenAudioLaptop;
                    }
                    else
                    {
                        // decrease the delay based on time
                        m_fltDelayBetweenAudio -= Time.deltaTime;
                    }
                }
                else
                {
                    // set interview to false
                    blnLaptopInterviewStart = false;
                }
            }
        }

        // if the radio trigger is on
        if (blnTriggerRadio)
        {
            // if the radio has not started
            if (!blnRadioStart)
            {
                // set the text to inform the user they can trigger the radio
                VOManager.Instance.uiTextObject.text = "Press [E] to turn on the radio.";
            }

            // if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if the radio has not started
                if (!blnRadioStart)
                {
                    // start the radio and set index to 0
                    blnRadioStart = true;
                    m_intCurrentIndex = 0;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = 0;
                }

                // if the current mode is force play mode
                if (blnForcePlay)
                {
                    // force play the first audio clip and set the index to 1
                    blnRadioStart = true;
                    VOManager.Instance.ForcePlay(audSrcRadio, col_intRadio[0]);
                    m_intCurrentIndex = 1;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = fltDelayBetweenAudioRadio;
                }
            }
        }

        // if the radio has started 
        if (blnRadioStart)
        {
            // if the audio source isn't playing
            if (!VOManager.Instance.IsPlaying())
            {
                // if the current index is less than the list length
                if (m_intCurrentIndex < col_intRadio.Length)
                {
                    // if the delay between audio files is 0
                    if (m_fltDelayBetweenAudio <= 0)
                    {
                        // play the audio at the audio source and increment the index
                        VOManager.Instance.Play(audSrcLaptop, col_intRadio[m_intCurrentIndex]);
                        m_intCurrentIndex++;

                        // set the default delay
                        m_fltDelayBetweenAudio = fltDelayBetweenAudioRadio;
                    }
                    else
                    {
                        // decrease the delay based on time
                        m_fltDelayBetweenAudio -= Time.deltaTime;
                    }
                }
                else
                {
                    // set radio to false
                    blnRadioStart = false;
                }
            }
        }

        // if there are no triggers
        if (!blnTriggerDoor && !blnDoorConvoStart && !blnTriggerLaptop && !blnLaptopInterviewStart)
        {
            // set the text to an empty string
            VOManager.Instance.uiTextObject.text = "";
        }
    }

    // vo options
    private void VOOptions()
    {
        // if number 1 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
            // play audio normally
            blnForcePlay = false;

        // if number 2 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha2))
            // play audio in force mode
            blnForcePlay = false;

        // if number 3 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // stop audio players
            VOManager.Instance.Stop();

            // turn of all triggers
            blnDoorConvoStart = false;
            blnLaptopInterviewStart = false;
            blnRadioStart = false;
        }
    }
}