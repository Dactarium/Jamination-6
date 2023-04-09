using UnityEngine;

namespace Waypoint_System.Demo.Scripts
{
	public class Killer : Follower
	{

		void Update(){
			Move();
			if(target == null) return;

			float targetDistance = (target.transform.position - transform.position).magnitude;
			if(targetDistance < 1f) _targetPosition = target.transform.position;
		}

		protected override bool IsTargetChanged(){
			if(target == null) target = GameObject.FindGameObjectWithTag("Npc");

			return base.IsTargetChanged();
		}

		protected override void OnReached()
		{
			if(IsTargetChanged()) ChangeRoute();
        
			if(_route != null && _route.Count > 0)_targetPosition = _route.Pop().RandomPosition;
			else _targetPosition = target.transform.position;
		}

		void OnTriggerEnter(Collider other){
			if(!other.CompareTag("Npc")) return;

			waypointRoot.GetComponent<NpcSpawner>().Spawn();
			target = null;
			Destroy(other.gameObject);
		}
	}
}
