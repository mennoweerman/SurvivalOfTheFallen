using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 100f;

    public ItemDrop getItem;


    private void Start()
    {
        getItem = GetComponent<ItemDrop>();
    }
    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (getItem != null)
            {
                getItem.DropItem();
                Debug.Log("Dropped an Item " + getItem);
            }
            Destroy(gameObject);
        }
        Debug.Log(health);
    }

}
