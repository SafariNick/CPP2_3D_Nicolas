using UnityEngine;

public class Pickups : MonoBehaviour
{
    public enum PickupType
    {
        Life = 0,
        Score = 1,
        Powerup = 2,
        Ammo = 3
    }
    public AudioClip pickupSound; // Sound to play on pickup
    private AudioSource audioSource;

    public PickupType pickupType = PickupType.Life; // Type of the pickup
    void Start()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSource?.PlayOneShot(pickupSound);


            switch (pickupType)
            {

                case PickupType.Life:
                    GameManager.Instance.lives++;
                    //Debug.Log("Life collected! Current lives: " + pc.lives);
                    break;
                case PickupType.Score:
                    GameManager.Instance.score += 50;
                    //GameManager.Instance.score++;
                    //Debug.Log("Score collected! Current score: " + GameManager.Instance.score);
                    break;
                case PickupType.Powerup:
                    PlayerController pc = collision.GetComponent<PlayerController>();
                    pc.ActivateJumpForceChange();
                    break;
            }

            GetComponent<SpriteRenderer>().enabled = false; // Hide the pickup visually
            GetComponent<Collider2D>().enabled = false; // Disable the collider to prevent further interactions
            Destroy(gameObject, 0.5f); // Destroy the pickup after collection
        }
    }
}
