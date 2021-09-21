/*
 * GunAnimator -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    [RequireComponent(typeof(Animator))]
    public class GunAnimator : MonoBehaviour
    {
        private GunMaster _gunMaster;
        private Animator _animator;
        private static readonly int Shoot = Animator.StringToHash("Shoot");


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            if (_gunMaster.playerEquipped) _gunMaster.EventPlayerInput -= PlayShootAnimation;
            //else if(_gunMaster.npcEquipped)  _gunMaster.EventNpcInput -= PlayNpcShootAnimation;
        }

        private void Initialize()
        {
            _animator = GetComponent<Animator>();
            _gunMaster = GetComponent<GunMaster>();
            if (!_gunMaster.playerEquipped && !_gunMaster.npcEquipped)
            {
                // Debug.Log("    - GunAnimator is being disabled");
                enabled = false;
                return;
            }

            if (_gunMaster.playerEquipped) _gunMaster.EventPlayerInput += PlayShootAnimation;
            //else _gunMaster.EventNpcInput += PlayNpcShootAnimation;
        }

        private void PlayShootAnimation()
        {
            Debug.Log("GunAnimator::PlayShootAnimation()");
            if (_animator != null)
            {
                _animator.SetTrigger(Shoot);
            }
        }

        private void PlayNpcShootAnimation(float dummy, Vector3 dummy2)
        {
            Debug.Log("GunAnimator::PlayNpcShootAnimation()");
            //PlayShootAnimation();
        }
    }
}
