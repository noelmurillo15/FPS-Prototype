/*
 * Controller - Holds a reference to the InputController, used to setup various inputs from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 8/24/2020
 */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ANM.Scriptable
{
    [CreateAssetMenu(menuName = "SingleInstance/Controller")]
    public class Controller : ScriptableObject, InputController.IPlayerActions
    {
        [SerializeField] private bool debug;

        public event UnityAction<Vector2> OnMovementEvent = delegate { };
        public event UnityAction<Vector2> OnCameraLookEvent = delegate { };
        public event UnityAction OnQuitEvent = delegate { };

        public event UnityAction OnShootEvent = delegate { };
        public event UnityAction OnReloadEvent = delegate { };
        public event UnityAction OnBurstToggleEvent = delegate { };
        public event UnityAction OnJumpEvent = delegate { };
        public event UnityAction<bool> OnSprintEvent = delegate { };
        public event UnityAction<bool> OnCastSpellEvent = delegate { };
        public event UnityAction<bool> OnLaunchSpellEvent = delegate { };
        public event UnityAction<bool> OnInteractEvent = delegate { };
        public event UnityAction<InputActionPhase> OnSelectSpellEvent = delegate { };

        private InputController myInputController;


        #region Unity Funcs

        private void OnEnable()
        {
            myInputController ??= new InputController();
            myInputController.Enable();
            myInputController.Player.Enable();
            myInputController.Player.SetCallbacks(this);
            OnQuitEvent += EndGameInputHandler;
            Log("Registering Input");
        }

        private void OnDisable()
        {
            if (myInputController == null) return;
            Log("De-registering Input");
            OnQuitEvent -= EndGameInputHandler;
            myInputController.Disable();
        }

        private void OnDestroy()
        {
            if (myInputController == null) return;
            Log("Destroying Controller");
            myInputController.Dispose();
            myInputController = null;
        }

        #endregion

        #region Public Funcs

        public InputController GetInput() => myInputController;

        public void OnMovement(InputAction.CallbackContext context)
        {
            Log("OnMovement");
            OnMovementEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnCameraLook(InputAction.CallbackContext context)
        {
            Log("OnCameraLook");
            OnCameraLookEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            Log("OnShoot");
            OnShootEvent?.Invoke();
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            Log("OnReload");
            OnReloadEvent?.Invoke();
        }

        public void OnBurstToggle(InputAction.CallbackContext context)
        {
            Log("OnBurstToggle");
            OnBurstToggleEvent?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;
            Log("OnJump");
            OnJumpEvent?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            Log("OnSprint");
            if (context.phase == InputActionPhase.Performed) OnSprintEvent?.Invoke(true);
            else if (context.phase == InputActionPhase.Canceled) OnSprintEvent?.Invoke(false);
        }

        public void OnCastSpell(InputAction.CallbackContext context)
        {
            Log("OnCastSpell");
            if (context.phase == InputActionPhase.Performed) OnCastSpellEvent?.Invoke(true);
            else if (context.phase == InputActionPhase.Canceled) OnCastSpellEvent?.Invoke(false);
        }

        public void OnLaunchSpell(InputAction.CallbackContext context)
        {
            Log("OnLaunchSpell");
            if (context.phase == InputActionPhase.Performed) OnLaunchSpellEvent?.Invoke(true);
            else if (context.phase == InputActionPhase.Canceled) OnLaunchSpellEvent?.Invoke(false);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            Log("OnInteract");
            if (context.phase == InputActionPhase.Performed) OnInteractEvent?.Invoke(true);
            else if (context.phase == InputActionPhase.Canceled) OnInteractEvent?.Invoke(false);
        }

        public void OnSelectSpell(InputAction.CallbackContext context)
        {
            Log("OnSelectSpell");
            OnSelectSpellEvent?.Invoke(context.phase);
        }

        public void OnQuit(InputAction.CallbackContext context)
        {
            Log("OnQuit");
            OnQuitEvent?.Invoke();
        }

        #endregion

        #region Private Funcs

        private void Log(string msg)
        {
            if (!debug) return;
            Debug.Log("[Controller] : " + msg);
        }

#if !UNITY_EDITOR
        private static void EndGameInputHandler() => Application.Quit();
#else
        private static void EndGameInputHandler() => UnityEditor.EditorApplication.isPlaying = false;
#endif

        #endregion
    }
} 
