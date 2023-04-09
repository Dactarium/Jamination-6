using System;
using Enums;
using UnityEngine;
using Waypoint_System.Scripts;

namespace Controllers
{
	public class DimensionController : MonoBehaviour
	{
		public Action<Dimension, Dimension> OnDimensionChange;
		
		public Dimension Dimension { get; private set; }

		public WaypointRoot WaypointRoot => Dimension switch {
			Dimension.Red => redWaypointRoot,
			Dimension.Blue => blueWaypointRoot,
			Dimension.Green => greenWaypointRoot,
			_ => throw new ArgumentOutOfRangeException()
		};

		private WaypointRoot redWaypointRoot;
		private WaypointRoot greenWaypointRoot;
		private WaypointRoot blueWaypointRoot;
		
		public void Setup(Dimension dimension) => Dimension = dimension;

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
	}
}