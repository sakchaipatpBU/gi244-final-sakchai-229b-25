using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float gravityMultiplier = 1f;
    private Rigidbody rb;
    private InputAction jumpAction;
    private bool isOnGround = true;
    public bool isGameOver = false;

    public Animator playerAnim;

    public AudioSource playerAudio;
    public AudioClip jumpFx;
    public AudioClip crashFx;

    public ParticleSystem dirtParticle;
    public ParticleSystem explosionParticle;


    private static PlayerController instance;
    public static PlayerController GetInstance()
    {
        return instance;
    }
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        rb = GetComponent<Rigidbody>();
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log(Physics.gravity);
        Physics.gravity *= gravityMultiplier;

        playerAnim.SetFloat("Speed_f", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpAction.triggered && isOnGround && isGameOver == false)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            playerAudio.PlayOneShot(jumpFx, 1f);
            dirtParticle.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();

        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAudio.PlayOneShot(crashFx, 1f);
            dirtParticle.Stop();
            explosionParticle.Play();

            // หยุดและบันทึกคะแนน
            Score sc = Score.GetInstance();
            sc.StopScore();
            float score = sc.score;

            GameManager gm = GameManager.GetInstance();
            gm.SaveScore(score);
            gm.gameOverScreen.SetActive(true);
        }
    }
}
