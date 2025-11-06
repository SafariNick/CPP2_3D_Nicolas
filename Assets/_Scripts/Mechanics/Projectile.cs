using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public ProjectileType projectileType;

    [SerializeField, Range(1, 20)] private float lifetime = 1.0f;
    [SerializeField] private float bulletVFXLifetime = 0.5f; // Lifetime of the bullet VFX in seconds
    [SerializeField] private float damage = 5;
    [SerializeField] private GameObject bulletPrefab; // Prefab for the bullet explosion VFX

    public void SetVelocity(Vector2 velocity) => GetComponent<Rigidbody2D>().linearVelocity = velocity;




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (projectileType == ProjectileType.SmallBullet)
        {
            // Handle player projectile hitting an enemy
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // Adjust damage value as necessary
                GameManager.Instance.score += 1;

            }
        }
        if (projectileType == ProjectileType.BigBullet)
        {
            // Handle player projectile hitting an enemy
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(5); // Adjust damage value as necessary
                GameManager.Instance.score += 10;

            }
        }
        if (projectileType == ProjectileType.Missle)
        {
            // Handle player projectile hitting an enemy
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(5); // Adjust damage value as necessary
                GameManager.Instance.score += 30;

            }
        }

        if (projectileType == ProjectileType.Enemy && collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.lives--;
            GameManager.Instance.score -= 100;
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        CancelInvoke(nameof(Expire));
        Invoke(nameof(Expire), lifetime);
    }
    private void OnDisable()
    {
        CancelInvoke(nameof(Expire));
    }
    private void Expire()
    {
        if (bulletPrefab != null)
        {
            var bulletVFX = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            if (bulletVFXLifetime > 0f) Destroy(bulletVFX, bulletVFXLifetime);

        }


        Destroy(gameObject);
    }
}

public enum ProjectileType
{

    Enemy,
    Missle,
    BigBullet,
    SmallBullet
}