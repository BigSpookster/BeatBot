using UnityEngine;
using System.Collections;

public class DeathScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private CanvasGroup deathText;
    [SerializeField] private CanvasGroup deathBackground;
    [SerializeField] private float deathTextFadeTime;
    [SerializeField] private float blackScreenFadeTime;
    private float elapsedTime;
    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        deathText.alpha = 0f;
        deathBackground.alpha = 0f;

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Fade Screen to Black
        yield return StartCoroutine(FadeToBlack());

        // Keep Visible
        yield return new WaitForSeconds(0.5f);

        // Fade in "You Died!"
        yield return StartCoroutine(FadeInDeathText());

        

        
    }

    private IEnumerator FadeInDeathText() 
    {
        elapsedTime = 0f;

        while(elapsedTime < deathTextFadeTime)
        {
            elapsedTime += Time.deltaTime;

            deathText.alpha = Mathf.Lerp(0f, 1f, elapsedTime / deathTextFadeTime);

            yield return null;
        }

        deathText.alpha = 1f;
    }

    private IEnumerator FadeToBlack() 
    {
        elapsedTime = 0f;

        while(elapsedTime < blackScreenFadeTime)
        {
            elapsedTime += Time.deltaTime;

            deathBackground.alpha = Mathf.Lerp(0f, 1f, elapsedTime / blackScreenFadeTime);
            yield return null;
        }

        deathBackground.alpha = 1f;
    }
}
