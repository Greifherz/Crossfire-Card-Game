using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LobbyController : GameController {

    public UnityEngine.UI.Text StatusText;

    public override void Begin()
    {
        base.Begin();
        if(StatusText != null)
        {
            StatusText.text = GameController.EndGameMessage;
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Play", LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("New Game", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
