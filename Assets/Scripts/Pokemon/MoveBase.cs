using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new move")]

public class MoveBase : ScriptableObject
{
    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PokemonType type;

    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    // Getters
    public string Name {
        get { return name; }
    }
    public string Description {
        get { return description; }
    }
    public PokemonType Type {
        get { return type; }
    }
    public int Power {
        get { return power; }
    }
    public int Accuracy {
        get { return accuracy; }
    }
    public int Pp {
        get { return pp; }
    }
}
