﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors{

	//0 total frames
	//1 recovery frames
	//3 damage
	//4 chip damage DO THIS
	//5 blockstun

	static Action crouchL = new Action() { frames = new int[] {0,0,0,0,0,0,1,1,1,3,3,3,3,3,3,3,3,3,3 }, damage = new int[] {300}, level = 0, cancels = new int[]{a5m, a2m, a6m, a5h, a2h, a1s, a2s, a3s, a4s, a5s, a6s} };
	static Action crouchM = new Action() { frames = new int[] {0,0,0,0,0,0,0,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3 }, damage = new int[] {650}, level = 1, cancels = new int[] {a5m, a5h, a2h, a1s, a2s, a3s, a4s, a5s, a6s } };
	static Action crouchH = new Action() { frames = new int[] {0,0,0,0,0,0,0,0,1,1,2,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3 }, damage = new int[] { 200, 650 }, level = 2, cancels = new int[] {aJump, a1s, a2s, a3s, a4s, a5s, a6s } };
	static Action standL = new Action() { frames = new int[] {0,0,0,0,1,1,1,3,3,3,3,3,3 }, damage = new int[] { 200 }, level = 0, cancels = new int[] {a5l, a2l, a5m, a2m, a6m, a5h, a2h, a1s, a2s, a3s, a4s, a5s, a6s } };
    static Action standM = new Action() { frames = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3 }, damage = new int[] { 625 }, level = 1, cancels = new int[] { a2m, a5h, a5h, a1s, a2s, a3s, a4s, a5s, a6s } };
    static Action standH = new Action() { frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, damage = new int[] { 3 }, level = 3, cancels = new int[] { a2h, a1s, a2s, a3s, a4s, a5s, a6s } };
    static Action jumpL = new Action() { frames = new int[] { }, damage = new int[] { }, level = 2, cancels = new int[] { } };
    static Action jumpM = new Action() { frames = new int[] { }, damage = new int[] { }, level = 2, cancels = new int[] { } };
    static Action jumpH = new Action() { frames = new int[] { }, damage = new int[] { }, level = 3, cancels = new int[] { } };
    static Action Throw = new Action() { frames = new int[] { }, damage = new int[] { }, level = 5, cancels = new int[] { } };
    static Action oneS = new Action() { frames = new int[] { }, damage = new int[] { }, level = 0, cancels = new int[] { } };
    static Action twoS = new Action() { frames = new int[] { }, damage = new int[] { }, level = 5, cancels = new int[] { } };
    static Action threeS = new Action() { frames = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3 }, damage = new int[] { }, level = 4, cancels = new int[] { } };
    static Action fourS = new Action() { frames = new int[] {0,0,0,0,0,0,1,1,1,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,3,3,3,3 }, damage = new int[] {400, 400, 600}, level = 2, cancels = new int[] { } };
    static Action fiveS = new Action() { frames = new int[] { }, damage = new int[] { }, level = 0, cancels = new int[] { } };
    static Action sixS = new Action() { frames = new int[] {0,0,1,1,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3 }, damage = new int[] {1500}, level = 5, cancels = new int[] { } };

    static Action turns = new Action() { frames = new int[] {3,3,3,3,3,3 }, cancels = new int[] { } };
	static Action jump = new Action() { frames = new int[] {3,3,3,3,3,3,3,3}, cancels = new int[] { } };
	static Action backDash = new Action() { frames = new int[] {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3 }, cancels = new int[] {aJump} };
	static Action forwardDash = new Action() { frames = new int[] {3 }, infinite = true, cancels = new int[] { aJump } };
    

	static Action[] konkyActions = new Action[]
	{
		crouchL,
		crouchM,
		crouchH,
		crouchL,
		crouchM,
		crouchH,
		crouchL,
		crouchM,
		crouchH,
		null,
		null,
		null,
		standL,
		standL,
		standL,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		turns,
		turns,
		jump,
		backDash,
		forwardDash,
		null,
		null,
		null,
		null,
		null
	};

	public KonkyBehaviours() : base(konkyActions) { }
}
//Level | Hitstop | Hitstun | Counterhit | Blockstun | Scaling
//0     | 8       | 12      | 23         | 9         | .75
//1     | 10      | 14      | 26         | 11        | .8
//2     | 12      | 16      | 28         | 13        | .85
//3     | 14      | 19      | 33         | 16        | .89
//4     | 16      | 21      | 36         | 18        | .92
//5     | 18      | 24      | 40         | 20        | .94

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