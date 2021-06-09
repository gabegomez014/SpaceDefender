using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject _howToOverlay;

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void DisplayHowToScreen()
    {
        _howToOverlay.SetActive(true);
    }

    public void HideHowToScreen()
    {
        _howToOverlay.SetActive(false);
    }
}
