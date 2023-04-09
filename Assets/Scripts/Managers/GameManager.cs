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
		
		private void Start()
		{
			DimensionController = LevelGenerator.Instance.GenerateLevel(0);
			DimensionController.OnDimensionChange += (previous, current) => { CurrentDimension = current; };
			
			CurrentDimension = DimensionController.Dimension;
		}
		public void ChangeDimension(Dimension dimension) => DimensionController.ChangeDimension(dimension);
	}
}