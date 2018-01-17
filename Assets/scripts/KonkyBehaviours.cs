﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors{

    static int[,] konkyMoveMap = new int[4, 9] {
        {
            4, 4, 4,
            0, 0, 0,
            7, 7, 7,
        },
        {
            5, 5, 5,
            1, 1, 2,
            8, 8, 8,
        },
        {
            6, 6, 6,
            3, 3, 3,
            9, 9, 9,
        },
		{
			10, 13, 12,
			-1, -1, 11,
			-1, -1, -1,
		},
    };

	//0 total frames
	//1 recovery frames
	//3 damage
	//4 knockback
	//5 hitstun
	//6 blockstun
	//7 hit stop
	//8 meter consumption

	static int[,] konkyFrames = new int[,]
    {
        {15,  8},//5L  0  | Knockback: , Angle: , Damage: 350, Chip Damage: 35, Attribute: Mid, Level: 0, P1 Scaling: 1
        {20, 12},//5M  1  | Knockback: , Angle: , Damage: 625, Chip Damage: 63, Attribute: Mid, Level: 1, P1 Scaling: 1
        {34, 18},//6M  2  | Knockback: , Angle: , Damage: 450, Chip Damage: 45, Attribute: High, Level: 0, P1 Scaling: 1.1
        {30, 17},//5H  3  | Knockback: , Angle: , Damage: 950, Chip Damage: 95, Attribute: Mid, Level: 2, P1 Scaling: 1
        {19, 10},//2L  4  | Knockback: , Angle: , Damage: 250, Chip Damage: 25, Attribute: Low, Level: 0, P1 Scaling: 1
        {24, 13},//2M  5  | Knockback: , Angle: , Damage: 650, Chip Damage: 65, Attribute: Mid, Level: 1, P1 Scaling: 1
        {20, 23},//2H  6  | Knockback: , Angle: , Damage: 200+650, Chip Damage: 20+65, Attribute: Low, Level: 2, P1 Scaling: 1
        {16,  7},//jL  7  | Knockback: , Angle: , Damage: 350, Chip Damage: 35, Attribute: High, Level: 2, P1 Scaling: .8
        {26, 13},//jM  8  | Knockback: , Angle: , Damage: 650, Chip Damage: 65, Attribute: High, Level: 2, P1 Scaling: .8
        {31, 17},//jH  9  | Knockback: , Angle: , Damage: 1000, Chip Damage: 100, Attribute: High, Level: 3+KD, P1 Scaling: .8
        {52, 40},//1S  10 | Knockback: , Angle: , Damage: 1200, Chip Damage: -, Attribute: Throw, Level: KD, P1 Scaling: 1
        {26, 13},//6S  11 | Knockback: , Angle: , Damage: 1500, Chip Damage: 150, Attribute: Mid, Level: 5, P1 Scaling: 1
        {34, 20},//3S  12 | Knockback: , Angle: , Damage: 400, Chip Damage: 40, Attribute: Mid, Level: 4, P1 Scaling: 1
        {53, 33},//2S  13 | Knockback: , Angle: , Damage: 500, Chip Damage: 50, Attribute: Mid, Level: 5, P1 Scaling: 1
		{999, 989},//4S   | Knockback: , Angle: , Damage: 400+400+600, Chip Damage: 40+40+60, Attribute: High, Level: 2+KD, P1 Scaling: 1
        // Knockback: , Angle: , Damage: 1500, Chip Damage: -, Attribute: Throw, Level: 5, P1 Scaling: .5
		{0,0 },//15
		{0,0 },//16
		{0,0 },//17
		{0,0 },//18
		{0,0 },//19
		{0,0 },//20
		{0,0 },//21
		{0,0 },//22
		{0,0 },//23
		{0,0 },//24
		{0,0 },//25
		{0,0 },//26
		{0,0 },//27
		{0,0 },//28
		{0,0 },//29
		{0,0 },//30
		{0,0 },//31
		{6, 0},//32  TURN AROUND
		{8, 0},//33  JUMP SQUAT
		{20,0},//34  BACK DASH
		{20,0},//35  FORWARD DASH
		{6, 0},//36  GROUND TURN
    };

    public KonkyBehaviours() : base(konkyMoveMap, konkyFrames) {}
}
//Level | Hitstop | Hitstun | Blockstun | Scaling
//0     | 8       | 12      | 9         | .75
//1     | 10      | 14      | 11        | .8
//2     | 12      | 16      | 13        | .85
//3     | 14      | 19      | 16        | .89
//4     | 16      | 21      | 18        | .92
//5     | 18      | 24      | 20        | .94

//Whiff Medium/Hard Normal: 1 point
//Whiff Special Move: 4 points
//Connect Light: 4 points
//Connect Medium: 8 points
//Connect Heavy: 12 points
//Connect Special: 8 points
//Land Throw: 5 points
//Have an Attack Blocked: 2 points
//Take Damage: 2 points
//Meter Size: 70