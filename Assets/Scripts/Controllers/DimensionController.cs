using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.Controllers
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