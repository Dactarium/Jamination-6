using System.Collections.Generic;
using UnityEngine;
using Waypoint_System.Scripts;

namespace Waypoint_System.Demo.Scripts
{
	public class Follower : MonoBehaviour
	{
		[SerializeField] protected WaypointRoot waypointRoot;
		[SerializeField] protected GameObject target;
    
		[SerializeField] protected float speed = 1f;
		[SerializeField] float _reachDistance = .1f;
    
		protected Waypoint _targetWaypoint;

		protected Stack<Waypoint> _route;

		protected Vector3 _targetPosition;
		void Start()
		{
			IsTargetChanged();
			ChangeRoute();
		}

		void Update()
		{
			Move();
		}

		protected virtual bool IsTargetChanged(){
			Waypoint newTargetWaypoint = waypointRoot.GetNearestWaypoint(target.transform.position);

			if(_targetWaypoint == newTargetWaypoint)return false;

			_targetWaypoint = newTargetWaypoint;
			return true;
		}

		protected void ChangeRoute(){
			Waypoint nearestWaypoint = waypointRoot.GetNearestWaypoint(transform.position);
			_route = WaypointNavigator.Navigate(nearestWaypoint, _targetWaypoint);
			_targetPosition = _route.Pop().RandomPosition;
        
		}

		protected void Move(){
			if((_targetPosition - transform.position).magnitude < _reachDistance){
				OnReached();
			} 

			Vector3 direction = (_targetPosition - transform.position).normalized;

			transform.position += direction * Time.deltaTime * speed;

			Debug.DrawLine(transform.position, _targetPosition, Color.magenta);
		}

    
		protected virtual void OnReached(){
			if(_route != null && _route.Count > 0)_targetPosition = _route.Pop().RandomPosition;
			if(IsTargetChanged()) ChangeRoute();
		}
    
	}
}
