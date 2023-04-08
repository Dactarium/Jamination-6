using Managers;
using UnityEngine;

namespace Controllers
{
	public class Entity2D : Entity
	{

		private void Start()
		{
			GameManager.Instance.Player.OnRotate += OnRotate;
		}

		private void OnRotate(Vector3 angles) => Model.eulerAngles = angles;
	}
}