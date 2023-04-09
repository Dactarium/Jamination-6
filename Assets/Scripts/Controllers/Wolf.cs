using System;
using Managers;
using UnityEngine;
using Enums;

namespace Controllers
{
	public class Wolf : Follower2D
	{
		[SerializeField]
		private GameObject Puf;
		[SerializeField]
		private GameObject PufSpawn;
		protected override void Start()
		{
			target = GameManager.Instance.Player.transform;
			waypointRoot = GameManager.Instance.DimensionController.WaypointRoot;
			base.Start();
		}

		public void appleTouch(Dimension dimension)
        {
			print(dimension);
        }


    }
}