using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFramer : MonoBehaviour
{
    private static Queue<float> times;

    private static IEnumerator run;

    // Start is called before the first frame update
    private void Start()
    {
        times = new Queue<float>();
    }

    private void Update()
    {
        if (times.Count > 0)
            if (run == null)
            {
                var t = times.Dequeue();
                run = FreezeCam(t);
                StartCoroutine(run);
            }
    }

    private static IEnumerator FreezeCam(float t)
    {
        //yield return null;
        var flags = Camera.main.clearFlags;
        Camera.main.clearFlags = CameraClearFlags.Nothing;
        yield return null;
        var mask = Camera.main.cullingMask;
        Camera.main.cullingMask = 0;
        yield return new WaitForSeconds(t);
        Camera.main.clearFlags = flags;
        yield return null;
        Camera.main.cullingMask = mask;
        run = null;
    }

    public static void freeze(float time)
    {
        times.Enqueue(time);
    }
}