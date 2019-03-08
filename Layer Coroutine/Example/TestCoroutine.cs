using LuviKunG;
using System.Collections;
using UnityEngine;

public class TestCoroutine : MonoBehaviour
{
    LayerCoroutine layerRoutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("==== Stop and Start Coroutine ====");
            layerRoutine = layerRoutine ?? new LayerCoroutine(this);
            layerRoutine.StopAllCoroutine();
            layerRoutine.StartCoroutine(FirstCoroutine(layerRoutine));
        }
    }

    Coroutine layerRoutineFirst;
    private IEnumerator FirstCoroutine(LayerCoroutine layer)
    {
        Debug.Log("FirstCoroutine: Start");
        yield return layer.StartCoroutine(SecondCoroutine(layer));
        Debug.Log("FirstCoroutine: End");
    }

    private IEnumerator SecondCoroutine(LayerCoroutine layer)
    {
        Debug.Log("SecondCoroutine: Start");
        yield return layer.StartCoroutine(ThirdCoroutine(layer));
        Debug.Log("SecondCoroutine: End");
    }

    private IEnumerator ThirdCoroutine(LayerCoroutine layer)
    {
        Debug.Log("ThirdCoroutine: Start");
        yield return layer.StartCoroutine(FourthCoroutine(layer));
        Debug.Log("ThirdCoroutine: End");
    }

    private IEnumerator FourthCoroutine(LayerCoroutine layer)
    {
        Debug.Log("FourthCoroutine: Start");
        yield return new WaitForTest(2.0f);
        Debug.Log("FourthCoroutine: End");
    }
}

public class WaitForTest : CustomYieldInstruction
{
    private float duration;

    public WaitForTest(float duration)
    {
        Debug.Log("WaitForTest Execute");
        this.duration = duration;
    }

    public override bool keepWaiting
    {
        get
        {
            duration -= Time.deltaTime;
            return duration > 0;
        }
    }
}