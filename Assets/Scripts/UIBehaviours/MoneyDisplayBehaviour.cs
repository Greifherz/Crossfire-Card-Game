using System;
using UnityEngine;

public class MoneyDisplayBehaviour : MonoBehaviour, IMoneyListener
{
    private bool Registered = false;

    public UnityEngine.UI.Text MoneyText;

    void Update()
    {
        if (!Registered)
        {
            if (PlayController.Instance.CurrentPlayer != null)
            {
                PlayController.Instance.CurrentPlayer.RegisterListener(this);
                MoneyText.text = PlayController.Instance.CurrentPlayer.Money.ToString();
            }
        }
    }

    public void OnMoneyChange(int NewValue)
    {
        MoneyText.text = NewValue.ToString();
    }

    void OnDestroy()
    {
        PlayController.Instance.CurrentPlayer.DisposeListener(this);
    }
}
