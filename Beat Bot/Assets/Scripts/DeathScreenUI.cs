using UnityEngine;
using System.Collections;

public class DeathScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private CanvasGroup deathText;
    [SerializeField] private CanvasGroup deathBackground;
    [SerializeField] private float deathTextFadeTime;
    [SerializeField] private float blackScreenFadeTime;
    [SerializeField] private PlayerDeath playerDeath;
    private float elapsedTime;

    // Activate death screen
    public void ShowDeathScreen()
    {
        // Turn death screen obkject on and set opacity to 0.
        deathScreen.SetActive(true);
        deathText.alpha = 0f;
        deathBackground.alpha = 0f;

        // Start Death Screen animation
        StartCoroutine(DeathSequence());
    }

    // Perform death screen animation
    private IEnumerator DeathSequence()
    {
        // Fade Screen to Black
        yield return StartCoroutine(FadeToBlack());

        // Keep Visible
        yield return new WaitForSeconds(0.5f);

        // Fade in "You Died!"
        yield return StartCoroutine(FadeInDeathText());

        // Keep Visible
        yield return new WaitForSeconds(1f);

        playerDeath.ReloadScene();
    }

    // Fade in "You Died!" text
    private IEnumerator FadeInDeathText() 
    {
        // Set time elapsed to 0
        elapsedTime = 0f;

        // While the elapsed time is less than set duration
        while(elapsedTime < deathTextFadeTime)
        {
            // Update time elasped
            elapsedTime += Time.deltaTime;

            // Update opacity of text
            deathText.alpha = Mathf.Lerp(0f, 1f, elapsedTime / deathTextFadeTime);

            yield return null;
        }

        // Make sure text is fully visible
        deathText.alpha = 1f;
    }

    // Fade in black screen
    private IEnumerator FadeToBlack() 
    {
        // Set time elapsed to 0
        elapsedTime = 0f;

         // While the elapsed time is less than set duration
        while(elapsedTime < blackScreenFadeTime)
        {
            // Update time elasped
            elapsedTime += Time.deltaTime;

            // Update opacity of background
            deathBackground.alpha = Mathf.Lerp(0f, 1f, elapsedTime / blackScreenFadeTime);

            yield return null;
        }

        // Make sure background is fully visible
        deathBackground.alpha = 1f;
    }
}
