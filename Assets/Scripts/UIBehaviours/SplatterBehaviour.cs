using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplatterBehaviour : MonoBehaviour {

    public Sprite[] Splatters;
    public Image SelfImage;

	// Use this for initialization
	void Start ()
    {
        SelfImage.sprite = Splatters[Random.Range(0, Splatters.Length)];
        StartCoroutine(ShowAndDisappear());
	}
	
    private IEnumerator ShowAndDisappear()
    {
        yield return null;

        while (SelfImage.color.a < 1)
        {
            SelfImage.color = new Color(SelfImage.color.r, SelfImage.color.g, SelfImage.color.b, SelfImage.color.a + 1f);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);


        while (SelfImage.color.a > 0)
        {
            SelfImage.color = new Color(SelfImage.color.r, SelfImage.color.g, SelfImage.color.b, SelfImage.color.a - 0.01f);
            yield return null;
        }

        yield return null;

        Destroy(gameObject);

    }
	
}
