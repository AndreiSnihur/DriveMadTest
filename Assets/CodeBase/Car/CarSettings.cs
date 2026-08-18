using System;
using UnityEngine;

namespace CodeBase.Car
{
    [CreateAssetMenu(menuName = "Drive Mad/Car Settings", fileName = "CarSettings")]
    public sealed class CarSettings : ScriptableObject
    {
        [Header("World")]
        [SerializeField, Min(0f)] private float gravity;

        [Header("Motor")]
        [SerializeField, Min(0f)] private float motorTorque;
        [SerializeField, Min(0f)] private float maxWheelSpeed;

        [Header("Chassis")]
        [SerializeField, Min(0.1f)] private float chassisMass;
        [SerializeField] private Vector3 centerOfMass;
        [SerializeField, Min(0f)] private float chassisAngularDamping;

        [Header("Wheels")]
        [SerializeField, Min(0.1f)] private float wheelMass;
        [SerializeField, Min(0.01f)] private float wheelSpinInertia;

        [Header("Suspension")]
        [SerializeField, Min(0f)] private float suspensionSpring;
        [SerializeField, Min(0f)] private float suspensionDamper;

        public float Gravity => gravity;
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
