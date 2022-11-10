/*
 * NpcStatePattern -
 * Created by : Allan N. Murillo
 * Last Edited : 4/9/2020
 */

using UnityEngine;
using UnityEngine.AI;
using ANM.FPS.Npc.States;
using System.Collections;
using ANM.FPS.Items.Guns;

namespace ANM.FPS.Npc
{
    public class NpcStatePattern : MonoBehaviour
    {
        #region Variables
        [Header("Dependencies")] 
        public Transform head;

        [Header("Attack Properties")] public GameObject gun;
        public float meleeAttackDmg = 10f;
        public float gunAttackDmg = 5f;
        public float meleeAttackRate = 2f;
        public float gunAttackRate = 1.5f;
        public float meleeAttackRange = 5f;
        public float gunAttackRange = 30f;
        public float rangedAttackSpread = 0.1f;
        public bool hasGun;
        public bool hasMeleeAttack;
        public bool hasBeenHit;
        public bool isFleeing;
        public float recoverRate = 1.25f;
        public bool isAttacking;

        [Header("Detection Properties")] 
        public Transform followTarget;
        public LayerMask sightLayers;
        public float sightRange = 30f;
        public float detectBehindRange = 5f;
        public float offset = 0.4f;
        public int requiredDetectionCount = 15;
        [HideInInspector] public float checkRate = 0f;
        [HideInInspector] public float capturedRate = 0f;
        public float patrolRate = 1f;
        public float fleeRate = 0.5f;
        public float alertRate = 0.1f;
        public float investigateRate = 0.333f;
        public float chaseRate = 0.333f;

        [Header("Layers & Tags")] 
        public LayerMask enemyLayers;
        public string[] enemyTags;
        public LayerMask friendlyLayers;
        public string[] friendlyTags;

        [Header("Pathfinding")] 
        public bool canMove;
        public Transform[] waypoints;
        [HideInInspector] public Vector3 wanderDestination;
        [HideInInspector] public Vector3 lookAt;

        [Header("Debugging")] 
        [SerializeField] private MeshRenderer meshRendererFlag;
        [SerializeField] private Color _debugColor;
        [HideInInspector] public Color capturedColor;

        //  References         
        [HideInInspector] public Transform root;
        [HideInInspector] public NavMeshAgent myNavMeshAgent;
        [HideInInspector] public Collider[] enemyCollidersInRange;

        //  State Behaviours
        [HideInInspector] public INpcState currentState;
        [HideInInspector] public INpcState capturedState;
        [HideInInspector] public NpcFleeState fleeState;
        [HideInInspector] public NpcAlertState alertState;
        [HideInInspector] public NpcChaseState chaseState;
        [HideInInspector] public NpcPatrolState patrolState;
        [HideInInspector] public NpcGetHitState getHitState;
        [HideInInspector] public NpcFollowState followState;
        [HideInInspector] public NpcInvestigateState investigateState;
        [HideInInspector] public NpcMeleeAttackState meleeAttackState;
        [HideInInspector] public NpcRangedAttackState rangeAttackState;
        
        private NpcMaster _npcMaster;
        
        private float _nextScan;
        private float _nextUpdate;
        private const float ScanInterval = 1f;
        private float _nextAnimationCheck;
        private const float AnimCheckInterval = .01f;
        private const float Speed = 0.01f;

        private MaterialPropertyBlock mpb;

        public MaterialPropertyBlock Mpb
        {
            get
            {
                if (mpb == null)
                    mpb = new MaterialPropertyBlock();
                return mpb;
            }
        }

        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        #endregion
        
        //    TODO : use a velocity variable for moving NPCs
        //    TODO : set walking animation if velocity is not Zero
        //    TODO : pass velocity to animation parameters ?.
        
        private void Awake()
        {
            Initialize();
            _npcMaster.EventNpcDie += ActivateDeath;
            _npcMaster.EventNpcTakeDmg += ActivateGetHitState;
            _npcMaster.EventNpcCriticalHealth += ActivateFleeState;
        }

        private void Start()
        {
            ActivatePatrolState();
        }

        private void Initialize()
        {
            canMove = true;
            SetupStateRefs();
            root = transform;
            _npcMaster = GetComponent<NpcMaster>();
            myNavMeshAgent = GetComponent<NavMeshAgent>();
            if (gun == null)
            {
                hasGun = false;
                return;
            }
            gunAttackRate = gun.GetComponent<GunStandardInput>().GetAttackRate();
            gunAttackDmg = gun.GetComponent<GunApplyDamage>().GetAttackDamage();
            gunAttackRange = gun.GetComponent<GunShoot>().GetAttackRange();
            hasGun = true;
        }
        
        private void SetupStateRefs()
        {
            patrolState = new NpcPatrolState(this);
            alertState = new NpcAlertState(this);
            chaseState = new NpcChaseState(this);
            fleeState = new NpcFleeState(this);
            followState = new NpcFollowState(this);
            getHitState = new NpcGetHitState(this);
            investigateState = new NpcInvestigateState(this);
            meleeAttackState = new NpcMeleeAttackState(this);
            rangeAttackState = new NpcRangedAttackState(this);
        }
        
        private void OnDisable()
        {
            _npcMaster.EventNpcCriticalHealth -= ActivateFleeState;
            _npcMaster.EventNpcTakeDmg -= ActivateGetHitState;
            _npcMaster.EventNpcDie -= ActivateDeath;
            StopAllCoroutines();
        }

        private void Update()
        {
            UpdateState();
        }

        #region State Pattern

        /// <summary>
        /// Update Current NPC State
        /// Check rate is updated based on current state
        /// </summary>
        private void UpdateState()
        {
            if (Time.time > _nextAnimationCheck)
            {
                if (lookAt != Vector3.zero)
                {
                    var dirToLook = lookAt - root.position;
                    root.rotation = Quaternion.RotateTowards(root.rotation,
                        Quaternion.LookRotation(dirToLook), Time.timeScale * Speed);
                }
                canMove = ReachedDestination();
                _nextAnimationCheck = Time.time + AnimCheckInterval;
            }

            if (Time.time > _nextScan)
            {
                EnemyScan();
                _nextScan = Time.time + ScanInterval;
            }

            if (!(Time.time > _nextUpdate)) return;
            currentState.UpdateState();
            _nextUpdate = Time.time + checkRate;
        }

        private void EnemyScan()
        {
            enemyCollidersInRange = Physics.OverlapSphere(root.position, detectBehindRange, enemyLayers);
            if (enemyCollidersInRange.Length == 0)
                enemyCollidersInRange = Physics.OverlapSphere(root.position, sightRange, enemyLayers);
        }

        private void ActivateFleeState()
        {
            if (currentState == fleeState) return;

            myNavMeshAgent.speed = 4f;

            if (currentState == getHitState)
            {
                // Debug.Log(name + " will attempt to flee after get hit recovery");
                StoreState(fleeRate, fleeState, Color.gray);
                return;
            }

            lookAt = Vector3.zero;
            checkRate = fleeRate;
            currentState = fleeState;
            SetDebugColor(Color.black);
            StopWalking();
            // Debug.Log(name + "'s state pattern is forcing flee");
        }

        private void ActivatePatrolState()
        {
            checkRate = patrolRate;
            myNavMeshAgent.speed = 2f;
            currentState = patrolState;
            capturedColor = Color.green;
            SetDebugColor(capturedColor);
            //Debug.Log(name + "'s state pattern is forcing patrol");
        }

        private void ActivateGetHitState(int dmg)
        {
            //Debug.Log(name + " has lost " + dmg + " health points called from TakeDamageEvent");

            if (hasBeenHit) { return; }
            
            if (dmg <= 25)
            {
                //  Non-critical hit
                if (currentState != patrolState) return;
                SetDebugColor(Color.cyan);
                checkRate = investigateRate;
                currentState = investigateState;
                //Debug.Log(name + "'s state pattern is forcing investigate");
            }
            else
            {
                //  Critical Hit
                StopAllCoroutines();
                
                if (currentState != getHitState || capturedState != investigateState)
                {
                    capturedRate = checkRate;
                    capturedState = currentState;
                    capturedColor = _debugColor;
                }

                if (hasGun) gun.SetActive(false);

                SetDebugColor(Color.gray);
                currentState = getHitState;
                checkRate = recoverRate * 0.5f;
                StopWalking();
                _npcMaster.CallEventNpcGetHit();
                StartCoroutine(RecoverFromGetHit());
            }
        }

        private IEnumerator RecoverFromGetHit()
        {
            IsPlayingAttackAnimation = false;
            hasBeenHit = true;
            isAttacking = false;
            lookAt = Vector3.zero;
            yield return new WaitForSeconds(recoverRate);
            _npcMaster.CallEventNpcRecovered();
            hasBeenHit = false;
            if (hasGun) gun.SetActive(true);
            SetDebugColor(capturedColor);
            checkRate = capturedRate;
            currentState = capturedState;
            lookAt = Vector3.zero;
            // Debug.Log(name + "'s state pattern has recovered from getHit state, applying captured state");
        }
        
        private void ActivateDeath()
        {
            root.tag = "Untagged";
            SetDebugColor(Color.clear);
        }

        public bool IsPlayingAttackAnimation
        {
            get => isAttacking;
            set => isAttacking = value;
        }

        public Transform AttackTarget { get; set; }

        public void Distract(Vector3 distractionPos)
        {
            Debug.Log("NPCStatePattern::Distract was triggered");
            // locationOfInterest = distractionPos;
            // if (currentState == patrolState)
            // {
            //     checkRate = alertRate;
            //     currentState = alertState;
            //     MoveTo(locationOfInterest);
            //     // Debug.Log(name + "'s state pattern is being distracted to alert state, checkRate = " + checkRate);
            // }
        }

        #endregion
        
        private void ApplyColor()
        {
            Mpb.SetColor(EmissionColor, _debugColor * 5f);
            meshRendererFlag?.SetPropertyBlock(Mpb);
        }
        
        private void SetDebugColor(Color color)
        {
            _debugColor = color;
            ApplyColor();
        }

        public Color GetDebugColor() => _debugColor;
        
        #region Movement
        
        public bool MoveTo(Vector3 targetPos)
        {
            targetPos.y += myNavMeshAgent.height;
            if (!NavMesh.SamplePosition(targetPos, out var hit,
                2f, NavMesh.AllAreas)) return false;
            
            Debug.DrawLine(root.position, hit.position, Color.green, 2f);
            myNavMeshAgent.SetDestination(hit.position);
            KeepWalking();
            return true;
        }
        
        public bool MoveToOverride(Vector3 destination)
        {
            myNavMeshAgent.ResetPath();
            return MoveTo(destination);
        }
        
        public void MoveToNoSample(Vector3 targetPos)
        {
            Debug.DrawLine(root.position, targetPos, Color.green, 2f);
            myNavMeshAgent.SetDestination(targetPos);
            KeepWalking();
        }
        
        public void MoveTowardsNoSample(Vector3 targetLocation, float percentage, float maxDistance)
        {
            var position = root.position;
            var pointOfInterest = targetLocation - position;

            pointOfInterest.Normalize();
            pointOfInterest = position + maxDistance * percentage * pointOfInterest;

            MoveToNoSample(pointOfInterest);
        }

        private void KeepWalking()
        {
            myNavMeshAgent.isStopped = false;
            _npcMaster.CallEventNpcWalk();
        }
        
        private void StopWalking()
        {
            _npcMaster.CallEventNpcIdle();
            if (!myNavMeshAgent.isOnNavMesh) return;
            myNavMeshAgent.velocity = Vector3.zero;
            myNavMeshAgent.isStopped = true;
        }

        private bool ReachedDestination()
        {
            if (myNavMeshAgent.velocity != Vector3.zero) return false;
            if (!canMove) StopWalking();
            return true;
        }

        #endregion

        #region Animation Triggers

        /// <summary>
        /// MUST be called by Npc Melee Attack Animation.
        /// Deals Melee Damage to Attack Target
        /// </summary>
        public void OnEnemyMeleeAttack()
        {
            // Debug.Log("NPCStatePattern::OnEnemyMeleeAttack was triggered from animation!");
            isAttacking = false;
            if (AttackTarget == null) return;
            if (!(Vector3.Distance(root.position, AttackTarget.position) <= meleeAttackRange)) return;
            Vector3 toOther = AttackTarget.position - root.position;
            if (!(Vector3.Dot(toOther, root.forward) > 0.5f)) return;
            if (AttackTarget.root.GetComponent<NpcMaster>() == null) return;
            // attackTarget.root.GetComponent<NpcMaster>().CallEventNpcDeductHealth((int)meleeAttackDmg);
            AttackTarget.SendMessage("CallEventNpcDeductHealth", meleeAttackDmg,
                SendMessageOptions.DontRequireReceiver);
            AttackTarget.SendMessage("ProcessDamage", meleeAttackDmg, SendMessageOptions.DontRequireReceiver);
            AttackTarget.SendMessage("SetMyAttacker", root.root, SendMessageOptions.DontRequireReceiver);
            //  TODO : PLAYER IS CURRENTLY INVINCIBLE
            // if (myNpcAttackTarget.root.GetComponent<PlayerMaster>() != null)
            // {
            //     myNpcAttackTarget.root.GetComponent<PlayerMaster>().CallEventPlayerHpDeduction((int)meleeAttackDmg);
            //     return;
            // }
        }

        /// <summary>
        /// MUST be called by Npc Ranged Attack Animation.
        /// Deals Ranged Damage to Attack Target
        /// </summary>
        public void OnEnemyRangedAttack()
        {
            Debug.Log("NPCStatePattern::OnEnemyRangedAttack was triggered from animation");
            isAttacking = false;
            if (AttackTarget == null) return;
            if (!(Vector3.Distance(root.position, AttackTarget.position) <= gunAttackRange)) return;
            Vector3 toOther = AttackTarget.position - root.position;
            if (!(Vector3.Dot(toOther, root.forward) > 0.5f)) return;
            if (AttackTarget.root.GetComponent<NpcMaster>() == null) return;
            Debug.Log(AttackTarget.root.name + " has taken gun damage : " + gunAttackDmg);
            AttackTarget.root.GetComponent<NpcMaster>().CallEventNpcDeductHealth((int) gunAttackDmg);
            // if (myNpcAttackTarget.root.GetComponent<PlayerMaster>() != null)
            // {
            //     myNpcAttackTarget.root.GetComponent<PlayerMaster>().CallEventPlayerHpDeduction((int)rangedAttackDmg);
            //     return;
            // }
        }

        #endregion

        public void ActivatePatrol()
        {
            checkRate = patrolRate;
            myNavMeshAgent.speed = 2f;
            currentState = patrolState;
            SetDebugColor(Color.green);
        }
        
        public void ActivateAlert()
        {
            checkRate = alertRate;
            myNavMeshAgent.speed = 3f;
            currentState = alertState;
            SetDebugColor(Color.yellow);
        }
        
        public void ActivateInvestigate(Transform target)
        {
            SetDebugColor(Color.cyan);
            myNavMeshAgent.speed = 4f;
            checkRate = investigateRate;
            currentState = investigateState;
            AttackTarget = target;
        }
        
        public void ActivateChase()
        {
            checkRate = chaseRate;
            myNavMeshAgent.speed = 4f;
            currentState = chaseState;
            SetDebugColor(Color.blue);
        }
        
        public void ActivateMelee()
        {
            if(hasGun) gun.SetActive(false);
            checkRate = meleeAttackRate * 0.5f;
            currentState = meleeAttackState;
            myNavMeshAgent.speed = 0f;
            SetDebugColor(Color.red);
        }
        
        public void ActivateRanged()
        {
            gun.SetActive(true);
            myNavMeshAgent.speed = 1f;
            SetDebugColor(Color.magenta);
            currentState = rangeAttackState;
            checkRate = gunAttackRate * 0.5f;
        }
        
        public void ActivateStoredState()
        {
            checkRate = capturedRate;
            currentState = capturedState;
            SetDebugColor(capturedColor);
        }
        
        public void StoreState(float rate, INpcState state, Color color)
        {
            capturedRate = rate;
            capturedState = state;
            capturedColor = color;
        }

        public NpcMaster GetMaster()
        {
            return _npcMaster;
        }
    }
}
