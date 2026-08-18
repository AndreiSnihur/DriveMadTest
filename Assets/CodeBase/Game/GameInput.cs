using System;
using CodeBase.Input;
using UnityEngine;

namespace CodeBase.Game
{
    public sealed class GameInput : MonoBehaviour
    {
        private CarControls controls;

        public float Drive { get; private set; }

        public event Action RestartPressed;

        private void Awake()
        {
            controls = new CarControls();
            controls.Car.Restart.performed += _ => RestartPressed?.Invoke();
        }

        private void OnEnable() =>
            controls.Enable();

        private void OnDisable() =>
            controls.Disable();

        private void Update() =>
            Drive = controls.Car.Drive.ReadValue<float>();

        private void OnDestroy() =>
            controls.Dispose();
    }
}
