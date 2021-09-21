/*
 * NpcTurnOffDetectionCollider -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc
{
    [RequireComponent(typeof(Collider))]
    public class NpcTurnOffDetectionCollider : MonoBehaviour
    {
        private NpcMaster _npcMaster;
        private Collider _myCollider;
    
        
        private void OnEnable()
        {
            _myCollider = GetComponent<Collider>();
            _npcMaster = GetComponentInParent<NpcMaster>();
            _npcMaster.EventNpcDie += OnEventNpcDie;
        }

        private void OnDisable()
        {
            _npcMaster.EventNpcDie -= OnEventNpcDie;
        }

        private void OnEventNpcDie()
        {
            _myCollider.gameObject.layer = LayerMask.NameToLayer("Default");
            _myCollider.enabled = false;
            _npcMaster.EventNpcDie -= OnEventNpcDie;
        }
    }
}
