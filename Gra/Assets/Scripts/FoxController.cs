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
    [SerializeField] public GameObject block;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private bool isWalking = false;

    [SerializeField] public AudioClip bSound;
    [SerializeField] public AudioClip kill;
    [SerializeField] public AudioClip dead;
    [SerializeField] public AudioClip win;
    [SerializeField] public AudioClip keySound;
    [SerializeField] public AudioClip jump;
    [SerializeField] public AudioClip open;
    AudioSource source;

    int lives = 3;
    Vector2 startPosition;

    int keysFound = 0;
    const int keysNumber = 3;

    private void Awake()
    {
        startPosition = transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
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

            if ( Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }
            animator.SetBool("isGrounded", isGrounded());
            animator.SetBool("Walking", isWalking);
            Debug.DrawRay(transform.position + new Vector3(przesuniecieX, przesuniecieY, 0), rayLength * Vector2.left, Color.white, 1, false);
        }
        
    }

    bool isGrounded()
    {
        return Physics2D.Raycast(this.transform.position + new Vector3(przesuniecieX, przesuniecieY,0), Vector3.left, rayLength, groundLayer.value);
    }

    void Jump()
    {
        if (isGrounded())
        {
            source.PlayOneShot(jump, AudioListener.volume);
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
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
        GameManager.instance.StoleLives();
        Debug.Log("Fall out of map. Reset position.");
        Debug.Log("Lives: " + lives);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Bonus"))
        {
            other.gameObject.SetActive(false);
            source.PlayOneShot(bSound, AudioListener.volume);
            GameManager.instance.AddPoints(1);
        }
        if (other.CompareTag("Enemy") && this.transform.position.y > other.gameObject.transform.position.y)
        {
            GameManager.instance.AddPoints(1);
            GameManager.instance.AddDeads(1);
            source.PlayOneShot(kill, AudioListener.volume);
            Debug.Log("Killed an enemy");
        }
        if (other.CompareTag("Enemy") && this.transform.position.y <= other.gameObject.transform.position.y)
        {
            GameManager.instance.StoleLives();
            if (lives > 0)
            {
                source.PlayOneShot(dead, AudioListener.volume);
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
            source.PlayOneShot(keySound, AudioListener.volume);
            GameManager.instance.AddKeys();
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("ExtraLive"))
        {

            GameManager.instance.AddLives();
            Debug.Log("Lives: " + lives);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("FallLevel"))
        {
            GameManager.instance.AddDeads(1);
            Death();
        }
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
        if (other.CompareTag("Exit"))
        {
            if (GameManager.instance.getKeys() == 3)
            {
                source.PlayOneShot(win, AudioListener.volume);
                GameManager.instance.AddPoints(100* GameManager.instance.getLives());
                GameManager.instance.LevelCompleted();
            }
        }
        if (other.CompareTag("lever"))
        {
            if (GameManager.instance.getKeys() == 3)
            {
                source.PlayOneShot(open, AudioListener.volume);
                block.SetActive(false);
            }
        }
        if (other.CompareTag("DeadZone"))
        {
                source.PlayOneShot(dead, AudioListener.volume);
                Debug.Log("Lives: " + lives);
                this.transform.position = startPosition;
        }
    }
}
