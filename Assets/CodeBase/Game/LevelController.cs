using System.Collections;
using CodeBase.Car;
using CodeBase.Level;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Game
{
    public sealed class LevelController : MonoBehaviour
    {
        private enum State { Playing, Won, Lost }

        [SerializeField] private GameInput input;
        [SerializeField] private CarMover carMover;
        [SerializeField] private CarDetacher carDetacher;
        [SerializeField] private RoofSensor roofSensor;
        [SerializeField] private CarTriggerZone finishZone;
        [SerializeField] private CarTriggerZone killZone;
        [SerializeField] private HudView hud;
        
        [SerializeField, Min(0f)] private float autoRestartDelay;
        [SerializeField, Min(0f)] private float upsideDownTimeout;

        private State state = State.Playing;
        private float upsideDownTime;

        private void OnEnable()
        {
            finishZone.CarEntered += OnFinishReached;
            killZone.CarEntered += Lose;
            roofSensor.Hit += Lose;
            input.RestartPressed += Restart;
        }

        private void OnDisable()
        {
            finishZone.CarEntered -= OnFinishReached;
            killZone.CarEntered -= Lose;
            roofSensor.Hit -= Lose;
            input.RestartPressed -= Restart;
        }

        private void Update()
        {
            carMover.DriveInput = state == State.Playing ? input.Drive : 0f;

            if (state == State.Playing)
                CheckUpsideDown();
        }

        private void CheckUpsideDown()
        {
            var upsideDown = carMover.Chassis.transform.up.y < 0.1f;
            upsideDownTime = upsideDown ? upsideDownTime + Time.deltaTime : 0f;
            if (upsideDownTime >= upsideDownTimeout)
                Lose();
        }

        private void OnFinishReached()
        {
            if (state != State.Playing)
                return;

            state = State.Won;
            hud.ShowWin();
        }

        private void Lose()
        {
            if (state != State.Playing)
                return;

            state = State.Lost;
            carDetacher.Detach();
            hud.ShowLose();
            StartCoroutine(RestartAfterDelay());
        }

        private IEnumerator RestartAfterDelay()
        {
            yield return new WaitForSeconds(autoRestartDelay);
            Restart();
        }

        private void Restart() =>
            SceneManager.LoadScene(gameObject.scene.buildIndex);
    }
}
