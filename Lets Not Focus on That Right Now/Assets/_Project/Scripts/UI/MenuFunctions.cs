using UnityEngine;
using UnityEngine.UIElements;

public class MenuFunctions : MonoBehaviour
{
    public static MenuFunctions instance;

    private bool initialPause = true;

    private void Awake()
    {
        instance = this;
    }

    public void LoadScene(int sceneIndex)
    {
        GameManager.LoadSceneByIndex(sceneIndex);
    }

    public void ToggleMenu(UIDocument menu)
    {
        menu.gameObject.SetActive(!menu.gameObject.activeInHierarchy);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        ToggleMenu(GetComponent<UIDocument>());
    }
}
