using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebBrowser : MonoBehaviour {
    public GameObject ThingPrefab;
    public Transform ThingContainer;

	// Use this for initialization
	void Awake () {
        
        foreach(Transform transform in ThingContainer)
        {
            DestroyImmediate(transform.gameObject);
        }

		foreach(Thing thing in Game.Things)
        {
            if(thing.Item != null && thing.Item.IsCool)
            {
                GameObject obj = Instantiate(ThingPrefab) as GameObject;
                obj.transform.SetParent(ThingContainer, false);
                Product product = obj.GetComponent<Product>();
                product.SetThing(thing);
            }
        }

        Close();
    }

    void UpdateThings()
    {
        foreach(Transform transform in ThingContainer)
        {
            Product product = transform.GetComponent<Product>();
            if(product == null || product.Thing == null)
            {
                DestroyImmediate(transform.gameObject);
            }
            else
            {

                if (product.Thing.Acquired)
                {
                    DestroyImmediate(transform.gameObject);
                }
                else
                {
                    product.BuyButton.interactable = product.Thing.Item.Price <= Game.Player.Money;
                }
            }
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        UpdateThings();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
