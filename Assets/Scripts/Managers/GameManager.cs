using Controllers;
using Enums;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
		
		private void Start()
		{
			Time.timeScale = 1;
			DimensionController = LevelGenerator.Instance.GenerateLevel(0);
			DimensionController.OnDimensionChange += (previous, current) => { CurrentDimension = current; };
			
			CurrentDimension = DimensionController.Dimension;
		}
		public void ChangeDimension(Dimension dimension) => DimensionController.ChangeDimension(dimension);

		public void resumeGame() => Time.timeScale = 1;

		public void backToMenu() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

		public void changeText(string text) => MenuText.text = text;
	}
}