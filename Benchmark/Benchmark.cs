using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Text)), DisallowMultipleComponent, AddComponentMenu("LuviKunG/Benchmark/Benchmark Score")]
public class Benchmark : MonoBehaviour
{
    [Header("Components")]
    public Text text;
    [Header("Adjustment")]
    public float time = 10.0f;
    [Header("Events")]
    public UnityEvent onBenchmarkStart;
    public UnityEvent onBenchmarkStop;

    StringBuilder sb = new StringBuilder();
    float currentTime;
    float currentScore;

    public void StartBenchmarking()
    {
        onBenchmarkStart.Invoke();
        currentTime = 0;
        currentScore = 0;
        enabled = true;
    }

    public void StopBenchmarking()
    {
        onBenchmarkStop.Invoke();
        enabled = false;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        currentScore += 1f / Time.deltaTime;
        sb.Clear();
        sb.Append("Score: ");
        sb.Append(currentScore);
        text.text = sb.ToString();
        if (currentTime > time)
            StopBenchmarking();
    }

#if UNITY_EDITOR
    private void Reset()
    {
        if (text == null)
        {
            text = GetComponent<Text>();
            if (text != null)
                text.text = "Score: ?";
        }
    }
#endif
}
