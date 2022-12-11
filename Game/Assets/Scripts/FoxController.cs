using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] public float jumpForce = 6f;
    public float przesuniecieX = 0.37f; 
    public float przesuniecieY = -0.82f; 
    public LayerMask groundLayer;
    float rayLength = 0.8f;
    bool isFacingRight = true;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private bool isWalking = false;
    int score = 0;

    int lives = 3;
    Vector2 startPosition;

    int keysFound = 0;
    const int keysNumber = 3;

    private void Awake()
    {
        startPosition = transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        
        isWalking = false;
        float horizontalValue = Input.GetAxis("Horizontal");
        if (horizontalValue != 0)
        {
            if (horizontalValue < 0 && isFacingRight == true)
                Flip();
            if (horizontalValue > 0 && isFacingRight == false)
                Flip();
            isWalking = true;
            float moveSpeed = horizontalValue * horizontalSpeed * Time.deltaTime;
            transform.Translate(moveSpeed, 0, 0, Space.World);
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        animator.SetBool("isGrounded", isGrounded());
        animator.SetBool("Walking", isWalking);
        Debug.DrawRay(transform.position + new Vector3(przesuniecieX, przesuniecieY, 0), rayLength * Vector2.left, Color.white, 1, false);
    }

    bool isGrounded()
    {
        return Physics2D.Raycast(this.transform.position + new Vector3(przesuniecieX, przesuniecieY,0), Vector3.left, rayLength, groundLayer.value);
    }

    void Jump()
    {
        if(isGrounded())
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Death()
    {
        this.transform.position = startPosition;
        lives--;
        Debug.Log("Fall out of map. Reset position.");
        Debug.Log("Lives: " + lives);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Bonus"))
        {
            score++;
            Debug.Log("Score: " + score);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Enemy") && this.transform.position.y > other.gameObject.transform.position.y)
        {
            score++;
            Debug.Log("Killed an enemy");
            Debug.Log("Score: " + score);
        }
        if (other.CompareTag("Enemy") && this.transform.position.y <= other.gameObject.transform.position.y)
        {
            lives--;
            if (lives > 0)
            {
                Debug.Log("Lives: " + lives);
                this.transform.position = startPosition;
            }
            else
            {
                Debug.Log("End game");
            }
            
        }
        if (other.CompareTag("Key"))
        {
            keysFound++;
            Debug.Log("Keys found: " + keysFound);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("ExtraLive"))
        {
            lives++;
            Debug.Log("Lives: " + lives);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("FallLevel"))
        {
            Death();
        }
    }
}
