using System.Collections.Generic;
using UnityEngine;
using Waypoint_System.Scripts;

namespace Controllers
{
	public class Follower2D : Entity2D
	{
		[SerializeField] protected WaypointRoot waypointRoot;
		[SerializeField] protected Transform target;
    
		[SerializeField] protected float speed = 1f;
		[SerializeField] float _reachDistance = .1f;
    
		protected Waypoint _targetWaypoint;

		protected Stack<Waypoint> _route;

		protected virtual Vector3 TargetPosition => target.position;
		protected Vector3 _movePosition;

		protected Vector3 direction;
		protected override void Start()
		{
			base.Start();
			IsTargetChanged();
			ChangeRoute();
		}

		protected virtual void Update()
		{
			Move();
		}

		protected virtual bool IsTargetChanged(){
			Waypoint newTargetWaypoint = waypointRoot.GetNearestWaypoint(TargetPosition);

			if(_targetWaypoint == newTargetWaypoint)return false;

			_targetWaypoint = newTargetWaypoint;
			return true;
		}

		protected void ChangeRoute(){
			Waypoint nearestWaypoint = waypointRoot.GetNearestWaypoint(transform.position);
			_route = WaypointNavigator.Navigate(nearestWaypoint, _targetWaypoint);
			if(_route is { Count: > 1 })
				_route.Pop();
			if(_route is { Count: > 0 })
				_movePosition = _route.Pop().transform.position;
		}

		protected void Move(){
			if((_movePosition - transform.position).magnitude < _reachDistance){
				OnReached();
				return;
			} 

			direction = (_movePosition - transform.position).normalized;

			transform.position += direction * Time.deltaTime * speed;

			Debug.DrawLine(transform.position, _movePosition, Color.magenta);
		}

    
		protected virtual void OnReached()
		{
			if(_route is { Count: > 0 }) _movePosition = _route.Pop().transform.position;

			if(IsTargetChanged()) ChangeRoute();
		}
    
	}
}
