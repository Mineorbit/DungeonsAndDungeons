using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFramer : MonoBehaviour
{
    static Queue<float> times;
    static IEnumerator run;
    // Start is called before the first frame update
    void Start()
    {
        times = new Queue<float>();
    }
    static IEnumerator FreezeCam(float t)
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
    void Update()
    {
        if(times.Count>0)
        {
            if(run == null)
            {
            float t = times.Dequeue();
            run = FreezeCam(t);
            StartCoroutine(run);
            }
        }
    }
    public static void freeze(float time)
    {
        times.Enqueue(time);
    }
}
