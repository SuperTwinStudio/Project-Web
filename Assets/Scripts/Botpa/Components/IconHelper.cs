using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Botpa {

    public class IconHelper : MonoBehaviour {

        //Input & event
        [SerializeField] private InputActionReference input;
        [SerializeField] private UnityEvent<Sprite> onUpdate;


        private void Start() {
            //Add on input method changed
            InputManager.AddOnInputMethodChanged(UpdateIcon);

            //Update icon if icon manager is already init
            if (InputManager.current) UpdateIcon();
        }

        private void OnDestroy() {
            //Remove on input method changed
            InputManager.RemoveOnInputMethodChanged(UpdateIcon);
        }

        private void UpdateIcon(InputMethod oldInputMethod, InputMethod newInputMethod) {
            //Call update event
            UpdateIcon();
        }

        public void UpdateIcon() {
            //Call update event
            onUpdate.Invoke(InputManager.GetIcon(input));
        }

    }

}


