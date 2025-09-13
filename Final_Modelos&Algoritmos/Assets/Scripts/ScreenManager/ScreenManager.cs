using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{

    public static ScreenManager Instance;

    [SerializeField] Stack<BaseScreen> _screens = new();

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool ScreenActive { get { return _screens.Count > 0; } }

    public void ActivateScreen(BaseScreen screen)
    {
        if (_screens.Count > 0)
            _screens.Peek().Deactivate();

        screen.Activate();

        _screens.Push(screen);
    }

    public void DeactivateScreen()
    {
        if (_screens.Count <= 0) return;

        _screens.Pop().Deactivate();

        if (_screens.Count <= 0) return;

        _screens.Peek().Activate();
    }
}
