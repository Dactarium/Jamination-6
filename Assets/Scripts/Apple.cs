using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    float moveX = 0f;
    float moveZ = 0f;
    public string ThrowDirection;
    private bool throwApple = false;
    public GameObject AppleBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) throwApple = true;
        if (throwApple == true)
        {
            if (ThrowDirection == "Up") moveZ = +1f;
            if (ThrowDirection == "Down") moveZ = -1f;
            if (ThrowDirection == "Left") moveX = -1f;
            if (ThrowDirection == "Right") moveX = +1f;
            Vector3 moveDir = new Vector3(moveX, 0, moveZ).normalized;
            float moveSpeed = 30f;
            AppleBody.transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

    }
}
