using Controllers;
using Enums;
using Helpers;
using UnityEngine;

namespace Managers
{
	public class GameManager : Singleton<GameManager>
	{
		[field:SerializeField]
		public Player Player { get; private set; }
		
		public Dimension CurrentDimension { get; private set; }

		public DimensionController DimensionController { get; private set; }
		
		public static Vector2Int GridSize { get; private set; }
		
		private void Start()
		{
			LevelGenerator.Instance.GenerateLevel(0);
			DimensionController.OnDimensionChange += (previous, current) => { CurrentDimension = current; };
			
			CurrentDimension = DimensionController.Dimension;
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
	}
}