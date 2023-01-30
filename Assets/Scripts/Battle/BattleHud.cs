using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    public void SetData(Pokemon pokemon) {
        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl " + pokemon.Level;

        // Normalize the HP and use it to scale the UI bar
        // i.e. Set UI bar scale to 0.5 if current heatlh is half the max
        hpBar.SetHP((float) pokemon.HP/ pokemon.MaxHp);
    }
}
