using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 5f;
    private Transform player;
    private Renderer[] renderers;
    private MaterialPropertyBlock propertyBlock;
    private Color originalColor;

    private float alpha = 1f;
    private float targetAlpha = 1f;

    [Range(0f, 1f)] public float initialTransparency = 0.3f;

    // Health
    public int maxHealth = 100;
    private int currentHealth;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (renderers.Length > 0)
        {
            originalColor = renderers[0].sharedMaterial.GetColor("_BaseColor");
            originalColor.a = initialTransparency;
            alpha = targetAlpha = initialTransparency;
            ApplyAlpha(alpha);
        }
    }

    private void Update()
    {
        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;

        // Smoothly rotate to face player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);

        // Adjust visibility based on facing player or facing away from player
        float dot = Vector3.Dot(transform.forward, direction);
        targetAlpha = Mathf.Lerp(0.2f, 1f, (dot + 1f) / 2f); // More transparent when facing away





        // Smooth transition
        alpha = Mathf.Lerp(alpha, targetAlpha, Time.deltaTime * 2f);
        ApplyAlpha(alpha);
    }

    private void ApplyAlpha(float alphaValue)
    {
        if (renderers == null) return;

        foreach (var r in renderers)
        {
            r.GetPropertyBlock(propertyBlock);
            Color color = originalColor;
            color.a = alphaValue;
            propertyBlock.SetColor("_BaseColor", color);
            r.SetPropertyBlock(propertyBlock);
        }
    }

    // --- Health System ---
    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}