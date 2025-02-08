using UnityEngine;
using UnityEngine.SceneManagement; // For scene loading
using System.Collections;

public class ExitLevel : MonoBehaviour
{
    public float stayTime = 3f;       // Time in seconds before the player starts fading
    public float fadeDuration = 3f;   // Time for the fade-out effect
    public SpriteRenderer playerSprite; // The player's SpriteRenderer to control the sprite's alpha
    public GameObject player;         // The player GameObject

    public LayerMask playerLayer;     // Assign this to "Player" layer in the Inspector

    private float timer = 0f;
    private bool playerInArea = false;
    private bool fadingOut = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0) // Check if collided object is in Player Layer
        {
            playerInArea = true;
            timer = 0f; // Reset timer if the player enters the area
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0) // Check if object exiting is in Player Layer
        {
            playerInArea = false;
            timer = 0f; // Reset the timer if the player leaves the area

        }
    }

    void Update()
    {
        if (playerInArea && !fadingOut)
        {
            // Increment the timer while the player is in the area
            timer += Time.deltaTime;

            if (timer >= stayTime)
            {
                // If the player stays for the required time, start fading
                fadingOut = true;
                StartCoroutine(FadeOut());
            }
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        // Fade out the player's sprite alpha
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            Color spriteColor = playerSprite.color;
            spriteColor.a = alpha;
            playerSprite.color = spriteColor;
            yield return null;
        }

        // Ensure the player is fully faded out
        Color finalColor = playerSprite.color;
        finalColor.a = 0f;
        playerSprite.color = finalColor;

        // After fading out, load next level
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        // Load the next scene (replace with your scene name)
        SceneManager.LoadScene("SampleScene");
    }
}
