using UnityEngine;
using UnityEngine.UI;

namespace _Script
{
    /// <inheritdoc />
    /// <summary>
    ///     Control score .
    /// </summary>
    public class ScoreController : MonoBehaviour
    {
        #region  Fields

        /// <summary>
        ///     Start score .
        /// </summary>
        private float _startScore;

        /// <summary>
        ///     Update score .
        /// </summary>
        private bool _updateScore = true;

        /// <summary>
        ///     Current score .
        /// </summary>
        private static int HighestScore
        {
            get { return PlayerPrefs.GetInt("_highest_score", 0); }
            set { PlayerPrefs.SetInt("_highest_score", value); }
        }

        /// <summary>
        ///     Score text.
        /// </summary>
        [Header("Current store.")] public Text scoreText;

        /// <summary>
        ///     Highest Score.
        /// </summary>
        [Header("highest Score")] public Text highestScoreText;

        /// <summary>
        ///     Highest score object .
        /// </summary>
        [Header("highest Score Object")] public GameObject highestScoreObject;

        #endregion


        #region Unity Functions

        /// <summary>
        ///     Awake the instance.
        /// </summary>
        private void Awake()
        {
            //Return
            if (highestScoreObject == null) return;

            //Deactivate the highest score.
            highestScoreObject.SetActive(false);
        }

        /// <summary>
        ///     update the instance .
        /// </summary>
        private void Update()
        {
            if (!_updateScore) return;
            //Update score .
            var score = _startScore + 1;
            //Set to start score .
            _startScore = score;
            //set to text.
            scoreText.text = _startScore.ToString();
        }

        /// <summary>
        ///     Attach the event.
        /// </summary>
        private void OnEnable()
        {
            PlayerController.PlayerStateChange += PlayerStatusChange;
        }

        #endregion


        #region Helper Function

        /// <summary>
        ///     On status change .
        /// </summary>
        private void PlayerStatusChange(object sender, PlayerStateChangeEventArgs eventArgs)
        {
            if (eventArgs.NextPlayerState == PlayerState.Fallen)
            {
                //no more update.
                _updateScore = false;

                if (_startScore > HighestScore) HighestScore = (int) _startScore;

                //Activate.
                highestScoreObject.SetActive(true);

                //set up.
                highestScoreText.text = HighestScore.ToString();
            }
            //Restart Condition.
            else if (eventArgs.PreviousState == PlayerState.Fallen && eventArgs.NextPlayerState == PlayerState.Idle)
            {
                //Deactivate the highest score.
                highestScoreObject.SetActive(false);
                //reset start score.
                _startScore = 0;
                //Start updating score .
                _updateScore = true;
            }
        }

        #endregion
    }
}