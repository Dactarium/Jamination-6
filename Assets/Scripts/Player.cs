using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace.Managers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

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

	[SerializeField]
    private float moveSpeed = 5f;
    
    private Direction direction = Direction.Up;
    private Dimension appleDimension = Dimension.Red;
    private int RedApple = 0;
    private int GreenApple = 0;
    private int BlueApple = 0;
	private int TotalApple => RedApple + BlueApple + GreenApple;
    private float angle;

    [SerializeField]
    private Transform carrotSlots;
    public GameObject carrotImage;

    private Tree tree;

    [SerializeField]
    private AnimationCurve rotateCurve;
    
    [SerializeField]
    private float rotateDuration = 0.25f;
    
    [SerializeField]
    private Rigidbody rb;

    private bool isRotating = false;
    
    private void Update()
    {
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

	    #region Changing Dimension

	    if(Input.GetButtonDown("Dimension") && CanSpendCurrentApple() && appleDimension != GameManager.Instance.CurrentDimension)
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
		    if (move.y > 0) direction = Direction.Up;
		    else if(move.y < 0) direction = Direction.Down;
	    } else
	    {
		    if (move.x > 0) direction = Direction.Right;
		    else if(move.x < 0) direction = Direction.Left;
	    }
	    
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
            Destroy(other.gameObject);
            Instantiate(carrotImage, carrotSlots);
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
