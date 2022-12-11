using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    bool isFacingRight = true;
    [SerializeField] private float horizontalSpeed = 4f;
    private Animator animator;
    private Rigidbody2D rigidbody;

    float startPositionX;
    float moveRange = 5.0f;

    bool isMovingRight = true;

    void MoveRight()
    {
        float moveSpeed = horizontalSpeed * Time.deltaTime;
        transform.Translate(moveSpeed, 0, 0, Space.World);
    }

    void MoveLeft()
    {
        float moveSpeed = (-1)* horizontalSpeed * Time.deltaTime;
        transform.Translate(moveSpeed, 0, 0, Space.World);
    }

    private void Awake()
    {
        startPositionX = this.transform.position.x;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            if(this.transform.position.x <= (startPositionX + moveRange))
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

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        isMovingRight = !isMovingRight;
    }

    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.gameObject.transform.position.y > this.transform.position.y)
            {
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }

    }
}
