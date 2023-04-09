using Managers;
using UnityEngine;

namespace Controllers
{
	public class Entity2D : Entity
	{

		protected virtual void Start()
		{
			GameManager.Instance.Player.OnRotate += OnRotate;
		}

		private void OnRotate(Vector3 angles) => Model.eulerAngles = angles;
	}
}