using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTrace : MonoBehaviour
{
    public float timer;
    public Color startColor = new Color(1, 0, 0);
    public Vector3 start;
    public Vector3 end;
    
    private LineRenderer line;
    private Color color;
    private float maxTimer;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }


    public void Reset()
    {
        line = gameObject.GetComponent<LineRenderer>();

        line.SetPositions(new Vector3[] { start, end });

        startColor.a = 1;
        color = startColor;
        line.startColor = color;
        line.endColor = color;

        maxTimer = timer;
        if (maxTimer <= 0) maxTimer = Mathf.Epsilon;

    }

    // Update is called once per frame
    void Update()
    {
        var fraction = timer / maxTimer;
        fraction = fraction > 0 ? fraction : 0;
        color.a = fraction;

        line.startColor = color;
        line.endColor = color;

        timer -= Time.deltaTime;
        if (timer < Time.deltaTime) Destroy(gameObject);
    }
}
