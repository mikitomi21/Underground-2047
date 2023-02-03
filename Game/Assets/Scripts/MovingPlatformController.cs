using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    bool isFacingRight = true;
    [SerializeField] private float horizontalSpeed = 4f;
    float startPositionX;
    float moveRange = 5.0f;
    bool isMovingRight = true;

    private Animator animator;
    private Rigidbody2D rigidbody;


    private void Awake()
    {
        startPositionX = this.transform.position.x;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isMovingRight)
        {
            if (this.transform.position.x <= (startPositionX + moveRange))
            {
                MoveRight();
            }
            else
            {
                Flip();
                MoveLeft();
            }
        }
        else
        {
            if (this.transform.position.x > startPositionX)
            {
                MoveLeft();
            }
            else
            {
                Flip();
                MoveRight();
            }
        }
    }
    void MoveRight()
    {
        float moveSpeed = horizontalSpeed * Time.deltaTime;
        transform.Translate(moveSpeed, 0, 0, Space.World);
    }

    void MoveLeft()
    {
        float moveSpeed = (-1) * horizontalSpeed * Time.deltaTime;
        transform.Translate(moveSpeed, 0, 0, Space.World);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        isMovingRight = !isMovingRight;
    }
}
