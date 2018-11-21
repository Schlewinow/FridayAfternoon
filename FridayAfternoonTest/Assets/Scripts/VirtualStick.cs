using UnityEngine;

public class VirtualStick : MonoBehaviour {
    private struct InputState
    {
        public bool stickDown;
        public bool stickRelease;
        public Vector2 pointerPosition;
    }

    public Rect screenArea;

    public Texture2D sticktexture;

    public Texture2D areaTexture;

    private Vector2 stickStartPos;

    private Vector2 currentStickDelta;

    private bool isUsingStick;

    private int currentTouchIndex;

	// Use this for initialization
	private void Start () {
        this.isUsingStick = false;
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
        InputState state = this.GetInputStateMouse();
#elif UNITY_ANDROID
        InputState state = this.GetInputStateTouch();
#endif

        if (state.stickDown && this.screenArea.Contains(state.pointerPosition))
        {
            this.stickStartPos = state.pointerPosition;
            this.isUsingStick = true;
        }
        else if (state.stickRelease)
        {
            this.isUsingStick = false;
        }

        if(this.isUsingStick)
        {
            float maxDistance = (float)Screen.height * 0.2f;
            this.currentStickDelta = new Vector2(state.pointerPosition.x, state.pointerPosition.y) - this.stickStartPos;
            this.currentStickDelta.x = Mathf.Clamp(this.currentStickDelta.x, -maxDistance, maxDistance) / maxDistance;
            this.currentStickDelta.y = Mathf.Clamp(this.currentStickDelta.y, -maxDistance, maxDistance) / maxDistance;
        }
	}

    private InputState GetInputStateMouse()
    {
        InputState state = new InputState
        {
            stickDown = Input.GetMouseButtonDown(0),
            stickRelease = Input.GetMouseButtonUp(0),
            pointerPosition = Input.mousePosition
        };
        return state;
    }

    /// <summary>
    /// Compute the current input state of the virtual stick based on touch inputs.
    /// </summary>
    /// <returns>The current input state of this virtual stick.</returns>
    private InputState GetInputStateTouch()
    {
        Vector2 pointerPosition = Vector2.zero;
        bool stickDown = false;
        bool stickRelease = false;

        if (this.currentTouchIndex < 0)
        {
            for (int touchIndex = 0; touchIndex < Input.touchCount; ++touchIndex)
            {
                if (Input.GetTouch(touchIndex).phase == TouchPhase.Began &&
                    this.screenArea.Contains(Input.GetTouch(touchIndex).position))
                {
                    this.currentTouchIndex = Input.GetTouch(touchIndex).fingerId;
                    pointerPosition = Input.GetTouch(touchIndex).position;
                    stickDown = true;
                }
            }
        }
        else
        {
            pointerPosition = this.FindCurrentTouch().position;
        }

        // Release if the proper touch ended.
        if ((this.currentTouchIndex >= 0 && this.FindCurrentTouch().phase == TouchPhase.Ended) ||
            Input.touchCount == 0)
        {
            this.currentTouchIndex = -1;
            stickRelease = true;
        }

        InputState state = new InputState
        {
            stickDown = stickDown,
            stickRelease = stickRelease,
            pointerPosition = pointerPosition
        };
        return state;
    }

    private Touch FindCurrentTouch()
    {
        if(this.currentTouchIndex < 0)
        {
            return new Touch();
        }

        for (int touchIndex = 0; touchIndex < Input.touchCount; ++touchIndex)
        {
            if (Input.GetTouch(touchIndex).fingerId == this.currentTouchIndex)
            {
                return Input.GetTouch(touchIndex);
            }
        }

        return new Touch();
    }

    /// <summary>
    /// Draw the current stick position and the start position to visualize the virtual stick.
    /// </summary>
    private void OnGUI()
    {
#if UNITY_EDITOR
        GUI.DrawTexture(this.screenArea, this.areaTexture);
#endif

        if(this.isUsingStick)
        {
            Vector2 stickSize = new Vector2(Screen.height * 0.1f, Screen.height * 0.1f);
            Vector2 stickStartScreenPos = new Vector2(stickStartPos.x , Screen.height - stickStartPos.y) - (stickSize * 0.5f);
            GUI.DrawTexture(new Rect(stickStartScreenPos, stickSize), this.sticktexture);

#if UNITY_EDITOR || UNITY_WINDOWS
            this.DrawStickKnobMouse(stickSize);
#elif UNITY_ANDROID
            this.DrawStickKnobTouch(stickSize);
#endif

        }
    }

    private void DrawStickKnobMouse(Vector2 stickSize)
    {
        Vector2 currentStickScreenPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y) - (stickSize * 0.5f);
        GUI.DrawTexture(new Rect(currentStickScreenPos, stickSize), this.sticktexture);
    }

    private void DrawStickKnobTouch(Vector2 stickSize)
    {
        if (this.currentTouchIndex >= 0)
        {
            Touch currentTouch = this.FindCurrentTouch();
            Vector2 currentStickScreenPos = new Vector2(currentTouch.position.x, Screen.height - currentTouch.position.y) - (stickSize * 0.5f);
            GUI.DrawTexture(new Rect(currentStickScreenPos, stickSize), this.sticktexture);
        }
    }

    /// <summary>
    /// Used to allow external access to the current control stick input.
    /// </summary>
    /// <returns>Current virtual stick input in x and y axis.</returns>
    public Vector2 GetCurrentStickDelta()
    {
        if(this.isUsingStick)
        {
            return this.currentStickDelta;
        }
        else
        {
            return Vector2.zero;
        }
    }
}
