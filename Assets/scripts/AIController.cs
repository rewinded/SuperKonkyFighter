﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController
{

    private int difficulty { get; set; }

    public AIController()
    {
        difficulty = 0;
    }

    public AIController(int difficulty)
    {
        this.difficulty = difficulty;
    }

    void observe(int oposingCharacter, Vector2 opponentPosition, int opposingAction)
    {
        switch (oposingCharacter)
        {
            // Konky
            case 0:
                break;
            case 1:
                break;
        }
    }

    int getInput()
    {
        int input = 0;
        
        return input;
    }
}
