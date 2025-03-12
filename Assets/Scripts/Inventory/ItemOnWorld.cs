using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item item;
    public Inventory PlayerInventory;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Pick up item: "+gameObject.name);
            PlayerInventory.AddItem(item);
            Destroy(gameObject);
        }
    }
}
