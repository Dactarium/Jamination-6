using Enums;
using UnityEngine;

namespace Controllers
{
	public class Tree : Entity2D
	{
		[field: SerializeField]
		public Dimension Dimension
		{
			get;
			private set;
		}
	}
}
