using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuFunctions : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ToggleMenu(UIDocument menu)
    {
        menu.gameObject.SetActive(!menu.gameObject.activeInHierarchy);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
