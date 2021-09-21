/*
 * GunApplyForce -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    public class GunApplyForce : MonoBehaviour
    {
        [SerializeField] private float forceToApply = 300f;
        private GunMaster _gunMaster;
        private Transform _transform;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            if(_gunMaster.playerEquipped) _gunMaster.EventShotDefault -= ApplyForce;
        }

        private void Initialize()
        {
            _transform = transform;
            _gunMaster = GetComponent<GunMaster>();
            if (!_gunMaster.playerEquipped && !_gunMaster.npcEquipped)
            {
                // Debug.Log("    - GunApplyForce is being disabled");
                enabled = false;
                return;
            }
            if(_gunMaster.playerEquipped) _gunMaster.EventShotDefault += ApplyForce;
        }

        private void ApplyForce(RaycastHit hit, Transform t)
        {
            Debug.Log("GunApplyForce::ApplyForce()");
            if (t.GetComponent<Rigidbody>() != null)
            {
                t.GetComponent<Rigidbody>().AddForce(_transform.forward * forceToApply, ForceMode.Impulse);
            }
        }
    }
}
