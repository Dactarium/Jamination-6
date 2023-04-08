using UnityEngine;

namespace DefaultNamespace
{
	public class Key : MonoBehaviour
	{
		[field: SerializeField]
		public Dimension Dimension { get; private set; }
	}
}