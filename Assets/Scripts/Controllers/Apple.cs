using Enums;
using UnityEngine;

namespace Controllers
{
	public class Apple : MonoBehaviour
	{
		[SerializeField]
		private Sprite red;
		[SerializeField]
		private Sprite blue;
		[SerializeField]
		private Sprite green;
		[SerializeField]
		private SpriteRenderer Sprite;

		private Dimension appledimension;

		[SerializeField]
		private GameObject Puf;

		[SerializeField]
		private float moveSpeed = 5f;

		[SerializeField]
		private AudioSource audio;

		[SerializeField]
		private AudioClip puff;

		[SerializeField]
		private float rotateSpeed = 60f;

		private int rotateDir = 1;
		private Vector3 move;

		public void Setup(Dimension dimension, Direction direction, Transform sender)
		{
			appledimension = dimension;
			switch (dimension)
			{
				case Dimension.Red:
					Sprite.sprite = red;
					break;
				case Dimension.Green:
					Sprite.sprite = green;
					break;
				case Dimension.Blue:
					Sprite.sprite = blue;
					break;

			}

			switch(direction)
			{
				case Direction.Up:
					move.z = +1f;
					break;
				case Direction.Down:
					move.z = -1f;
					rotateDir = -1;
					break;
				case Direction.Left:
					move.x = -1f;
					break;
				case Direction.Right:
					move.x = +1f;
					rotateDir = -1;
					break;
			}
	    
			move = sender.forward * move.z + sender.right * move.x;
		}

		// Update is called once per frame
		void Update()
		{
			Vector3 moveDir = move.normalized;
			transform.position += moveDir * moveSpeed * Time.deltaTime;
        
			Vector3 angles = transform.eulerAngles;
			angles.z = (angles.z + rotateSpeed * rotateDir * Time.deltaTime) % 360f;
			transform.eulerAngles = angles;
		}

		public void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Wolf")
			{
				Instantiate(Puf, other.transform.position + Vector3.up, other.transform.rotation);
				audio.clip = puff;
				audio.Play();
				audio.transform.SetParent(null);
				other.GetComponent<Wolf>().ChangeDimension(appledimension);
				Destroy(this.gameObject);
				Destroy(audio.gameObject, 2f);
			}
		}
	}
}
