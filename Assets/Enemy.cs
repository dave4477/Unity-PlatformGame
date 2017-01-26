using UnityEngine;
using System.Collections;
using Assets;

/**
 * The Enemy.
 * TODO: Clean this up as it's too big, messy and tangled with dependencies.
 */
public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField]
    public float interval = 0.5f;

    public float dieInterval = 1f;
    public NinjaStar ninjaStar;
    public int energy;

    public float speedX;
    public float speedY;
    public float jumpStrength;
    public float gravity;
    public float flipOffset = 0.3f;
    Vector2 bounds;

    float orgSpeedX;

    // Character movement flags.
    int direction = 1;

    bool isDead;

    int charDie = 13;

    Animator animator;

    // Use this for initialization.
    void Start ()
    {
        bounds = GetComponent<BoxCollider2D>().bounds.size; 
        orgSpeedX = speedX;
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("State", 1);
        StartCoroutine(ThrowStars());
    }

    // Update is called once per frame.
    void Update ()
    {
        if (!isDead)
        {
            transform.Translate(new Vector3(speedX * Time.deltaTime, 0, 0));
            CheckEnergy();
        }
    }

    void FixedUpdate()
    {
        Vector2 enemyPos = this.transform.position;
        float offset = (bounds.x + flipOffset) / 2;
        enemyPos.x += offset * direction; 
        RaycastHit2D hit = Physics2D.Raycast(enemyPos, Vector2.down, (bounds.y + 1) / 2);
        Debug.DrawLine(enemyPos, enemyPos + (Vector2.down * ((bounds.y + 1) / 2)));
        if (hit)
        {
            if (hit.transform.tag != "Platform")
            {
            }
        }
        else
        {
            Flip();
            if (!isDead)
            {
                //StartCoroutine(ThrowStars());
            }
        }
    }

    public void CheckEnergy()
    {
        if (energy < 1)
        {
            PlayAnimation(charDie);
        }
    }

    void Flip()
    {
        direction *= -1;

        var currScale = transform.localScale;
        currScale.x = direction;
        transform.localScale = currScale;
        speedX *= -1;
    }

    IEnumerator ThrowStars()
    {
        yield return new WaitForSeconds(interval);

        if (!isDead)
        {
            ninjaStar.Throw(direction);
            StartCoroutine(ThrowStars());
        }
    }

    IEnumerator EnemyDie()
    {
        yield return new WaitForSeconds(dieInterval);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.gameObject.tag != "Player" || other.gameObject.tag != "NinjaStar")
        {
        }
    }

    public void PlayAnimation(int anim)
    {
        if (anim == 4) anim = 5;

        animator.SetInteger("State", anim);

        switch (anim)
        {
            case 1:
                break;

            case 13:
                isDead = true;
                GetComponent<Rigidbody2D>().isKinematic = false;
                StartCoroutine(EnemyDie());
                break;

            default:
                break;
        }
    }
}
