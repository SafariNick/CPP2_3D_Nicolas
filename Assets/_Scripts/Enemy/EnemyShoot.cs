using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    //[Header("Setup")]
    //public Transform player;             // assign in Inspector
    //public Transform firePoint;          // empty child where bullets spawn
    //public Projectile projectilePrefab;  // must have a Rigidbody + Collider
    //public float projectileSpeed = 20f;
    //public float shootInterval = 1.25f;
    //public float muzzleOffset = 0.2f;    // spawn slightly in front of the muzzle

    //[Header("Ignore Self Collision (optional)")]
    //public Collider[] shooterColliders;  // assign all colliders on this enemy

    //float timer;

    //void Update()
    //{
    //    if (!player) return;

    //    timer += Time.deltaTime;
    //    if (timer >= shootInterval)
    //    {
    //        Shoot();
    //        timer = 0f;
    //    }
    //}

    //void Shoot()
    //{
    //    // Direction to player
    //    Vector3 dir = (player.position - firePoint.position).normalized;

    //    // Spawn slightly ahead so it doesn't overlap the muzzle collider
    //    Vector3 spawnPos = firePoint.position + dir * muzzleOffset;

    //    // IMPORTANT: do NOT pass a parent argument to Instantiate.
    //    GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(dir));

    //    // Make 100% sure it isn't parented
    //    proj.transform.SetParent(null);

    //    // Rigidbody setup
    //    var rb = proj.GetComponent<Rigidbody>();
    //    if (!rb)
    //    {
    //        Debug.LogError("Projectile prefab needs a Rigidbody.");
    //        return;
    //    }

    //    rb.isKinematic = false;
    //    rb.useGravity = false;
    //    rb.constraints = RigidbodyConstraints.None;
    //    rb.drag = 0f;                 // high drag makes it look stuck
    //    rb.angularDrag = 0.05f;
    //    rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    //    rb.interpolation = RigidbodyInterpolation.Interpolate;

    //    // Kick it
    //    rb.velocity = dir * projectileSpeed;

    //    // Avoid hitting our own colliders on frame 1
    //    var projCol = proj.GetComponent<Collider>();
    //    if (projCol && shooterColliders != null)
    //    {
    //        foreach (var c in shooterColliders)
    //            if (c) Physics.IgnoreCollision(projCol, c, true);
    //    }

    //    // Optional debug to verify motion
    //    Debug.DrawRay(spawnPos, rb.velocity * 0.1f, Color.red, 1f);
    //    // Debug.Log("Projectile velocity: " + rb.velocity);
    //}
}