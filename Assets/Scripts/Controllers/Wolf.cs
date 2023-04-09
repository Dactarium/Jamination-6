using System;
using Managers;
using UnityEngine;

namespace Controllers
{
	public class Wolf : Follower2D
	{
		[SerializeField]
		private Animator animator;
		
		protected override void Start()
		{
			target = GameManager.Instance.Player.transform;
			waypointRoot = GameManager.Instance.DimensionController.WaypointRoot;
			base.Start();
		}

		protected override void Update()
		{
			base.Update();
			
			
		}
	}
}