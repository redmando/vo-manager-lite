using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public bool blnConvoStart;  // check if a conversation is happening
    public bool blnInstructionsTriggered;   // check if the on screen instructions are triggered
    public bool blnSelfAudioTriggered;  // check if first person audio is triggered
    public Text txtAudioMode;    // display the current audio mode
    public Text txtSecondary;   // secondary text incase main vo is taken up
    
    [Header("Door")]
    public AudioSource audSrcDoor;   // the audio source for the door conversation
    public bool blnTriggerDoor; // check if the door audio is triggerable
    public bool blnDoorAudioTriggered;  // check if the door conversation has been triggered
    public float fltDelayBetweenAudioDoor = 1.0f;   // the delay between audio files
    public int[] col_intHangingPictureConvo;  // an array to store the order for the hanging picture conversation
    public GameObject goDoorBreadcrumb; // breadcrumb for door

    [Header("Laptop")]
    public AudioSource audSrcLaptop;   // the audio source for the laptop interview
    public bool blnTriggerLaptop; // check if the laptop interview is triggerable
    public bool blnLaptopAudioTriggered;    // check if the laptop audio is triggered
    public float fltDelayBetweenAudioLaptop = 1.0f;   // the delay between audio files
    public int[] col_intLaptopInterview;  // an array to store the order for the laptop interview.
    public GameObject goLaptopBreadcrumb; // breadcrumb for laptop

    [Header("Radio")]
    public AudioSource audSrcRadio;   // the audio source for the radio
    public bool blnTriggerRadio; // check if the radio is triggerable
    public bool blnRadioAudioTriggered;    // check if the radio audio is triggered
    public float fltDelayBetweenAudioRadio = 1.0f;   // the delay between audio files
    public int[] col_intRadio;  // an array to store the order for the radio
    public GameObject goRadioBreadcrumb; // breadcrumb for radio

    [Header("Bookshelf")]
    public bool blnTriggerBookshelf; // check if the bookshelf is triggerable
    public string[] col_strBookshelf;  // an array to store our different bookshelf audio clips
    public GameObject goBookshelfBreadcrumb; // breadcrumb for bookshelf

    [Header("Bed")]
    public bool blnTriggerBed; // check if the bed is triggerable
    public string[] col_strBed;  // an array to store our different bed audio clips
    public GameObject goBedBreadcrumb; // breadcrumb for bed

    // private variables
    private int m_intCurrentIndex;  // keep track of the current audio's index
    private float m_fltDelayBetweenAudio;   // the delay between audio files
    private int m_intBedIndex;  // keep track of which audio is playing
    private int m_intBookshelfIndex;  // keep track of which audio is playing

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

        // door conversation
        DoorConversation();

        // laptop interview
        LaptopInterview();

        // radio broadcast
        RadioBroadcast();

        // bed and bookshelf
        BedAndBookshelf();

        // if there are no triggers
        if (!blnConvoStart && !blnInstructionsTriggered)
        {
            // set the text to an empty string
            VOManager.Instance.uiTextObject.text = "";
        }

        // if play mode is on
        if(!blnForcePlay)
        {
            txtAudioMode.text = "Play Mode";
        } else
        {
            txtAudioMode.text = "Force Play Mode";

            // if there is no trigger
            if(!blnInstructionsTriggered)
            {
                // set the text to an empty string
                txtSecondary.text = "";
            }
        }

        // if there is no conversation happening and secondary text is not empty
        if(!blnConvoStart && txtSecondary.text != "")
        {
            // empty the field
            txtSecondary.text = "";
        }

        // if the r key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    // turn off all other triggers and audio
    private void TurnOffAllTriggers()
    {
        blnConvoStart = false;
        blnDoorAudioTriggered = false;
        blnLaptopAudioTriggered = false;
        blnRadioAudioTriggered = false;
    }

    // door conversation
    private void DoorConversation()
    {
        // if the door trigger is on
        if (blnTriggerDoor)
        {
            // if a converstation has not started
            if (!blnConvoStart)
            {
                // set the text to inform the user they can trigger the conversation
                VOManager.Instance.uiTextObject.text = "Press [E] to listen to the conversation in the hallway.";
            } else
            {
                // is force play mode is on
                if (blnForcePlay)
                {
                    // set the text to inform the user they can trigger the conversation
                    txtSecondary.text = "Press [E] to listen to the conversation in the hallway.";
                }
            }

            // if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // disable breadcrumb if it has not been
                if (goDoorBreadcrumb.activeInHierarchy)
                    // disable it
                    goDoorBreadcrumb.SetActive(false);

                // if a converstation has not started
                if (!blnConvoStart)
                {
                    // start the convo and set index to 0
                    blnConvoStart = true;
                    blnDoorAudioTriggered = true;
                    m_intCurrentIndex = 0;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = 0;
                }

                // if the current mode is force play mode
                if (blnForcePlay)
                {
                    // turn off all other triggers and audio
                    TurnOffAllTriggers();

                    // force play the first audio clip and set the index to 1
                    blnConvoStart = true;
                    blnDoorAudioTriggered = true;
                    VOManager.Instance.ForcePlay(audSrcDoor, col_intHangingPictureConvo[0]);
                    m_intCurrentIndex = 1;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = fltDelayBetweenAudioDoor;
                }
            }
        }

        // if the door audio is triggered
        if (blnDoorAudioTriggered)
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
                    blnConvoStart = false;
                    blnDoorAudioTriggered = false;
                }
            }
        }
    }

    // laptop interview
    private void LaptopInterview()
    {
        // if the laptop trigger is on
        if (blnTriggerLaptop)
        {
            // if a converstation has not started
            if (!blnConvoStart)
            {
                // set the text to inform the user they can trigger the interview
                VOManager.Instance.uiTextObject.text = "Press [E] to play the interview.";
            }
            else
            {
                // is force play mode is on
                if (blnForcePlay)
                {
                    // set the text to inform the user they can trigger the interview
                    txtSecondary.text = "Press [E] to play the interview.";
                }
            }

            // if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // disable breadcrumb if it has not been
                if (goLaptopBreadcrumb.activeInHierarchy)
                    // disable it
                    goLaptopBreadcrumb.SetActive(false);

                // if a converstation has not started
                if (!blnConvoStart)
                {
                    // start the interview and set index to 0
                    blnConvoStart = true;
                    blnLaptopAudioTriggered = true;
                    m_intCurrentIndex = 0;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = 0;
                }

                // if the current mode is force play mode
                if (blnForcePlay)
                {
                    // turn off all other triggers and audio
                    TurnOffAllTriggers();

                    // force play the first audio clip and set the index to 1
                    blnConvoStart = true;
                    blnLaptopAudioTriggered = true;
                    VOManager.Instance.ForcePlay(audSrcLaptop, col_intLaptopInterview[0]);
                    m_intCurrentIndex = 1;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = fltDelayBetweenAudioLaptop;
                }
            }
        }

        // if the laptop audio is triggered
        if (blnLaptopAudioTriggered)
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
                    blnConvoStart = false;
                    blnDoorAudioTriggered = false;
                }
            }
        }
    }

    // radio broadcast
    private void RadioBroadcast()
    {
        // if the radio trigger is on
        if (blnTriggerRadio)
        {
            // if a converstation has not started
            if (!blnConvoStart)
            {
                // set the text to inform the user they can trigger the radio
                VOManager.Instance.uiTextObject.text = "Press [E] to turn on the radio.";
            }
            else
            {
                // is force play mode is on
                if (blnForcePlay)
                {
                    // set the text to inform the user they can trigger the radio
                    txtSecondary.text = "Press [E] to turn on the radio.";
                }
            }

            // if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // disable breadcrumb if it has not been
                if (goRadioBreadcrumb.activeInHierarchy)
                    // disable it
                    goRadioBreadcrumb.SetActive(false);

                // if a converstation has not started
                if (!blnConvoStart)
                {
                    // start the radio and set index to 0
                    blnConvoStart = true;
                    blnRadioAudioTriggered = true;
                    m_intCurrentIndex = 0;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = 0;
                }

                // if the current mode is force play mode
                if (blnForcePlay)
                {
                    // turn off all other triggers and audio
                    TurnOffAllTriggers();

                    // force play the first audio clip and set the index to 1
                    blnConvoStart = true;
                    blnRadioAudioTriggered = true;
                    VOManager.Instance.ForcePlay(audSrcRadio, col_intRadio[0]);
                    m_intCurrentIndex = 1;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = fltDelayBetweenAudioRadio;
                }
            }
        }

        // if the radio audio is triggered
        if (blnRadioAudioTriggered)
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
                        VOManager.Instance.Play(audSrcRadio, col_intRadio[m_intCurrentIndex]);
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
                    blnConvoStart = false;
                    blnRadioAudioTriggered = false;
                }
            }
        }
    }

    // bed and bookshelf
    private void BedAndBookshelf()
    {
        // if the bookshelf or bed trigger is on
        if (blnTriggerBookshelf || blnTriggerBed)
        {
            // if a converstation has not started
            if (!blnConvoStart)
            {
                // set the text to inform the user they can trigger the action
                VOManager.Instance.uiTextObject.text = "Press [E] to examine.";
            }
            else
            {
                // is force play mode is on
                if (blnForcePlay)
                {
                    // set the text to inform the user they can trigger the action
                    txtSecondary.text = "Press [E] to examine.";
                }
            }

            // if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if a converstation has not started
                if (!blnConvoStart)
                {
                    // start the convo and set index to 0
                    blnConvoStart = true;
                    blnSelfAudioTriggered = true;

                    // if the trigger is the bed
                    if(blnTriggerBed)
                    {
                        // disable breadcrumb if it has not been
                        if (goBedBreadcrumb.activeInHierarchy)
                            // disable it
                            goBedBreadcrumb.SetActive(false);

                        // play a random clip from the list
                        VOManager.Instance.Play(col_strBed[m_intBedIndex]);

                        // increase bed index
                        if(m_intBedIndex == 2)
                        {
                            m_intBedIndex = 0;
                        } else
                        {
                            m_intBedIndex++;
                        }
                    }

                    // else if the trigger is the bookshelf
                    else if (blnTriggerBookshelf)
                    {
                        // disable breadcrumb if it has not been
                        if (goBookshelfBreadcrumb.activeInHierarchy)
                            // disable it
                            goBookshelfBreadcrumb.SetActive(false);

                        // play a random clip from the list
                        VOManager.Instance.Play(col_strBookshelf[m_intBookshelfIndex]);

                        // increase bookshelf index
                        if (m_intBookshelfIndex == 2)
                        {
                            m_intBookshelfIndex = 0;
                        }
                        else
                        {
                            m_intBookshelfIndex++;
                        }
                    }
                }

                // if the current mode is force play mode
                if (blnForcePlay)
                {
                    // turn off all other triggers and audio
                    TurnOffAllTriggers();

                    // force play the first audio clip and set the index to 1
                    blnConvoStart = true;
                    blnSelfAudioTriggered = true;

                    // if the trigger is the bed
                    if (blnTriggerBed)
                    {
                        // disable breadcrumb if it has not been
                        if (goBedBreadcrumb.activeInHierarchy)
                            // disable it
                            goBedBreadcrumb.SetActive(false);

                        // play a random clip from the list
                        VOManager.Instance.ForcePlay(col_strBed[m_intBedIndex]);

                        // increase bed index
                        if (m_intBedIndex == 2)
                        {
                            m_intBedIndex = 0;
                        }
                        else
                        {
                            m_intBedIndex++;
                        }
                    }

                    // else if the trigger is the bookshelf
                    else if (blnTriggerBookshelf)
                    {
                        // disable breadcrumb if it has not been
                        if (goBookshelfBreadcrumb.activeInHierarchy)
                            // disable it
                            goBookshelfBreadcrumb.SetActive(false);

                        // play a random clip from the list
                        VOManager.Instance.ForcePlay(col_strBookshelf[m_intBookshelfIndex]);

                        // increase bookshelf index
                        if (m_intBookshelfIndex == 2)
                        {
                            m_intBookshelfIndex = 0;
                        }
                        else
                        {
                            m_intBookshelfIndex++;
                        }
                    }
                }
            }
        }

        // if self conversation is triggered
        if(blnSelfAudioTriggered)
        {
            // check if the audio is still playing
            if(!VOManager.Instance.IsPlaying())
            {
                // turn off the trigger and convo
                blnConvoStart = false;
                blnSelfAudioTriggered = false;
            }
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
            blnForcePlay = true;

        // if number 3 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // stop audio players
            VOManager.Instance.Stop();

            // turn of all triggers
            blnConvoStart = false;
            blnDoorAudioTriggered = false;
            blnLaptopAudioTriggered = false;
            blnRadioAudioTriggered = false;
        }
    }
}