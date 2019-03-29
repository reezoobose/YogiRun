using System;
using UnityEngine;

namespace _Script
{
    /// <summary>
    ///     Player state.
    /// </summary>
    public enum PlayerState
    {
        Idle,
        Running,
        Jumping,
        Fallen
    }

    /// <inheritdoc />
    /// <summary>
    ///     Player Controller .
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        #region Public Fields

        /// <summary>
        ///     Jump Force.
        /// </summary>
        public float jumpForce;

        /// <summary>
        ///     Holds Player current State .
        /// </summary>
        public PlayerState playerCurrentState;

        /// <summary>
        ///     player state changed notification.
        /// </summary>
        public static EventHandler<PlayerStateChangeEventArgs> PlayerStateChange;

        /// <summary>
        ///     Jump sound.
        /// </summary>
        public AudioSource jumpSound;

        #endregion

        #region Private Filelds

        /// <summary>
        ///     Rigid Body .
        /// </summary>
        private Rigidbody2D _rigidBody;

        /// <summary>
        ///     Player animator .
        /// </summary>
        private Animator _playerAnim;

        /// <summary>
        ///     Initial transform for reset.
        /// </summary>
        private Transform _initialTransform;

        /// <summary>
        ///     Fall sound.
        /// </summary>
        private AudioSource _fall;

        /// <summary>
        ///     Animation control bool cached.
        /// </summary>
        private static readonly int Run = Animator.StringToHash("StartRun");

        private static readonly int HighJump = Animator.StringToHash("JumpNow");
        private static readonly int Fall = Animator.StringToHash("DeathFall");

        #endregion

        #region Unity Functions

        /// <summary>
        ///     Awake the instance .
        /// </summary>
        private void Awake()
        {
            //Audio listener.
            _fall = GetComponent<AudioSource>();
            //Rigid Body
            _rigidBody = GetComponent<Rigidbody2D>();
            //Set up current state.
            playerCurrentState = PlayerState.Idle;
            //Get the Animation
            _playerAnim = GetComponent<Animator>();
            //store transform.
            _initialTransform = transform;
        }

        /// <summary>
        ///     Enable the script.
        /// </summary>
        private void OnEnable()
        {
            //connect with jump.
            UserInput.Jumped += JumpUp;
            UserInput.ResetRequested += UserInputOnResetRequested;
        }

        /// <summary>
        ///     Collusion detector
        /// </summary>
        /// <param name="other"></param>
        private void OnCollisionEnter2D(Collision2D other)
        {
            //Obstacle collision .
            if (other.collider.tag.Contains("Obstacle"))
            {
                //Destroy the obstacle.
                Destroy(other.gameObject);
                //change state.
                PlayerStateChangeRequest(PlayerState.Fallen);
                //play sound.
                if (_fall != null) _fall.Play();
            }

            //When it touches the ground .
            if (other.collider.tag.Contains("Ground"))
            {
                //If fallen ignore.
                if (playerCurrentState == PlayerState.Fallen) return;

                //change state.
                PlayerStateChangeRequest(PlayerState.Running);
            }
        }

        #endregion

        #region Helper Functon

        /// <summary>
        ///     Jump
        /// </summary>
        private void JumpUp()
        {
            //check jump condition.
            var jumpCondition = playerCurrentState != PlayerState.Jumping && playerCurrentState != PlayerState.Fallen;
            //check state.
            if (!jumpCondition) return;
            //change state.
            PlayerStateChangeRequest(PlayerState.Jumping);
            //Jump.
            _rigidBody.velocity = Vector2.up * jumpForce;
            //On space jump .
            if (jumpSound != null)
            {
                jumpSound.Play();
            }
        }

        /// <summary>
        ///     Animation control Switch
        /// </summary>
        private void AnimationControlSwitch(PlayerState switchToPlayerState)
        {
            switch (switchToPlayerState)
            {
                //Switch to idle state .
                case PlayerState.Idle:
                    if (_playerAnim.GetBool(Run)) _playerAnim.SetBool(Run, false);

                    if (_playerAnim.GetBool(HighJump)) _playerAnim.SetBool(HighJump, false);

                    if (_playerAnim.GetBool(Fall)) _playerAnim.SetBool(Fall, false);

                    break;

                case PlayerState.Running:

                    //Switch to running state .
                    if (!_playerAnim.GetBool(Run)) _playerAnim.SetBool(Run, true);

                    if (_playerAnim.GetBool(HighJump)) _playerAnim.SetBool(HighJump, false);

                    if (_playerAnim.GetBool(Fall)) _playerAnim.SetBool(Fall, false);

                    break;
                case PlayerState.Jumping:

                    //Switch to jump state .

                    if (_playerAnim.GetBool(Run)) _playerAnim.SetBool(Run, false);

                    if (!_playerAnim.GetBool(HighJump)) _playerAnim.SetBool(HighJump, true);

                    if (_playerAnim.GetBool(Fall)) _playerAnim.SetBool(Fall, false);

                    break;
                case PlayerState.Fallen:

                    //Switch to fallen state .

                    if (_playerAnim.GetBool(Run)) _playerAnim.SetBool(Run, false);

                    if (_playerAnim.GetBool(HighJump)) _playerAnim.SetBool(HighJump, false);

                    if (!_playerAnim.GetBool(Fall)) _playerAnim.SetBool(Fall, true);

                    break;
            }
        }

        /// <summary>
        ///     When user hit retry.
        /// </summary>
        private void UserInputOnResetRequested()
        {
            //Reset transform.
            transform.position = _initialTransform.position;
            transform.rotation = Quaternion.identity;
            //change state first idle.
            PlayerStateChangeRequest(PlayerState.Idle);
            //change state then run
            PlayerStateChangeRequest(PlayerState.Running);
        }

        /// <summary>
        ///     Player state change request.
        /// </summary>
        private void PlayerStateChangeRequest(PlayerState toState)
        {
            //fire event.
            if (PlayerStateChange != null)
                PlayerStateChange.Invoke(this, new PlayerStateChangeEventArgs(playerCurrentState, toState));
            //Change state.
            playerCurrentState = toState;
            //change animation state .
            AnimationControlSwitch(toState);
        }
    }

    #endregion
}