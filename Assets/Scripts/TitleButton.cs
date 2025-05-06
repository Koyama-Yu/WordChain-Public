using UnityEngine;

public class TitleButton : MonoBehaviour
{
    
    public void OnStartButtonClicked()
    {
        SceneChanger.StartGame();
    }

    public void OnTutorialButtonClicked()
    {
        //SceneChanger.StartTutorial();
    }

    public void OnHowToPlayButtonClicked()
    {
        //SceneChanger.ShowHowToPlay();
    }
}
