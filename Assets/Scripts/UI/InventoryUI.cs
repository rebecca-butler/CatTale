using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public void HandleUpdate(Action onBack) {
        // If x key is pressed, close inventory
        if (Input.GetKeyDown(KeyCode.X)) {
            onBack?.Invoke();
        }
    }
}
