using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoxController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 4f;
    public float jumpForce = 6f;
    private Rigidbody2D rigidBody;
    public LayerMask groundLayer;
    const float rayLenght= 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveDir = Input.GetAxis("Horizontal");
        transform.Translate(moveDir * Time.deltaTime * moveSpeed, 0f, 0f, Space.World);

        if (Input.GetKeyDown("up"))
            Jump();


        Debug.DrawRay(transform.position, rayLenght * Vector3.down, Color.white, 1, false);
    }


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    bool isGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLenght, groundLayer.value);
    }

    void Jump()
    {
        if (isGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("jumping");
        }
    }
}
