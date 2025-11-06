using UnityEngine;
using UnityEngine.UI;

public class EndPointTrigger : MonoBehaviour
{
    public GameObject endMessageUI; // UI panel or text shown when player reaches end
    public GameObject quitButton;

    private bool playerAtEnd = false;

    public Vector3 rotationSpeed = new Vector3(0f, 30f, 0f); // Degrees per second

    void Start()
    {
        if (endMessageUI != null)
            endMessageUI.SetActive(false);
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);

        // If player reached the end and presses Q, quit the game
        if (playerAtEnd && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Quitting game...");
            QuitGame();
        }
        if (quitButton != null)
        {
                Button btn = quitButton.GetComponent<Button>();
                btn.onClick.AddListener(QuitGame);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerAtEnd = true;
            if (endMessageUI != null)
                endMessageUI.SetActive(true);
            //show mouse cursor to allow player to click quit button
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


        }
    }
    

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerAtEnd = false;
            if (endMessageUI != null)
                endMessageUI.SetActive(false);
            //hide mouse cursor when player leaves end area
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in editor
#else
        Application.Quit(); // Quit in build
#endif
    }
}