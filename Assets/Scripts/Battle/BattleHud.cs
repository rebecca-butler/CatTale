using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon) {
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl " + pokemon.Level;

        hpBar.SetHP((float) pokemon.HP/ pokemon.MaxHp);
    }

    // Normalize the HP and use it to scale the UI bar
    // i.e. Set UI bar scale to 0.5 if current heatlh is half the max
    public IEnumerator UpdateHP() {
        yield return hpBar.SetHPSmooth((float) _pokemon.HP/ _pokemon.MaxHp);
    }
}
