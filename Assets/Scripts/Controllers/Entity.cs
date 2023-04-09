using UnityEngine;

namespace Controllers
{
	public class Entity : MonoBehaviour
	{
		[field: SerializeField]
		public Transform Model { get; private set; }
	}
}