using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text intelligenceText;
    [SerializeField] Text strengthText;
    [SerializeField] HPBar hpBar;

    public void SetName(string name) {
        nameText.text = name;
    }

    public void SetIntelligence(int intelligence) {
        intelligenceText.text = $"INT: {intelligence}";
    }

    public void SetStrength(int strength) {
        strengthText.text = $"STR: {strength}";
    }

    public void SetHP(int hP, int maxHP) {
        hpBar.SetHP((float) hP / maxHP);
    }
}

[Serializable]

public class PlayerStats
{
    [SerializeField] string playerName;
    [SerializeField] int hP;
    [SerializeField] int maxHP;
    [SerializeField] int intelligence;
    [SerializeField] int strength;

    public string PlayerName => playerName;
    public int HP => hP;
    public int MaxHP => maxHP;
    public int Intelligence => intelligence;
    public int Strength => strength;
}
