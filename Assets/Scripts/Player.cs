using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI UIText;
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
        if (Input.GetKey(KeyCode.W)) moveZ = +1f;
        if (Input.GetKey(KeyCode.S)) moveZ = -1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = +1f;
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
                    Debug.Log("Red Apple : " + RedApple);
                }
            }
            if (tree.Dimension == Dimension.Blue && Input.GetKeyDown(KeyCode.E)) {
                if (tree.Apple > 0)
                {
                    BlueApple++;
                    tree.Apple--;
                    Debug.Log("Blue Apple : " + BlueApple);
                }
            }
            if (tree.Dimension == Dimension.Green && Input.GetKeyDown(KeyCode.E)) {
                if (tree.Apple > 0)
                {
                    GreenApple++;
                    tree.Apple--;
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
