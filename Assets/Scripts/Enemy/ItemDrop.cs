using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDrop : MonoBehaviour
{
    [SerializeField]
    private GameObject[] itemList; // Stores the game items
    [SerializeField] private int itemNum; // Selects a number to choose from the itemList
    [SerializeField] private int randNum; // chooses a random number to see if loot os dropped- Loot chance
    [SerializeField] private Transform Epos; // enemy position
    [SerializeField] private int dropQuantity; // number of items to drop
    [SerializeField] private float dropRadius; // radius of the item drop zone


    private void Start()
    {
        Epos = GetComponent<Transform>();
    }

    public void DropItem()
    {
        for (int i = 0; i < dropQuantity; i++)
        {
            randNum = Random.Range(0, itemList.Length);
            Debug.Log("Random Number is " + randNum);


            if (randNum != null)
            {
                itemNum = randNum;
                Vector3 randomPos = new Vector3(Random.Range(-dropRadius, dropRadius), 0, Random.Range(-dropRadius, dropRadius));
                Instantiate(itemList[itemNum], Epos.position + randomPos, Quaternion.identity);


            }
        }
        

    }

}
