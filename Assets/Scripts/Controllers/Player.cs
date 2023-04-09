using System;
using System.Threading.Tasks;
using Enums;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		[field: SerializeField]
		private TextMeshProUGUI MenuText;


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

		[SerializeField]
		private GameObject escMenu;

		private Direction direction = Direction.Down;
		private Dimension appleDimension = Dimension.Red;
		public  int RedApple   { get; private set; } = 0;
		public  int GreenApple { get; private set; } = 0;
		public  int BlueApple  { get; private set; } = 0;
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

		[SerializeField]
		private AudioSource audio;

		[SerializeField]
		private AudioClip applePick;

		[SerializeField]
		private AudioClip carrotPick;

		[SerializeField]
		private AudioClip appleThrow;

		[SerializeField]
		private AudioClip rabbiteatCarrot;

		private bool isRunning = false;
		private bool isRotating = false;

		private Vector2Int _lastGridIndex;
		
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
			CheckGridIndex();
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
					RedApple = CollectingApple(RedApple, ref RedAppleCounter);
				}else if (tree.Dimension == Dimension.Green) {
					GreenApple = CollectingApple(GreenApple, ref GreenAppleCounter);
				}else if (tree.Dimension == Dimension.Blue) {
					BlueApple = CollectingApple(BlueApple, ref BlueAppleCounter);
				}
				
				BagCounter.text = TotalApple + " / 5";
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
				transform.position = GameManager.Instance.DimensionController.GetSpawnPoint(EntityType.Player, appleDimension);
				animator.SetFloat("Dimension", (int)appleDimension / 3f);
				UIText.gameObject.SetActive(false);
			}

			#endregion

			if (Input.GetKeyDown(KeyCode.Escape))
            {
				escMenu.SetActive(true);
				Time.timeScale = 0;
				MenuText.gameObject.SetActive(true);
			}
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
			{
				animator.SetBool("Walk", true);
			}
			else
			{
				animator.SetBool("Walk", false);
			}
	    
			Vector3 moveDir = (move.y * transform.forward + move.x * transform.right).normalized;
			rb.position += moveDir * moveSpeed * Time.deltaTime;
		}

		private void CheckGridIndex()
		{
			Vector2Int gridIndex = new Vector2Int(Mathf.RoundToInt(transform.position.x - 0.5f + GameManager.GridSize.x / 2f), Mathf.RoundToInt(transform.position.z - 0.5f + GameManager.GridSize.y / 2f));
			gridIndex.x = Mathf.Clamp(gridIndex.x, 0, GameManager.GridSize.x - 1);
			gridIndex.y = Mathf.Clamp(gridIndex.y, 0, GameManager.GridSize.y - 1);
			if(_lastGridIndex == gridIndex)
				return;

			_lastGridIndex = gridIndex;
			GameManager.Instance.DimensionController.SetSpawnIndex(gridIndex);
		}
		
		private int CollectingApple(int count, ref TextMeshProUGUI counter)
		{
			if(count > 2)
				return count;
			if (TotalApple > 4)
				return count;
			
			audio.clip = applePick;
			audio.Play();
			count++;
			counter.text = count.ToString();
			return count;
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
		
		private void ShootApple()
		{
			audio.clip = appleThrow;
			audio.Play();
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
				audio.clip = carrotPick;
				audio.Play();
				carrotAmount++;
				Destroy(other.gameObject);
				Instantiate(carrotImage, carrotSlots);
			}
			if (other.tag.Equals("Door") && other.TryGetComponent(out Door door))
			{
				if(carrotAmount >= door.RequiredKey)
				{
					audio.clip = rabbiteatCarrot;
					audio.Play();
					carrotAmount = 0;
					carrotSlots.gameObject.SetActive(false);
					other.gameObject.SetActive(false);
					NextLevel();
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

		private async void NextLevel()
		{
			PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 0) + 1);
			await Task.Delay(500);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
