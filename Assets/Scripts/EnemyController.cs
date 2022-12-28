using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 10f;
    private Animator animator;
    private Rigidbody2D rigidbody;
    private bool isFacingRight = false;
    private bool isMovingRight = false;
    public float moveRange = 10.0f;
    private float startPositionX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.transform.position.y > this.transform.position.y)
            {
                horizontalSpeed = 0.0f;
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
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
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    // Update is called once per frame
    void Update()
    {
        if(isMovingRight)
        {
            if (this.transform.position.x < startPositionX + moveRange)
                MoveRight();
            else
            {
                Flip();
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
                Flip();
                isMovingRight = !isMovingRight;
                MoveRight();
            }
        }
    }
}
