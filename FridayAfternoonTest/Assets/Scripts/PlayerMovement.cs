using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed = 1.0f;
    public float cameraSpeedX = 1.0f;
    public float cameraSpeedY = 1.0f;

    private GameObject playerCamera;
    private GameObject head;

	// Use this for initialization
	void Start () {
        this.head = this.transform.Find("Head").gameObject;
        this.playerCamera = this.head.transform.Find("Main Camera").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * 100.0f, ForceMode.Impulse);
        }

        Vector3 movement = new Vector3();

		if(Input.GetKey(KeyCode.W))
        {
            movement.z = this.movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement.z = -this.movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement.x = -this.movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement.x = this.movementSpeed * Time.deltaTime;
        }

        this.transform.Translate(movement);

        Vector3 mouseMovement = new Vector3(this.cameraSpeedX * Input.GetAxis("Mouse X"), this.cameraSpeedY * Input.GetAxis("Mouse Y"), 0.0f);

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
}
