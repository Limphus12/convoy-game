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
        [Range(0f, 1f)] [SerializeField] private float chanceToHit = 1.0f;
        [SerializeField] private bool useChanceToHit = false;

        private bool isAttacking;

        private Target currentTarget;

        public event EventHandler<EventArgs> OnStartAttackEvent;

        protected void OnStartAttack() => OnStartAttackEvent?.Invoke(this, EventArgs.Empty);

        public int GetDamage() => damage;

        private void Start()
        {
            InvokeRepeating(nameof(FindTargets), 0f, targetingInterval);
        }

        private void Update()
        {
            if (PauseManager.IsPaused) return;

            if (currentTarget != null) CalculateRotation(currentTarget.transform.position);

            CheckTargets();
        }

        private void CheckTargets()
        {
            if (currentTarget == null) return;

            //TODO: Line of sight check, to ensure we cannot shoot over hills.
            if (needLOS && HasLOS(currentTarget) == false) return;

            if (!isAttacking && !currentTarget.IsDead()) StartAttack();
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

            if (targets == null) return;

            switch (targetPriority)
            {
                case TargetPriority.CLOSE:

                    float nearestDistance = float.MaxValue;

                    //loop through the targets and find the closest one within attack range.
                    foreach (Target target in targets)
                    {
                        if (target.IsDead() || target == GetComponent<Target>()) continue;

                        float distance = Vector3.Distance(target.transform.position, transform.position);

                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;

                            if (useRange && nearestDistance <= attackRange)
                            {
                                currentTarget = target;
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

                        float distance = Vector3.Distance(target.transform.position, transform.position);

                        if (distance > furthestDistance)
                        {
                            furthestDistance = distance;

                            if (useRange && distance <= attackRange)
                            {
                                currentTarget = target;
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
                    
                        int damage = target.GetMaxHealth();
                    
                        if (damage > highestDamage)
                        {
                            highestDamage = damage;
                            currentTarget = target;
                        }
                    }

                    break;
                case TargetPriority.WEAK:

                    int lowestDamage = int.MaxValue;
                    
                    //loop through the targets and find the weakest one, health-wise.
                    foreach (Target target in targets)
                    {
                        if (target.IsDead()) continue;
                    
                        int damage = target.GetMaxHealth();
                    
                        if (damage < lowestDamage)
                        {
                            lowestDamage = damage;
                            currentTarget = target;
                        }
                    }

                    break;

                case TargetPriority.RANDOM:

                    int x = UnityEngine.Random.Range(0, targets.Count);

                    currentTarget = targets[x];

                    break;

                default:
                    break;
            }
        }

        private List<Target> GetPlayerTargets()
        {
            //if the enemy isn't even on the screen (according to the target system), then don't let them fire at the player!
            if (!TargetSystem.visibleEnemyTargets.Contains(transform.GetComponent<Target>()))
            {
                return null;
            }
            
            else return TargetSystem.visiblePlayerTargets;
        }

        private List<Target> GetEnemyTargets()
        {
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
            Invoke(nameof(EndAttack), 1 / fireRate);
        }

        private void Attack()
        {
            //call the hit function
            Hit();
        }

        private void EndAttack() => isAttacking = false;

        private void Hit()
        {
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
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        public override void Pause() 
        {
            CancelInvoke(nameof(FindTargets));
        }

        public override void Unpause()
        {
            InvokeRepeating(nameof(FindTargets), 0f, targetingInterval);
        }
    }
}