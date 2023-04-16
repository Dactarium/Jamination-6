using System;
using System.Threading.Tasks;
using Enums;
using Managers;
using UnityEngine;
using Waypoint_System.Scripts;

namespace Controllers
{
	public class Ghost : Entity2D
	{
		[field:SerializeField]
		public Dimension Dimension { get; private set; }

		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private float appearDuration = 1f;

		[SerializeField]
		private AnimationCurve appearCurve;

		private bool _isModelActive = false;
		
		//protected override Vector3 TargetPosition => GameManager.Instance.DimensionController.GetSpawnPoint(EntityType.Player, Dimension);

		public void SetWaypointRoot(WaypointRoot waypointRoot)
		{
			//this.waypointRoot = waypointRoot;
		}

		protected override void Start()
		{
			base.Start();
			SetAlpha(0);
		}

		private void Update()
		{
			transform.position = GameManager.Instance.DimensionController.GetSpawnPoint(EntityType.Player, Dimension);
			//TODO: Optimizde edilecek
			bool active = Dimension switch
			{
				Dimension.Red => GameManager.Instance.Player.RedApple > 0,
				Dimension.Green => GameManager.Instance.Player.GreenApple > 0,
				Dimension.Blue => GameManager.Instance.Player.BlueApple > 0
			};
			
			active = active && transform.position != GameManager.Instance.DimensionController.GetSpawnPoint(EntityType.Player, GameManager.Instance.CurrentDimension);

			if(!_isModelActive && active)
				AppearEffect();
			
			if(_isModelActive && !active)
				DisappearEffect();
			
			_isModelActive = active;
		}

		public override void OnDimensionChange(Dimension previous, Dimension next)
		{
			if(previous == Dimension)
				gameObject.SetActive(true);
			
			
			if(next == Dimension)
				gameObject.SetActive(false);
			
			base.OnDimensionChange(previous, next);
		}

		private async void AppearEffect()
		{
			float elapsedTime = 0;

			while (elapsedTime < appearDuration && gameObject && gameObject.activeInHierarchy)
			{
				await Task.Yield();
				elapsedTime += Time.deltaTime;
				SetAlpha(appearCurve.Evaluate(elapsedTime / appearDuration));
			}
		}
		
		private async void DisappearEffect()
		{
			float elapsedTime = 0;

			while (elapsedTime < appearDuration && gameObject && gameObject.activeInHierarchy)
			{
				await Task.Yield();
				elapsedTime += Time.deltaTime;
				SetAlpha(appearCurve.Evaluate(1f - elapsedTime / appearDuration));
			}
		}

		private void SetAlpha(float alpha)
		{
			if(!spriteRenderer)
				return;
			
			Color color = spriteRenderer.color;
			color.a = alpha;
			spriteRenderer.color = color;
		}
	}
}