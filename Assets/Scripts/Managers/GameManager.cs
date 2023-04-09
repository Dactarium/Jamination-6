using System;
using System.Collections.Generic;
using Controllers;
using Enums;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering;

namespace Managers
{
	public class GameManager : Singleton<GameManager>
	{
		[field:SerializeField]
		public Player Player { get; private set; }
		[field: SerializeField]
		private TextMeshProUGUI MenuText;

		public Dimension CurrentDimension { get; private set; }

		public DimensionController DimensionController { get; private set; }
		
		public static Vector2Int GridSize { get; private set; }

		[SerializeField]
		private List<Ghost> ghosts;

		[SerializeField]
		private Camera _camera;
		
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

		public void resumeGame() => Time.timeScale = 1;

		public void backToMenu() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

		public void changeText(string text) => MenuText.text = text;
	}
}