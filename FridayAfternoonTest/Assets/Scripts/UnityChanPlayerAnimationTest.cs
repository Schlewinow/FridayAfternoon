using UnityEngine;

public class UnityChanPlayerAnimationTest : MonoBehaviour {

    public Animator animator;
    public float inputH;
    public float inputV;

	// Use this for initialization
	void Start () {
        this.animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Y))
        {
            this.animator.Play("WAIT01", -1, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            this.animator.Play("WAIT02", -1, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            this.animator.Play("WAIT03", -1, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            this.animator.Play("WAIT04", -1, 0.0f);
        }

        this.inputH = this.ControlsWASD().x;
        this.inputV = this.ControlsWASD().z;
        this.animator.SetFloat("inputH", this.inputH);
        this.animator.SetFloat("inputV", this.inputV);
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
}
