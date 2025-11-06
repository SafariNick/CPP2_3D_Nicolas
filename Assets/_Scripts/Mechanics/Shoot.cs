using UnityEngine;

public class Shoot : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Vector2 initShotVelocity = Vector2.zero; //bullet speed
    
    [SerializeField] private Projectile misslePrefab = null;
    [SerializeField] private Projectile bigBulletPrefab = null;
    [SerializeField] private Projectile smallBulletPrefab = null;
    //[SerializeField] private Transform rightSpawn;
    [SerializeField] private Transform leftSpawn;
    [SerializeField] private Transform currentSpawn;
    [SerializeField] private Transform missleSpawn;
    [SerializeField] private Transform bigBulletSpawn;
    [SerializeField] private Transform smallBulletSpawn;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (initShotVelocity == Vector2.zero)
        {
            initShotVelocity = new Vector2(10f, 0f);
            Debug.LogWarning("Initial shot velocity not set, using default value: " + initShotVelocity);
        }
        if (currentSpawn == null)
        {
            Debug.LogError("Spawn points not set. Please assign leftSpawn and rightSpawn in the inspector.");
        }

        if (misslePrefab == null || bigBulletPrefab == null || smallBulletPrefab == null)
        {
            Debug.LogError("Projectile prefabs not assigned. Please assign misslePrefab, bigBulletPrefab, and smallBulletPrefab in the inspector.");
        }
    }

    public void FireMissle()
    {
        currentSpawn = missleSpawn;
        Fire(misslePrefab, currentSpawn);
    }


    public void FireBigBullet()
    {
        currentSpawn = bigBulletSpawn;
        Fire(bigBulletPrefab, currentSpawn);
    }

    public void FireSmallBullet()
    {
        currentSpawn = smallBulletSpawn;
        Fire(smallBulletPrefab, currentSpawn);
    }
    private void Fire(Projectile projectileToFire, Transform Spawn )
    {
        Projectile curProjectile;

        if (!sr.flipX)
        {
            curProjectile = Instantiate(projectileToFire, currentSpawn.position, Quaternion.identity);
            curProjectile.SetVelocity(initShotVelocity);
        }
        else
        {
            currentSpawn = leftSpawn; // Assuming leftSpawn is the spawn point for flipped shots
            curProjectile = Instantiate(projectileToFire, currentSpawn.position, Quaternion.identity);
            curProjectile.SetVelocity(new Vector2(-initShotVelocity.x, initShotVelocity.y));
            //slow the bullet if going left
            curProjectile.SetVelocity(new Vector2(-initShotVelocity.x * 0.1f, initShotVelocity.y));
        }
    }
}