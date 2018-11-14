using UnityEngine;

public class VirtualStick : MonoBehaviour {
    public Rect screenArea;

    public Texture2D sticktexture;

    public Texture2D areaTexture;

    private Vector2 stickStartPos;

    private Vector2 currentStickDelta;

    private bool isActive;

    private int currentTouchIndex;

	// Use this for initialization
	private void Start () {
        this.isActive = false;
        this.screenArea = new Rect(
            this.screenArea.x * (float)Screen.width,
            this.screenArea.y * (float)Screen.height,
            this.screenArea.width * (float)Screen.width,
            this.screenArea.height * (float)Screen.height);
        this.currentTouchIndex = -1;
	}
	
	// Update is called once per frame
	private void Update () {
#if UNITY_EDITOR  || UNITY_WINDOWS
        Vector2 pointerPosition = Input.mousePosition;
        bool stickDown = Input.GetMouseButtonDown(0);
        bool stickRelease = Input.GetMouseButtonUp(0);
#elif UNITY_ANDROID
        if (Input.touchCount == 0 && this.currentTouchIndex < 0)
        {
            return;
        }

        Vector2 pointerPosition = Vector2.zero;
        bool stickDown = false;
        bool stickRelease = false;

        if (this.currentTouchIndex < 0)
        {
            for (int touchIndex = 0; touchIndex < Input.touchCount; ++touchIndex)
            {
                if(Input.GetTouch(touchIndex).phase == TouchPhase.Began &&
                    this.screenArea.Contains(Input.GetTouch(touchIndex).position))
                {
                    this.currentTouchIndex = touchIndex;
                    stickDown = true;
                    pointerPosition = Input.GetTouch(this.currentTouchIndex).position;
                }
            }
        }
        else
        {
            pointerPosition = Input.GetTouch(this.currentTouchIndex).position;
        }
        
        // Release if the proper touch ended.
        if((this.currentTouchIndex >= 0 && Input.GetTouch(this.currentTouchIndex).phase == TouchPhase.Ended) ||
            Input.touchCount == 0)
        {
            this.currentTouchIndex = -1;
            stickRelease = true;
        }
#endif

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

#if UNITY_EDITOR || UNITY_WINDOWS
            Vector2 currentStickScreenPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y) - (stickSize * 0.5f);
            GUI.DrawTexture(new Rect(currentStickScreenPos, stickSize), this.sticktexture);
#elif UNITY_ANDROID
            if(this.currentTouchIndex >= 0)
            {
                Vector2 currentStickScreenPos = new Vector2(Input.GetTouch(this.currentTouchIndex).position.x, Screen.height - Input.GetTouch(this.currentTouchIndex).position.y) - (stickSize * 0.5f);
                GUI.DrawTexture(new Rect(currentStickScreenPos, stickSize), this.sticktexture);
            }
#endif

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
