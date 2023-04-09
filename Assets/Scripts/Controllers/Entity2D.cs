using System;
using Enums;
using Managers;
using UnityEngine;

namespace Controllers
{
	public class Entity2D : Entity
	{
		
		protected virtual void OnEnable()
		{ 
			OnRotate(GameManager.Instance.Player.transform.eulerAngles);
		}

		protected virtual void Start()
		{
			GameManager.Instance.Player.OnRotate += OnRotate;
		}

		protected void OnRotate(Vector3 angles) => Model.eulerAngles = angles;

		public virtual void OnDimensionChange(Dimension previous, Dimension next)
		{
			OnRotate(GameManager.Instance.Player.transform.eulerAngles);
		}
	}
}