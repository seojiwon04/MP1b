using System.Collections;
using TMPro;
using UnityEngine;

public class ComputerLabTimer : MonoBehaviour
{
    [Min(0f)] public float durationSeconds = 300f;
    public TMP_Text timerText;
    public SwitchSceneManager sceneSwitcher;

    private IEnumerator Start()
    {
        double deadline = Time.unscaledTimeAsDouble + Mathf.Max(0f, durationSeconds);
        var interval = new WaitForSecondsRealtime(0.1f);
        while (true)
        {
            int seconds = Mathf.CeilToInt((float)System.Math.Max(0d, deadline - Time.unscaledTimeAsDouble));
            if (timerText) timerText.text = $"{seconds / 60:00}:{seconds % 60:00}";
            if (seconds == 0) break;
            yield return interval;
        }
        sceneSwitcher.SwitchToMainRoom();
    }

    private void OnDisable() => StopAllCoroutines();
}
