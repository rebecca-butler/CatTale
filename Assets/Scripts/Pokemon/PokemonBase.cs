using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]

public class PokemonBase : ScriptableObject
{
    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    // Base stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;

    [SerializeField] List<LearnableMove> learnableMoves;

    // Getters
    public string Name {
        get { return name; }
    }
    public string Description {
        get { return description; }
    }
    public Sprite FrontSprite {
        get { return frontSprite; }
    }
    public Sprite BackSprite {
        get { return backSprite; }
    }
    public PokemonType Type1 {
        get { return type1; }
    }
    public PokemonType Type2 {
        get { return type2; }
    }
    public int MaxHp {
        get { return maxHp; }
    }
    public int Attack {
        get { return attack; }
    }
    public int Defense {
        get { return defense; }
    }
    public List<LearnableMove> LearnableMoves {
        get { return learnableMoves; }
    }
}

[System.Serializable]

public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase MoveBase {
        get { return moveBase; }
    }
    public int Level {
        get { return level; }
    }
}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Grass
}