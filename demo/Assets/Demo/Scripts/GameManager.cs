using UnityEngine;
using System.Collections;

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
                if(blnForcePlay)
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

        // if there are no triggers
        if (!blnTriggerDoor && !blnDoorConvoStart)
        {
            // set the text to an empty string
            VOManager.Instance.uiTextObject.text = "";
        }
    }
}