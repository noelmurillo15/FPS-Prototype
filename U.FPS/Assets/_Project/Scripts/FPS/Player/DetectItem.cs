/*
 * DetectItem -
 * Created by : Allan N. Murillo
 * Last Edited : 3/24/2020
 */

using ANM.FPS.Items;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ANM.FPS.Player
{
    public class DetectItem : MonoBehaviour
    {
        [Tooltip("What layers is being used for items.")]
        public LayerMask layerToDetect;
        [Tooltip("what transform will the ray be fired from?")]
        public Transform rayTransformPivot;
        [Tooltip("the editor input button that will be used for picking items")]
        public string buttonPickUp;

        private Transform _itemAvailableForPickup;
        private RaycastHit _hit;
        private const float DetectRange = 5;
        private const float DetectRadius = 0.5f;
        private bool _itemInRange;

        private const float LabelWidth = 200;
        private const float LabelHeight = 50;

        private InputAction _pickupAction;


        private void Awake()
        {
            _pickupAction = new InputAction(buttonPickUp, binding: "<Keyboard>/e");
            _pickupAction.AddBinding("<Gamepad>/A");
            _pickupAction.Enable();
            _pickupAction.performed += PickupItemInput;
        }

        private void OnDisable()
        {
            _pickupAction.performed -= PickupItemInput;
            _pickupAction.Disable();
        }

        private void Update()
        {
            CastRayForDetectingItems();
        }

        private void CastRayForDetectingItems()
        {
            if (Physics.SphereCast(rayTransformPivot.position, DetectRadius, rayTransformPivot.forward, out _hit, DetectRange, layerToDetect))
            {
                if (_hit.transform.GetComponent<ItemMaster>() == null) return;
                _itemAvailableForPickup = _hit.transform;
                _itemInRange = true;
            }
            else
            {
                _itemInRange = false;
            }
        }

        private void CheckForItemPickupAttempt()
        {
            if (Time.timeScale <= 0f) { return; }

            if (!_itemInRange) return;
            if (!_itemAvailableForPickup.root.CompareTag("Player"))
            {
                _itemAvailableForPickup.GetComponent<ItemMaster>().CallEventPickupAction(transform);
            }
        }

        private void OnGUI()
        {
            if (_itemInRange && _itemAvailableForPickup != null)
            {
                GUI.Label(new Rect(Screen.width / 2 - LabelWidth / 2, Screen.height / 2, LabelWidth, LabelHeight), _itemAvailableForPickup.name);
            }
        }

        #region Detect Input

        private void PickupItemInput(InputAction.CallbackContext context)
        {
            CheckForItemPickupAttempt();
        }
        #endregion
    }
}