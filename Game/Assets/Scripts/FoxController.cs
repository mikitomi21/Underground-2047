using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoxController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 4f;
    public float jumpForce = 1f;
    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
            transform.Translate(moveSpeed * Time.deltaTime, 0f, 0f, Space.World);
        if(Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(-moveSpeed * Time.deltaTime, 0f, 0f, Space.World);

        if (Input.GetKey("up"))
            rigidBody.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);

    }


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

}
