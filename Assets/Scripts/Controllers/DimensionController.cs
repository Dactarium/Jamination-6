using System;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Waypoint_System.Scripts;

namespace Controllers
{
	public class DimensionController : MonoBehaviour
	{
		public Action<Dimension, Dimension> OnDimensionChange;
		
		public Dimension Dimension { get; private set; }

		public WaypointRoot WaypointRoot => GetWaypointRoot(Dimension);
		
		public WaypointRoot GetWaypointRoot(Dimension dimension) => dimension switch {
			Dimension.Red => redWaypointRoot,
			Dimension.Blue => blueWaypointRoot,
			Dimension.Green => greenWaypointRoot,
			_ => throw new ArgumentOutOfRangeException()
		};

		private WaypointRoot redWaypointRoot;
		private WaypointRoot greenWaypointRoot;
		private WaypointRoot blueWaypointRoot;

		private Transform red;
		private Transform green;
		private Transform blue;


		private bool[,] redEmptys;
		private bool[,] greenEmptys;
		private bool[,] blueEmptys;

		public Vector2Int RedSpawn   { get; private set; }
		public Vector2Int GreenSpawn { get; private set; }
		public Vector2Int BlueSpawn  { get; private set; }
		
		public void Setup(Dimension dimension) => Dimension = dimension;

		public void SetSize(int x, int y)
		{
			redEmptys = new bool[x, y];
			greenEmptys = new bool[x, y];
			blueEmptys = new bool[x, y];
		}

		public void SetDimensionTransform(Dimension dimension, Transform transform) 
		{
			switch (dimension)
			{
				case Dimension.Red:
					red = transform;
					break;
				case Dimension.Green:
					green = transform;
					break;
				case Dimension.Blue:
					blue = transform;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(dimension), dimension, null);
			}
		}

		public void ChangeDimension(Dimension dimension)
		{
			OnDimensionChange?.Invoke(Dimension, dimension);
			Dimension = dimension;
		}

		public void SetWaypointRoot(Dimension dimension, WaypointRoot waypointRoot)
		{
			switch(dimension)
			{
				case Dimension.Red:
					redWaypointRoot = waypointRoot;
					break;
				case Dimension.Green:
					greenWaypointRoot = waypointRoot;
					break;
				case Dimension.Blue:
					blueWaypointRoot = waypointRoot;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(dimension), dimension, null);
			}
		}
		public void SetSpawnIndex(Vector2Int gridIndex, Dimension? dimension = null)
		{
			if(dimension is null or Dimension.Red)
				if(redEmptys[gridIndex.x, gridIndex.y])
					RedSpawn = gridIndex;
			
			if(dimension is null or Dimension.Green)
				if(greenEmptys[gridIndex.x, gridIndex.y])
					GreenSpawn = gridIndex;
			
			if(dimension is null or Dimension.Blue)
				if(blueEmptys[gridIndex.x, gridIndex.y])
					BlueSpawn = gridIndex;
		}

		public void SetEmpty(int x, int y, Dimension dimension)
		{
			var lenY = GameManager.GridSize.y - 1;
			switch(dimension)
			{
				case Dimension.Red:
					redEmptys[x, lenY - y] = true;
					break;
				case Dimension.Green:
					greenEmptys[x, lenY - y] = true;
					break;
				case Dimension.Blue:
					blueEmptys[x, lenY - y] = true;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(dimension), dimension, null);
			}
		}
		public Vector3 GetSpawnPoint(EntityType entity, Dimension dimension)
		{
			switch(entity)
			{
				case EntityType.Player:
					switch(dimension)
					{
						case Dimension.Red:
							return new Vector3(RedSpawn.x + 0.5f - GameManager.GridSize.x / 2f, 0, RedSpawn.y + 0.5f - GameManager.GridSize.y / 2f);
						case Dimension.Green:
							return new Vector3(GreenSpawn.x + 0.5f- GameManager.GridSize.x / 2f, 0, GreenSpawn.y + 0.5f - GameManager.GridSize.y / 2f);
						case Dimension.Blue:
							return new Vector3(BlueSpawn.x + 0.5f - GameManager.GridSize.x / 2f, 0, BlueSpawn.y + 0.5f - GameManager.GridSize.y / 2f);
						default:
							throw new ArgumentOutOfRangeException(nameof(dimension), dimension, null);
					}
				case EntityType.Wolf:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(entity), entity, null);
			}

			return Vector3.zero;
		}

		public Transform GetDimensionTransform(Dimension dimension)
        {
			switch (dimension)
			{
				case Dimension.Red:
					return red;
				case Dimension.Green:
					return green;
				case Dimension.Blue:
					return blue;
				default:
					throw new ArgumentOutOfRangeException(nameof(dimension), dimension, null);
			}
		}
	}
}