using System;
using System.Threading.Tasks;
using Enums;
using Managers;
using TMPro;
using UnityEngine;

namespace Controllers
{
	public class Player : MonoBehaviour
	{
		// Start is called before the first frame update
		public TextMeshProUGUI UIText;
		public Apple Apple;
		public GameObject AppleSpawn;
		public TextMeshProUGUI RedAppleCounter;
		public TextMeshProUGUI GreenAppleCounter;
		public TextMeshProUGUI BlueAppleCounter;
		public TextMeshProUGUI BagCounter;

	
		private float moveSpeed = 5f;
		[SerializeField]
		private float runSpeed = 7.5f;
		[SerializeField]
		private float walkSpeed = 5f;
		private float stamina = 40f;
		[SerializeField]
		private float staminaCost = 0.5f;
		[SerializeField]
		private float staminaRegen = 1f;
		[SerializeField]
		private float maxStamina = 40f;
		[SerializeField]
		private float runStartStamina = 0;

		private Direction direction = Direction.Down;
		private Dimension appleDimension = Dimension.Red;
		private int RedApple = 0;
		private int GreenApple = 0;
		private int BlueApple = 0;
		private int TotalApple => RedApple + BlueApple + GreenApple;
		private float angle;

		[SerializeField]
		private Transform carrotSlots;
		public GameObject carrotImage;
		private int carrotAmount;

		private Tree tree;

		[SerializeField]
		private AnimationCurve rotateCurve;
    
		[SerializeField]
		private float rotateDuration = 0.25f;

		[SerializeField]
		private Rigidbody rb;

		[SerializeField]
		private Animator animator;
    
		private bool isRunning = false;
		private bool isRotating = false;

		public Action<Vector3> OnRotate;


		private void Start()
		{
			moveSpeed = walkSpeed;
			stamina = maxStamina;
		}

		private void Update()
		{
			if (!isRunning && stamina < maxStamina)
			{
				stamina += staminaRegen * Time.deltaTime;
				if (stamina > maxStamina) stamina = maxStamina;
			}
			HandleMovement();
			HandleInputs();
		}

		private void HandleInputs()
		{

			#region Apple Selection

			if (Input.GetKeyDown(KeyCode.Alpha1)) appleDimension = Dimension.Red;
			else if (Input.GetKeyDown(KeyCode.Alpha2)) appleDimension = Dimension.Green;
			else if (Input.GetKeyDown(KeyCode.Alpha3)) appleDimension = Dimension.Blue;

			#endregion

			#region Rotate

			if(!isRotating)
			{
				if (Input.GetKeyDown(KeyCode.Q)) 
				{
					Rotate(angle = (angle - 90) % 360);
				}
				else if (Input.GetKeyDown(KeyCode.E))
				{
					Rotate(angle = (angle + 90) % 360);
				}
			}
	    

			#endregion

			#region Shooting Apple

			if (Input.GetButtonDown("Fire1") && CanSpendCurrentApple())
			{
				SpendCurrentApple();
				ShootApple();
			}

			#endregion

			#region Collecting Apple
	    
			if (Input.GetKeyDown(KeyCode.F) && tree)
			{
				if (tree.Dimension == Dimension.Red) {
					CollectingApple(ref RedApple, ref RedAppleCounter);
				}else if (tree.Dimension == Dimension.Green) {
					CollectingApple(ref GreenApple, ref GreenAppleCounter);
				}else if (tree.Dimension == Dimension.Blue) {
					CollectingApple(ref BlueApple, ref BlueAppleCounter);
				}
			}

			#endregion

			#region Running

			if (Input.GetKeyDown(KeyCode.LeftShift) && stamina > runStartStamina)
			{
				isRunning = true;
			}
			if (isRunning)
			{
				stamina -= staminaCost * Time.deltaTime;
				moveSpeed = runSpeed;
				if (stamina < 0)
				{
					moveSpeed = walkSpeed;
					isRunning = false;
				}
			}
			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				isRunning = false;
				moveSpeed = walkSpeed;
			}

			#endregion
			

			#region Changing Dimension

			if (Input.GetButtonDown("Dimension") && CanSpendCurrentApple() && appleDimension != GameManager.Instance.CurrentDimension)
			{
				SpendCurrentApple();
				GameManager.Instance.ChangeDimension(appleDimension);
				UIText.gameObject.SetActive(false);
			}

			#endregion
		}

		private void CollectingApple(ref int count, ref TextMeshProUGUI counter)
		{
			if(count > 2)
				return;
			if (TotalApple > 4)
				return;
		
			count++;
			BagCounter.text = TotalApple.ToString() + " / 5";
			counter.text = count.ToString();
		}

		private void SpendCurrentApple()
		{
			switch (appleDimension)
			{
				case Dimension.Red:
					RedApple--;
					RedAppleCounter.text = RedApple.ToString();
					BagCounter.text = TotalApple + " / 5";
					break;
				case Dimension.Blue:
					BlueApple--;
					BlueAppleCounter.text = BlueApple.ToString();
					BagCounter.text = TotalApple + " / 5";
					break;
				case Dimension.Green:
					GreenApple--;
					GreenAppleCounter.text = GreenApple.ToString();
					BagCounter.text = TotalApple + " / 5";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
    
		private bool CanSpendCurrentApple()
		{
			return appleDimension switch { 
				Dimension.Red => RedApple > 0,
				Dimension.Green => GreenApple > 0,
				Dimension.Blue => BlueApple > 0,
				_ => false};
		}

		private void HandleMovement()
		{
			Vector2 move = Vector2.zero; 
			move.y = Mathf.RoundToInt(Input.GetAxis("Vertical"));
			move.x = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
	    
			if(Mathf.Abs(move.y) > Mathf.Abs(move.x))
			{
				if(move.y > 0 && direction is not Direction.Up)
				{
					direction = Direction.Up;
					animator.SetTrigger("Up");
				}
				else if(move.y < 0 && direction is not Direction.Down)
				{
					direction = Direction.Down;
					animator.SetTrigger("Down");
				}
			} else
			{
				if(move.x > 0 && direction is not Direction.Right)
				{
					direction = Direction.Right;
					animator.SetTrigger("Right");
				}
				else if(move.x < 0 && direction is not Direction.Left)
				{
					direction = Direction.Left;
					animator.SetTrigger("Left");
				}
			}

			if(move.magnitude > 0)
				animator.SetBool("Walk", true);
			else
				animator.SetBool("Walk", false);
	    
			Vector3 moveDir = (move.y * transform.forward + move.x * transform.right).normalized;
			rb.position += moveDir * moveSpeed * Time.deltaTime;
		}

		private void ShootApple()
		{
			Instantiate(Apple, AppleSpawn.transform.position, AppleSpawn.transform.rotation).Setup(appleDimension, direction, transform);
		}

		private async void Rotate(float target)
		{
			isRotating = true;
			float angle = transform.eulerAngles.y;
			float passedTime = 0f;
			while (passedTime <= rotateDuration)
			{
				await Task.Yield();
				passedTime += Time.deltaTime;
				transform.eulerAngles = Vector3.up * Mathf.LerpAngle(angle, target, rotateCurve.Evaluate(passedTime / rotateDuration));
				OnRotate?.Invoke(transform.eulerAngles);
			}
			isRotating = false;
		}
		private void OnTriggerEnter(Collider other)
		{
			if (other.tag.Equals("Tree")) 
			{
				tree = other.GetComponent<Tree>();
				UIText.gameObject.SetActive(true);
			}
			if (other.tag.Equals("Key"))
			{
				carrotAmount++;
				Destroy(other.gameObject);
				Instantiate(carrotImage, carrotSlots);
			}
			if (other.tag.Equals("Door"))
			{
				if(carrotAmount > 2)
				{
					carrotAmount = 0;
					carrotSlots.gameObject.SetActive(false);
					other.gameObject.SetActive(false);
				}
			}
		}
		private void OnTriggerExit(Collider other)
		{
			if (other.tag.Equals("Tree"))
			{
				tree = null;
				UIText.gameObject.SetActive(false);
			}
		}
	}
}
