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
            else direction = Direction.Down;
        } else
        {
            if (moveX > 0) direction = Direction.Right;
            else direction = Direction.Left;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) appleDimension = Dimension.Red;
        if (Input.GetKeyDown(KeyCode.Alpha2)) appleDimension = Dimension.Green;
        if (Input.GetKeyDown(KeyCode.Alpha3)) appleDimension = Dimension.Blue;

        if (Input.GetButtonDown("Fire1")) {
            Instantiate(Apple, AppleSpawn.transform.position, AppleSpawn.transform.rotation).Setup(appleDimension, direction);
        }
        Vector3 moveDir = new Vector3(moveX, 0, moveZ).normalized;
        float moveSpeed = 10f;
        rb.position += moveDir * moveSpeed * Time.deltaTime;

        if (tree)
        {
            if (tree.Dimension == Dimension.Red && Input.GetKeyDown(KeyCode.E)) {
                if(tree.Apple > 0)
                {
                    RedApple++;
                    tree.Apple--;
                    RedAppleCounter.text = RedApple.ToString();
                    Debug.Log("Red Apple : " + RedApple);
                }
            }
            if (tree.Dimension == Dimension.Blue && Input.GetKeyDown(KeyCode.E)) {
                if (tree.Apple > 0)
                {
                    BlueApple++;
                    tree.Apple--;
                    BlueAppleCounter.text = BlueApple.ToString();
                    Debug.Log("Blue Apple : " + BlueApple);
                }
            }
            if (tree.Dimension == Dimension.Green && Input.GetKeyDown(KeyCode.E)) {
                if (tree.Apple > 0)
                {
                    GreenApple++;
                    tree.Apple--;
                    GreenAppleCounter.text = GreenApple.ToString();
                    Debug.Log("Green Apple : " + GreenApple);
                }
            } 
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Red Apple : " + RedApple);
        Debug.Log("Blue Apple : " + BlueApple);
        Debug.Log("Green Apple : " + GreenApple);
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
