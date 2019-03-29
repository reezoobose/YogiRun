using UnityEngine;
using UnityEngine.UI;

namespace _Script
{
    public class UserInput : MonoBehaviour
    {
        #region  Fields

        /// <summary>
        ///     Jump key pressed delegate .
        /// </summary>
        public delegate void JumpKeyPressed();

        /// <summary>
        ///     Jump key press event .
        /// </summary>
        public static event JumpKeyPressed Jumped;

        /// <summary>
        ///     Jump button.
        /// </summary>
        public Button jumButton;

        /// <summary>
        ///     Retry Button.
        /// </summary>
        public Button retryButton;

        /// <summary>
        ///     reset delegate.
        /// </summary>
        public delegate void Reset();

        /// <summary>
        ///     Reset requested.
        /// </summary>
        public static event Reset ResetRequested;

        /// <summary>
        ///     Jump sound.
        /// </summary>
        private AudioSource jumpSound;

        #endregion


        #region  Unity Functions

        /// <summary>
        ///     Awake the Instance .
        /// </summary>
        private void Awake()
        {
            //Connect jump.
            if (jumButton != null)
            {
#if UNITY_EDITOR
                jumpSound = jumButton.gameObject.GetComponent<AudioSource>();
#endif
                jumButton.onClick.AddListener(() =>
                {
                    if (Jumped != null) Jumped.Invoke();
                });
            }

            //Retry.
            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnResetRequested);
                //Deactivate at start.
                retryButton.gameObject.SetActive(false);
            }
        }

        /// <summary>
        ///     On enable .
        /// </summary>
        private void OnEnable()
        {
            PlayerController.PlayerStateChange += PlayerStatusChange;
        }

#if UNITY_EDITOR

        /// <summary>
        ///     Update the instance .
        /// </summary>
        private void Update()
        {
            //No space .
            if (!Input.GetKeyDown("space")) return;
            //On space jump .
            if (Jumped != null)
            {
                Jumped.Invoke();
                jumpSound.Play();
            }

            //play sound.
        }

#endif

        #endregion

        #region Helper Function

        /// <summary>
        ///     Player state changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void PlayerStatusChange(object sender, PlayerStateChangeEventArgs eventArgs)
        {
            //fall condition.
            if (eventArgs.NextPlayerState == PlayerState.Fallen)
                retryButton.gameObject.SetActive(true);

            //Restart Condition.
            else if (eventArgs.PreviousState == PlayerState.Fallen && eventArgs.NextPlayerState == PlayerState.Idle)
                retryButton.gameObject.SetActive(false);
        }

        /// <summary>
        ///     On reset request .
        /// </summary>
        private static void OnResetRequested()
        {
            var handler = ResetRequested;
            if (handler != null) handler();
        }

        #endregion
    }
}