using UnityEngine.SceneManagement;

public class SceneChanger
{
    public static void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public static void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public static void ShowHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public static void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
