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

    public bool isPowerup;

    public Animator playerAnim;

    public AudioSource playerAudio;
    public AudioClip jumpFx;
    public AudioClip crashFx;

    public ParticleSystem dirtParticle;
    public ParticleSystem explosionParticle;

    // feature เพิ่มความเร็ว, อมตะ
    public MoveLeft bg;
    public float boostSpeed = 15f;
    private float normalSpeed;
    private bool isImmune = false;
    private float immuneTime = 0f;


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

        normalSpeed = bg.speed;
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
        if (isImmune)
        {
            immuneTime -= Time.deltaTime;
            if (immuneTime <= 0f)
            {
                isImmune = false;
                bg.speed = normalSpeed;
            }
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
            if (isImmune)
            {
                collision.gameObject.GetComponent<DestroyOutOfBound>().ReturnObj();
                return;
            }
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedPowerup"))
        {
            ActivateImmuneBoost();
            Destroy(other.gameObject);
        }
    }
    void ActivateImmuneBoost()
    {
        isImmune = true;
        immuneTime = 5f;
        bg.speed = boostSpeed;
    }
}
