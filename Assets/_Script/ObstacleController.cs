using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Script
{
	/// <inheritdoc />
	/// <summary>
	/// Obstacle controller .
	/// </summary>
	public class ObstacleController : MonoBehaviour
	{
		#region Public Fields.

		/// <summary>
		/// All obstacle prefabs .
		/// </summary>
		[Header("Obstacle prefabs")] public List<GameObject> obstacleList;

		/// <summary>
		/// After how much time obstacle will be created .
		/// </summary>
		[Header("Obstacle interval")] [Range(2f, 10f)]
		public float obstacleCreationInterval;

		/// <summary>
		/// Hold obstacles
		/// </summary>
		[Header("Holds all obstacles")] public GameObject obstacleHolder;

		#endregion

		#region Private Fields.

		/// <summary>
		/// Camera display area length.
		/// </summary>
		private float _cameraDisplayAreaLength = 0f;

		/// <summary>
		/// co routine that create obstacles.
		/// </summary>
		private Coroutine _obstacleCoRoutine;

		/// <summary>
		/// Obstacle creation process started.
		/// </summary>
		private bool _obstacleCreationStarted = false;

		#endregion

		#region Unity Function

		/// <summary>
		/// Awake the instance .
		/// </summary>
		private void Awake()
		{
			//Get display area horizon 
			if (Camera.main != null)
			{
				//Orthographic size provide half size.
				//it should be multiplied by Aspect ratio to correct the calculation .
				_cameraDisplayAreaLength = Camera.main.orthographicSize * Screen.width / Screen.height;
			}
		}

		/// <summary>
		/// on enable
		/// </summary>
		private void OnEnable()
		{
			PlayerController.PlayerStateChange += PlayerStatusChange;
		}

		#endregion

		#region Helper Function

		/// <summary>
		/// Player state change .
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void PlayerStatusChange(object sender, PlayerStateChangeEventArgs eventArgs)
        {


            //Check player have failed.
            if (eventArgs.PreviousState == PlayerState.Fallen)
            {
                //But it want to restart.
                if (eventArgs.NextPlayerState == PlayerState.Idle)
                {

                    //If not already started.
                    if (!_obstacleCreationStarted)
                    {
                        StartObstacleCreate();
                    }

                    return;
                }
            }

            if (eventArgs.NextPlayerState == PlayerState.Fallen)
            {
                //stop the routine
                StopCoroutine(_obstacleCoRoutine);
                //reset.
                _obstacleCreationStarted = false;
            }
            else
            {

                //If not already started.
                if (!_obstacleCreationStarted)
                {
                    StartObstacleCreate();
                }

            }
            


        }

		/// <summary>
		/// Start obstacle create.
		/// </summary>
		private void StartObstacleCreate()
		{
			var routine = StartCreatingObstacle();
			_obstacleCoRoutine = StartCoroutine(routine);
			_obstacleCreationStarted = true;
		}

		/// <summary>
		/// Start Creating Obstacle.
		/// </summary>
		private IEnumerator StartCreatingObstacle()
		{
			while (true)
			{
				//Create obstacle .
				var obstacle = Instantiate(SelectObstacleToGenerate(), obstacleHolder.transform, true);
				//Set up obstacle  out of bound
				obstacle.transform.position =
					new Vector3(_cameraDisplayAreaLength * 2.5f, obstacle.transform.position.y, 0f);
				//Set rotation
				obstacle.transform.rotation = Quaternion.identity;
				//Wait for next
				yield return new WaitForSecondsRealtime(obstacleCreationInterval);
			}
		}

		/// <summary>
		/// Select which obstacle will be generated.
		/// </summary>
		/// <returns></returns>
		private GameObject SelectObstacleToGenerate()
		{
			//Set up obstacle list .
			if (obstacleList == null) return null;
			//Get a random
			var rand = new System.Random();
			//Set upper limit
			var upperLimit = obstacleList.Count - 1;
			//Set a random number range .
			var range = rand.Next(0, 10000);
			//Make a modulo division
			var moduloResult = range % upperLimit;
			//select that prefab.
			return obstacleList[moduloResult];
		}
	}

	#endregion
}
