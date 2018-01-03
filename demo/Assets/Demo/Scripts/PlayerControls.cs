using UnityEngine;

/// <summary>
/// Player Controls
/// Description: Handles all player movement and input.
/// </summary>

public class PlayerControls : MonoBehaviour
{
    // public variables
    public float fltSpeed = 6.5f;                   // the player's walking speed
    public float fltGravity = 20.0f;                // the gravity force
    public float fltMouseSensitivity = 100.0f;      // the mouse's sensitivity

    // private variables
    private Vector3 m_vecMoveDirection;             // the player's current direction
    private float m_fltClampAngle = 25.0f;          // angle of which to clamp camera
    private float m_fltRotationX = 0.0f;            // x axis of rotation
    private float m_fltRotationY = 0.0f;            // y axis of rotation
    private CharacterController m_charController;   // the character controller of this object

    // on loaded
    void Awake()
    {
        Vector3 vecRotation = transform.localRotation.eulerAngles;  // get the current rotation
        m_fltRotationX = vecRotation.x;     // set the x axis rotation
        m_fltRotationY = vecRotation.y;     // set the y axis rotation
        m_charController = this.GetComponent<CharacterController>();    // get the character controller component of this object
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
        // if the escape button is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // unlock cursor and show
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

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

        // raycast the object in front of the player 
        // and toggle the appropriate manager functions
        RaycastTarget();
    }

    // raycast the object in front of the player
    private void RaycastTarget()
    {
        // create a raycast
        RaycastHit hit;

        // detect if our raycast is hitting an object
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 8))
        {
            // if the object is tagged door
            if(hit.transform.gameObject.tag == "Door")
            {
                // set the game manager door trigger to true
                GameManager.Instance.blnTriggerDoor = true;
                GameManager.Instance.blnInstructionsTriggered = true;
            } else if (hit.transform.gameObject.tag != "Door")
            {
                // else set it to false
                GameManager.Instance.blnTriggerDoor = false;
            }

            // if the object is tagged laptop
            if (hit.transform.gameObject.tag == "Laptop")
            {
                // set the game manager laptop trigger to true
                GameManager.Instance.blnTriggerLaptop = true;
                GameManager.Instance.blnInstructionsTriggered = true;
            }
            else if (hit.transform.gameObject.tag != "Laptop")
            {
                // else set it to false
                GameManager.Instance.blnTriggerLaptop = false;
            }

            // if the object is tagged radio
            if (hit.transform.gameObject.tag == "Radio")
            {
                // set the game manager radio trigger to true
                GameManager.Instance.blnTriggerRadio = true;
                GameManager.Instance.blnInstructionsTriggered = true;
            }
            else if (hit.transform.gameObject.tag != "Radio")
            {
                // else set it to false
                GameManager.Instance.blnTriggerRadio = false;
            }

            // if the object is tagged bed
            if (hit.transform.gameObject.tag == "Bed")
            {
                // set the game manager bed trigger to true
                GameManager.Instance.blnTriggerBed = true;
                GameManager.Instance.blnInstructionsTriggered = true;
            }
            else if (hit.transform.gameObject.tag != "Bed")
            {
                // else set it to false
                GameManager.Instance.blnTriggerBed = false;
            }

            // if the object is tagged bookshelf
            if (hit.transform.gameObject.tag == "Bookshelf")
            {
                // set the game manager bookshelf trigger to true
                GameManager.Instance.blnTriggerBookshelf = true;
                GameManager.Instance.blnInstructionsTriggered = true;
            }
            else if (hit.transform.gameObject.tag != "Bookshelf")
            {
                // else set it to false
                GameManager.Instance.blnTriggerBookshelf = false;
            }
        } else
        {
            // set trigger to false
            GameManager.Instance.blnInstructionsTriggered = false;
            GameManager.Instance.blnTriggerDoor = false;
            GameManager.Instance.blnTriggerLaptop = false;
            GameManager.Instance.blnTriggerRadio = false;
        }
    }
}