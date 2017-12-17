using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // global variables
    public static GameManager Instance; // this instance
    public float fltDelayBetweenAudio = 1.0f;   // the delay between audio files

    // public variables
    [Header("Door")]
    public bool blnTriggerDoor; // check if the door conversation is triggerable
    public bool blnDoorConvoStart;  // check if the door conversation has started
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
	void Update ()
    {
	    // if the door trigger is on
        if(blnTriggerDoor)
        {
            // if the door conversation has not started
            if(!blnDoorConvoStart)
            {
                // set the text to inform the user they can trigger the conversation
                VOManager.Instance.uiTextObject.text = "Press [E] to listen to the conversation in the hallway.";
            }

            // if the door conversation has started 
            if(blnDoorConvoStart)
            {
                // if the audio source isn't playing
                if(!VOManager.Instance.IsPlaying())
                {
                    // if the current index is less than the list length
                    if (m_intCurrentIndex < col_intHangingPictureConvo.Length)
                    {
                        // if the delay between audio files is 0
                        if (m_fltDelayBetweenAudio <= 0)
                        {
                            // play the first audio and increment the index
                            VOManager.Instance.Play(col_intHangingPictureConvo[m_intCurrentIndex]);
                            m_intCurrentIndex++;

                            // set the default delay
                            m_fltDelayBetweenAudio = fltDelayBetweenAudio;
                        } else
                        {
                            // decrease the delay based on time
                            m_fltDelayBetweenAudio -= Time.deltaTime;
                        }
                    }
                }
            }

            // if the E key is pressed
            if(Input.GetKeyDown(KeyCode.E))
            {
                // start the convo and set index to 0
                blnDoorConvoStart = true;
                m_intCurrentIndex = 0;

                // set the delay between audio
                m_fltDelayBetweenAudio = 0;
            }
        }
	}
}