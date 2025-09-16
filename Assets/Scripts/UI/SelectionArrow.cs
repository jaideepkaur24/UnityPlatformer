

using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;

    private RectTransform arrow;
    private int currentPosition;

    private Vector2 touchStartPos;
    private bool swipeDetected;

    private void Awake()
    {
        arrow = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        currentPosition = 0;
        ChangePosition(0);
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // Keyboard input (for testing on PC)
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            ChangePosition(1);

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact();
#endif

#if UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            // Start swipe
            if (t.phase == TouchPhase.Began)
            {
                touchStartPos = t.position;
                swipeDetected = false;
            }

            // Detect swipe direction
            if (t.phase == TouchPhase.Moved && !swipeDetected)
            {
                float swipeY = t.position.y - touchStartPos.y;

                if (Mathf.Abs(swipeY) > 50f) // swipe threshold
                {
                    if (swipeY > 0)
                        ChangePosition(-1); // swipe up
                    else
                        ChangePosition(1);  // swipe down

                    swipeDetected = true;
                }
            }

            // Tap for interact
            if (t.phase == TouchPhase.Ended && !swipeDetected)
            {
                if (t.position.x > Screen.width / 2)
                {
                    Interact(); // tap right half = interact
                }
            }
        }
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
            SoundManager.instance.PlaySound(changeSound);

        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;
        else if (currentPosition > buttons.Length - 1)
            currentPosition = 0;

        AssignPosition();
    }

    private void AssignPosition()
    {
        // Move the arrow to the new option
        arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].position.y);
    }

    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound);
        buttons[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
