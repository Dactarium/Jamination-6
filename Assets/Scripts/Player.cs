using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI UIText;
    public Apple Apple;
    public GameObject AppleSpawn;
    public TextMeshProUGUI RedAppleCounter;
    public TextMeshProUGUI BlueAppleCounter;
    public TextMeshProUGUI GreenAppleCounter;
    private Direction direction = Direction.Up;
    private Dimension appleDimension = Dimension.Red;
    private Apple AppleBullet;
    private int RedApple;
    private int GreenApple;
    private int BlueApple;

    private Tree tree;
    [SerializeField]
    private Rigidbody rb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = 0f;
        float moveZ = 0f;
        moveZ = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        moveX = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        if(Mathf.Abs(moveZ) > Mathf.Abs(moveX))
        {
            if (moveZ > 0) direction = Direction.Up;
            else if(moveZ < 0) direction = Direction.Down;
        } else
        {
            if (moveX > 0) direction = Direction.Right;
            else if(moveX < 0) direction = Direction.Left;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) appleDimension = Dimension.Red;
        if (Input.GetKeyDown(KeyCode.Alpha2)) appleDimension = Dimension.Green;
        if (Input.GetKeyDown(KeyCode.Alpha3)) appleDimension = Dimension.Blue;

        if (Input.GetButtonDown("Fire1")) {
            if(appleDimension == Dimension.Red)
            {
                if(RedApple > 0)
                {
                    RedApple--;
                    shootApple();
                    RedAppleCounter.text = RedApple.ToString();
                }
            }
            if (appleDimension == Dimension.Green)
            {
                if (GreenApple > 0)
                {
                    GreenApple--;
                    shootApple();
                    GreenAppleCounter.text = GreenApple.ToString();
                }
            }
            if (appleDimension == Dimension.Blue)
            {
                if (BlueApple > 0)
                {
                    BlueApple--;
                    shootApple();
                    BlueAppleCounter.text = BlueApple.ToString();
                }
            }
        }
        Vector3 moveDir = new Vector3(moveX, 0, moveZ).normalized;
        float moveSpeed = 10f;
        rb.position += moveDir * moveSpeed * Time.deltaTime;

        if (tree)
        {
            if (tree.Dimension == Dimension.Red && Input.GetKeyDown(KeyCode.E)) {
                if(RedApple < 3)
                {
                    RedApple++;
                    RedAppleCounter.text = RedApple.ToString();
                }
            }
            if (tree.Dimension == Dimension.Blue && Input.GetKeyDown(KeyCode.E)) {
                if (BlueApple < 3)
                {
                    BlueApple++;
                    BlueAppleCounter.text = BlueApple.ToString();
                }
            }
            if (tree.Dimension == Dimension.Green && Input.GetKeyDown(KeyCode.E)) {
                if (GreenApple < 3)
                {
                    GreenApple++;
                    GreenAppleCounter.text = GreenApple.ToString();
                }
            } 
        }

    }
    private void shootApple()
    {
        Instantiate(Apple, AppleSpawn.transform.position, AppleSpawn.transform.rotation).Setup(appleDimension, direction);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Tree")) 
        {
            tree = other.GetComponent<Tree>();
            UIText.gameObject.SetActive(true);
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
