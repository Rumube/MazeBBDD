using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    //Refs
    Transform player;
    CharacterController controller;
    float xRotation;

    [Header("Movement")]
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public float speed, sensitivity, gravity, jumpHeight;
    Vector3 verticalMovement;

    [Header("Messages - Prefab & UI")]
    public GameObject signPrefab;
    public GameObject readingWindow, writingWindow, ePrompt;
    public Button leftButton, rightButton;
    Text readingText, writingText;
    int currentMessage;

    [Header("Messages - Script variables")]
    public bool canWrite;
    public bool isReading, isWriting;

    void Start()
    {
        #region References

        player = GameObject.FindGameObjectWithTag("Player").transform;
        controller = transform.GetComponentInParent<CharacterController>();

        readingText = readingWindow.GetComponentInChildren<Text>();
        writingText = writingWindow.transform.Find("InputField/Text").gameObject.GetComponent<Text>();

        #endregion References

        leftButton.onClick.AddListener(PreviousMessage);
        rightButton.onClick.AddListener(NextMessage);

        player.transform.position = GameObject.FindGameObjectWithTag("Start").transform.position;
        player.transform.position += new Vector3(0, 0.5f, 0);
    }

    private void Update()
    {
        if (!isReading && !isWriting)
        {
            Movement();
            Rotation();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Messages();
    }

    #region Character Controller
    void Movement()
    {
        #region Horizontal

        float right = Input.GetAxis("Horizontal");
        float forward = Input.GetAxis("Vertical");

        Vector3 horizontalMovement = player.right * right + transform.forward * forward;
        controller.Move(speed * Time.deltaTime * horizontalMovement);

        #endregion Horizontal

        #region Vertical

        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, 1 << 8);

        if (isGrounded && verticalMovement.y < 0) verticalMovement.y = -2f;
        else verticalMovement.y += gravity * Time.deltaTime;

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalMovement.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        controller.Move(verticalMovement * Time.deltaTime);

        #endregion Vertical
    }

    void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //Y rotation
        player.Rotate(Vector3.up * mouseX); //X rotation
    }

    #endregion Character Controller

    void Messages()
    {
        int layerMask = 1 << 6 | 1 << 7 | 1 << 8;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Floor"))
            {
                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Wall")) // If the player sees a message, allow reading
                {
                    if (!isReading) ePrompt.SetActive(true);
                    Read(hit);
                }
                else // If the player sees a wall, prevent reading and allow message creation
                {
                    ePrompt.SetActive(false);
                    Write();
                }
            }
        }
    }

    #region Create Messages

    void Write()
    {
        #region Input Field

        if (Input.GetKeyDown(KeyCode.E) && canWrite)
        {
            WritingUI(true);
            ShowCursor();
            canWrite = false;
        }

        #endregion Input Field

        #region Instantiate Sign

        if (Input.GetKeyDown(KeyCode.Return) && isWriting)
        {
            GameObject mySign = Instantiate(signPrefab);
            Orientation(mySign);
            mySign.GetComponentInChildren<Message>().message = writingText.text;
            WritingUI(false);
            canWrite = true;
        }

        #endregion Instantiate Sign

        #region Cancel Sign

        if (Input.GetKeyDown(KeyCode.Escape) && isWriting)
        {
            WritingUI(false);
            canWrite = true;
        }

        #endregion Cancel Sign
    }

    void Orientation(GameObject sign)
    {
        int layerMask = 1 << 7;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            sign.transform.localRotation = Quaternion.LookRotation(hit.normal);
            sign.transform.position = hit.point;
        }
    }

    void WritingUI(bool on)
    {
        isWriting = on;
        writingWindow.SetActive(on);
        writingWindow.GetComponentInChildren<InputField>().ActivateInputField();
    }

    #endregion Create Messages

    #region Read Messages

    void Read(RaycastHit hit)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isReading = !isReading;
            currentMessage = 0;
            ePrompt.SetActive(false);
            readingWindow.SetActive(isReading);
        }

        if (isReading) DisplayMessages(hit.transform.gameObject);
    }
    void DisplayMessages(GameObject sign)
    {
        Message msg = sign.GetComponent<Message>();
        readingText.text = msg.messages[currentMessage];

        if (msg.messages.Count > 1)
        {
            if (currentMessage == msg.messages.Count - 1)
            {
                leftButton.interactable = true;
                rightButton.interactable = false;
            }
            else if (currentMessage == 0)
            {
                leftButton.interactable = false;
                rightButton.interactable = true;
            }
            else
            {
                leftButton.interactable = true;
                rightButton.interactable = true;
            }

            ShowCursor();
            ArrowsUI(true);
        }
        else ArrowsUI(false);
    }

    void ArrowsUI(bool on)
    {
        leftButton.gameObject.SetActive(on);
        rightButton.gameObject.SetActive(on);
    }

    void PreviousMessage()
    {
        currentMessage--;
    }

    void NextMessage()
    {
        currentMessage++;
    }

    #endregion Read Messages

    void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}