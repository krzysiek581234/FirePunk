using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] public float jumpForce = 8f;
    public LayerMask groundLayer;
    float rayLength = 0.85f;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private bool isWalking = false;
    private float yVelocity = 0.0f;
    public int lives = 3;
    public Vector2 startPosition;
    private Vector2 velocity;
    private bool isFacingRight = true;
    const int keysNumber = 3;

    public AudioClip bSound;
    private AudioSource source;
    public AudioClip EnemyCollisionSound;
    public AudioClip deadSound;
    public AudioClip heartSound;
    public AudioClip diamondSound;
    public AudioClip FinishSound;
    public AudioClip GameOverSound;
    public AudioClip FallDeathSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
        if (other.CompareTag("Bonus"))
        {
            GameManager.instance.AddPoints(5);
            other.gameObject.SetActive(false);
            source.PlayOneShot(bSound, AudioListener.volume);
        }
        if (other.CompareTag("Heart"))
        {
            GameManager.instance.AddLives();
            source.PlayOneShot(heartSound, AudioListener.volume);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Key"))
        {
            GameManager.instance.AddKeys();
            source.PlayOneShot(diamondSound, AudioListener.volume);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Finish"))
        {
            if(GameManager.instance.keysFound == keysNumber)
            {
                source.PlayOneShot(FinishSound, AudioListener.volume);
                GameManager.instance.AddPoints(100 * GameManager.instance.lives);
                GameManager.instance.LevelCompleted();
            }
            else
            Debug.Log("You must find " + (keysNumber - GameManager.instance.keysFound) + " more keys!");
            //other.gameObject.tag = "Default";
        }
        if (other.CompareTag("Enemy"))
        {
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                GameManager.instance.AddKill();
                source.PlayOneShot(EnemyCollisionSound, AudioListener.volume);
            }
            else
            {
                source.PlayOneShot(deadSound, AudioListener.volume);
                Death();
            }
        }
        if(other.CompareTag("FallLevel"))
        {
            source.PlayOneShot(FallDeathSound, AudioListener.volume);
            Death();
        }
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    private void Death()
    {
        GameManager.instance.SubtractLives();
        if (GameManager.instance.currentGameState != GameManager.GameState.GS_GAME_OVER)
        {
            transform.position = startPosition;
            //source.PlayOneShot(deadSound, AudioListener.volume);
        }
            
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Update()
    {

        float horizontalValue;
        if (GameManager.instance.currentGameState == GameManager.GameState.GS_GAME)
        {
            velocity = rigidbody.velocity;
            yVelocity = velocity.y;
            isWalking = false;
            horizontalValue = Input.GetAxis("Horizontal");
            if (horizontalValue != 0)
            {
                isWalking = true;

                if ((horizontalValue > 0 && isFacingRight == false) || (horizontalValue < 0 && isFacingRight == true))
                    Flip();

                float moveSpeed = horizontalValue * horizontalSpeed * Time.deltaTime;
                transform.Translate(moveSpeed, 0, 0, Space.World);
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }
            animator.SetBool("isGrounded", isGrounded());
            animator.SetBool("isWalking", isWalking);
            animator.SetFloat("ySpeed", yVelocity);
            if (yVelocity == 0)
                animator.SetBool("notMidAir", true);
            else
                animator.SetBool("notMidAir", false);
        }
        
        //Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);
    }

    bool isGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    void Jump()
    {
        if(isGrounded())
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }  
    }
}
