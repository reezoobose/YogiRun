using UnityEngine;

namespace _Script
{
    /// <inheritdoc />
    /// <summary>
    ///     BackGround Scrolled
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackGroundScrolled : MonoBehaviour
    {
        #region Public Field

        /// <summary>
        ///     Scroll speed .
        /// </summary>
        [Header("Scroll Speed")] public float scrollSpeed;

        #endregion

        #region private Field

        /// <summary>
        ///     Length of the scrollable .
        /// </summary>
        private float _length;

        /// <summary>
        ///     Back ground transform .
        /// </summary>
        private Transform _backgroundTransform;

        /// <summary>
        ///     Duplicate Child.
        /// </summary>
        private SpriteRenderer _duplicateChild;

        /// <summary>
        /// Scroll speed for the last time .
        /// </summary>
        private float _lastScrollSpeed;

        /// <summary>
        /// X offset.
        /// </summary>
        public float offset = 0f;

        #endregion

        #region Unity Function

        /// <summary>
        ///     Awake the instance .
        /// </summary>
        private void Awake()
        {
            //Set length.
            _length = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
            //find the sprite renderer.
            _duplicateChild = gameObject.GetComponent<SpriteRenderer>();
            //Store Transform.
            _backgroundTransform = transform;
            //create child.
            CreateChild();
        }

        /// <summary>
        ///     Update per frame .
        /// </summary>
        private void Update()
        {
            //Move 
            Move();
            //Reset.
            Reset();
        }

        /// <summary>
        /// Attach event on enable.
        /// </summary>
        private void OnEnable()
        {
            PlayerController.PlayerStateChange += PlayerStatusChange;
        }

        #endregion

        #region Helper functions

        /// <summary>
        ///     Create child under the parent .
        /// </summary>
        private void CreateChild()
        {
            //create an empty game object .
            var child = new GameObject(gameObject.name + "___Child");
            //Add sprite renderer.
            var spriteRenderer = child.AddComponent<SpriteRenderer>();
            //set sprite.
            spriteRenderer.sprite = _duplicateChild.sprite;
            //set child.
            child.transform.SetParent(_backgroundTransform.transform);
            //move is left.
            child.transform.position = new Vector3( _length-offset, 0f, _backgroundTransform.position.z);
        }

        /// <summary>
        ///     Move toward right .
        /// </summary>
        private void Move()
        {
            //get position.
            var position = _backgroundTransform.position;
            //updated position in x.
            var updatedPositionInX = position.x + scrollSpeed;
            //Move the Object .
            var updatedPosition = new Vector3(updatedPositionInX, position.y, position.z);
            //set position.
            _backgroundTransform.position = updatedPosition;
        }

        /// <summary>
        ///     Reset the position .
        /// </summary>
        private void Reset()
        {
            //reset point.
            var resetPoint = _length - offset;
            //When the parent X have moved full length to the left .
            var check = transform.position.x < - resetPoint;
            //return if not true.
            if (!check) return;
            //get reset position.
            var resetPosition = _backgroundTransform.position;
            //set reset position.
            _backgroundTransform.position = new Vector3(0f, resetPosition.y, resetPosition.z);
        }

        /// <summary>
        /// Player event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>

        private void PlayerStatusChange(object sender, PlayerStateChangeEventArgs eventArgs)
        {
            if (eventArgs.NextPlayerState == PlayerState.Fallen)
            {
                //hold previous speed.
                _lastScrollSpeed = scrollSpeed;
                //set to zero.
                scrollSpeed = 0f;
            }
           else if (eventArgs.PreviousState == PlayerState.Fallen && eventArgs.NextPlayerState == PlayerState.Idle)
            {
                //Start over.
                scrollSpeed = _lastScrollSpeed;
            }

        }

        #endregion
    }
}