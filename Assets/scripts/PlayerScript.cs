﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private float FLOOR_HEIGHT = 0;
    private float BASE_GRAVITY = -0.05f;

    private float[,] levelScaling = new float[,]
    {
        { 8,  12, 23, 9,  .75f },
        { 10, 14, 26, 11, .8f  },
        { 12, 16, 28, 13, .85f },
        { 14, 19, 33, 16, .89f },
        { 16, 21, 36, 18, .92f },
        { 18, 24, 40, 20, .94f }
    };

    public bool passedPlayerInAir;     // true if pass other player while airborn
    public bool passedPlayerInAction;  // true if pass other player while they're in an action
    public bool airborn;               // true if in the air
    public bool hitStopped;            // true if in hitstop
    public bool hitStunned;            // true if in hitstun
    public bool shouldFlip;            // true when pass other player in air, signifying flipping upon landing
    public bool facingRight;           // true if the player is facing right
    public bool playerSide;            // true if left of the other player, false if right
    public bool dashingForwards;       // true if dashing forward, false if dashing back
    public bool airbornActionUsed;     // true if player has spent airial action
    public bool damageDealt;           // true if damage was dealt this frame
    public bool inPushCollision;       // true if player push boxes are currently colliding

    public int bufferedMove;           // the move currently buffered
    public int maxHealth;              // starting health of the player
    public int health;                 // current health of the player
    public int currentFrameType;       // frame type of the currently executing action
    public int actionFrameCounter;     // current frame number of the executing action
    public int activeFrameCounter;     // current active frame number of the executing action
    public int playerID;               // the players id, either 1 or 0
    public int meterCharge;            // current meter charge
    public int dashTimer;              // counts number of frames player has been dashing for
    public int executingAction;        // currently executing action
    public int stunTimer;              // timer counting down the time player has been in hitstun
    public int previousBasicState;     // basic state last frame
    public int basicAnimFrame;         // current frame number of playing animation

    /* MOVEMENT NUMPAD NOTATION
     * [ jump back ][ jump ][ jump forwards ]
     * [ walk back ][      ][ walk forwards ]
     * [crouch back][crouch][crouch forwards]
     */
    public int basicState; // current movement state

    /* ATTACK NUMPAD NOTATION
     * [       ][        ][       ]
     * [ light ][ medium ][ heavy ]
     * [       ][        ][       ]
     */
    public int attackState; // current attack state

    /* ADVANCED STATES
     * 1 - forward dash
     * 2 - back dash
     * 3 - forward air dash
     * 4 - forward back dash
     * 5 - stun
     */
    public int advancedState; // current advanced state

    /* JUMP STATES
     * 0 - no jump
     * 7 - back jump
     * 8 - up jump
     * 9 - forward jump
     */
    public int jumpDirection;   // current jump direction

    /* UPDATE STATES
     * 0 - ganeUpdate
     * 1 - hitstop / no update
     * 2 - updateEnd
     */
    public int updateState;            // keeps track of what type of update the player should execute

    public float hKnockback;          // horizontal knockback
    public float vKnockback;          // vertical knockback
    public float gravity;             // gravity constant, default to BASE_GRAVITY
    public float vVelocity;           // vertical velocity
    public float hVelocity;           // horizontal velocity
    public float hPush;               // amount the player will be horizontally pushed
    public float vPush;               // amount the player will be vertically pushed
    public float forwardSpeed;        // forward movement speed constant
    public float backwardSpeed;       // backwards movement speed constant
    public float jumpDirectionSpeed;  // jump speed constant

    public SpriteRenderer    spriteRenderer;
    public Animator          animator;
    public Behaviors         behaviors;
    public PolygonCollider2D hitbox;
    public GameObject        otherPlayer;
    public JoyScript         JoyScript;
    public InputManager      inputManager;

    public List<float> livingHitboxesIds;
    public List<float> livingHitboxesLifespans;
    public List<float> livingHurtboxesIds;
    public List<float> livingHurtboxesLifespans;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(transform.position);
    }

    void Start()
    {
        this.tag = playerID.ToString();
        this.transform.GetChild(0).tag = "collisionHitbox" + playerID.ToString();
        hitbox = GetComponentInChildren<PolygonCollider2D>();

        vVelocity = 0;
        hVelocity = 0;
        behaviors = new KonkyBehaviours();
        behaviors.setStats();
        forwardSpeed = behaviors.getForwardSpeed();
        backwardSpeed = behaviors.getBackwardSpeed();
        jumpDirectionSpeed = behaviors.getJumpDirectionSpeed();
        maxHealth = behaviors.getMaxHealth();
        gravity = behaviors.getGravity();
        health = maxHealth;

        livingHitboxesIds = new List<float>();
        livingHitboxesLifespans = new List<float>();

        if (CompareTag("1"))
            inputManager = new InputManager(1);
        else if (CompareTag("2"))
            inputManager = new InputManager(2);
    }

    private void Update()
    {
        inputManager.pollInput(0);

        basicState = inputConvert(inputManager.currentInput);
        setAttackInput(inputManager.currentInput);

        if (attackState % 10 >= 7 && !airborn)
            attackState = 0;
        setAdvancedInput(inputManager.currentInput);

        hPush = 0;
        vPush = 0;

        if (hitStopped)
        {
            if (advancedState != 0)
                buffer(advancedState + 40);
            else if (attackState != 0)
                buffer(attackState);

            updateState = 1;
        }
        else
        {
            if (executingAction != 0)
            {
                if (previousBasicState != 0)
                {
                    previousBasicState = 0;
                    killAllBoxes();
                }

                incrementFrame(behaviors.getAction(executingAction).frames);
            }
            stateCheck();

            if (executingAction > 40)
                advancedMove();

            updateState = 2;
        }
        Debug.Log("SDFSDFSDFSDFSDFSD: " + Time.timeScale);
    }

    private void setAttackInput(bool[] input)
    {
        if (input[4])
            attackState = basicState;
        else if (input[5])
            attackState = basicState + 10;
        else if (input[6])
            attackState = basicState + 20;
        else if (input[7])
            attackState = basicState + 30;
        else
            attackState = 0;
    }

    private void setAdvancedInput(bool[] input)
    {
        if (executingAction != 41)
        {
            if (dashTimer == 0 && advancedState <= 4)
                advancedState = 0;

            if (input[8] && !dashingForwards && dashTimer != 0)
            {
                if (facingRight)
                {
                    if (airborn)
                    {
                        if (!airbornActionUsed)
                        {
                            advancedState = 4;
                            airbornActionUsed = true;
                        }
                    }
                    else
                        advancedState = 2;
                }
                else
                {
                    if (airborn)
                    {
                        if (!airbornActionUsed)
                        {
                            advancedState = 3;
                            airbornActionUsed = true;
                        }
                    }
                    else
                        advancedState = 1;
                }
                dashTimer = 0;
            }
            else if (input[9] && dashingForwards && dashTimer != 0)
            {
                if (facingRight)
                {
                    if (airborn)
                    {
                        if (!airbornActionUsed)
                        {
                            advancedState = 3;
                            airbornActionUsed = true;
                        }
                    }
                    else
                        advancedState = 1;
                }
                else
                {
                    if (airborn)
                    {
                        if (!airbornActionUsed)
                        {
                            advancedState = 4;
                            airbornActionUsed = true;
                        }
                    }
                    else
                        advancedState = 2;
                }
                dashTimer = 0;
            }

            if (passedPlayerInAir && !airborn)
            {
                passedPlayerInAir = false;
                ActionEnd();
                advancedState = 9;
            }

            if ((!input[8] || !input[9]) && dashTimer != 0)
                dashTimer--;

            if (input[8] || input[9])
            {
                dashTimer = 15;
                if (input[8])
                    dashingForwards = false;
                else
                    dashingForwards = true;
            }
        }

        if (shouldFlip)
        {
            shouldFlip = false;
            if (executingAction != 0)
                passedPlayerInAction = true;

            if (airborn)
                passedPlayerInAir = true;

            if (!airborn && executingAction == 0)
            {
                if (basicState <= 3)
                    advancedState = 10;
                else
                    advancedState = 9;
                dashTimer = 0;
            }
        }
    }

    private void buffer(int bufferedInput)
    {
        foreach (int action in behaviors.getAction(executingAction).actionCancels)
            if (action == bufferedInput)
                bufferedMove = bufferedInput;
    }

    private void incrementFrame(int[] frames)
    {
        placeHurtboxes(actionFrameCounter);

        int previousFrame = currentFrameType;
        currentFrameType = frames[actionFrameCounter];
        actionFrameCounter++;

        // if first active frame in action
        if (previousFrame != 1 && currentFrameType == 1)
        {
            otherPlayer.GetComponentInChildren<CollisionScript>().initialFrame = false;
        }

        // if active frame
        if (currentFrameType == 1)
        {
            if (executingAction < 40)
                placeHitboxes();

            // if not hitsopped buffer move
            if (!hitStopped)
                if (advancedState != 0)
                    buffer(advancedState + 40);
                else if (attackState != 0)
                    buffer(attackState);

            activeFrameCounter++;
        }
        else
        {
            damageDealt = false;
        }

        // if recovery frame
        if (currentFrameType == 3)
        {
            // if grounded from a non-infinite action that passed the other player
            if (!airborn && passedPlayerInAction && !passedPlayerInAir && !behaviors.getAction(executingAction).infinite)
            {
                ActionEnd();
            }
            // if executing advanced action and not passed the other player
            else if (advancedState != 0 && !passedPlayerInAction)
            {
                foreach (int Actions in behaviors.getAction(executingAction).actionCancels)
                    if (Actions == advancedState + 40)
                        bufferedMove = advancedState + 40;
            }
            else if (basicState >= 7 && !passedPlayerInAction)
            {
                foreach (int Actions in behaviors.getAction(executingAction).actionCancels)
                    if (Actions == 40 && inputManager.currentInput[12] && !airbornActionUsed)
                    {
                        bufferedMove = 40;
                        if (basicState == 7)
                            jumpDirection = 7;
                        else if (basicState == 8)
                            jumpDirection = 8;
                        else
                            jumpDirection = 9;


                    }
            }
            else if (attackState != 0 && !passedPlayerInAction)
            {
                foreach (int action in behaviors.getAction(executingAction).actionCancels)
                    if (action == attackState)
                        bufferedMove = attackState;
            }

            if (bufferedMove != 0 && !passedPlayerInAction)
            {
                if (bufferedMove > 40)
                    advancedState = bufferedMove - 40;
                else if (bufferedMove == 40)
                {
                    advancedState = 0;
                    attackState = 0;
                }
                else
                    attackState = bufferedMove;
                bufferedMove = 0;
                ActionEnd();
            }
        }
    }

    private void stateCheck()
    {
        if (executingAction != 0)
        {
            advancedState = 0;
            attackState = 0;
            if (!airborn)
                hVelocity = 0;
            if (actionFrameCounter >= behaviors.getAction(executingAction).frames.Length)
            {
                if (behaviors.getAction(executingAction).infinite)
                    actionFrameCounter--;
                else
                    ActionEnd();
            }
        }
        else if (advancedState != 0)
        {
            executingAction = advancedState + 40;
        }
        else if (attackState != 0)
        {
            executingAction = attackState;
        }
        else
        {
            basicMove();
        }
    }

    private int inputConvert(bool[] input)
    {
        if (!airborn)
        {
            if (input[0])
            {
                if (input[2])
                    return (facingRight ? 7 : 9);
                else if (input[3])
                    return (facingRight ? 9 : 7);
                else
                    return 8;
            }
            else if (input[1])
            {
                if (input[2])
                    return (facingRight ? 1 : 3);
                else if (input[3])
                    return (facingRight ? 3 : 1);
                else
                    return 2;
            }
            else if (input[2])
                return (facingRight ? 4 : 6);
            else if (input[3])
                return (facingRight ? 6 : 4);
            else
                return 5;
        }
        else
        {
            return basicState;
        }
    }

    private void basicMove()
    {
        if (!airborn)
        {
            jumpDirection = 0;

            if (basicState == 8)
                vVelocity = jumpDirectionSpeed;
            else if (basicState == 7)
            {
                vVelocity = jumpDirectionSpeed;
                hVelocity = backwardSpeed;
            }
            else if (basicState == 9)
            {
                vVelocity = jumpDirectionSpeed;
                hVelocity = forwardSpeed;
            }
            else if (basicState == 5)
            {
                vVelocity = 0;
                hVelocity = 0;
            }
            else if (basicState == 6 || basicState == 4)
            {
                hVelocity = (basicState == 6 ? forwardSpeed : backwardSpeed);
            }

            if (basicState < 4)
            {
                hVelocity = 0;
            }
        }

        if (airborn)
        {
            if ((inputManager.currentInput[12] && inputManager.currentInput[2] && facingRight) || (inputManager.currentInput[12] && inputManager.currentInput[3] && !facingRight))
            {
                jumpDirection = 7;

                if (!airbornActionUsed)
                    basicState = 7;
            }
            else if ((inputManager.currentInput[12] && inputManager.currentInput[3] && facingRight) || (inputManager.currentInput[12] && inputManager.currentInput[2] && !facingRight))
            {
                jumpDirection = 9;

                if (!airbornActionUsed)
                    basicState = 9;
            }
            else if (inputManager.currentInput[12])
            {
                jumpDirection = 8;
                if (!airbornActionUsed)
                    basicState = 8;
            }

            if (!airbornActionUsed && jumpDirection >= 7)
            {
                airbornActionUsed = true;

                if (jumpDirection == 7)
                {
                    vVelocity = jumpDirectionSpeed;
                    hVelocity = backwardSpeed;
                }
                else if (jumpDirection == 9)
                {
                    vVelocity = jumpDirectionSpeed;
                    hVelocity = forwardSpeed;
                }
                else
                {
                    vVelocity = jumpDirectionSpeed;
                    hVelocity = 0;
                }
            }
        }

        if (previousBasicState == basicState)
        {
            if (basicAnimFrame >= behaviors.getAction(basicState + 100).hurtboxData.GetLength(0))
            {
                //killAllBoxes();
                basicAnimFrame = 0;
            }

            placeHurtboxes(basicAnimFrame);
            basicAnimFrame++;
        }
        else
        {
            Debug.Log("basic Move ran");
            killAllBoxes();
            placeHurtboxes(0);
            basicAnimFrame = 1;
            previousBasicState = basicState;
        }

    }

    private void advancedMove()
    {
        switch (executingAction - 40)
        {
            case 1:
                hVelocity = forwardSpeed * 3;
                if ((!inputManager.currentInput[2] && !dashingForwards) || (!inputManager.currentInput[3] && dashingForwards))
                {
                    hVelocity = 0;
                    ActionEnd();
                }
                break;
            case 2:
                hVelocity = backwardSpeed * 3f;
                break;
            case 3:
                vVelocity = 0;
                hVelocity = forwardSpeed * 3;
                break;
            case 4:
                vVelocity = 0;
                hVelocity = backwardSpeed * 3f;
                break;
            case 5:
                stunTimer--;
                if (stunTimer <= 0)
                {
                    ActionEnd();
                }
                break;
            case 6:
                break;
            case 7:
                break;
        }
    }

    public void UpdateEnd()
    {
        GetComponent<SpriteRenderer>().flipX = facingRight ? false : true;

        knockbackDecrease();

        movePlayer();

        updateAnimation();
    }

    public void knockbackDecrease()
    {
        if (hKnockback != 0)
        {
            if (!airborn)
            {
                hKnockback *= .75f;
                if (Mathf.Abs(hKnockback) < 0.005f)
                    hKnockback = 0;
            }
            else
            {
                hKnockback *= .5f;
                if (Mathf.Abs(hKnockback) < 0.005f)
                    hKnockback = 0;
            }
        }

        if (vKnockback != 0)
        {
            vKnockback *= .85f;
            if (Mathf.Abs(vKnockback) < 0.005f)
            {
                vKnockback = 0;
            }
        }
    }

    private void movePlayer()
    {
        if (executingAction != 43 && executingAction != 44)
            vVelocity += gravity;

        if (vVelocity < -1)
        {
            vVelocity = -1;
        }

        moveX((facingRight ? hVelocity - hPush : -hVelocity + hPush) + hKnockback);
        moveY(vVelocity + vKnockback + vPush);

        if (x() < -64f)
        {
            setX(-64);
        }
        else if (x() > 64f)
        {
            setX(64);
        }

        if (y() <= FLOOR_HEIGHT) //ground snap
        {
            if (airborn)
            {
                airborn = false;
                airbornActionUsed = false;

                if (executingAction > 0)
                    ActionEnd();
            }
            vVelocity = 0;
            setY(FLOOR_HEIGHT);
        }
        else
        {
            airborn = true;
        }
    }

    public void updateAnimation()
    {
        if (executingAction != 0)
        {
            Debug.Log("CURRENT ACTIONNN: " + executingAction);
            animInt(Animator.StringToHash("Action"), behaviors.getAnimAction(behaviors.getAction(executingAction)));
            animInt(Animator.StringToHash("Basic"), 0);
        }
        else
        {
            animInt(Animator.StringToHash("Basic"), basicState);
            animInt(Animator.StringToHash("Action"), 0);
        }
    }  

    private void placeHitboxes()
    {
        Action.rect[,] hitboxData = behaviors.getAction(executingAction).hitboxData;

        for (int i = 0; i < hitboxData.GetLength(1); i++)
        {
            Action.rect hitbox = hitboxData[activeFrameCounter, i];

            if (!livingHitboxesIds.Contains(hitbox.id) && hitbox.id != -1)
            {
                livingHitboxesIds.Add(hitbox.id);
                livingHitboxesLifespans.Add(hitbox.timeActive);
                addBoxCollider2D(hitbox.id.ToString(), new Vector2(hitbox.width, hitbox.height),  new Vector2((facingRight ? hitbox.x : -hitbox.x), hitbox.y), true);
            }
        }
    }

    private void placeHurtboxes(int frame)
    {
        Action.rect[,] hurtboxData;

        if (executingAction != 0)
        {
            hurtboxData = behaviors.getAction(executingAction).hurtboxData;
        }
        else
        {
            hurtboxData = behaviors.getAction(basicState + 100).hurtboxData;
        }
        for (int i = 0; i < hurtboxData.GetLength(1); i++)
        {
            Action.rect hurtbox = hurtboxData[frame, i];

            if (!livingHurtboxesIds.Contains(hurtbox.id) && hurtbox.id != -1)
            {
                livingHurtboxesIds.Add(hurtbox.id);
                livingHurtboxesLifespans.Add(hurtbox.timeActive);
                addBoxCollider2D((hurtbox.id + 100).ToString(), new Vector2(hurtbox.width, hurtbox.height), new Vector2((facingRight ? hurtbox.x : -hurtbox.x), hurtbox.y), false);
            }
            else if (livingHurtboxesIds.Contains(hurtbox.id))
            {
                Debug.Log(livingHurtboxesIds.Count);
                Debug.Log("repeat call");
            }
        }
    }

    public void decreaseHitboxLifespan()
    {
        for (int j = livingHitboxesLifespans.Count; j > 0; j--)
            if (livingHitboxesLifespans[j - 1] > 0)
            {
                livingHitboxesLifespans[j - 1]--;
            }
            else
            {
                removeBoxCollider2D(livingHitboxesIds[j - 1].ToString(), hitbox);
                livingHitboxesIds.RemoveAt(j - 1);
                livingHitboxesLifespans.RemoveAt(j - 1);
            }
    }

    public void decreaseHurtboxLifespan()
    {
        for (int j = livingHurtboxesLifespans.Count; j > 0; j--)
        {
            if (livingHurtboxesLifespans[j - 1] > 1)
            {
                livingHurtboxesLifespans[j - 1]--;
            }
            else
            {
                removeBoxCollider2D((livingHurtboxesIds[j - 1] + 100).ToString(), false);
                livingHurtboxesIds.RemoveAt(j - 1);
                livingHurtboxesLifespans.RemoveAt(j - 1);
            }
        }
    }

    private void addBoxCollider2D(String name, Vector2 size, Vector2 offset, bool boxType)
    {
        GameObject childbox = new GameObject(name);

        childbox.transform.position = transform.position;
        childbox.transform.SetParent(boxType ? transform.GetChild(1) : transform.GetChild(2));

        childbox.tag = (boxType ? "hitbox" + playerID : "hurtbox" + playerID);

        /*if (!boxType)
            childbox.AddComponent<HurtboxScript>();
        else
            childbox.AddComponent<RealHitboxScript>();
        */
        childbox.AddComponent<BoxCollider2D>();
        childbox.GetComponent<BoxCollider2D>().size = size;
        childbox.GetComponent<BoxCollider2D>().offset = offset;
        if (!boxType)
            childbox.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void removeBoxCollider2D(String name, bool boxType)
    {
        //boxType true = hitbox, false = hurtbox
        foreach (Transform child in boxType ? transform.GetChild(1) : transform.GetChild(2))
        {
            if (child.gameObject.name.Equals(name))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void killAllHurtboxes()
    {
        livingHurtboxesIds.Clear();
        livingHurtboxesLifespans.Clear();

        foreach (Transform child in transform.GetChild(2))
        {
            if (child.gameObject.tag.Equals("hurtbox1") || child.gameObject.tag.Equals("hurtbox2"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void killAllHitboxes()
    {
        livingHitboxesIds.Clear();
        livingHitboxesLifespans.Clear();

        foreach (Transform child in transform.GetChild(1))
        {
            if (child.gameObject.tag.Equals("hitbox1") || child.gameObject.tag.Equals("hitbox2"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void killAllBoxes()
    {
        killAllHitboxes();
        killAllHurtboxes();
    }

    public void onPush(float otherHVel, float otherVVelocity)
    {
        hPush = facingRight ? (hVelocity + otherHVel) / 2 : (otherHVel + hVelocity) / 2;
        vPush = airborn ? (vVelocity + otherVVelocity) / 2 : 0;
    }

    private void ActionEnd()
    {
        Debug.Log("action end");

        if (executingAction == 49 || executingAction == 50)
        {
            facingRight = !facingRight;
        }

        executingAction = 0;
        currentFrameType = 0;
        actionFrameCounter = 0;
        activeFrameCounter = 0;
        basicAnimFrame = 0;
        previousBasicState = 0;
        killAllBoxes();

        if (passedPlayerInAction)
        {
            passedPlayerInAction = false;
            if (!passedPlayerInAir)
            {
                if (basicState <= 3)
                    executingAction = 50;
                else
                    executingAction = 49;
            }
        }
    }

    public void damage(int damage, float knockback, int angle)
    {
        health -= damage;
        hKnockback = knockback * Mathf.Cos(((float)angle / 180f) * Mathf.PI) * (facingRight ? -1 : 1);
        vKnockback = knockback * Mathf.Sin(((float)angle / 180f) * Mathf.PI);

        ActionEnd();

        stun();

        if (vKnockback > 0)
        {
            airborn = true;
        }

        vVelocity = 0;
    }

    public void stun()
    {
        executingAction = 45;
        stunTimer = (int)otherPlayer.GetComponent<PlayerScript>().level(1);
    }

    public void block(int amm)
    {

    }

    public float level(int wanted)
    {
        return levelScaling[behaviors.getAction(executingAction).level, wanted];
    }


    private void animInt(int hash, int value)
    {
        animator.SetInteger(hash, value);
    }

    private void animBool(bool b, string s)
    {
        animator.SetBool(s, b);
    }

    public void moveX(float amm)
    {
        Vector3 position = this.transform.position;
        position.x += amm;
        this.transform.position = position;
    }

    public void moveY(float amm)
    {
        Vector3 position = this.transform.position;
        position.y += amm;
        this.transform.position = position;
    }

    public void setY(float amm)
    {
        Vector3 position = this.transform.position;
        position.y = amm;
        this.transform.position = position;
    }

    public void setX(float amm)
    {
        Vector3 position = this.transform.position;
        position.x = amm;
        this.transform.position = position;
    }

    public float y()
    {
        return this.transform.position.y;
    }

    public float x()
    {
        return this.transform.position.x;
    }
}