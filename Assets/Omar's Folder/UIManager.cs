/*
using PSXShaderKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject mainCamera;


    // Method called when a button is clicked

    public void ButtonClicked(int id)
    {
        var post = mainCamera.GetComponent<PSXPostProcessEffect>();
        switch (id)
        {
            case 0:
                Debug.Log("1");
                fade();
                break;
            case 1:
                Debug.Log("d");
                post._PixelationFactor = 0.0f;
                break;
        }

    }
    public void fade() {
        var post = mainCamera.GetComponent<PSXPostProcessEffect>();
        while (post._PixelationFactor != 0)
        {
            post._PixelationFactor -= 0.0002f;
        }
        post._PixelationFactor = 1.0f;
    }

}
*/
using PSXShaderKit;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject mainCamera;
    public GameObject panel;
    public GameObject panel2;
    public GameObject MainMenu;
    public GameObject CustomMenu;
    public GameObject CustomChar;
    public Color startFadeColor = Color.white; // Start color of the fade
    public Color endFadeColor = Color.clear; // End color of the fade
    public SceneLoader s;
    AsyncOperation asyncLoad;

    // Public variables for fade duration and start/end values
    public float fadeDuration = 2.0f;
    public float startFadeValue = 0.0f;
    public float endFadeValue = 1.0f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance of UIManager exists
        }
    }


    // Method called when a button is clicked
    public void ButtonClicked(int id)
    {
        var post = mainCamera.GetComponent<PSXPostProcessEffect>();
        var customChar = CustomChar.transform.GetComponent<CustomCharacter>();
        switch (id)
        {
            case 0:
                Debug.Log("Fade started.");
                StartCoroutine(FadeEffect(post));
                StartCoroutine(FadePanel(panel));
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("DEMOScene");
                //StartCoroutine(FadePanel(panel2));
                break;
            case 1:
                Debug.Log("Reset Pixelation Factor.");
                StartCoroutine(FadeEffect(post));
                StartCoroutine(FadePanel(panel));
                post._PixelationFactor = 1.0f;
                break;
            case 2:
                customChar.CycleNextCharacter();
                break;
        }
    }

    // Coroutine for fading the PixelationFactor
    private IEnumerator FadeEffect(PSXPostProcessEffect post)
    {
        float elapsedTime = 0.0f;
        float startValue = post._PixelationFactor;

        while (elapsedTime < fadeDuration)
        {
            post._PixelationFactor = Mathf.Lerp(startValue, endFadeValue, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        post._PixelationFactor = endFadeValue; // Ensure the target value is reached exactly
        Debug.Log("Fade completed.");
        post._PixelationFactor = 1.0f;
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadePanel(GameObject panelObject)
    {
        // Get the Image component from the panel
        Image panelImage = panelObject.GetComponent<Image>();

        // Ensure the panel has an Image component
        if (panelImage != null)
        {
            Material panelMaterial = panelImage.material;

            // Ensure the panel has a material
            if (panelMaterial != null)
            {
                float elapsedTime = 0.0f;
                Color startColor = panelMaterial.color;

                while (elapsedTime < fadeDuration)
                {
                    float t = elapsedTime / fadeDuration;
                    panelMaterial.color = Color.Lerp(startColor, endFadeColor, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                
                panelMaterial.color = endFadeColor; // Ensure the target color is reached exactly
                MainMenu.SetActive(!true);
                //CustomMenu.SetActive(!false);
                //CustomChar.SetActive(true);
                //UIManager.instance.LoadScene("DEMOScene");
                Debug.Log("Panel Fade completed.");

            }
            else
            {
                Debug.LogError("Panel material is missing.");
            }
        }
        else
        {
            Debug.LogError("Panel image is missing.");
        }
    }
}

