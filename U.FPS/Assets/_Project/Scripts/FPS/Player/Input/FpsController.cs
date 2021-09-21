/*
 * FPSController -
 * Created by : Allan N. Murillo
 * Last Edited : 3/26/2020
 */

using UnityEngine;
using ANM.Scriptable;
using ANM.SpellSystem;
using ANM.FPS.Items.Guns;
using ANM.FPS.Player.Input.Ui;
using InputPhase = UnityEngine.InputSystem.InputActionPhase;

namespace ANM.FPS.Player.Input
{
    public class FpsController : MonoBehaviour
    {
        [Header("Movement")] 
        [SerializeField] [Range(10f, 1f)] private float walkSpeed = 6f;
        [SerializeField] [Range(10f, 1f)] private float strafingSpeed = 5f;
        [SerializeField] [Range(100f, 500f)] private float sprintSpeed = 300f;
        
        [Header("Rotation")] 
        [SerializeField] private Transform camTransform;
        [SerializeField] [Range(100f, 50f)] private float rotateSpeed = 70f;
        [SerializeField] [Range(-85f, -35f)] private float minCamAngle = -45f;
        [SerializeField] [Range(35f, 85f)] private float maxCamAngle = 45f;

        [Header("Jump")] 
        [SerializeField] [Range(2f, 20f)] private float jumpForce = 10f;
        [SerializeField] [Range(2f, 3f)] private float fallMultiplier = 2.36f;
        [SerializeField] [Range(1.6f, 2.5f)] private float slowFallMultiplier = 1.8f;

        private CharacterController _myCharacterControl;
        private Controller _myControllerRef;
        private GunStandardInput _myGunInput;
        private Transform _myTransform;
        private RadialMenu _spellRadialMenu;
        private Rigidbody _rigidbody;
        private SpellUI _spellUi;

        private TimeManipulation _timeControl;
        private Coroutine _timeRoutine;

        [SerializeField] private bool _autoGun;
        [SerializeField] private bool _isPaused;
        [SerializeField] private bool _canSprint;
        [SerializeField] private bool _isJumping;
        [SerializeField] private bool _jumpInput;
        [SerializeField] private bool _isGrounded;
        [SerializeField] private bool _sprintInput;
        [SerializeField] private bool _isTripleJump;
        [SerializeField] private bool _isDoubleJump;
        [SerializeField] private bool _useSpellInput;
        [SerializeField] private bool _isCastingSpell;
        [SerializeField] private bool _selectSpellInput;

        private float _delta;
        private float _gravity;
        private float _camAngle;
        private float _jumpDelay;
        private float _timeDelta;
        private float _fixedDelta;
        private float _verticalInput;

        [SerializeField] private Vector2 _moveInput;
        [SerializeField] private Vector2 _rotationInput;
        [SerializeField] private Vector3 _movementVelocity;

        //    TODO : implement crouch input and swap character material to dark one to depict stealth

        private void Awake()
        {
            _isJumping = true;
            _myTransform = transform;
            _gravity = Physics.gravity.y;
            _spellUi = GetComponent<SpellUI>();
            _rigidbody = GetComponent<Rigidbody>();
            _spellRadialMenu = GetComponent<RadialMenu>();
            _myCharacterControl = GetComponent<CharacterController>();
            if (camTransform == null) Debug.LogError("No Camera Found");
            if (_rigidbody == null) Debug.LogError("No Rigidbody Found");
            _timeControl = Resources.Load<TimeManipulation>("TimeControl");
            InputSetup();
        }

        private void OnEnable()
        {
            GetComponent<PlayerMaster>().PlayerDieEvent += OnPlayerDeath;
        }

        private void OnDisable()
        {
            GetComponent<PlayerMaster>().PlayerDieEvent -= OnPlayerDeath;
        }

        private void Update()
        {
            if (_isPaused) return;
            _delta = Time.deltaTime;
            RadialSpellMenuInput();
            SprintInput();
            SpellCastInput();
            VerticalInput();
            GunInput();
        }

        private void SpellCastInput()
        {
            if (!_useSpellInput) return;
            _spellRadialMenu.CastSpell();
        }

        private void RadialSpellMenuInput()
        {
            if (!_selectSpellInput) return;
            _spellRadialMenu.CheckRadialDegrees(_rotationInput);
        }

        private void SprintInput()
        {
            if (!_sprintInput) return;
            if (!_canSprint || _selectSpellInput) return;
            var moveDir = _myTransform.forward * (sprintSpeed * _delta);
            moveDir = _moveInput.y < 0 ? moveDir * -1 : moveDir;
            _movementVelocity += moveDir;
        }

        private void VerticalInput()
        {
            GroundCheck();

            if (_isGrounded)
            {
                HasLanded();
                AssertGravity();
            }
            else
            {
                if (_isJumping && _jumpInput && _jumpDelay <= Time.time)
                {
                    if (_isDoubleJump && !_isTripleJump) TripleJump();
                    else if (!_isDoubleJump) DoubleJump();
                }

                ApplyGravity(_isDoubleJump ? fallMultiplier : slowFallMultiplier);
            }

            if (_jumpInput && !_isJumping) JumpMechanic();
            else _jumpInput = false;

            ApplyVerticalMovement();
        }

        //  TODO : use different input event
        private void GunInput()
        {
            if (!_autoGun) return;
            var current = _myControllerRef.GetInput().Player.Shoot.phase;
            if (current == InputPhase.Waiting || _selectSpellInput) return;
            //    Gun Auto fire
            _myGunInput.FireInput();
        }

        private void FixedUpdate()
        {
            if (_isPaused) return;
            _fixedDelta = Time.fixedDeltaTime;
            Movement();
            Rotation();
            OutOfBoundsCheck();
        }

        private void Movement()
        {
            _movementVelocity = _myTransform.forward * (_moveInput.y * walkSpeed * _fixedDelta);
            _movementVelocity += _myTransform.right * (_moveInput.x * strafingSpeed * _fixedDelta);
            _myCharacterControl.Move(_movementVelocity);
        }

        private void Rotation()
        {
            if (_selectSpellInput) return;

            var speedToUse = _isCastingSpell ? rotateSpeed * 0.25f : rotateSpeed;
            _myTransform.Rotate(new Vector3(0, _rotationInput.x, 0) * (speedToUse * _fixedDelta));
            _camAngle += _rotationInput.y * speedToUse * _fixedDelta;

            _camAngle = Mathf.Clamp(_camAngle, minCamAngle, maxCamAngle);
            camTransform.localEulerAngles = new Vector3(-_camAngle, 0, 0);
        }

        private void OutOfBoundsCheck()
        {
            if (_myTransform.position.y < -50f)
            {
                GetComponent<PlayerMaster>().EventCallPlayerDie();
            }
        }

        #region Helper Methods

        private void InputSetup()
        {
            _myControllerRef = Resources.Load<Controller>("PlayerControls");
            if (_myControllerRef == null)
            {
                Debug.LogError("PlayerControls does not exist in Resources Folder!");
                return;
            }

            _myControllerRef.OnMovementEvent += move => _moveInput = move;
            _myControllerRef.OnCameraLookEvent += look => _rotationInput = look;

            _myControllerRef.OnJumpEvent += () => _jumpInput = true;
            _myControllerRef.OnSprintEvent += isPressed => _sprintInput = isPressed;
            _myControllerRef.OnInteractEvent += isPressed => _spellUi.casting = isPressed;
            _myControllerRef.OnCastSpellEvent += isPressed => _isCastingSpell = isPressed;

            _myControllerRef.OnSelectSpellEvent += phase =>
            {
                switch (phase)
                {
                    case InputPhase.Started:
                        TimeScaleHelper(0.01f, 0.21f);
                        break;
                    case InputPhase.Performed:
                        _selectSpellInput = true;
                        break;
                    case InputPhase.Canceled:
                        StopCoroutine(_timeRoutine);
                        _timeControl.Reset();
                        _selectSpellInput = false;
                        break;
                }
            };

            _myControllerRef.OnLaunchSpellEvent += isPressed =>
            {
                if (isPressed)
                {
                    if (Mathf.Approximately(_timeControl.CurrentTimeScale(), 1f)) _useSpellInput = true;
                }
                else
                {
                    _useSpellInput = false;
                }
            };
        }

        private void TimeScaleHelper(float target, float time)
        {
            if (Mathf.Approximately(_timeControl.CurrentTarget(), target)) return;
            if (_timeRoutine != null) StopCoroutine(_timeRoutine);
            _timeRoutine = StartCoroutine(_timeControl.IncrementTowardsTargetScale(target, time));
        }

        private void GroundCheck()
        {
            _isGrounded = _myCharacterControl.isGrounded;
        }

        private void AssertGravity()
        {
            _verticalInput = _gravity * _delta;
        }

        private void ApplyGravity(float multiplier)
        {
            _verticalInput += _gravity * (multiplier - 1) * _delta;
        }

        private void JumpMechanic()
        {
            _isJumping = true;
            _verticalInput = jumpForce;
            ResetJumpDelta();
        }

        private void DoubleJump()
        {
            _canSprint = false;
            _isDoubleJump = true;
            _verticalInput = jumpForce * 0.85f;
            ResetJumpDelta();
        }

        private void TripleJump()
        {
            _canSprint = false;
            _isTripleJump = true;
            _verticalInput = jumpForce * 0.65f;
            ResetJumpDelta();
        }

        private void ResetJumpDelta()
        {
            _jumpInput = false;
            _jumpDelay = Time.time + 0.2f;
        }

        private void HasLanded()
        {
            if (!_isJumping) return;
            _canSprint = true;
            _isJumping = false;
            _isDoubleJump = false;
            _isTripleJump = false;

            var landVel = _movementVelocity.y;
            if (landVel <= -20f && landVel > -30f)
                GetComponent<PlayerMaster>().EventCallPlayerHpDeduction(2f);
            else if (landVel <= -30f)
            {
                var dmg = .5f * -landVel;
                GetComponent<PlayerMaster>().EventCallPlayerHpDeduction(dmg);
            }
        }

        private void ApplyVerticalMovement()
        {
            _movementVelocity.y = _verticalInput;
            _myCharacterControl.Move(_movementVelocity * _delta);
        }

        private void Respawn()
        {
            _myTransform.position = new Vector3(-25, 1f, -25);
        }

        #endregion

        public void AssignGunInput(GunStandardInput gunInput = null, bool isAuto = false)
        {
            if (_myGunInput != null)
            {
                var oldGunActions = _myControllerRef.GetInput().Player;
                oldGunActions.Shoot.Disable();
                oldGunActions.Reload.Disable();
                oldGunActions.BurstToggle.Disable();
            }

            _autoGun = false;
            _myGunInput = gunInput;
            if (_myGunInput == null) return;

            var newGunActions = _myControllerRef.GetInput().Player;
            if (!isAuto)
            {
                newGunActions.Shoot.started += context => { _myGunInput.FireInput(); };
            }
            else _autoGun = true;

            newGunActions.Reload.performed += context => { _myGunInput.ReloadInput(); };
            newGunActions.BurstToggle.performed += context => { _myGunInput.BurstFireToggleInput(); };

            newGunActions.BurstToggle.Enable();
            newGunActions.Reload.Enable();
            newGunActions.Shoot.Enable();
        }

        public void OnPause()
        {
            _isPaused = true;
        }

        public void OnResume()
        {
            _isPaused = false;
        }

        public void OnPlayerDeath()
        {
            //    TODO : Kill Player Animation
            Respawn();
        }
    }
}
