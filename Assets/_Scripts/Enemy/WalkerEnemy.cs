using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkerEnemy : Enemy
{
    private Rigidbody2D rb;
    [SerializeField, Range(1f, 10f)] private float xVelocity = 2f;
   //[SerializeField, Range(1f, 10f)] private float yVelocity = 0f;
   // start walking velocity left


    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }
    void Update()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Walk"))
        {
            rb.linearVelocityX = (sr.flipX) ? xVelocity : -xVelocity;
        }

    }
    public override void TakeDamage(int damageValue, DamageType damageType = DamageType.Default)
    {
        if (damageType == DamageType.JumpedOn)
        {
            //anim.SetTrigger("Squish");
            // Destroy the enemy after the death animation is complete
            Destroy(transform.parent.gameObject, 0.5f); // Adjust the delay as needed for the animation
            return;
        }

        base.TakeDamage(damageValue, damageType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.CompareTag("Barrier"))
            {
                //anim.SetTrigger("Turn");
                sr.flipX = !sr.flipX;
            }
    }
}
