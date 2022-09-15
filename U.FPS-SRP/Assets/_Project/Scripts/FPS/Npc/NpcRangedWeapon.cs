/*
 * NpcRangedWeapon -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc
{
    public class NpcRangedWeapon : MonoBehaviour
    {
        [SerializeField] private bool isLeftHanded;
        private Transform _myRightHand;
        private Transform _myLeftHand;
        private NpcStatePattern _npc;
        private Animator _animator;


        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _animator = GetComponent<Animator>();
            _npc = GetComponent<NpcStatePattern>();
            _myRightHand = _animator.GetBoneTransform(HumanBodyBones.RightThumbProximal);
            _myLeftHand = _animator.GetBoneTransform(HumanBodyBones.LeftThumbProximal);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!_animator.enabled) return;
            if (_npc.gun == null) return;
            if (!_npc.gun.activeSelf) return;
            
            var position = _npc.gun.transform.position;
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            _animator.SetIKPosition(AvatarIKGoal.RightHand, position);
            //_animator.SetIKRotation(AvatarIKGoal.RightHand, _myRightHand.rotation);

            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            _animator.SetIKPosition(AvatarIKGoal.LeftHand, position);
            //_animator.SetIKRotation(AvatarIKGoal.LeftHand, _myLeftHand.rotation);
        }
    }
}
