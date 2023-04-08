using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    float moveX = 0f;
    float moveZ = 0f;
    [SerializeField]
    private Sprite red;
    [SerializeField]
    private Sprite blue;
    [SerializeField]
    private Sprite green;
    private SpriteRenderer Sprite;
    public GameObject AppleBody;
    // Start is called before the first frame update
    public void Setup(Dimension dimension, Direction direction)
    {
        print(dimension);
        print(red);
        print(green);
        print(blue);
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

        if (direction == Direction.Up) moveZ = +1f;
        if (direction == Direction.Down) moveZ = -1f;
        if (direction == Direction.Left) moveX = -1f;
        if (direction == Direction.Right) moveX = +1f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = new Vector3(moveX, 0, moveZ).normalized;
        float moveSpeed = 30f;
        AppleBody.transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
