using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;

    public event Action<int> onMenuSelected;
    public event Action onMenuExited;

    List<Text> menuItems;
    int selectedItem = 0;

    private void Awake() {
        menuItems = menu.GetComponentsInChildren<Text>().ToList();
    }

    public void OpenMenu() {
        menu.SetActive(true);
        UpdateItemSelectionUI(selectedItem);
    }

    public void CloseMenu() {
        menu.SetActive(false);
    }

    public void HandleUpdate() {
        int prevSelection = selectedItem;

        // If down arrow is pressed, increase menu index
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            ++selectedItem;
        }
        // If up arrow is pressed, decrease menu index
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            --selectedItem;
        }

        // Clamp index between 0 and max index
        selectedItem = Mathf.Clamp(selectedItem, 0, menuItems.Count - 1);

        // If item changed, update UI for new selection
        if (prevSelection != selectedItem) {
            UpdateItemSelectionUI(selectedItem);
        }

        // If z key is pressed, invoke selection event
        if (Input.GetKeyDown(KeyCode.Z)) {
            onMenuSelected?.Invoke(selectedItem);
            CloseMenu();
        }

        // If x key is pressed, invoke exit event
        if (Input.GetKeyDown(KeyCode.X)) {
            onMenuExited?.Invoke();
            CloseMenu();
        }
    }

    // Highlight text for selected menu item
    public void UpdateItemSelectionUI(int selectedItem) {
        for (int i = 0; i < menuItems.Count; ++i) {
            if (i == selectedItem) {
                menuItems[i].color = GlobalSettings.i.HighlightedColor;
            } else {
                menuItems[i].color = Color.black;
            }
        }
    }
}
