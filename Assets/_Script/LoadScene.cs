using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Script
{
    /// <inheritdoc />
    /// <summary>
    /// Load scene.
    /// </summary>
    public class LoadScene : MonoBehaviour
    {
        /// <summary>
        /// Play button.
        /// </summary>
        public Button playButton;

        // Use this for initialization
        private void Start () {

            if (playButton != null)
            {
                playButton.onClick.AddListener(() =>
                {
                    SceneManager.LoadScene(1);
                });
            }

        }
    }
}
