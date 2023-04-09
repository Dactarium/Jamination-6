using System.Collections.Generic;
using UnityEngine;
using Waypoint_System.Scripts;

namespace Waypoint_System.Demo.Scripts
{
	public class Npc : MonoBehaviour
	{
		[HideInInspector] public Waypoint CurrentWaypoint;
		public float Speed = 1f;
		protected Waypoint _previousWaypoint {get; private set;}
		protected float _previousBias = .1f;
		private float _reachDistance = .1f;
		private Vector3 _targetPosition;
		private Rigidbody _rigidbody;
		protected virtual void Start(){
			_rigidbody = GetComponent<Rigidbody>();

			SetColor();

			_targetPosition = CurrentWaypoint.RandomPosition;
			transform.position = _targetPosition;
		}

		void Update(){
			CheckReached();
			Move();
		}

		protected virtual void SetColor() => SetColor(null);
		protected virtual void SetColor(Color? color){
			Material material = GetComponentInChildren<MeshRenderer>().material;
			material.color = (color.HasValue)? color.Value : new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f));
			GetComponentInChildren<MeshRenderer>().material = material;
		}

		void CheckReached(){
			if((_targetPosition - transform.position).magnitude < _reachDistance){
				Waypoint nextWaypoint = null;

				List<Waypoint> connections = GetConnections();

				if(connections.Count == 0) return;
            
				Waypoint connection = connections[Random.Range(0, connections.Count)];
               
				if(connections.Count>0)nextWaypoint = connection;
				else nextWaypoint = _previousWaypoint;

				_previousWaypoint = CurrentWaypoint;
				CurrentWaypoint = nextWaypoint;

				_targetPosition = CurrentWaypoint.RandomPosition;
			}
		}

		protected virtual List<Waypoint> GetConnections(){
			List<Waypoint> connections = CurrentWaypoint.Connections;
			if(Random.Range(0f, 1f) >= _previousBias) connections.Remove(_previousWaypoint);
			return connections;
		}

		void Move(){
			Vector3 direction = (_targetPosition - transform.position).normalized;
			_rigidbody.velocity = Vector3.zero;
			_rigidbody.position += direction * Time.deltaTime * Speed;
		}

	}
}
