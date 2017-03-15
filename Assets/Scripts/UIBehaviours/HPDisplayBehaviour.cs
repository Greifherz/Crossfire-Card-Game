
using UnityEngine;

public class HPDisplayBehaviour : MonoBehaviour, IHPListener
{
    private bool Registered = false;

    public UnityEngine.UI.Text HPText;

    public void OnHpChange(int NewValue)
    {
        HPText.text = NewValue.ToString();
    }

    void Update()
    {
        if(!Registered)
        {
            if(PlayController.Instance.CurrentPlayer != null)
            {
                PlayController.Instance.CurrentPlayer.RegisterListener(this);
                HPText.text = PlayController.Instance.CurrentPlayer.HP.ToString();
            }
        }
    }

    void OnDestroy()
    {
        PlayController.Instance.CurrentPlayer.DisposeListener(this);
    }
}
