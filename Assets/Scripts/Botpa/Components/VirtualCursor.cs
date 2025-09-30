using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Botpa {

    public class VirtualCursor : MonoBehaviour {

        //Input
        [Header("Input")]
        [SerializeField] private InputActionReference virtualMove;
        [SerializeField] private InputActionReference virtualClick;
        [SerializeField] private InputActionReference virtualScroll;
        [SerializeField] private InputActionReference realPosition;

        //Canvas
        private Canvas canvas;
        private RectTransform canvasTransform;
        private Vector2 canvasSize = new();
        private Vector2 screenSize = new();

        public Vector2 workspaceSize => new(canvasSize.x, canvasSize.y);

        //Cursor
        [Header("Cursor")]
        [SerializeField] private bool startActive = true;
        [SerializeField] private float _moveSpeed = 300;
        [SerializeField] private float _scrollSpeed = 300;
        [SerializeField] private RectTransform cursor;

        private Vector2 moveInput, scrollInput;
        private bool isMoving, isScrolling, isPointerDown, isPointerUp;

        public float moveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
        public float scrollSpeed { get => _scrollSpeed; set => _scrollSpeed = value; }
        public Vector2 position { get; private set; } = new();
        public bool isActive { get; private set; } = false;

        //Events
        private PointerEventData pointerEventData = new(EventSystem.current);
        private readonly List<RaycastResult> raycastResults = new();
        private List<GameObject> hoveredOld = new();
        private List<GameObject> hoveredNew = new();
        private HashSet<GameObject> downed = new();


        //State
        private void Start() {
            //Enable input (just in case)
            virtualMove.Enable();
            virtualClick.Enable();
            virtualScroll.Enable();
            realPosition.Enable();

            //Get canvas
            canvas = cursor.GetComponentInParent<Canvas>();
            canvasTransform = canvas.GetComponent<RectTransform>();
            canvasSize = canvasTransform.rect.size;
            screenSize = new Vector2(Screen.width, Screen.height);

            //Toggle cursor
            SetActive(startActive);
        }

        private void Update() {
            //Not active
            if (!isActive) return;



             /*$$$$$                                 /$$
            |_  $$_/                                | $$
              | $$   /$$$$$$$   /$$$$$$  /$$   /$$ /$$$$$$
              | $$  | $$__  $$ /$$__  $$| $$  | $$|_  $$_/
              | $$  | $$  \ $$| $$  \ $$| $$  | $$  | $$
              | $$  | $$  | $$| $$  | $$| $$  | $$  | $$ /$$
             /$$$$$$| $$  | $$| $$$$$$$/|  $$$$$$/  |  $$$$/
            |______/|__/  |__/| $$____/  \______/    \___/
                              | $$
                              | $$
                              |_*/

            //Read move input
            moveInput = virtualMove.ReadValue<Vector2>();
            isMoving = moveInput.sqrMagnitude > 0;

            //Read scroll input
            scrollInput = virtualScroll.ReadValue<Vector2>();
            isScrolling = scrollInput.sqrMagnitude > 0;

            //Read click input
            isPointerDown = virtualClick.WasPressedThisFrame();
            isPointerUp = virtualClick.WasReleasedThisFrame();



             /*$      /$$                              
            | $$$    /$$$                              
            | $$$$  /$$$$  /$$$$$$  /$$    /$$ /$$$$$$ 
            | $$ $$/$$ $$ /$$__  $$|  $$  /$$//$$__  $$
            | $$  $$$| $$| $$  \ $$ \  $$/$$/| $$$$$$$$
            | $$\  $ | $$| $$  | $$  \  $$$/ | $$_____/
            | $$ \/  | $$|  $$$$$$/   \  $/  |  $$$$$$$
            |__/     |__/ \______/     \_/    \______*/

            //Update cursor position if it moved
            if (isMoving) UpdatePosition(position + Time.unscaledDeltaTime * moveSpeed * Vector2.ClampMagnitude(moveInput, 1));

            //Update scroll delta
            pointerEventData.scrollDelta = Time.unscaledDeltaTime * scrollSpeed * scrollInput;



             /*$$$$$$$                              /$$             
            | $$_____/                             | $$             
            | $$    /$$    /$$ /$$$$$$  /$$$$$$$  /$$$$$$   /$$$$$$$
            | $$$$$|  $$  /$$//$$__  $$| $$__  $$|_  $$_/  /$$_____/
            | $$__/ \  $$/$$/| $$$$$$$$| $$  \ $$  | $$   |  $$$$$$ 
            | $$     \  $$$/ | $$_____/| $$  | $$  | $$ /$$\____  $$
            | $$$$$$$$\  $/  |  $$$$$$$| $$  | $$  |  $$$$//$$$$$$$/
            |________/ \_/    \_______/|__/  |__/   \___/ |______*/

            //Call events to notify the current state of the pointer
            CallEvents();
        }
        
        private void OnRectTransformDimensionsChange() {
            //Canvas wasn't init yet
            if (!canvas) return;

            //Update canvas & screen size
            canvasSize = canvasTransform.rect.size;
            screenSize = new Vector2(Screen.width, Screen.height);
            
            //Clamp position in canvas (to prevent cursor from going outside)
            UpdatePosition(position);
        }

        //Info
        public void SetActive(bool newActive) {
            //Update active
            isActive = newActive;

            //Toggle visibility
            cursor.gameObject.SetActive(isActive);
        }

        //Events
        private void CallEvents(bool ignoreClicks = false) {
            //Raycast to see what element are being hovered
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            foreach (var result in raycastResults) hoveredNew.Add(result.gameObject);
            raycastResults.Clear();

            //Pointer exit event for elements that no longer are hovered
            foreach (var element in hoveredOld)
                if (!hoveredNew.Contains(element))
                    ExecuteEvents.Execute(element, pointerEventData, ExecuteEvents.pointerExitHandler);

            //Pointer up event for all elements that had pointer down called previously
            if (!ignoreClicks && isPointerUp)
                foreach (var obj in downed)
                    ExecuteEvents.Execute(obj, pointerEventData, ExecuteEvents.pointerUpHandler);

            //Rest of events
            foreach (var obj in hoveredNew) {
                //Pointer enter event for elements that were not hovered before
                if (!hoveredOld.Contains(obj))
                    ExecuteEvents.Execute(obj, pointerEventData, ExecuteEvents.pointerEnterHandler);

                //Pointer move event for all elements if the cursor moved
                if (isMoving)
                    ExecuteEvents.Execute(obj, pointerEventData, ExecuteEvents.pointerMoveHandler);

                //Pointer move event for all elements if the cursor moved
                if (isScrolling)
                    ExecuteEvents.Execute(obj, pointerEventData, ExecuteEvents.scrollHandler);

                //Clicks
                if (!ignoreClicks) {
                    //Pointer down event for all elements if the key was just pressed
                    if (isPointerDown)
                        ExecuteEvents.Execute(obj, pointerEventData, ExecuteEvents.pointerDownHandler);

                    //Pointer click event for elements that had pointer down event called previously
                    if (isPointerUp && downed.Contains(obj))
                        ExecuteEvents.Execute(obj, pointerEventData, ExecuteEvents.pointerClickHandler);
                }
            }

            //Clicks
            if (!ignoreClicks) {
                //Check if pointer down/up
                if (isPointerDown) {
                    //Pointer down -> Save currently hovered elements to check on pointer up if a click was performed
                    downed = new(hoveredNew);
                } else if (isPointerUp) {
                    //Pointer up -> Clear downed elements since the cursor is no longer down
                    downed.Clear();
                }
            }

            //Swap lists & clear new one to use it next frame
            (hoveredOld, hoveredNew) = (hoveredNew, hoveredOld);
            hoveredNew.Clear();
        }

        //Position
        private void UpdatePosition(Vector2 newPosition) {
            //Update position
            position = new Vector2(Mathf.Clamp(newPosition.x, 0, canvasSize.x), Mathf.Clamp(newPosition.y, 0, canvasSize.y));

            //Update cursor transform
            cursor.localPosition = position;

            //Update our PointerEventData position
            pointerEventData.position = position / canvasSize * screenSize;
        }

        public void MoveTo(Vector2 newPosition) {
            //Update position
            UpdatePosition(newPosition);

            //Call events
            CallEvents(true);
        }

        public void MoveToCenter() {
            //Move to canvas center
            MoveTo(canvasSize / 2);
        }
        
        public void MoveToRealCursor() {
            //Move to cursor
            MoveTo(realPosition.ReadValue<Vector2>() * canvasSize / screenSize);
        }
        
    }

}
