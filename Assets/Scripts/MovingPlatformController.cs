using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 10f;
    private bool isMovingRight = false;
    public float moveRange = 10.0f;
    private float startPositionX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void MoveRight()
    {
        float moveSpeed = horizontalSpeed * Time.deltaTime;
        this.transform.Translate(moveSpeed, 0, 0, Space.World);
    }

    private void MoveLeft()
    {
        float moveSpeed = horizontalSpeed * Time.deltaTime;
        this.transform.Translate(-moveSpeed, 0, 0, Space.World);
    }

    private void Awake()
    {
        startPositionX = this.transform.position.x;
        //rigidbody = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            if (this.transform.position.x < startPositionX + moveRange)
                MoveRight();
            else
            {
                isMovingRight = !isMovingRight;
                MoveLeft();
            }
        }
        else
        {
            if (this.transform.position.x > startPositionX - moveRange)
                MoveLeft();
            else
            {
                isMovingRight = !isMovingRight;
                MoveRight();
            }
        }
    }
}
