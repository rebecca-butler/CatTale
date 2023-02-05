using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MoveBase Base { get; set; }
    public int PP { get; set; }

    // Constructor
    public Move(MoveBase pBase) {
        Base = pBase;
        PP = pBase.Pp;
    }
}
