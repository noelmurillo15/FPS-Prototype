/*
 * GunDynamicCrosshair -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    public class GunDynamicCrosshair : MonoBehaviour
    {
        [SerializeField] private Transform canvasDynamicCrosshair = null;
        [SerializeField] private Animator crosshairAnimator = null;
        [SerializeField] private Transform playerTransform = null;
        [SerializeField] private Transform weaponCamera = null;

        private GunMaster _gunMaster;

        private float _playerSpeed;
        private float _nextCaptureTime;
        private const float CaptureInterval = 0.5f;
        private Vector3 _lastPosition;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private const string WeaponCameraName = "WeaponCamera";
        
        //    TODO :  delete this script
        
        private void Start()
        {
            // if (transform.root.CompareTag("Player"))
            // {
            //     Initialize();
            // }
            // else
            // {
            //     enabled = false;
            // }

            enabled = false;
            // Debug.Log("GunDynamicCrossHair is being disabled for reasons...");
        }

        private void Update()
        {
            // CapturePlayerSpeed();
            // ApplySpeedToAnimation();
        }

        private void Initialize()
        {
            Debug.Log("GunDynamicCrosshair::Initialize()");
            _gunMaster = GetComponent<GunMaster>();
            playerTransform = global::FPS.GameReferences.myPlayer.transform;
            FindWeaponCamera(playerTransform);
            SetCameraOnDynamicCrosshairCanvas();
            SetPlaneDistanceOnDynamicCrosshairCanvas();
        }

        private void CapturePlayerSpeed()
        {
            if (!(Time.time > _nextCaptureTime)) return;
            Debug.Log("GunDynamicCrosshair::CapturePlayerSpeed()");
            _nextCaptureTime = Time.time + CaptureInterval;
            _playerSpeed = (playerTransform.position - _lastPosition).magnitude / CaptureInterval;
            _lastPosition = playerTransform.position;
            _gunMaster.CallEventSpeedCaptured(_playerSpeed);
        }

        private void ApplySpeedToAnimation()
        {
            Debug.Log("GunDynamicCrosshair::ApplySpeedToAnimation()");
            if (crosshairAnimator != null)
            {
                crosshairAnimator.SetFloat(Speed, _playerSpeed);
            }
        }

        private void FindWeaponCamera(Transform transformToSearchThrough)
        {
            Debug.Log("GunDynamicCrosshair::FindWeaponCamera()");
            if (transformToSearchThrough == null) return;
            if (transformToSearchThrough.name == WeaponCameraName)
            {
                weaponCamera = transformToSearchThrough;
                return;
            }

            for (int i = 0; i < transformToSearchThrough.childCount; i++)
            {
                FindWeaponCamera(transformToSearchThrough.GetChild(i));
            }
        }

        private void SetCameraOnDynamicCrosshairCanvas()
        {
            Debug.Log("GunDynamicCrosshair::SetCameraOnDynamicCrosshairCanvas()");
            if (canvasDynamicCrosshair == null || weaponCamera == null) return;
            canvasDynamicCrosshair.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            canvasDynamicCrosshair.GetComponent<Canvas>().worldCamera = weaponCamera.GetComponent<Camera>();
        }

        private void SetPlaneDistanceOnDynamicCrosshairCanvas()
        {
            Debug.Log("GunDynamicCrosshair::SetPlaneDistanceOnDynamicCrosshairCanvas()");
            if (canvasDynamicCrosshair != null)
            {
                canvasDynamicCrosshair.GetComponent<Canvas>().planeDistance = 1;
            }
        }

        //  Crosshair Camera Raycasts don't line up with GunShoot Raycast
        // void OnDrawGizmos()
        // {
        //     Debug.DrawRay(weaponCamera.position, weaponCamera.forward * 100);
        // }
    }
}
