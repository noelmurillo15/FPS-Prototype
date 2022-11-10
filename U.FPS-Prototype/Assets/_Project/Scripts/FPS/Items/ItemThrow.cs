/*
 * ItemThrow -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using ANM.FPS.Items.Guns;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ANM.FPS.Items
{
    public class ItemThrow : MonoBehaviour
    {
        private ItemMaster _itemMaster;
        private Transform _transform;
        private Rigidbody _rigidbody;
        private Vector3 _throwDirection;
        private InputAction _throwAction;

        public bool canThrow;
        public float throwForce;
        public string throwButtonName;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            if (_throwAction == null) return;
            _throwAction.performed -= ThrowItemInput;
            _throwAction.Disable();
        }

        private void Initialize()
        {
            _transform = transform;
            if (!_transform.root.CompareTag("Player")) return;
            Debug.Log("ItemThrow::Throw Action Created");
            _itemMaster = GetComponent<ItemMaster>();
            _rigidbody = GetComponent<Rigidbody>();
            _throwAction = new InputAction("Throw", binding: "<Keyboard>/f");
            _throwAction.AddBinding("<Gamepad>/B");
            _throwAction.Enable();
            _throwAction.performed += ThrowItemInput;
        }

        private void CheckCanThrow()
        {
            if (throwButtonName != "" && canThrow)
            {
                if (!_transform.root.CompareTag("Player")) return;
                ThrowAction();
            }
            else Debug.LogError("Throw Button Input is Empty");
        }

        private void ThrowAction()
        {
            GetComponent<GunStandardInput>().ThrowGunAway();
            _throwDirection = _transform.parent.forward;
            _transform.parent = null;
            _itemMaster.CallEventObjectThrow();
            Throw();
        }

        private void Throw()
        {
            _rigidbody.AddForce(_throwDirection * throwForce, ForceMode.Impulse);
        }
        
        private void ThrowItemInput(InputAction.CallbackContext context)
        {
            CheckCanThrow();
        }
    }
}
