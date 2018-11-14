using UnityEngine;

public class VirtualStick : MonoBehaviour {
    public Rect screenArea;

    public Texture2D sticktexture;

    public Texture2D areaTexture;

    private Vector2 stickStartPos;

    private Vector2 currentStickDelta;

    private bool isActive;

	// Use this for initialization
	private void Start () {
        this.isActive = false;
        this.screenArea = new Rect(
            this.screenArea.x * (float)Screen.width,
            this.screenArea.y * (float)Screen.height,
            this.screenArea.width * (float)Screen.width,
            this.screenArea.height * (float)Screen.height);
	}
	
	// Update is called once per frame
	private void Update () {
/*#if UNITY_ANDROID
        if(Input.touchCount == 0)
        {
            return;
        }

        Vector2 pointerPosition = Input.GetTouch(0).position;
        bool stickDown = Input.GetTouch(0).phase == TouchPhase.Began;
        bool stickRelease = Input.GetTouch(0).phase == TouchPhase.Ended;
#else*/
        Vector2 pointerPosition = Input.mousePosition;
        bool stickDown = Input.GetMouseButtonDown(0);
        bool stickRelease = Input.GetMouseButtonUp(0);
//#endif

        if (stickDown && this.screenArea.Contains(pointerPosition))
        {
            this.stickStartPos = pointerPosition;
            this.isActive = true;
        }
        else if (stickRelease)
        {
            this.isActive = false;
        }

        if(this.isActive)
        {
            float maxDistance = (float)Screen.height * 0.2f;
            this.currentStickDelta = new Vector2(pointerPosition.x, pointerPosition.y) - this.stickStartPos;
            this.currentStickDelta.x = Mathf.Clamp(this.currentStickDelta.x, -maxDistance, maxDistance) / maxDistance;
            this.currentStickDelta.y = Mathf.Clamp(this.currentStickDelta.y, -maxDistance, maxDistance) / maxDistance;
        }

        Debug.Log(currentStickDelta);
	}

    /// <summary>
    /// Draw the current stick position and the start position to visualize the virtual stick.
    /// </summary>
    private void OnGUI()
    {
#if UNITY_EDITOR
        GUI.DrawTexture(this.screenArea, this.areaTexture);
#endif

        if(this.isActive)
        {
            Vector2 stickSize = new Vector2(Screen.height * 0.1f, Screen.height * 0.1f);
            Vector2 stickStartScreenPos = new Vector2(stickStartPos.x , Screen.height - stickStartPos.y) - (stickSize * 0.5f);
            GUI.DrawTexture(new Rect(stickStartScreenPos, stickSize), this.sticktexture);

            Vector2 currentStickScreenPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y) - (stickSize * 0.5f);
            GUI.DrawTexture(new Rect(currentStickScreenPos, stickSize), this.sticktexture);
        }
    }

    public Vector2 GetCurrentStickDelta()
    {
        if(this.isActive)
        {
            return this.currentStickDelta;
        }
        else
        {
            return Vector2.zero;
        }
    }
}
