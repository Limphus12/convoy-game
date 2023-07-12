using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.convoy
{
    public enum TurretRotationMode { Instant, Lerped, Slerped, Fixed }

    public class Turret : MonoBehaviour
    {
        [Header("Turret Movement")]
        [SerializeField] private TurretRotationMode rotationMode;

        [Tooltip("Turret Speed and Barrel Speed only apply to Lerped, Slerped and Fixed Rotation Modes")]
        [SerializeField] private float turretSpeed = 5f, barrelSpeed = 5f;

        [Space]
        [SerializeField] private Transform turretPivot;
        [SerializeField] private Transform barrelPivot;

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
    }
}