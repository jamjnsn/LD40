using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingLight : MonoBehaviour {
    public Gradient ColorRange;
    public float FlickerRate;

    SpriteRenderer spriteRenderer;
    Light light;

    float nextFlicker;
    float currentGradientPosition;

	// Use this for initialization
	void Awake () {
        light = GetComponentInChildren<Light>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    void Flicker()
    {
        nextFlicker = FlickerRate;

        currentGradientPosition += Random.Range(0f, 1f);
        if(currentGradientPosition > 1f)
        {
            currentGradientPosition = currentGradientPosition - 1f;
        }

        Color color = ColorRange.Evaluate(currentGradientPosition);
        light.color = color;
        spriteRenderer.color = color;
    }
	
	// Update is called once per frame
	void Update () {
        nextFlicker -= Time.deltaTime;
        if(nextFlicker <= 0)
        {
            Flicker();
        }
	}
}
