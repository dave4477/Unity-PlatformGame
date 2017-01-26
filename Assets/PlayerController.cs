using UnityEngine;
using UnityEngine.UI;

/**
 * The Player class. This class controls the player.
 * TODO: Clean this up as it's too big, messy and tangled with dependencies.
 */
public class PlayerController : MonoBehaviour
{
    public int score = 0;
    public int numLives = 4;

    public float energy = 10f;
    public float maxEnergy = 10f;
    public float speedX;
    public float speedY;
    public float jumpStrength;
    public float gravity;

    float tmpSpeed;

    // Character movement flags.
    int direction = 1;
    public bool isIdle;
    public bool isWalking;
    public bool isJumping;
    public bool isOnLadder;
    public bool isAirborne;
    public int currentState;

    public bool walkRightPressed;
    public bool walkLeftPressed;
    public bool jumpPressed;
    public bool fightPressed;
    public bool upPressed;
    public bool downPressed;
    public bool buttonReleased;

    int charIdle = 0;
    int charWalk = 1;
    int charJump = 2;
    int charKick = 3;
    int charLowKick = 4;
    int charHighPunch = 5;
    int charDie = 13;

    SharedObject so;

    Animator animator;

    // Camera related.
    Vector3 startingPos;

    [Range(0,10)]
    public float cameraFollowSpeed = 2f;

    [SerializeField]
    Camera stageCamera;

    [SerializeField]
    PlatformEffector2D platformEffector;

    [SerializeField]
    GameObject scoreText;
    
	// Use this for initialization.
	void Start ()
    {
        tmpSpeed = speedX;
        animator = GetComponentInChildren<Animator>();
        startingPos = transform.position;
        GetComponent<Rigidbody2D>().gravityScale = gravity;
        LoadScore();
    }

    public void LoadScore()
    {
        int myscore = int.Parse(SharedObject.GetString("score"));
        score = myscore;
        scoreText.GetComponent<Text>().text = score + "";
    }

    public void SetScore()
    {
        score++;
        scoreText.GetComponent<Text>().text = score +"";
        SharedObject.SetString("score", score.ToString());
    }

    public void pressWalkRight()
    {
        buttonReleased = false;
        walkLeftPressed = false;
        walkRightPressed = true;
        fightPressed = false;
        upPressed = false;
        downPressed = false;
    }

    public void pressWalkLeft()
    {
        buttonReleased = false;
        walkRightPressed = false;
        walkLeftPressed = true;
        fightPressed = false;
        upPressed = false;
        downPressed = false;
    }

    public void pressDown()
    {
        buttonReleased = false;
        walkRightPressed = false;
        walkLeftPressed = false;
        fightPressed = false;
        upPressed = false;
        downPressed = true;
    }

    public void pressUp()
    {
        buttonReleased = false;
        walkRightPressed = false;
        walkLeftPressed = false;
        fightPressed = false;
        upPressed = true;
        downPressed = false;
    }

    public void Fight()
    {
        buttonReleased = false;
        walkRightPressed = false;
        walkLeftPressed = false;
        fightPressed = true;
        jumpPressed = false;
        upPressed = false;
        downPressed = false;
    }

    public void pressReleased()
    {
        walkRightPressed = false;
        walkLeftPressed = false;
        buttonReleased = true;
        fightPressed = false;
        jumpPressed = false;
        upPressed = false;
        downPressed = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKey(KeyCode.RightArrow) || walkRightPressed)
        {
            WalkRight();
        }

        if (Input.GetKey(KeyCode.LeftArrow) || walkLeftPressed)
        {
            WalkLeft();
        }

        if (Input.GetKey(KeyCode.UpArrow) || upPressed)
        {
            if (isOnLadder)
            {
                ChangePlatformEffector(true, "Player");
                WalkUp();
            }
        }

        if (((isWalking || isIdle) && !isJumping && Input.GetKey(KeyCode.Space)) || ((isWalking || isIdle) && !isJumping && jumpPressed))
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.DownArrow) || downPressed)
        {
            if (isOnLadder)
            {
                ChangePlatformEffector(false, "Player");
                WalkDown();
            }
        }

        if ((!isJumping && !isOnLadder && Input.GetKey(KeyCode.LeftControl)) || !isJumping && !isOnLadder && fightPressed)
        {
            speedX = 0;

            float myRand = Random.Range(0, 2);

            if (myRand == 0)
            {
                MiddleKick();
            }
            else
            {
                HighPunch();
            }
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftControl) || buttonReleased)
        {
            speedX = tmpSpeed;
            DoKeyUp();
        }

        var currScale = transform.localScale;
        currScale.x = direction;
        transform.localScale = currScale;

        CheckEnergy();

    }

    void ChangePlatformEffector(bool isEnabled, string layerName)
    {
        if (!platformEffector) return;

        if (isEnabled)
        {
            platformEffector.colliderMask |= (1 << LayerMask.NameToLayer(layerName));

        }
        else
        {
            platformEffector.colliderMask ^= (1 << LayerMask.NameToLayer(layerName));
        }
    }

    void CheckEnergy()
    {
        if (energy <= 0)
        {
            PlayerDie();
        }
    }

    public void WalkRight()
    {
        direction = 1;

        if (!isJumping)
        {
            PlayAnimation(charWalk);
        }
        transform.Translate(new Vector3(speedX * Time.deltaTime, 0, 0));
    }

    public void WalkLeft()
    {
        direction = -1;

        if (!isJumping)
        {
            PlayAnimation(charWalk);
        }
        transform.Translate(new Vector3(-speedX * Time.deltaTime, 0, 0));
    }

    public void WalkUp()
    {
        PlayAnimation(charWalk);
        transform.Translate(new Vector3(0, speedY * Time.deltaTime, 0));
    }

    public void WalkDown()
    {
        PlayAnimation(charWalk);
        transform.Translate(new Vector3(0, -speedY * Time.deltaTime, 0));
    }

    public void Jump()
    {
        if (!isJumping)
        {
            PlayAnimation(charJump);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    void LowKick()
    {
        PlayAnimation(charLowKick);
    }

    void MiddleKick()
    {
        PlayAnimation(charKick);
    }

    void HighPunch()
    {
        PlayAnimation(charHighPunch);
    }

    void PlayAnimation(int anim)
    {
        animator.SetInteger("State", anim);

        switch (anim)
        {
            case 0:
                isIdle = true;
                isWalking = false;
                break;

            case 1:
                isIdle = false;
                isWalking = true;
                break;

            case 2:
                isIdle = false;
                break;

            case 3:
                isIdle = false;
                isWalking = false;
                break;

            case 4:
                isIdle = false;
                isWalking = false;
                break;

            default:
                break;
        }
        currentState = anim;
    }

    void LateUpdate()
    {
        CheckCamera();
    }

    void CheckCamera()
    {
        var playerPos = transform.position;

        Vector3 cameraPosFrom = stageCamera.transform.position;
        Vector3 cameraPosTo = new Vector3(playerPos.x, playerPos.y, stageCamera.transform.position.z);
        Vector3 result = Vector3.Lerp(cameraPosFrom, cameraPosTo, cameraFollowSpeed * Time.deltaTime);
        if (result.y < -2.8f)
        {
            result.y = -2.8f;
        }
        stageCamera.transform.position = result;
    }

    void DoKeyUp()
    {
        // Stopped.
        PlayAnimation(charIdle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            platformEffector = other.gameObject.transform.parent.parent.GetComponent<PlatformEffector2D>();
            isOnLadder = true;
            GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
        if (other.gameObject.tag == "NinjaStar")
        {
            energy -= other.GetComponent<NinjaStar>().damage;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            isOnLadder = false;
            GetComponent<Rigidbody2D>().gravityScale = gravity;
            platformEffector = null;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        isJumping = false;

        if (other.gameObject.tag == "Abbys")
        {
            PlayerDie();
        }
        if (other.gameObject.tag == "NextLevel")
        {
            Application.LoadLevel("Level02");
        }
        PlayAnimation(charIdle);
    }

    public void PlayerDie()
    {
        transform.position = startingPos;
        energy = 10f;
        numLives--;
        if (numLives == 0)
        {
            Application.LoadLevel("Level01");
        }
    }
}
