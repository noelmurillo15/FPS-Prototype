/*
 * GunNpcInput -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using ANM.FPS.Npc;
using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    public class GunNpcInput : MonoBehaviour
    {
        private LayerMask _layersToDamage = 0;
        private Transform _gunBarrelTransform;
        private Transform _ownerTransform;
        private RaycastHit _gunShotHit;
        private GunMaster _gunMaster;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            if(_gunMaster.npcEquipped) _gunMaster.EventNpcInput -= NpcFireGun;
        }

        private void Initialize()
        {
            _layersToDamage = 1 << 16;
            _gunBarrelTransform = transform;
            _gunMaster = GetComponent<GunMaster>();
            _ownerTransform = _gunBarrelTransform.root;
            
            if (_gunMaster.playerEquipped || !_gunMaster.npcEquipped)
            {
                //Debug.Log("    - GunNpcInput is being disabled");
                enabled = false;
                return;
            }
            
            if(_gunMaster.npcEquipped) _gunMaster.EventNpcInput += NpcFireGun;
        }

        private void NpcFireGun(float attackSpread, Vector3 attackTarget)
        {
            // Debug.Log("GunNpcInput::NpcFireGun()");
            _gunBarrelTransform.LookAt(attackTarget);
            var noise = new Vector3(Random.Range(-attackSpread, attackSpread),
                Random.Range(-attackSpread, attackSpread), 0f);
            var forward = _gunBarrelTransform.forward;
            attackTarget += noise + forward;
            var start = _gunBarrelTransform.position + forward;
            Debug.DrawLine(start, attackTarget, Color.white, 0.1f);
            if (!Physics.Linecast(start, attackTarget, out _gunShotHit, _layersToDamage))
            {
                //Debug.Log("NPC missed gun shot input");
                return;
            }

            if (_gunShotHit.transform.root == _ownerTransform)
            {
                Debug.Log("NPC gun input shot on self : " + _gunShotHit.transform.name);
                //Debug.DrawLine(start, attackTarget, Color.black, 1f);
                return;
            }

            if (_gunShotHit.transform.GetComponent<NpcTakeDamage>() != null)
            {
                // Debug.Log(_gunShotHit.transform.root.name + " was shot by " + transform.root.name);
                // Debug.Log("NPC gun input shot confirmed on target");
                Debug.DrawLine(start, attackTarget, Color.red, 1f);
                _gunMaster.CallEventShoot(_gunShotHit, _gunShotHit.transform);
            }
            else
            {
                if (_gunShotHit.transform.root.CompareTag("Player"))
                {
                    Debug.Log("NPC gun input shot confirmed on the Player!!");
                }
                else
                {
                    // Debug.Log(transform.root.name + "'s shot was obstructed by : " + _gunShotHit.transform.name);
                    Debug.Log("NPC gun input shot confirmed on unidentified object");
                    //Debug.DrawLine(start, attackTarget, Color.blue, 1f);
                    _gunMaster.CallEventShotDefault(_gunShotHit, _gunShotHit.transform);
                }
            }
        }
    }
}
