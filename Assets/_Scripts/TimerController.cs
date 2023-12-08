using UnityEngine;
using TMPro;
using Unity.LEGO.Minifig;

public class TimerController : MonoBehaviour
{
    private float timer = 0f;
    public TMP_Text TMP_Timer;

    private void Update()
    {
        // Increase the timer by the time passed since the last frame 
        timer += Time.deltaTime;

        // Check if one second has passed
        if (timer >= 1f)
        {
            // Do something every second (e.g., increase TMP timer)
            IncreaseTMPTimer();

            // Reset the timer
            timer = 0f;
        }
    }

    private void IncreaseTMPTimer()
    {
        // Assuming TMP_Timer is a reference to your TextMeshProUGUI component
        int currentTimerValue = int.Parse(TMP_Timer.text);
        currentTimerValue++;

        // Update the TextMeshProUGUI component with the new timer value
        TMP_Timer.text = currentTimerValue.ToString();
    }
}
