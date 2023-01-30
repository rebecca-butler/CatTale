using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    public PokemonBase Base { get; set; }
    public int Level { get; set; }

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    // Constructor
    public Pokemon(PokemonBase pBase, int pLevel) {
        Base = pBase;
        Level = pLevel;
        HP = MaxHp;

        Moves = new List<Move>();

        // For each possible learnable move, add it to Moves list if pokemon level is high enough
        foreach(var learnableMove in Base.LearnableMoves) {
            if (Level >= learnableMove.Level) {
                Moves.Add(new Move(learnableMove.MoveBase));
            }
        }
    }

    // Compute current stats as a function of base stats and current level
    public int Attack {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }
    public int Defense {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
    }
    public int MaxHp {
        get { return Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10; }
    }
}
