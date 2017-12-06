using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {
    public Gradient Gradient;
    Light light;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();	
	}
	
	// Update is called once per frame
	public void Update () {
        light.color = Gradient.Evaluate((float)Game.Time.Hour / 24f);
	}
}
