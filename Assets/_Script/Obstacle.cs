using UnityEngine;

namespace _Script
{
    /// <summary>
    ///     Obstacle.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Obstacle : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Speed.
        /// </summary>
        [Header("Velocity")] public float speed;

        /// <summary>
        ///     Auto Destroy time .
        /// </summary>
        [Range(1f, 7f)] [Header("Auto destroy")]
        public float autoDestroy;

        /// <summary>
        ///     Obstacle Rigid body
        /// </summary>
        private Rigidbody2D _rigidBody;

        #endregion

        #region Unity Function

        /// <summary>
        ///     Awake the instance.
        /// </summary>
        private void Awake()
        {
            //Get the rigid body attached.
            _rigidBody = GetComponent<Rigidbody2D>();
            //Mark object for Auto destroy.
            Destroy(gameObject, autoDestroy);
        }

        /// <summary>
        ///     Update the instance .
        /// </summary>
        private void Start()
        {
            _rigidBody.velocity = Vector2.left * speed;
        }

        /// <summary>
        ///     on enable
        /// </summary>
        private void OnEnable()
        {
            PlayerController.PlayerStateChange += PlayerStatusChange;
        }

        #endregion

        #region Helper Functions

        /// <summary>
        ///     Player state change .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void PlayerStatusChange(object sender, PlayerStateChangeEventArgs eventArgs)
        {
            //if fallen.
            if (eventArgs.NextPlayerState == PlayerState.Fallen)
            {
                if (_rigidBody != null) _rigidBody.velocity = Vector2.zero;
            }
            //Restart Condition.
            else if (eventArgs.PreviousState == PlayerState.Fallen && eventArgs.NextPlayerState == PlayerState.Idle)
            {
                if (_rigidBody != null) _rigidBody.velocity = Vector2.left * speed;
            }
        }

        #endregion
    }
}