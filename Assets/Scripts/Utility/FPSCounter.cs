using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    private Text textOutput;

    [SerializeField]
    [Range(0, 60)]
    private int framesPerUpdate = 5;


    float avgFps = 0;
    int frame = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (framesPerUpdate > 1)
        {
            if (frame < framesPerUpdate)
            {
                avgFps += 1f / (Time.unscaledDeltaTime * framesPerUpdate);
                frame++;
            }
            else
            {
                textOutput.text = $"Fps {(int)avgFps}";
                frame = 0;
                avgFps = 0;
            }
        }
        else
        {
            avgFps = 1f / Time.unscaledDeltaTime;
            textOutput.text = $"Fps {(int)avgFps}";
        }    
    }
}
