using UnityEngine;

namespace CodeBase.Car
{
    public sealed class Wheel : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private SphereCollider tyre;
        [SerializeField] private HingeJoint hinge;
        [SerializeField] private ConfigurableJoint suspension;
        
        public float Radius => tyre.radius;

        public void Drive(float targetDegreesPerSecond, float torque)
        {
            var motor = hinge.motor;

            motor.targetVelocity = -targetDegreesPerSecond;
            motor.force = torque;
            motor.freeSpin = false;
            hinge.motor = motor;
            hinge.useMotor = true;
        }

        public void Release() => 
            hinge.useMotor = false;

        public void ApplySettings(CarSettings settings)
        {
            body.mass = settings.WheelMass;

            body.inertiaTensor = Vector3.one * settings.WheelSpinInertia;
            body.inertiaTensorRotation = Quaternion.identity;

            var drive = suspension.yDrive;
            drive.positionSpring = settings.SuspensionSpring;
            drive.positionDamper = settings.SuspensionDamper;
            drive.maximumForce = float.MaxValue;
            suspension.yDrive = drive;

            body.maxAngularVelocity = Mathf.Max(body.maxAngularVelocity, settings.MaxWheelSpeed * Mathf.Deg2Rad * 1.5f);
        }

        private void Reset()
        {
            body = GetComponent<Rigidbody>();
            tyre = GetComponent<SphereCollider>();
            hinge = GetComponent<HingeJoint>();
            if (hinge != null && hinge.connectedBody != null)
                suspension = hinge.connectedBody.GetComponent<ConfigurableJoint>();
        }
    }
}
