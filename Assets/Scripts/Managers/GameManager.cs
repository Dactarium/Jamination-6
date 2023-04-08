using System;
using DefaultNamespace.Controllers;
using Helpers;
using UnityEngine;

namespace DefaultNamespace.Managers
{
	public class GameManager : Singleton<GameManager>
	{
		public Dimension CurrentDimension { get; private set; }

		private DimensionController _dimensionController;
		private void Start()
		{
			_dimensionController = LevelGenerator.Instance.GenerateLevel(0);
			_dimensionController.OnDimensionChange += (previous, current) => { CurrentDimension = current; };
			
			CurrentDimension = _dimensionController.Dimension;
		}
		public void ChangeDimension(Dimension dimension) => _dimensionController.ChangeDimension(dimension);
	}
}