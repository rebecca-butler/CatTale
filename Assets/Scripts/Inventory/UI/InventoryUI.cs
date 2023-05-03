using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;

    [SerializeField] Image itemIcon;
    [SerializeField] Text itemDescription;

    [SerializeField] Image upArrow;
    [SerializeField] Image downArrow;

    int selectedItem = 0;
    int itemsInViewport = 8;

    List<ItemSlotUI> slotUIList;
    Inventory inventory;
    RectTransform itemListRect;

    private void Awake() {
        inventory = Inventory.GetInventory();
        itemListRect = itemList.GetComponent<RectTransform>();
    }

    private void Start() {
        UpdateItemList();
    }

    // Clear item list and fill with items from inventory
    void UpdateItemList() {
        // Clear existing items from itemList game object by removing its children
        foreach (Transform child in itemList.transform) {
            Destroy(child.gameObject);
        }

        // Create list to store item slots
        slotUIList = new List<ItemSlotUI>();

        foreach (var itemSlot in inventory.Slots) {
            // Instantiate each item slot and attach to itemList
            var slotUIObj = Instantiate(itemSlotUI, itemList.transform);
            slotUIObj.SetData(itemSlot);
            slotUIList.Add(slotUIObj);
        }

        UpdateItemSelectionUI(selectedItem);
    }

    public void HandleUpdate(Action onBack) {
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
        selectedItem = Mathf.Clamp(selectedItem, 0, inventory.Slots.Count - 1);

        // If item changed, update UI for new selection
        if (prevSelection != selectedItem) {
            UpdateItemSelectionUI(selectedItem);
        }

        // If x key is pressed, close inventory
        if (Input.GetKeyDown(KeyCode.X)) {
            onBack?.Invoke();
        }
    }

    // Highlight text for selected menu item
    public void UpdateItemSelectionUI(int selectedItem) {
        for (int i = 0; i < slotUIList.Count; ++i) {
            if (i == selectedItem) {
                slotUIList[i].NameText.color = GlobalSettings.i.HighlightedColor;
            } else {
                slotUIList[i].NameText.color = Color.black;
            }

            var item = inventory.Slots[selectedItem].Item;
            itemIcon.sprite = item.Icon;
            itemDescription.text = item.Description;

            HandleScrolling();
        }
    }

    private void HandleScrolling() {
        // Index to scroll to. If selected item is in top half, clamp to 0
        int scrollIndex = Mathf.Clamp(selectedItem - itemsInViewport / 2, 0, selectedItem);

        // Scroll position is the index to scroll to multiplied by the item slot height
        float scrollPos = scrollIndex * slotUIList[0].Height;

        // Update y position of item list rect transform. Use local position because itemList is a child object
        itemListRect.localPosition = new Vector2(itemListRect.localPosition.x, scrollPos);

        // Show arrows depending on where selected item is in viewport
        bool showUpArrow = selectedItem > (itemsInViewport / 2);
        bool showDownArrow = selectedItem + (itemsInViewport / 2) < slotUIList.Count;
        upArrow.gameObject.SetActive(showUpArrow);
        downArrow.gameObject.SetActive(showDownArrow);
    }
}
