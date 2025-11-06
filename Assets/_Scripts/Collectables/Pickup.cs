using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public AudioClip pickupSound;
    protected AudioSource audioSource;
    //Defined in child classes, this method is called when the player picks up the item
    abstract public void OnPickup();
    public virtual void Start()
    {
        if (pickupSound != null)
        {
            TryGetComponent(out audioSource);
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = GameManager.Instance.sfxMixerGroup;
                Debug.LogWarning("AudioSource component missing. Added one dynamically.");
            }
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPickup();
            audioSource?.PlayOneShot(pickupSound);
            Destroy(gameObject); // Destroy the pickup after it has been collected
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPickup();
            audioSource?.PlayOneShot(pickupSound);
            GetComponent<SpriteRenderer>().enabled = false; // Hide the pickup visually
            GetComponent<Collider2D>().enabled = false; // Disable the collider to prevent further interactions
            Destroy(gameObject, 0.5f); // Destroy the pickup after it has been collected
        }
    }
}