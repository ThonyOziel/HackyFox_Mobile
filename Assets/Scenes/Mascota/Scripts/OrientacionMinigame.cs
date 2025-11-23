#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrientacionMinigame : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Escena que debe estar en horizontal")]
    [SerializeField] private SceneAsset escenaHorizontal;
#endif

    void OnEnable()
    {
        SceneManager.sceneLoaded += CambiarOrientacion;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= CambiarOrientacion;
    }

    void CambiarOrientacion(Scene scene, LoadSceneMode mode)
    {
#if UNITY_EDITOR
        // En el editor: comparar con el asset arrastrado
        if (escenaHorizontal != null && scene.name == escenaHorizontal.name)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
#else
        // En build: usar el nombre directamente
        if (scene.name == "MiniGame")
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
#endif
    }
}