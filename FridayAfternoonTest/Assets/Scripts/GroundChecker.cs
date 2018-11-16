using UnityEngine;

public class GroundChecker : MonoBehaviour {
    private bool isTouchingGround;

    private void Start()
    {
        this.isTouchingGround = false;
    }

    private void OnTriggerEnter()
    {
        this.isTouchingGround = true;
    }

    private void OnTriggerExit()
    {
        this.isTouchingGround = false;
    }

    public bool IsTouchingGround()
    {
        return this.isTouchingGround;
    }
}
