using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.convoy
{
    public enum TurretRotationMode { Instant, Lerped, Slerped, Fixed }

    public class Turret : MonoBehaviour, IPauseable
    {
        [Header("Turret Movement")]
        [SerializeField] private TurretRotationMode rotationMode;

        [Tooltip("Turret Speed and Barrel Speed only apply to Lerped, Slerped and Fixed Rotation Modes")]
        [SerializeField] private float turretSpeed = 5f, barrelSpeed = 5f;

        [Space]
        [SerializeField] private Transform turretPivot;
        [SerializeField] private Transform barrelPivot;

        public virtual void Pause() {}
        public virtual void UnPause() {}

        protected void CalculateRotation(Vector3 point)
        {
            //I'm applying some code from - https://www.youtube.com/watch?v=ox8dvz6e6ws, So that I can freely rotate the turret in  any direction and it'll end up pointing in the right direction. Specifically the transform.InverseTransformPoint(point);

            //Variables
            Vector3 turretDirection, turretRotation;
            Vector3 barrelDirection, barrelRotation;
            Quaternion turretLookRotation, barrelLookRotation, turretFinalLookRotation, barrelFinalLookRotation;

            switch (rotationMode)
            {
                case TurretRotationMode.Instant:

                    //Instant Rotation - Turret & Barrel instantly rotates to where the camera is facing.

                    //Turret
                    if (turretPivot)
                    {
                        turretDirection = transform.InverseTransformPoint(point);
                        turretLookRotation = Quaternion.LookRotation(turretDirection);
                        turretRotation = turretLookRotation.eulerAngles;
                        turretPivot.localRotation = Quaternion.Euler(0f, turretRotation.y, 0f);
                    }

                    //Barrel
                    if (barrelPivot)
                    {
                        barrelDirection = transform.InverseTransformPoint(point);
                        barrelLookRotation = Quaternion.LookRotation(barrelDirection);
                        barrelRotation = barrelLookRotation.eulerAngles;
                        barrelPivot.localRotation = Quaternion.Euler(barrelRotation.x, 0f, 0f);
                    }

                    break;
                case TurretRotationMode.Lerped:

                    //Lerped Rotation - Turret & Barrel interpolate to where the camera is facing. The time to rotate is the
                    //same whether you rotate 1 degree or 100 degrees...

                    //Turret
                    if (turretPivot)
                    {
                        turretDirection = transform.InverseTransformPoint(point);
                        turretLookRotation = Quaternion.LookRotation(turretDirection);
                        turretRotation = Quaternion.Lerp(turretPivot.localRotation, turretLookRotation, Time.deltaTime * turretSpeed).eulerAngles;
                        turretPivot.localRotation = Quaternion.Euler(0f, turretRotation.y, 0f);
                    }

                    //Barrel
                    if (barrelPivot)
                    {
                        barrelDirection = transform.InverseTransformPoint(point);
                        barrelLookRotation = Quaternion.LookRotation(barrelDirection);
                        barrelRotation = Quaternion.Lerp(barrelPivot.localRotation, barrelLookRotation, Time.deltaTime * barrelSpeed).eulerAngles;
                        barrelPivot.localRotation = Quaternion.Euler(barrelRotation.x, 0f, 0f);
                    }

                    break;
                case TurretRotationMode.Slerped:

                    //Slerped Rotation - Similar to lerped rotation above, just changes Lerp to Slerp lol

                    //Turret
                    if (turretPivot)
                    {
                        turretDirection = transform.InverseTransformPoint(point);
                        turretLookRotation = Quaternion.LookRotation(turretDirection);
                        turretRotation = Quaternion.Slerp(turretPivot.localRotation, turretLookRotation, Time.deltaTime * turretSpeed).eulerAngles;
                        turretPivot.localRotation = Quaternion.Euler(0f, turretRotation.y, 0f);
                    }

                    //Barrel
                    if (barrelPivot)
                    {
                        barrelDirection = transform.InverseTransformPoint(point);
                        barrelLookRotation = Quaternion.LookRotation(barrelDirection);
                        barrelRotation = Quaternion.Slerp(barrelPivot.localRotation, barrelLookRotation, Time.deltaTime * barrelSpeed).eulerAngles;
                        barrelPivot.localRotation = Quaternion.Euler(barrelRotation.x, 0f, 0f);
                    }

                    break;
                case TurretRotationMode.Fixed:

                    //Constant Rotation - Fixed no. of degrees per second/turn rate.

                    //Turret
                    if (turretPivot)
                    {
                        turretDirection = transform.InverseTransformPoint(point);
                        turretDirection.y = 0.0f;

                        turretLookRotation = Quaternion.LookRotation(turretDirection);
                        turretFinalLookRotation = Quaternion.RotateTowards(turretPivot.localRotation, turretLookRotation, Time.deltaTime * turretSpeed);

                        turretPivot.localRotation = turretFinalLookRotation;
                    }

                    //Barrel
                    if (barrelPivot)
                    {
                        barrelDirection = transform.InverseTransformPoint(point);
                        barrelDirection.x = 0.0f;

                        barrelLookRotation = Quaternion.LookRotation(barrelDirection);
                        barrelFinalLookRotation = Quaternion.RotateTowards(barrelPivot.localRotation, barrelLookRotation, Time.deltaTime * barrelSpeed);

                        barrelPivot.localRotation = barrelFinalLookRotation;
                    }

                    break;

                default:
                    break;
            }
        }

        float t = 0f;
        float turretRot = 0f, barrelRot = 0f;

        protected void IdleRotation()
        {
            //Variables
            Vector3 turretRotation;
            Vector3 barrelRotation;
            Quaternion turretFinalLookRotation, barrelFinalLookRotation;

            switch (rotationMode)
            {
                case TurretRotationMode.Instant:

                    //Instant Rotation - Turret & Barrel instantly rotates to where the camera is facing.

                    //Turret
                    if (turretPivot) turretPivot.localRotation = Quaternion.Euler(0f, turretRot, 0f);

                    //Barrel
                    if (barrelPivot) barrelPivot.localRotation = Quaternion.Euler(0f, barrelRot, 0f);

                    break;
                case TurretRotationMode.Lerped:

                    //Lerped Rotation - Turret & Barrel interpolate to where the camera is facing. The time to rotate is the
                    //same whether you rotate 1 degree or 100 degrees...

                    //Turret
                    if (turretPivot)
                    {
                        turretRotation = Quaternion.Lerp(turretPivot.localRotation, Quaternion.Euler(Vector3.one * turretRot), Time.deltaTime * turretSpeed).eulerAngles;
                        turretPivot.localRotation = Quaternion.Euler(0f, turretRotation.y, 0f);
                    }

                    //Barrel
                    if (barrelPivot)
                    {
                        barrelRotation = Quaternion.Lerp(barrelPivot.localRotation, Quaternion.Euler(Vector3.one * barrelRot), Time.deltaTime * barrelSpeed).eulerAngles;
                        barrelPivot.localRotation = Quaternion.Euler(barrelRotation.x, 0f, 0f);
                    }

                    break;
                case TurretRotationMode.Slerped:

                    //Slerped Rotation - Similar to lerped rotation above, just changes Lerp to Slerp lol

                    //Turret
                    if (turretPivot)
                    {
                        turretRotation = Quaternion.Slerp(turretPivot.localRotation, Quaternion.Euler(Vector3.one * turretRot), Time.deltaTime * turretSpeed).eulerAngles;
                        turretPivot.localRotation = Quaternion.Euler(0f, turretRotation.y, 0f);
                    }

                    //Barrel
                    if (barrelPivot)
                    {
                        barrelRotation = Quaternion.Slerp(barrelPivot.localRotation, Quaternion.Euler(Vector3.one * barrelRot), Time.deltaTime * barrelSpeed).eulerAngles;
                        barrelPivot.localRotation = Quaternion.Euler(barrelRotation.x, 0f, 0f);
                    }

                    break;
                case TurretRotationMode.Fixed:

                    //Constant Rotation - Fixed no. of degrees per second/turn rate.

                    //Turret
                    if (turretPivot)
                    {
                        turretFinalLookRotation = Quaternion.RotateTowards(turretPivot.localRotation, Quaternion.Euler(Vector3.one * turretRot), Time.deltaTime * turretSpeed);

                        turretPivot.localRotation = turretFinalLookRotation;
                    }

                    //Barrel
                    if (barrelPivot)
                    {
                        barrelFinalLookRotation = Quaternion.RotateTowards(barrelPivot.localRotation, Quaternion.Euler(Vector3.one * barrelRot), Time.deltaTime * barrelSpeed);

                        barrelPivot.localRotation = barrelFinalLookRotation;
                    }

                    break;

                default:
                    break;
            }

            t += Time.deltaTime;

            if (t > 5f) NewIdleRotation();
        }

        protected void ResetIdleRotation()
        {
            turretRot = Random.Range(0f, 0f);
            barrelRot = Random.Range(0f, 0f);

            t = 0f;
        }

        protected void NewIdleRotation()
        {
            turretRot = Random.Range(-90f, 90f);
            barrelRot = Random.Range(-30f, 10f);

            t = 0f;
        }
    }
}