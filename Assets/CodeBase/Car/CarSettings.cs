using System;
using UnityEngine;

namespace CodeBase.Car
{
    [CreateAssetMenu(menuName = "Drive Mad/Car Settings", fileName = "CarSettings")]
    public sealed class CarSettings : ScriptableObject
    {
        [Header("Motor")]
        [SerializeField, Min(0f)] private float motorTorque = 800f;
        [SerializeField, Min(0f)] private float maxWheelSpeed = 2500f;

        [Header("Chassis")]
        [SerializeField, Min(0.1f)] private float chassisMass = 4f;
        [SerializeField] private Vector3 centerOfMass = new Vector3(0f, 1.5f, 0f);
        [SerializeField, Min(0f)] private float chassisAngularDamping = 0.1f;

        [Header("Wheels")]
        [SerializeField, Min(0.1f)] private float wheelMass = 0.5f;
        [SerializeField, Min(0.01f)] private float wheelSpinInertia = 0.2f;

        [Header("Suspension")]
        [SerializeField, Min(0f)] private float suspensionSpring = 80f;
        [SerializeField, Min(0f)] private float suspensionDamper = 4f;

        public float MotorTorque => motorTorque;
        public float MaxWheelSpeed => maxWheelSpeed;
        public float ChassisMass => chassisMass;
        public Vector3 CenterOfMass => centerOfMass;
        public float ChassisAngularDamping => chassisAngularDamping;
        public float WheelMass => wheelMass;
        public float WheelSpinInertia => wheelSpinInertia;
        public float SuspensionSpring => suspensionSpring;
        public float SuspensionDamper => suspensionDamper;

        public event Action Changed;

        private void OnValidate() => Changed?.Invoke();
    }
}
