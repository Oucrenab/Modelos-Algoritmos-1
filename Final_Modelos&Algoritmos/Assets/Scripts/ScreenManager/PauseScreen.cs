using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : BaseScreen
{



    public override void Deactivate()
    {
        Time.timeScale = 1.0f;
        base.Deactivate();
    }

    public void Resume()
    {
        ScreenManager.Instance.DeactivateScreen();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void BackToMenu()
    {
        ScreenManager.Instance.DeactivateScreen();

        SceneManager.LoadScene(0);
    }

    public void CallCheckpoint()
    {
        EventManager.Trigger("MementoLoad");
        ScreenManager.Instance.DeactivateScreen();
    }
}
