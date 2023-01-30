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

        // set the HP bar to the current HP as a a proportion of the max HP
        // i.e. HP bar is at scale 0.5 if the current heatlh is half the total
        hpBar.SetHP((float) pokemon.HP / pokemon.MaxHP);
    }
}
