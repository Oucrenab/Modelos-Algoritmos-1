using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : BaseScreen
{
    [SerializeField] TMP_Text _text;
    [SerializeField] GameObject _retryButton;

    private void Awake()
    {
        EventManager.Subscribe("PlayerDeath", Lose);
        EventManager.Subscribe("BossDeath", Win);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("PlayerDeath", Lose);
        EventManager.Unsubscribe("BossDeath", Win);
    }

    void Win(params object[] noUse)
    {
        _text.text = "YOU WIN";
        _text.color = Color.green;
        _retryButton.SetActive(false);

        ScreenManager.Instance.ActivateScreen(this);
    }

    void Lose(params object[] noUse)
    {
        _text.text = "YOU LOSE";
        _text.color = Color.red;
        _retryButton.SetActive(true);

        ScreenManager.Instance.ActivateScreen(this);
    }

    public override void Activate()
    {
        base.Activate();
        Time.timeScale = 0f;
    }

    public override void Deactivate()
    {
        Time.timeScale = 1f;
        base.Deactivate();
    }

    public void Restart()
    {
        ScreenManager.Instance.DeactivateScreen();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        //Deactivate();
    }
}
