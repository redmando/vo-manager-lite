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
    public Text txtAudioMode;    // display the current audio mode
    public bool blnForcePlay;   // check if the current mode is force play mode
    public bool blnAudioHasStarted; // check if the audio has started playing

    [Header("Hint")]
    public Text txtHint;    // text object that informs the player what to do

    [Header("Door")]
    public AudioSource audSrcDoor;   // the audio source for the door
    public GameObject goDoorBreadcrumb; // breadcrumb for door
    public float fltDelayBetweenDoorAudio = 0.8f;  // the delay between audio files
    public bool blnCanTriggerDoorAudio; // checks if the door audio can be triggered
    public bool blnDoorAudioIsTriggered;    // check if the door audio is triggered

    // an array to store the order of which our audio performs
    private int[] m_col_intDoorAudio = new int[] { 13, 0, 14, 1, 15, 2, 16, 3, 17, 4, 18, 5, 19, 6, 20, 7, 21, 8, 22 };

    [Header("Laptop")]
    public AudioSource audSrcLaptop;   // the audio source for the laptop
    public GameObject goLaptopBreadcrumb; // breadcrumb for laptop
    public float fltDelayBetweenLaptopAudio = 0.6f;  // the delay between audio files
    public bool blnCanTriggerLaptopAudio; // checks if the laptop audio can be triggered
    public bool blnLaptopAudioIsTriggered;    // check if the laptop audio is triggered

    // an array to store the order of which our audio performs
    private int[] m_col_intLaptopAudio = new int[] { 23, 31, 24, 32, 25, 33, 26, 34, 27, 35, 28, 36, 29, 37, 30 };

    [Header("Radio")]
    public AudioSource audSrcRadio;   // the audio source for the radio
    public GameObject goRadioBreadcrumb; // breadcrumb for radio
    public float fltDelayBetweenRadioAudio = 1.0f;  // the delay between audio files
    public bool blnCanTriggerRadioAudio; // checks if the radio audio can be triggered
    public bool blnRadioAudioIsTriggered;    // check if the radio audio is triggered

    // an array to store the order of which our audio performs
    private int[] m_col_intRadioAudio = new int[] { 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52 };

    [Header("Bed and Bookshelf")]
    public GameObject goBedBreadcrumb; // breadcrumb for bed
    public GameObject goBookshelfBreadcrumb; // breadcrumb for bookshelf
    public bool blnCanTriggerBedAudio;  // checks if the bed audio can be triggered
    public bool blnCanTriggerBookshelfAudio; // checks if the bookshelf audio can be triggered
    public bool blnSelfAudioIsTriggered;    // check if the bed or bookshelf audio is triggered

    // an array to store our different audio clips
    private string[] m_col_strBed = new string[] { "Bed 1", "Bed 2", "Bed 3" };
    private string[] m_col_strBookshelf = new string[] { "Bookshelf 1", "Bookshelf 2", "Bookshelf 3" };

    // private variables
    private int m_intCurrentIndex;  // keep track of the current audio's index
    private int m_intCurrentIndexBed;   // keep track of the current audio's index for the bed
    private int m_intCurrentIndexBookshelf;   // keep track of the current audio's index for the bookshelf
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

        // audio triggers
        DoorAudio();
        LaptopAudio();
        RadioAudio();
        BedAndBookshelfAudio();

        // if play mode is on
        if (!blnForcePlay)
            // inform the user is play mode
            txtAudioMode.text = "Play Mode";
        else
            // inform the user its force play mode
            txtAudioMode.text = "Force Play Mode";

        // if audio is not triggerable and hint text object is not empty 
        // and mode is not force play and audio has started
        if ((blnAudioHasStarted && txtHint.text != "" && !blnForcePlay) || (!blnCanTriggerDoorAudio && !blnCanTriggerLaptopAudio && !blnCanTriggerRadioAudio && !blnCanTriggerBedAudio && !blnCanTriggerBookshelfAudio))
            // empty the field
            txtHint.text = "";

        // if the r key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            // reset the scene
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    // turn off all all audio triggers
    private void TurnOffAllTriggers()
    {
        blnAudioHasStarted = false;
        blnDoorAudioIsTriggered = false;
        blnLaptopAudioIsTriggered = false;
        blnRadioAudioIsTriggered = false;
        blnSelfAudioIsTriggered = false;
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

            // turn of all audio triggers
            TurnOffAllTriggers();
        }
    }

    // holds all the functionality related to the door audio
    private void DoorAudio()
    {
        // if the door audio can be triggered
        if (blnCanTriggerDoorAudio)
        {
            // if the audio has not started playing or if it is force play mode
            if (!blnAudioHasStarted || blnForcePlay)
            {
                // set the text for the hint text object
                txtHint.text = "[E] Eavesdrop conversation in hallway.";
            }

            // if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if an audio has not started playing
                if (!blnAudioHasStarted)
                {
                    // disable the breadcrumb if it has not been
                    if (goDoorBreadcrumb.activeInHierarchy)
                        // disable the gameobject
                        goDoorBreadcrumb.SetActive(false);

                    // start the audio and set the index to 0
                    blnAudioHasStarted = true;
                    blnDoorAudioIsTriggered = true;
                    m_intCurrentIndex = 0;

                    // set the delay rate between the audio
                    m_fltDelayBetweenAudio = 0;
                }

                // if the current mode is force play mode
                if (blnForcePlay)
                {
                    // disable breadcrumb if it has not been
                    if (goDoorBreadcrumb.activeInHierarchy)
                        // disable the gameobject
                        goDoorBreadcrumb.SetActive(false);

                    // turn off all other audio triggers
                    TurnOffAllTriggers();

                    // force play the first audio clip and set the index to 1
                    blnAudioHasStarted = true;
                    blnDoorAudioIsTriggered = true;
                    VOManager.Instance.ForcePlay(audSrcDoor, m_col_intDoorAudio[0]);
                    m_intCurrentIndex = 1;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = fltDelayBetweenDoorAudio;
                }
            }
        }

        // if the door audio is triggered
        if (blnDoorAudioIsTriggered)
            // if the audio source isn't playing
            if (!VOManager.Instance.IsPlaying())
            {
                // if the current index is less than the list length
                if (m_intCurrentIndex < m_col_intDoorAudio.Length)
                {
                    // if the delay rate between the audio files is 0
                    if (m_fltDelayBetweenAudio <= 0)
                    {
                        // play the audio at the audio source and increment the index
                        VOManager.Instance.Play(audSrcDoor, m_col_intDoorAudio[m_intCurrentIndex]);
                        m_intCurrentIndex++;

                        // reset the delay rate
                        m_fltDelayBetweenAudio = fltDelayBetweenDoorAudio;
                    }
                    else
                        // decrease the delay rate based on delta time
                        m_fltDelayBetweenAudio -= Time.deltaTime;
                }
                else
                {
                    // turn off the audio triggers
                    TurnOffAllTriggers();
                    m_intCurrentIndex = 0;
                }
            }
    }

    // holds all the functionality related to the laptop audio
    private void LaptopAudio()
    {
        // if the laptop audio can be triggered
        if (blnCanTriggerLaptopAudio)
        {
            // if the audio has not started playing or if it is force play mode
            if (!blnAudioHasStarted || blnForcePlay)
            {
                // set the text for the hint text object
                txtHint.text = "[E] Listen to the online interview.";
            }

            // if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if an audio has not started playing
                if (!blnAudioHasStarted)
                {
                    // disable the breadcrumb if it has not been
                    if (goLaptopBreadcrumb.activeInHierarchy)
                        // disable the gameobject
                        goLaptopBreadcrumb.SetActive(false);

                    // start the audio and set the index to 0
                    blnAudioHasStarted = true;
                    blnLaptopAudioIsTriggered = true;
                    m_intCurrentIndex = 0;

                    // set the delay rate between the audio
                    m_fltDelayBetweenAudio = 0;
                }

                // if the current mode is force play mode
                if (blnForcePlay)
                {
                    // disable breadcrumb if it has not been
                    if (goLaptopBreadcrumb.activeInHierarchy)
                        // disable the gameobject
                        goLaptopBreadcrumb.SetActive(false);

                    // turn off all other audio triggers
                    TurnOffAllTriggers();

                    // force play the first audio clip and set the index to 1
                    blnAudioHasStarted = true;
                    blnLaptopAudioIsTriggered = true;
                    VOManager.Instance.ForcePlay(audSrcLaptop, m_col_intLaptopAudio[0]);
                    m_intCurrentIndex = 1;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = fltDelayBetweenLaptopAudio;
                }
            }
        }

        // if the laptop audio is triggered
        if (blnLaptopAudioIsTriggered)
            // if the audio source isn't playing
            if (!VOManager.Instance.IsPlaying())
            {
                // if the current index is less than the list length
                if (m_intCurrentIndex < m_col_intLaptopAudio.Length)
                {
                    // if the delay rate between the audio files is 0
                    if (m_fltDelayBetweenAudio <= 0)
                    {
                        // play the audio at the audio source and increment the index
                        VOManager.Instance.Play(audSrcLaptop, m_col_intLaptopAudio[m_intCurrentIndex]);
                        m_intCurrentIndex++;

                        // reset the delay rate
                        m_fltDelayBetweenAudio = fltDelayBetweenLaptopAudio;
                    }
                    else
                        // decrease the delay rate based on delta time
                        m_fltDelayBetweenAudio -= Time.deltaTime;
                }
                else
                {
                    // turn off the audio triggers
                    TurnOffAllTriggers();
                    m_intCurrentIndex = 0;
                }
            }
    }

    // holds all the functionality related to the radio audio
    private void RadioAudio()
    {
        // if the radio audio can be triggered
        if (blnCanTriggerRadioAudio)
        {
            // if the audio has not started playing or if it is force play mode
            if (!blnAudioHasStarted || blnForcePlay)
            {
                // set the text for the hint text object
                txtHint.text = "[E] Listen to the online interview.";
            }

            // if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if an audio has not started playing
                if (!blnAudioHasStarted)
                {
                    // disable the breadcrumb if it has not been
                    if (goRadioBreadcrumb.activeInHierarchy)
                        // disable the gameobject
                        goRadioBreadcrumb.SetActive(false);

                    // start the audio and set the index to 0
                    blnAudioHasStarted = true;
                    blnRadioAudioIsTriggered = true;
                    m_intCurrentIndex = 0;

                    // set the delay rate between the audio
                    m_fltDelayBetweenAudio = 0;
                }

                // if the current mode is force play mode
                if (blnForcePlay)
                {
                    // disable breadcrumb if it has not been
                    if (goRadioBreadcrumb.activeInHierarchy)
                        // disable the gameobject
                        goRadioBreadcrumb.SetActive(false);

                    // turn off all other audio triggers
                    TurnOffAllTriggers();

                    // force play the first audio clip and set the index to 1
                    blnAudioHasStarted = true;
                    blnRadioAudioIsTriggered = true;
                    VOManager.Instance.ForcePlay(audSrcRadio, m_col_intRadioAudio[0]);
                    m_intCurrentIndex = 1;

                    // set the delay between audio
                    m_fltDelayBetweenAudio = fltDelayBetweenRadioAudio;
                }
            }
        }

        // if the radio audio is triggered
        if (blnRadioAudioIsTriggered)
            // if the audio source isn't playing
            if (!VOManager.Instance.IsPlaying())
            {
                // if the current index is less than the list length
                if (m_intCurrentIndex < m_col_intRadioAudio.Length)
                {
                    // if the delay rate between the audio files is 0
                    if (m_fltDelayBetweenAudio <= 0)
                    {
                        // play the audio at the audio source and increment the index
                        VOManager.Instance.Play(audSrcRadio, m_col_intRadioAudio[m_intCurrentIndex]);
                        m_intCurrentIndex++;

                        // reset the delay rate
                        m_fltDelayBetweenAudio = fltDelayBetweenRadioAudio;
                    }
                    else
                        // decrease the delay rate based on delta time
                        m_fltDelayBetweenAudio -= Time.deltaTime;
                }
                else
                {
                    // turn off the audio triggers
                    TurnOffAllTriggers();
                    m_intCurrentIndex = 0;
                }
            }
    }

    // holds all the functionality related to the bed and bookshelf audio
    private void BedAndBookshelfAudio()
    {
        // if the bed or bookshelf audio can be triggered
        if (blnCanTriggerBedAudio || blnCanTriggerBookshelfAudio)
        {
            // if the audio has not started playing or if it is force play mode
            if (!blnAudioHasStarted || blnForcePlay)
                // set the text for the hint text object
                txtHint.text = "[E] Examine.";

            // if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if an audio has not started playing
                if (!blnAudioHasStarted)
                {
                    // start the audio
                    blnAudioHasStarted = true;
                    blnSelfAudioIsTriggered = true;

                    // if the audio is for the bed
                    if (blnCanTriggerBedAudio)
                    {
                        // disable breadcrumb if it has not been
                        if (goBedBreadcrumb.activeInHierarchy)
                            // disable the gameobject
                            goBedBreadcrumb.SetActive(false);

                        // play the audio clip
                        VOManager.Instance.Play(m_col_strBed[m_intCurrentIndexBed]);

                        // increase the index
                        if (m_intCurrentIndexBed == 2)
                            m_intCurrentIndexBed = 0;
                        else
                            m_intCurrentIndexBed++;
                    }

                    // if the audio is for the bookshelf
                    else if (blnCanTriggerBookshelfAudio)
                    {
                        // disable breadcrumb if it has not been
                        if (goBookshelfBreadcrumb.activeInHierarchy)
                            // disable the gameobject
                            goBookshelfBreadcrumb.SetActive(false);

                        // play the audio clip
                        VOManager.Instance.Play(m_col_strBookshelf[m_intCurrentIndexBookshelf]);

                        // increase the index
                        if (m_intCurrentIndexBookshelf == 2)
                            m_intCurrentIndexBookshelf = 0;
                        else
                            m_intCurrentIndexBookshelf++;
                    }
                }

                // if the current mode is force play mode
                if (blnForcePlay)
                {
                    // turn off all audio triggers
                    TurnOffAllTriggers();

                    // start the audio
                    blnAudioHasStarted = true;
                    blnSelfAudioIsTriggered = true;

                    // if the audio is for the bed
                    if (blnCanTriggerBedAudio)
                    {
                        // disable breadcrumb if it has not been
                        if (goBedBreadcrumb.activeInHierarchy)
                            // disable the gameobject
                            goBedBreadcrumb.SetActive(false);

                        // play the audio clip
                        VOManager.Instance.ForcePlay(m_col_strBed[m_intCurrentIndexBed]);

                        // increase the index
                        if (m_intCurrentIndexBed == 2)
                            m_intCurrentIndexBed = 0;
                        else
                            m_intCurrentIndexBed++;
                    }

                    // if the audio is for the bookshelf
                    else if (blnCanTriggerBookshelfAudio)
                    {
                        // disable breadcrumb if it has not been
                        if (goBookshelfBreadcrumb.activeInHierarchy)
                            // disable the gameobject
                            goBookshelfBreadcrumb.SetActive(false);

                        // play the audio clip
                        VOManager.Instance.ForcePlay(m_col_strBookshelf[m_intCurrentIndexBookshelf]);

                        // increase the index
                        if (m_intCurrentIndexBookshelf == 2)
                            m_intCurrentIndexBookshelf = 0;
                        else
                            m_intCurrentIndexBookshelf++;
                    }
                }
            }
        }

        // if the self audio is triggered
        if (blnSelfAudioIsTriggered)
        {
            // check if the audio is still playing
            if (!VOManager.Instance.IsPlaying())
                // turn off the audio triggers
                TurnOffAllTriggers();
        }
    }
}