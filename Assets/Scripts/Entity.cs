using UnityEngine;

namespace DefaultNamespace
{
	public class Entity : MonoBehaviour
	{
		[field: SerializeField]
		public Transform Model { get; private set; }
	}
}