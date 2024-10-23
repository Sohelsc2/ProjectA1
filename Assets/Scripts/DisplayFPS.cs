using UnityEngine;

public class DisplayFPS : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private float timeElapsed = 0.0f;
    private int frameCount = 0;
    private float minFPS = float.MaxValue;
    private float maxFPS = float.MinValue;
    private float totalFPS = 0.0f;
    private float interval = 10.0f; // Log output every 10 seconds
    public bool trackFPS = false;
void Start () {
    Application.targetFrameRate = 60;
}
    void Update()
    {
        DisplayFPSFunction();
    }
    private void DisplayFPSFunction(){
        if (!trackFPS){return;}
               // Calculate the delta time for the current frame
        deltaTime = Time.deltaTime;
        float fps = 1.0f / deltaTime;

        // Track min, max, and total FPS
        frameCount++;
        totalFPS += fps;
        minFPS = Mathf.Min(minFPS, fps);
        maxFPS = Mathf.Max(maxFPS, fps);

        // Accumulate time
        timeElapsed += deltaTime;

        // If the time has passed the interval, output the FPS data and reset
        if (timeElapsed >= interval)
        {
            float averageFPS = totalFPS / frameCount;

            // Log the average, minimum, and maximum FPS
            Debug.Log("FPS Report (Last seconds): \n" +
                      "Average FPS: " + Mathf.Ceil(averageFPS) + "\n" +
                      "Min FPS: " + Mathf.Ceil(minFPS) + "\n" +
                      "Max FPS: " + Mathf.Ceil(maxFPS));

            // Reset the counters for the next interval
            timeElapsed = 0.0f;
            frameCount = 0;
            totalFPS = 0.0f;
            minFPS = float.MaxValue;
            maxFPS = float.MinValue;
        }
    }
}
