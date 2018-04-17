﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors {

    /* 
     * Action ID FORMAT
     * id = numpad + power
     * - light    = + 0
     * - medium   = + 10
     * - heavy    = + 20
     * - special  = + 30
     * - advanced = + 40
     * example: standM = 5 + 10 = 15
     */

    IDictionary<int, Action> konkyActionIds;
    IDictionary<Action, int> konkyAnimAction;

    /* 
       * ADVANCED ID FORMAT
       * 1 - Forward Dash
       * 2 - Back Dash
       * 3 - Forward Air Dash
       * 4 - Back Air Dash
       * 5 - Stun
       * 6 - Block
       * 7 - crouchBlock
       * 8 - airBlock
       * 9 - flip
       * 10 - crouchFlip
       * Other
       * 0 - Jump
       */

    public KonkyBehaviours()
    {
        konkyActionIds = new Dictionary<int, Action>()
        {
            { 1,  crouchL },
            { 11, crouchM },
            { 21, crouchH },

            { 2,  crouchL },
            { 12, crouchM },
            { 22, crouchH },

            { 3,  crouchL },
            { 13, crouchM },
            { 23, crouchH },

            { 4,  standL },
            { 14, standM },
            { 24, standH },

            { 5,  standL },
            { 15, standM },
            { 25, standH },

            { 6,  standL   },
            { 16, forwardM },
            { 26, standH   },

            { 7,  jumpL },
            { 17, jumpM },
            { 27, jumpH },

            { 8,  jumpL },
            { 18, jumpM },
            { 28, jumpH },

            { 9,  jumpL },
            { 19, jumpM },
            { 29, jumpH },

            { 31, oneS   },
            { 32, twoS   },
            { 33, threeS },
            { 34, fourS  },
            { 35, fiveS  },
            { 36, sixS   },

            { 41,    forwardDash},
            { 42,       backDash},
            { 43, forwardAirDash},
            { 44,    backAirDash},
            { 45,          stun },
            { 46,         block },
            { 47,   crouchBlock },
            { 48,      airBlock },
            { 49,          flip },
            { 50,    crouchFlip },
            { 51,     jumpSquat },

            { 101, crouch},
            { 102, crouch},
            { 103, crouch},
            { 104, walkBack},
            { 105, idle},
            { 106, walkForward},
            { 107, jumpBack},
            { 108, jump},
            { 109, jumpForward}
        };

        konkyAnimAction = new Dictionary<Action, int>()
        {
            {crouchL, 2 },
            {crouchM, 12 },
            {crouchH, 22 },

            {standL, 5 },
            {standM, 15 },
            {forwardM, 16 },
            {standH, 25 },

            {jumpL, 8 },
            {jumpM, 18 },
            {jumpH, 28 },

            {forwardDash, 41},
            {backDash, 42},
            {forwardAirDash, 43},
            {backAirDash, 44},
            {stun, 45 },
            {block, 46 },
            {crouchBlock, 47 },
            {airBlock, 48 },
            {flip, 49 },
            {crouchFlip, 50 },
            {jumpSquat, 51 }
        };

        setIds(konkyActionIds, konkyAnimAction);
    }

    //0 - Startup
    //1 - Active
    //2 - Multihit Recovery
    //3 - Recovery
    //4 - Buffer Window

    //Action class:
    //0 = light
    //1 = medium
    //2 = heavy
    //3 = special
    //4 = super

    // Standing Light
    private Action standL = new Action()
    {
        tier = 0,
        frames = new int[] { 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 4 | 3 | 9
        damage = new int[] { 0, 0, 0, 0, 300, 300, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        hitboxFrames = 3,
        hurtboxFrames = 17,
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(4, 9, 5, 9, 3, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 17, 0), new Action.rect(1.5f, 9f, 4, 8, 17, 1), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
        level = 0,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 5, 2, 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = .5f,
        aAngle      = 30,
        aStrength   = 1
    };

    // Standing Medium
    private Action standM = new Action()
    {
        tier = 1,
        frames      = new int[] { 0, 0, 0, 4, 4, 4, 4, 4, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 8 | 2 | 12
        damage      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 600, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(5, 9, 6, 2, 2, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 22, 1), new Action.rect(1.5f, 9f, 4, 8, 22, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
        level       = 2,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 12, 25, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 2
    };

    // Standing Heavy
    private Action standH = new Action()
    {
        tier = 2,
        frames      = new int[] { 0, 0, 0, 0, 4, 4, 4, 4, 4, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 9 | 4 | 17
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 900, 900, 900, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(8, 6.5f, 7, 2, 4, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            {nullBox, },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1.5f, 2.5f, 6.5f, 5, 30, 1), new Action.rect(2.5f, 9f, 4, 8, 30, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
        level       = 4,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = 30,
        aStrength   = 4
    };

    // Crouching Light
    private Action crouchL = new Action()
    {
        tier = 0,
        frames      = new int[] { 0, 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 6 | 3 | 10
        damage = new int[] { 0, 0, 0, 0, 0, 0, 300, 300, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(3, 0.5f, 8, 1, 3, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 5, 5, 19, 1), new Action.rect(-1.5f, 5.5f, 4, 4, 19, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
        level       = 0,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = .5f,
        aAngle      = 30,
        aStrength   = .5f
    };

    // Crouching Medium
    private Action crouchM = new Action()
    {
        tier = 1,
        frames      = new int[] { 0, 0, 4, 4, 4, 4, 4, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 7 | 4 | 13
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 250, 250, 250, 250, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(2, 6, 8, 7, 2, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            { nullBox, },
            { new Action.rect(2, 6, 8, 7, 2, 0), },
            { nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2, 7, 4, 24, 5), },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, }
        },
        level       = 1,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 15, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Crouching Heavy
    private Action crouchH = new Action()
    {
        tier = 3,
        frames = new int[] { 0, 0, 0, 4, 4, 4, 4, 4, 1, 1, 2, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 8 | 2 (1) 5 | 24
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 700, 700, 700, 700, 700, 700, 700, 700, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(5.5f, 3, 4, 5, 2, 0), nullBox,  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, nullBox, },
            { new Action.rect(6, 9.5f, 4, 14, 5, 0), new Action.rect(9, 10, 3, 8, 5, 1), }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 4, 7, 8, 11, 2), nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            { new Action.rect(2, 2.5f, 8, 5, 27, 2), new Action.rect(3.5f, 9f, 4, 8, 27, 3), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {new Action.rect(0.5f, 4, 7, 8, 4, 2), nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
        level = 3,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 31, 32, 33, 34, 35, 36, 40 },
        gAngle      = 80,
        gStrength   = 4,
        aAngle      = 80,
        aStrength   = 6
    };

    // Jumping Light
    private Action jumpL = new Action()
    {
        tier = 0,
        frames      = new int[] { 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 5 | 5 | 13
        damage = new int[] { 0, 0, 0, 0, 0, 300, 300, 300, 300, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(3.5f, 6, 9, 3, 5, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1, 9, 4, 6, 23, 1),  new Action.rect(3, 6, 8, 2, 5, 0),},
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
        level       = 2,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 17, 18, 19, 27, 28, 29, 40, 43, 44 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 45,
        aStrength   = 1
    };

    // Jumping Medium
    private Action jumpM = new Action()
    {
        tier = 1,
        frames      = new int[] { 0, 0, 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 7 | 6 | 17
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 200, 200, 200, 200, 200, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(6, 8, 6, 7, 2, 0), }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            { new Action.rect(6, 8, 6, 7, 2, 0), },
            {nullBox, },
            { new Action.rect(6, 8, 6, 7, 2, 0), },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(-0.5f, 5, 5, 6, 30, 1), new Action.rect(4, 9, 5, 5, 30, 2), },
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox}
        },
        level       = 2,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 27, 28, 29, 40, 43, 44 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 45,
        aStrength   = 1
    };

    // Jumping Heavy
    private Action jumpH = new Action()
    {
        tier = 2,
        frames      = new int[] { 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 10 | 4 | 23
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 900, 900, 900, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(5, 9, 7, 9, 4, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            {nullBox, },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1.5f, 8.5f, 6, 12, 10, 2), nullBox,},
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, new Action.rect(5, 4, 6, 4, 17, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
        level       = 3,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 40, 43, 44 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = -80,
        aStrength   = 60
    };

    // Forward Medium
    private Action forwardM = new Action()
    {
        tier = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 13 | 2 | 19
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 800, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(8, 2, 12, 4, 2, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(-0.5f, 7, 6, 14, 13, 1), },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            { new Action.rect(4, 4, 15, 8, 21, 1), },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, }
        },
        level       = 0,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 25, 31, 32, 33, 34, 35, 36 },
        gAngle      = 180,
        gStrength   = 2,
        aAngle      = 330,
        aStrength   = 5
    };


    // One Super
    private Action oneS = new Action()
    {
        tier = 3,
        frames      = new int[] { 0 },
        damage      = new int[] { 1200 },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level       = 0,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 35 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 0,
        aStrength   = 0
    };

    // Two Super
    private Action twoS = new Action()
    {
        tier = 3,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level       = 5,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 35, 41, 42 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = 30,
        aStrength   = 2
    };

    // Three Super
    private Action threeS = new Action()
    {
        tier = 3,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level       = 4,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 35 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Four Super
    private Action fourS = new Action()
    {
        tier = 3,
        frames      = new int[] { 0, 4, 4, 4, 4, 4, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3 },
        damage      = new int[] { /*senbeans job*/ },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level       = 2,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 35 },
        gAngle      = 45,
        gStrength   = 1,
        aAngle      = 45,
        aStrength   = 1
    };

    // Five Super
    private Action fiveS = new Action()
    {
        tier = 4,
        frames      = new int[] { 4, 4, 4, 4, 4, 1, 3, 3, 3, 3, 3 },
        damage      = new int[] { /*senbeans job*/ },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level       = 0,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { },
        gAngle      = 0,
        gStrength   = 5,
        aAngle      = 0,
        aStrength   = 0
    };

    // Six Super
    private Action sixS = new Action()
    {
        tier = 3,
        frames      = new int[] { 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { /*senbeans job*/ },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level       = 5,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 35 },
        gAngle      = 60,
        gStrength   = 8,
        aAngle      = 60,
        aStrength   = 8
    };

    // Throw
    private Action Throw = new Action()
    {
        tier = 2,
        frames      = new int[] { 0 },
        damage      = new int[] { /*senbeans job*/ },
        level       = 5,
        //hitstun = ,
        //blockstun = ,
        actionCancels = new int[] { 1, 2 },
        gAngle      = 30,
        gStrength   = 10,
        aAngle      = 0,
        aStrength   = 0
    };


    // Jump Squat
    private Action jumpSquat = new Action() {frames = new int[] { 0, 0, 0 }, actionCancels = new int[] { },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 3, 1), new Action.rect(0.5f, 9f, 4, 8, 3, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
    };

    // Turns
    private Action flip = new Action()       {frames = new int[] { 0, 0, 0                                                     }, actionCancels  = new int[] {       },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 3, 1), new Action.rect(0.5f, 9f, 4, 8, 3, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
    };

    // crouch Turns
    private Action crouchFlip = new Action() {frames = new int[] { 0, 0, 0, }, actionCancels = new int[] { },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 4, 7, 8, 3, 5), },
            {nullBox },
            {nullBox },
        },
    };

    // Back Dash
    private Action backDash = new Action()    {frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, actionCancels  = new int[] {  },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 0, 0, 0, 10, 1), new Action.rect(0, 0, 0, 0, 10, 2), },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {new Action.rect(0.5f, 2.5f, 6.5f, 5, 10, 1), new Action.rect(-1.5f, 9f, 4, 8, 10, 2), },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, }
        },
    };

    // Forward Dash
    private Action forwardDash = new Action() {frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 }, actionCancels = new int[] { 40 }, infinite = true,
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(-4, 2.5f, 8, 5, 14, 1), new Action.rect(2.5f, 7, 5, 8, 14, 2), },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            { new Action.rect(-4, 2.5f, 8, 5, 1, 1), new Action.rect(2.5f, 7, 5, 8, 1, 2), }
        },
    };

    // Stun
    private Action stun = new Action()        {frames = new int[] { 3                                                          }, actionCancels = new int[] {       },
        infinite = true,
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 6, 4, 12, 40, 1)},
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox }
        },
    };

    // Block
    private Action block = new Action()       {frames = new int[] { 0                                                          }, actionCancels = new int[] {       } };

    // Crouching Block
    private Action crouchBlock = new Action() {frames = new int[] { 0 }, actionCancels = new int[] { } };

    // Air Block
    private Action airBlock = new Action()    {frames = new int[] { 0 }, actionCancels = new int[] { } };

    // Air Dash
    private Action forwardAirDash = new Action() {frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, }, actionCancels = new int[] {       },
        hurtboxData = new Action.rect[,]
        {
            //{ new Action.rect(2, 7, 14, 4, 15, 1),},
            { new Action.rect(-4, 2.5f, 8, 5, 15, 1), new Action.rect(2.5f, 7, 5, 8, 15, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
    };
    private Action backAirDash = new Action()    {frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, actionCancels = new int[] {       },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 0, 0, 0, 3, 1), new Action.rect(0, 0, 0, 0, 3, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            { new Action.rect(3, 3, 3, 6, 17, 1), new Action.rect(1, 9, 3, 6, 17, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
    };















    private Action crouch = new Action() {
       frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 4, 7, 8, 40, 5), },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox }
        },
    };
    private Action walkBack = new Action() {
       frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 20, 3), new Action.rect(1.5f, 9f, 4, 8, 20, 4), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
    };
    private Action idle = new Action() {
       frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 40, 1), new Action.rect(1.5f, 9f, 4, 8, 40, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
    };
    private Action walkForward = new Action() {
        frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 ,3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 25, 6), new Action.rect(1.5f, 9f, 4, 8, 25, 7), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
    };
    private Action jumpBack = new Action() {
       frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(3, 3, 3, 6, 10, 8), new Action.rect(1, 9, 3, 6, 10, 9), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
    };
    private Action jump = new Action() {
       frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1, 6, 4, 12, 10, 10), },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox }
        },
    };
    private Action jumpForward = new Action() {
       frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 3, 3, 6, 10, 11), new Action.rect(2, 9, 4, 8, 10, 12), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
    };




    public override void setStats()
    {
        forwardSpeed = 0.25f;
        backwardSpeed = -0.15f;
        jumpDirectionSpeed = 1.25f;
        dashForwardSpeed = 3f;
        dashBackSpeed = 3f;
        airDashForwardSpeed = 3f;
        airDashBackSpeed = 3f;
        gravity = -0.05f;
        maxHealth = 11000;
        infiniteDashForward = true;
    }

    public override float getAttackMovementHorizontal(int attackState)
    {
        switch (attackState) {
            case 2:
                return 1;
                break;
            case 12:
                return 1;
                break;
            case 15:
                return 1;
                break;
            case 25:
                return 1;
                break;
            case 16:
                return 1;
                break;
            case 8:
                return 1.5f;
                break;
            case 18:
                return 1.5f;
                break;
            case 28:
                return 1.5f;
                break;

            default:
                return 0;
                break;

        }
    }

    public override float getAttackMovementVertical(int attackState)
    {
        switch (attackState)
        {
            case 8:
                return 1.5f;
                break;
            case 18:
                return 1.5f;
                break;
            case 28:
                return 1.5f;
                break;

            default:
                return 0;
                break;

        }
    }




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
//Have an Action Blocked: 2 points
//Take Damage: 2 points
//Meter Size: 70 