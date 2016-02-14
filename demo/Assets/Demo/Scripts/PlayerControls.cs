using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Player Controls - Description:
/// Handles all player movement and input.
/// </summary>

public class PlayerControls : MonoBehaviour
{
    // public variables
    public float fltSpeed = 6.5f;                   // the player's walking speed
    public float fltGravity = 20.0f;                // the gravity force
    public float fltMouseSensitivity = 100.0f;      // the mouse's sensitivity
    public Text txtMode;                            // the ui text to display the current mode

    // private variables
    private Vector3 m_vecMoveDirection;             // the player's current direction
    private float m_fltClampAngle = 80.0f;          // angle of which to clamp camera
    private float m_fltRotationX = 0.0f;            // x axis of rotation
    private float m_fltRotationY = 0.0f;            // y axis of rotation
    private CharacterController m_charController;   // the character controller of this object
    private bool m_blnNormalMode;                   // boolean to check if audio play mode is normal

    // on loaded
    void Awake()
    {
        Vector3 vecRotation = transform.localRotation.eulerAngles;  // get the current rotation
        m_fltRotationX = vecRotation.x;     // set the x axis rotation
        m_fltRotationY = vecRotation.y;     // set the y axis rotation
        m_charController = this.GetComponent<CharacterController>();    // get the character controller component of this object
        m_blnNormalMode = true;     // set audio play mode to normal
    }

    // on initalization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // lock the cursor to the center of the screen
        Cursor.visible = false; // hide the cursor
    }

    // once per frame
    void Update()
    {
        // if [1] is pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // normal mode is true
            m_blnNormalMode = true;
            txtMode.text = "Normal";
        }
        // if [2] is pressed
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // normal mode is false
            m_blnNormalMode = false;
            txtMode.text = "Interrupt";
        }
        // if [R] is pressed
        if(Input.GetKeyDown(KeyCode.R))
        {
            // stop the current audio from playing
            VOManager.Instance.Stop();
        }

        // if the escape button is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // unlock cursor and show
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // raycast detection
        RaycastDetection();

        // if the contoller is grounded
        if (m_charController.isGrounded)
        {
            // set the move direction
            m_vecMoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            m_vecMoveDirection = transform.TransformDirection(m_vecMoveDirection);
            m_vecMoveDirection *= fltSpeed;
        }

        // move the player and apply gravity
        m_vecMoveDirection.y -= fltGravity * Time.deltaTime;
        m_charController.Move(m_vecMoveDirection * Time.deltaTime);

        // get the axis values for mouse axis
        float fltMouseX = Input.GetAxis("Mouse X");
        float fltMouseY = -Input.GetAxis("Mouse Y");

        // calculate the rotation axis
        m_fltRotationX += fltMouseY * fltMouseSensitivity * Time.deltaTime;
        m_fltRotationY += fltMouseX * fltMouseSensitivity * Time.deltaTime;

        // clamp the x position to prevent player from looking up to high or low
        m_fltRotationX = Mathf.Clamp(m_fltRotationX, -m_fltClampAngle, m_fltClampAngle);

        // allows the player to look left and right (full 360 turn)
        Quaternion quatLocalRotation = Quaternion.Euler(m_fltRotationX, m_fltRotationY, 0.0f);
        this.transform.rotation = quatLocalRotation;
    }

    // raycast detection
    void RaycastDetection()
    {
        // cast a raycast from the player's position
        Vector3 vecForward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        // if the raycast hits something in front of the player (distance of 5)
        if (Physics.Raycast(transform.position, vecForward, out hit, 5))
        {
            // draw the debug line so we can see it in the scene editor
            Debug.DrawLine(this.transform.position, hit.point, Color.red);

            // if the [E] key is pressed
            if(Input.GetKeyDown(KeyCode.E))
                // switch statement for the different tags
                switch(hit.transform.tag)
                {
                    // play the right sound clip according to the tags
                    case "Books":
                        int intRanBooks = Random.Range(0, 3);   // calls a random sound click between 0 - 3
                        PlaySound(intRanBooks);
                        break;
                    case "TV":
                        int intRandTV = Random.Range(8, 10);     // calls a random sound click between 8 - 10
                        PlaySound(intRandTV);
                        break;
                    case "Desk":
                        PlaySound(4);
                        break;
                    case "Bed":
                        PlaySound(6);
                        break;
                    case "Lamp":
                        PlaySound(5);
                        break;
                    case "Door":
                        PlaySound(4);
                        break;
                    case "Drawer":
                        PlaySound(7);
                        break;
                    case "Coatstand":
                        PlaySound(7);
                        break;
                    case "Computer":
                        PlaySound(3);
                        break;
                }
        }
    }

    // play sound function to handle calling VOManager functions
    void PlaySound(int id)
    {
        // if normal mode is on
        if (m_blnNormalMode)
            // call normal play
            VOManager.Instance.Play(id);
        // else if normal mode is off
        else
            // call interrupt play
            VOManager.Instance.PlayInterrupt(id);
    }
}