using UnityEngine;
using UnityEngine.UI;

public class StateText : MonoBehaviour, IStateListener
{
    public void OnStateChange(PlayState NewState)
    {
        GetComponent<Text>().text = NewState.ToString();
    }

    // Use this for initialization
    void Start ()
    {
        this.RegisterStateListener();
    }

    void OnDestroy()
    {
        this.DisposeStateListener();
    }
}
