using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public enum TargetPriority { CLOSE, FAR, STRONG, WEAK, RANDOM }

    public class AITurret : Turret
    {
        [Header("AI Targeting")]
        [SerializeField] private TargetType targetType;
        [SerializeField] private TargetPriority targetPriority;

        [Space]
        [SerializeField] private float targetingInterval;
        [SerializeField] private bool useRange;
        [SerializeField] private float attackRange;

        [Space]
        [SerializeField] private bool needLOS;

        [Header("AI Shooting")]
        [SerializeField] private float fireRate;
        [SerializeField] private int damage;

        [Space]
        [SerializeField] private bool useRandomFireRate;
        [SerializeField] private Vector2 fireRateRange;

        [Space]
        [SerializeField] private bool delayFirstAttack;
        [SerializeField] private Vector2 firstAttackDelayRange;

        [Space]
        [Range(0f, 1f)] [SerializeField] private float chanceToHit = 1.0f;
        [SerializeField] private bool useChanceToHit = false;

        [Space]
        [SerializeField] private bool delayHit;
        [SerializeField] private float delayHitAmount;

        [SerializeField] private bool useDelayRange;
        [SerializeField] private Vector2 delayHitAmountRange;

        private bool isAttacking, hasDoneFirstAttack;

        private Target currentTarget;
        private Vector3 currentTargetPos;

        public event EventHandler<EventArgs> OnStartAttackEvent;

        public event EventHandler<Events.Vector3EventArgs> OnHitEvent;

        protected void OnStartAttack() => OnStartAttackEvent?.Invoke(this, EventArgs.Empty);
        protected void OnHit() => OnHitEvent?.Invoke(this, new Events.Vector3EventArgs { i = currentTargetPos });

        public int GetDamage() => damage;

        private void Start()
        {
            InvokeRepeating(nameof(FindTargets), 0f, targetingInterval);
        }

        private void Update()
        {
            if (PauseManager.IsPaused) return;

            if (currentTarget != null) { CalculateRotation(currentTarget.transform.position); CheckTargets(); }
        }

        private void CheckTargets()
        {
            //TODO: Line of sight check, to ensure we cannot shoot over hills.
            if (needLOS && HasLOS(currentTarget) == false) return;

            if (!isAttacking && !currentTarget.IsDead()) 
            {
                if (delayFirstAttack)
                {
                    if (!hasDoneFirstAttack)
                    {
                        hasDoneFirstAttack = true;

                        float f = UnityEngine.Random.Range(firstAttackDelayRange.x, firstAttackDelayRange.y);

                        Invoke(nameof(StartAttack), f); isAttacking = true;
                    }

                    else StartAttack();
                }

                else StartAttack();
            }
        }

        private void FindTargets()
        {
            List<Target> targets;

            switch (targetType)
            {
                case TargetType.Player: targets = GetPlayerTargets(); break;

                case TargetType.Enemy: targets = GetEnemyTargets(); break;

                default: targets = TargetSystem.visibleEnemyTargets; break;
            }

            if (targets == null)
            {
                currentTarget = null;

                return;
            }

            float targetDistance;

            switch (targetPriority)
            {
                case TargetPriority.CLOSE:

                    float nearestDistance = float.MaxValue;

                    //loop through the targets and find the closest one within attack range.
                    foreach (Target target in targets)
                    {
                        if (target.IsDead() || target == GetComponent<Target>()) continue;

                        targetDistance = Vector3.Distance(target.transform.position, transform.position);

                        if (targetDistance < nearestDistance)
                        {
                            nearestDistance = targetDistance;

                            if (useRange && nearestDistance <= attackRange)
                            {
                                currentTarget = target;
                            }

                            else if (useRange && nearestDistance > attackRange)
                            {
                                currentTarget = null;
                            }

                            else if (!useRange) currentTarget = target;
                        }
                    }

                    break;
                case TargetPriority.FAR:

                    float furthestDistance = float.MinValue;

                    // Loop through the targets and find the furthest one within attack range.
                    foreach (Target target in targets)
                    {
                        if (target.IsDead()) continue;

                        targetDistance = Vector3.Distance(target.transform.position, transform.position);

                        if (targetDistance > furthestDistance)
                        {
                            furthestDistance = targetDistance;

                            if (useRange && furthestDistance <= attackRange)
                            {
                                currentTarget = target;
                            }

                            else if (useRange && furthestDistance > attackRange)
                            {
                                currentTarget = null;
                            }

                            else if (!useRange) currentTarget = target;
                        }
                    }

                    break;
                case TargetPriority.STRONG:

                    int highestDamage = int.MinValue;
                    
                    //loop through the targets and find the strongest one, health-wise.
                    foreach (Target target in targets)
                    {
                        if (target.IsDead()) continue;

                        targetDistance = Vector3.Distance(target.transform.position, transform.position);

                        int damage = target.GetMaxHealth();
                    
                        if (damage > highestDamage)
                        {
                            highestDamage = damage;

                            if (useRange && targetDistance <= attackRange)
                            {
                                currentTarget = target;
                            }

                            else if (useRange && targetDistance > attackRange)
                            {
                                currentTarget = null;
                            }

                            else if (!useRange) currentTarget = target;
                        }
                    }

                    break;
                case TargetPriority.WEAK:

                    int lowestDamage = int.MaxValue;
                    
                    //loop through the targets and find the weakest one, health-wise.
                    foreach (Target target in targets)
                    {
                        if (target.IsDead()) continue;

                        targetDistance = Vector3.Distance(target.transform.position, transform.position);

                        int damage = target.GetMaxHealth();
                    
                        if (damage < lowestDamage)
                        {
                            lowestDamage = damage;

                            if (useRange && targetDistance <= attackRange)
                            {
                                currentTarget = target;
                            }

                            else if (useRange && targetDistance > attackRange)
                            {
                                currentTarget = null;
                            }

                            else if (!useRange) currentTarget = target;
                        }
                    }

                    break;

                case TargetPriority.RANDOM:

                    int x = UnityEngine.Random.Range(0, targets.Count);

                    targetDistance = Vector3.Distance(targets[x].transform.position, transform.position);

                    if (useRange && targetDistance <= attackRange)
                    {
                        currentTarget = targets[x];
                    }

                    else if (useRange && targetDistance > attackRange)
                    {
                        currentTarget = null;
                    }

                    else if (!useRange) currentTarget = targets[x];

                    currentTarget = targets[x];

                    break;

                default:
                    break;
            }

            if (currentTarget) currentTargetPos = currentTarget.transform.position;
        }

        private List<Target> GetPlayerTargets()
        {
            //if the enemy isn't even on the screen (according to the target system), then don't let them fire at the player!
            foreach (Target target in TargetSystem.visibleEnemyTargets)
            {
                if (target == transform.parent.GetComponentInChildren<Target>()) return TargetSystem.visiblePlayerTargets;
            }

            return null;
        }

        private List<Target> GetEnemyTargets()
        {
            //if the player isn't even on the screen (according to the target system), then don't let them fire at the enemy!
            if (!TargetSystem.visiblePlayerTargets.Contains(transform.parent.GetComponentInChildren<Target>()))
            {
                currentTarget = null;
            
                return null;
            }

            List<Target> targets = new List<Target>();

            //if the player has selected a target and we're in range, just focus on that
            if (TargetSystem.playerSelectedTarget != null && targetType == TargetType.Enemy)
            {
                float distance = Vector3.Distance(TargetSystem.playerSelectedTarget.transform.position, transform.position);

                if (distance <= attackRange)
                {
                    targets.Add(TargetSystem.playerSelectedTarget);

                    return targets;
                }

                else return TargetSystem.visibleEnemyTargets;
            }

            else return TargetSystem.visibleEnemyTargets;
        }

        private bool HasLOS(Target target)
        {
            if (Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out RaycastHit hit, Mathf.Infinity))
            {
                return hit.transform == target.transform;
            }

            else return false;
        }

        private void StartAttack()
        {
            isAttacking = true;

            OnStartAttack();

            Attack();

            //invoke end shoot after our rate of fire
            if (useRandomFireRate)
            {
                float fireRate = UnityEngine.Random.Range(fireRateRange.x, fireRateRange.y);

                Invoke(nameof(EndAttack), 1 / fireRate);
            }

            else Invoke(nameof(EndAttack), 1 / fireRate);
        }

        private void Attack()
        {
            //call the hit function
            if (delayHit) 
            {
                //we cancel and then reinvoke the find targets so we have time to 
                CancelInvoke(nameof(FindTargets));

                if (useDelayRange)
                {
                    float delayHitAmount = UnityEngine.Random.Range(delayHitAmountRange.x, delayHitAmountRange.y);

                    Invoke(nameof(Hit), delayHitAmount);

                    InvokeRepeating(nameof(FindTargets), delayHitAmount, targetingInterval);
                }

                else
                {
                    Invoke(nameof(Hit), delayHitAmount);

                    InvokeRepeating(nameof(FindTargets), delayHitAmount, targetingInterval);
                }
            }

            else if (!delayHit) Hit();
        }

        private void EndAttack() => isAttacking = false;

        private void Hit()
        {
            if (currentTarget == null)
            {
                OnHit(); return;
            }

            if (useChanceToHit)
            {
                float x = UnityEngine.Random.Range(0f, 1f);

                if (x < chanceToHit)
                {
                    IDamageable damageable = currentTarget.GetComponent<IDamageable>();

                    if (damageable != null) damageable.Damage(damage);
                }
            }

            else
            {
                IDamageable damageable = currentTarget.GetComponent<IDamageable>();

                if (damageable != null) damageable.Damage(damage);
            }

            OnHit();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.green;

            if (currentTarget != null)
            {
                Gizmos.DrawRay(transform.position, (currentTarget.transform.position - transform.position));
            }
        }

        public override void Pause() 
        {
            CancelInvoke(nameof(FindTargets));
        }

        public override void UnPause()
        {
            InvokeRepeating(nameof(FindTargets), 0f, targetingInterval);
        }
    }
}