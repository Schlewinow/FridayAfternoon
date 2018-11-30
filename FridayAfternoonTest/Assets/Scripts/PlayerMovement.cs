using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed = 1.0f;
    public float maxMovementSpeed = 10.0f;
    public float cameraSpeedX = 1.0f;
    public float cameraSpeedY = 1.0f;

    private GameObject playerCamera;
    private GameObject head;
    private GameObject groundChecker;
    private VirtualStick leftStick, rightStick;
    private bool isJumpReady;

	// Use this for initialization
	void Start () {
        this.head = this.FindGameObject(this.gameObject, "Head");
        this.playerCamera = this.FindGameObject(this.head, "Main Camera");
        this.groundChecker = this.FindGameObject(this.gameObject, "GroundChecker");
        this.leftStick = this.transform.Find("LeftStick").GetComponent<VirtualStick>();
        this.rightStick = this.transform.Find("RightStick").GetComponent<VirtualStick>();
        this.isJumpReady = false;
    }

    private GameObject FindGameObject(GameObject parent, string objectName)
    {
        GameObject objectToFind = null;
        Transform objectToFindTransfrom = parent.transform.Find(objectName);

        if (objectToFindTransfrom != null)
        {
            objectToFind = objectToFindTransfrom.gameObject;
        }
        else
        {
            Debug.Log("Not able to find GameObject " + objectName);
        }

        return objectToFind;
    }

    // Update is called once per frame
    void Update () {
        if(this.groundChecker != null)
        {
            this.isJumpReady = this.groundChecker.GetComponent<GroundChecker>().IsTouchingGround();
        }

        if(Input.GetKeyDown(KeyCode.Space) && this.isJumpReady)
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * 500.0f, ForceMode.Impulse);
            this.isJumpReady = false;
        }

#if UNITY_EDITOR || UNITY_WINDOWS
        Vector3 playerMovement = this.ControlsWASD();
        Vector3 mouseMovement = this.ControlsFreeMouse();
#elif UNITY_ANDROID
        Vector3 playerMovement = new Vector3(this.leftStick.GetCurrentStickDelta().x, 0.0f, this.leftStick.GetCurrentStickDelta().y);
        Vector3 mouseMovement = this.rightStick.GetCurrentStickDelta();
#endif

        // Movement in space.
        playerMovement.x = playerMovement.x * this.movementSpeed * Time.deltaTime;
        playerMovement.z = playerMovement.z * this.movementSpeed * Time.deltaTime;
        //this.transform.Translate(playerMovement);

        Rigidbody rigidBody = this.GetComponent<Rigidbody>();
        if (rigidBody.velocity.magnitude < this.maxMovementSpeed)
        {
            rigidBody.AddRelativeForce(playerMovement, ForceMode.VelocityChange);
        }

        // Camera movement.
        this.transform.Rotate(Vector3.up, mouseMovement.x * cameraSpeedX * Time.deltaTime);
        this.playerCamera.transform.position = this.playerCamera.transform.position + new Vector3(0.0f, -mouseMovement.y * cameraSpeedY * Time.deltaTime, 0.0f);

        if(this.playerCamera.transform.localPosition.y > 4.0f)
        {
            this.playerCamera.transform.localPosition = new Vector3(this.playerCamera.transform.localPosition.x, 4.0f, this.playerCamera.transform.localPosition.z);
        }

        if (this.playerCamera.transform.localPosition.y < -1.0f)
        {
            this.playerCamera.transform.localPosition = new Vector3(this.playerCamera.transform.localPosition.x, -1.0f, this.playerCamera.transform.localPosition.z);
        }

        this.playerCamera.transform.LookAt(this.head.transform, Vector3.up);
    }

    /// <summary>
    /// Allows to control the player using WASD-keys
    /// </summary>
    /// <returns></returns>
    private Vector3 ControlsWASD()
    {
        Vector3 movement = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            movement.z += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement.z += -1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement.x += -1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement.x += 1.0f;
        }

        return movement;
    }

    private Vector3 ControlsFreeMouse()
    {
        return new Vector3(this.cameraSpeedX * Input.GetAxis("Mouse X"), this.cameraSpeedY * Input.GetAxis("Mouse Y"), 0.0f);
    }
}
