using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers;
using Enums;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Managers
{
	public class GameManager : Singleton<GameManager>
	{
		public static Vector2Int GridSize { get; private set; }
		
		[field:SerializeField]
		public Player Player { get; private set; }
		
		[field: SerializeField]
		private TextMeshProUGUI MenuText;

		public Dimension CurrentDimension { get; private set; }

		public DimensionController DimensionController { get; private set; }
		
		[SerializeField]
		private Light _light;

		[SerializeField]
		private float _lightRotateDuration;

		[SerializeField]
		private AnimationCurve _lightAngleXCurve;

		[SerializeField]
		private Gradient _lightGradient;
			
		[SerializeField]
		private List<Ghost> ghosts;
		
		private void Start()
		{
			Time.timeScale = 1;
			LevelGenerator.Instance.GenerateLevel(PlayerPrefs.GetInt("Level", 0));
			DimensionController.OnDimensionChange += (previous, next) => { CurrentDimension = next; };
			
			CurrentDimension = DimensionController.Dimension;

			foreach (var ghost in ghosts)
			{
				DimensionController.OnDimensionChange += ghost.OnDimensionChange;
				ghost.gameObject.SetActive(CurrentDimension != ghost.Dimension);
				ghost.transform.position = DimensionController.GetSpawnPoint(EntityType.Player, ghost.Dimension);
				ghost.SetWaypointRoot(DimensionController.GetWaypointRoot(ghost.Dimension));
			}
			
			RotateLight();
		}

		[ContextMenu("Reset Progress")]
		public void ResetProgress()
		{
			PlayerPrefs.SetInt("Level", 0);
		}
		
		[ContextMenu("Increase Progress")]
		public void IncreaseProgress()
		{
			PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 0) + 1);
		}
		
		public void ChangeDimension(Dimension dimension) => DimensionController.ChangeDimension(dimension);

		public void SetGridSize(int x, int y)
		{
			GridSize = new Vector2Int(x, y);
			DimensionController.SetSize(x, y);
		}
		public void SetDimensionController(DimensionController dimensionController)
		{
			DimensionController = dimensionController;
		}

		private async void RotateLight()
		{
			float elapsedTime = PlayerPrefs.GetFloat("LightTime", _lightRotateDuration * 330 / 360);

			while (this)
			{
				elapsedTime += Time.deltaTime;
				elapsedTime %= _lightRotateDuration;
				float normalizedTime = elapsedTime / _lightRotateDuration;
				Vector3 angle = _light.transform.eulerAngles;
				angle.x =  360 * _lightAngleXCurve.Evaluate(normalizedTime);
				angle.y = 360 * normalizedTime;
				_light.transform.eulerAngles = angle;
				_light.color = _lightGradient.Evaluate(normalizedTime);
				await Task.Yield();
			}
			
			PlayerPrefs.SetFloat("LightTime", elapsedTime);
		}

		public void resumeGame() => Time.timeScale = 1;

		public void backToMenu() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

		public void changeText(string text) => MenuText.text = text;
	}
}