using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text countText;

    public Text NameText => nameText;
    public Text CountText => countText;

    public void SetData(ItemSlot itemSlot) {
        nameText.text = itemSlot.Item.Name;
        countText.text = $"x {itemSlot.Count}";
    }
}
