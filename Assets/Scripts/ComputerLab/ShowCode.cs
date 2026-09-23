using UnityEngine;
using System.Collections;

public class ShowCode : MonoBehaviour
{
    public GameObject initialText, hiddenText;
    private bool state = false;

    public void Toggle()
    {
        if (!state)
        {
            StartCoroutine(DelayedToggle());
        }

        return;
    }

    private IEnumerator DelayedToggle()
    {
        initialText.SetActive(!initialText.activeSelf);
        hiddenText.SetActive(!hiddenText.activeSelf);
        state = !state;

        yield return new WaitForSeconds(5f);

        initialText.SetActive(!initialText.activeSelf);
        hiddenText.SetActive(!hiddenText.activeSelf);
        state = !state;
    }
}
