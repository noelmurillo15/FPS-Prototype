/*
 * ItemAnimator -
 * Created by : Allan N. Murillo
 * Last Edited : 4/11/2020
 */

using UnityEngine;

namespace ANM.FPS.Items
{
    [RequireComponent(typeof(Animator))]
    public class ItemAnimator : MonoBehaviour
    {
        private Animator _animator;
        private ItemMaster _itemMaster;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            _itemMaster.EventObjectPickup -= EnableAnimator;
            _itemMaster.EventObjectThrow -= DisableAnimator;
        }

        private void Initialize()
        {
            _animator = GetComponent<Animator>();
            _itemMaster = GetComponent<ItemMaster>();
            _itemMaster.EventObjectThrow += DisableAnimator;
            _itemMaster.EventObjectPickup += EnableAnimator;
        }

        private void EnableAnimator()
        {
            _animator.enabled = true;
        }

        private void DisableAnimator()
        {
            _animator.enabled = false;
        }
    }
}
