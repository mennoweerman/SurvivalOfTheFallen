using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    [SerializeField] private KeyCode reloadKey;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shootInput?.Invoke();
        }
        if (Input.GetKeyDown(reloadKey))
        {
            reloadInput?.Invoke();
        }
    }
}
