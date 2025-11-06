using UnityEngine;

public class TurretEnemy : Enemy
{
    [SerializeField] private Transform Player;
    [SerializeField] private float fireRate = 2.0f; // Time between shots
    private float timeSinceLastShot = 0.0f;
    [SerializeField] private float detectionRadius = 10.0f; // Detection range for the player


    bool isPlayerInRange = false; // Flag to track if the player is in range


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        // Additional initialization for TurretEnemy can go here

        GameManager.Instance.OnPlayerControllerCreated += (playerController) => Player = playerController.transform;

        if (fireRate <= 0)
        {
            Debug.LogError("Fire rate must be greater than 0. Setting to default value of 2.0f.");
            fireRate = 2.0f;
        }
    }
    private void FacePlayer ()
    {
        if (Player != null)
        {
            if (Player.position.x < transform.position.x)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        EnemyFire();
    }
    public void isPlayerInRange2 ( bool inRange)
    {
      
        if (Player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, Player.position);
        }
       

    }

    public void EnemyFire()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        FacePlayer();

        if (stateInfo.IsName("TurretIdle"))
        {
            // Fix: Check if player is within detectionRadius
            if (Player != null && Vector2.Distance(transform.position, Player.position) <= detectionRadius)
            {
                anim.SetTrigger("isPlayerInRange");
                timeSinceLastShot = Time.time; // Reset the timer after firing
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
