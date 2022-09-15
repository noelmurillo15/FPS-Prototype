/*
 * GunShoot -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using ANM.FPS.Npc;
using ANM.FPS.Player;
using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    public class GunShoot : MonoBehaviour
    {
        [SerializeField] private Transform bulletChamberLocation = null;

        //  Cached Variables
        private GunMaster _gunMaster;
        private RaycastHit _hit;
        private Camera _cam;

        private readonly Vector3 _screenCenter = new Vector3(0.5f, 0.5f, 0f);

        //  Public Variables
        [SerializeField] private float range = 400f;

        //  Privates
        private const float OffsetFactor = 7f;
        // private Vector3 _startPos;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            _cam = null;
            if (!_gunMaster.playerEquipped) return;
            _gunMaster.EventPlayerInput -= OpenFire;
            _gunMaster.EventSpeedCaptured -= SetStartShootingPosition;
        }

        private void Initialize()
        {
            _gunMaster = GetComponent<GunMaster>();
            if (!_gunMaster.playerEquipped && !_gunMaster.npcEquipped)
            {
                // Debug.Log("    - GunShoot is being disabled");
                enabled = false;
                return;
            }

            if (!_gunMaster.playerEquipped) return;
            _gunMaster.EventPlayerInput += OpenFire;
            _gunMaster.EventSpeedCaptured += SetStartShootingPosition;
        }

        private void OpenFire()
        {
            Debug.Log("GunShoot::OpenFire()");
            if (_cam == null) _cam = GetComponentInParent<PlayerMaster>().playerCam.GetComponent<Camera>();
            var ray = _cam.ViewportPointToRay(_screenCenter);
            if (!Physics.Raycast(ray, out _hit, range)) return;

            if (_hit.transform.GetComponent<NpcTakeDamage>() != null)
                _gunMaster.CallEventShoot(_hit, _hit.transform);
            else
                _gunMaster.CallEventShotDefault(_hit, _hit.transform);
        }

        private void SetStartShootingPosition(float playerSpeed)
        {
            Debug.Log("GunShoot::SetStartShootingPosition()");
            var offset = playerSpeed / OffsetFactor;
            // _startPos = new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), 1f);
        }

        public float GetAttackRange()
        {
            return range;
        }
    }
}
