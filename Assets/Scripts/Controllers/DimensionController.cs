using System;
using Enums;
using UnityEngine;

namespace Controllers
{
	public class DimensionController : MonoBehaviour
	{
		public Action<Dimension, Dimension> OnDimensionChange;
		
		public Dimension Dimension { get; private set; }

		public void Setup(Dimension dimension) => Dimension = dimension;

		public void ChangeDimension(Dimension dimension)
		{
			OnDimensionChange?.Invoke(Dimension, dimension);
			Dimension = dimension;
		}
	}
}