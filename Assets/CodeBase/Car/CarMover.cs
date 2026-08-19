using UnityEngine;

namespace CodeBase.Car
{
    public sealed class CarMover : MonoBehaviour
    {
        private const float InputDeadZone = 0.05f;

        [SerializeField] private CarSettings settings;
        [SerializeField] private Rigidbody chassis;
        [SerializeField] private Wheel frontWheel;
        [SerializeField] private Wheel rearWheel;

        public float DriveInput { get; set; }

        private Vector3 Forward =>
            -chassis.transform.forward;

        private void Awake() => 
            ApplySettings();

        private void OnEnable() => 
            settings.Changed += ApplySettings;

        private void OnDisable()
        {
            settings.Changed -= ApplySettings;
            DriveInput = 0f;
        }

        private void FixedUpdate()
        {
            var input = Mathf.Clamp(DriveInput, -1f, 1f);
            if (Mathf.Abs(input) < InputDeadZone)
                input = 0f;

            var torque = MotorTorque(input);
            DriveWheel(frontWheel, input, torque);
            DriveWheel(rearWheel, input, torque);
        }

        private float MotorTorque(float input)
        {
            if (input == 0f)
                return 0f;

            var topSpeed = settings.MaxWheelSpeed * Mathf.Deg2Rad * rearWheel.Radius;
            var forwardSpeed = Vector3.Dot(chassis.linearVelocity, Forward);
            var speedRatio = Mathf.Clamp01(forwardSpeed * Mathf.Sign(input) / topSpeed);
            return settings.MotorTorque * (1f - speedRatio);
        }

        private void DriveWheel(Wheel wheel, float input, float torque)
        {
            if (input == 0f)
            {
                wheel.Release();
                return;
            }

            wheel.Drive(input * settings.MaxWheelSpeed, torque);
        }

        private void ApplySettings()
        {
            chassis.mass = settings.ChassisMass;
            chassis.angularDamping = settings.ChassisAngularDamping;
            chassis.centerOfMass = settings.CenterOfMass;

            frontWheel.ApplySettings(settings);
            rearWheel.ApplySettings(settings);
        }
    }
}
