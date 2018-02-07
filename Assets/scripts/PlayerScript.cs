﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    //CONSTANT
    float FLOOR_HEIGHT = 0;
    float BASE_GRAVITY = -0.05f;
    int NO_ATTACK_INDEX = -1;
    int NO_ATTACK_STRENGTH = -1;
    int LIGHT_ATTACK = 0;
    int MEDIUM_ATTACK = 1;
    int HEAVY_ATTACK = 2;
	int SPECIAL_ATTACK = 3;
    int ANIM_STATE = Animator.StringToHash("state");

    public float[,] levelScaling = new float[,] { 
        { 8, 12, 23, 9, .75f }, 
        { 10, 14, 26, 11, .8f }, 
        { 12, 16, 28, 13, .85f }, 
        { 14, 19, 33, 16, .89f }, 
        { 16, 21, 36, 18, .92f }, 
        { 18, 24, 40, 20, .94f }
    };

    public bool juggle;
    public bool dashing;

    public KeyCode[] dashKey;

    public int dashDirectKey;
    public bool dashDirect;
    public bool previousDirect;
    public bool groundDash;

	bool dashed;
    public int DashTimer;
    bool DashCount;

	public bool passDir;
	public bool waitForGround;
	public bool waitForEnd;
	
	public int jumpPass;

	public float gravity;

    private int stunTimer;

    public float vVelocity;
    public float hVelocity;

    private float forwardSpeed;
    private float backwardSpeed;
    private float jumpSpeed;

	public float baseHeight;
	public float width;
	public float height;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Behaviors behaviors;
    public BoxCollider2D hitbox;
    public BoxCollider2D hurtbox;

    public bool air;
    public bool airLock;

    public bool hitStopped;

    public int maxHealth;
    public int health;

	public List<int> history;
	public List<int> delays;
	public int inputTimer;

    public int state;
    public int iState;
	public int heldState;

    public int storedAttackStrength;
    public int bufferFrames;
    public int iAttack = -1;//input attack strength (LMH)

	private int STARTUP = 0;
	private int ACTIVE = 1;
	private int BREAK = 2;
	private int RECOVERY = 3;
    public int actionState;//attack going on rn
    public bool action;//is there an attack
	public int type;//current type of frame
	public int[] attackTypes;//the list of frame types
    public int actionFrames; //total time
	public int actionCounter;//the timer that moves along (counting up)
	public int damageCounter;//counts up damage amounts
	public bool infiniteAction;//INFINTIYNIYIN
    public int lvl;

	public bool upLock = false;
	public bool leftLock = false;
	public bool downLock = false;
	public bool rightLock = false;

	public bool up = false;
    public bool left = false;
    public bool down = false;
    public bool right = false;

    public bool litLock = false;
    public bool medLock = false;
    public bool hevLock = false;
    public bool speLock = false;

    public bool lite = false;
    public bool medium = false;
    public bool heavy = false;
    public bool special = false;

	public bool up1 = false;
	public bool left1 = false;
	public bool down1 = false;
	public bool right1 = false;

	public bool facingRight;

	public int playerID;

    private KeyCode upKey;
    private KeyCode rightKey;
    private KeyCode downKey;
    private KeyCode leftKey;
    private KeyCode lightKey;
    private KeyCode mediumKey;
    private KeyCode heavyKey;
    private KeyCode specialKey;

    public GameObject otherPlayer;
	public Boolean joy;

    public JoyScript JoyScript;
    public Boolean button;

    public Button lightButton;
    public Button mediumButton;
    public Button heavyButton;

	public int[] damages;
	public int damagePass;
	List<int> cancel;

    void OnDrawGizmos()
    {
        if (hurtbox.enabled)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5F);
            Gizmos.DrawCube(new Vector2(transform.position.x + hurtbox.offset.x * this.transform.localScale.x, transform.position.y + hurtbox.offset.y), new Vector2(hurtbox.size.x, hurtbox.size.y));
        }

        Gizmos.color = new Color(0, 1, 0, 0.5F);
        Gizmos.DrawCube(new Vector2(transform.position.x + hitbox.offset.x * this.transform.localScale.x, transform.position.y + hitbox.offset.y), new Vector2(hitbox.size.x, hitbox.size.y));
    }

    void Start()
    {
        if (playerID == 1)
        {
            upKey = KeyCode.W;
            rightKey = KeyCode.D;
            downKey = KeyCode.S;
            leftKey = KeyCode.A;
            lightKey = KeyCode.J;
            mediumKey = KeyCode.K;
            heavyKey = KeyCode.L;
			specialKey = KeyCode.Slash;
        }
        else if (playerID == 2)
        {
            upKey = KeyCode.UpArrow;
            rightKey = KeyCode.RightArrow;
            downKey = KeyCode.DownArrow;
            leftKey = KeyCode.LeftArrow;
            lightKey = KeyCode.Keypad4;
            mediumKey = KeyCode.Keypad5;
            heavyKey = KeyCode.Keypad6;
			specialKey = KeyCode.KeypadEnter;
        }

		this.tag = playerID.ToString();
		hitbox.tag = playerID.ToString();
		hurtbox.tag = playerID.ToString();

        storedAttackStrength = NO_ATTACK_STRENGTH;
        forwardSpeed = 0.25f;
        backwardSpeed = 0.15f;
        jumpSpeed = 1.25f;
        vVelocity = 0;
        hVelocity = 0;
        gravity = BASE_GRAVITY;

		history = new List<int>(new int[] { 5, 5, 5, 5, 5, 5 });
		delays = new List<int>(new int[] { 0, 0, 0, 0, 0, 0 });
		inputTimer = 0;

		cancel = new List<int>();
		dashed = false;

		//konky specific things...
		maxHealth = 11000;
		health = maxHealth;
		behaviors = new KonkyBehaviours();
		baseHeight = 8;
		width = 8;

		//Time.timeScale = 0.1F;
	}

    // Update is called once per frame
    private void Update()
    {
        if (!hitStopped)
        {
            SubUpdate();
        }
    }

    private void SubUpdate()
	{
		joy = (JoyScript != null);

        button = (lightButton != null);

		if (!facingRight)
		{
			this.transform.localScale = new Vector3(-1, 1, 1);
		}
		else
		{
			this.transform.localScale = new Vector3(1, 1, 1);
		}

		up1 = false;
		right1 = false;
		down1 = false;
		left1 = false;
		up = false;
		right = false;
		down = false;
		left = false;
		lite = false;
		medium = false;
		heavy = false;
		special = false;

        if (button)
        {
            if (Input.GetKey(lightKey) || lightButton.isActiveAndEnabled)
            {
                if (!litLock)
                {
                    litLock = true;
                    lite = true;
                }
            }
            else
            {
                litLock = false;
            }
            if (Input.GetKey(mediumKey) || mediumButton.isActiveAndEnabled)
            {
                if (!medLock)
                {
                    medLock = true;
                    medium = true;
                }
            }
            else
            {
                medLock = false;
            }
            if (Input.GetKey(heavyKey) || heavyButton.isActiveAndEnabled)
            {
                if (!hevLock)
                {
                    hevLock = true;
                    heavy = true;
                }
            }
            else
            {
                hevLock = false;
            }
            if (Input.GetKey(specialKey))
            {
                if (!speLock)
                {
                    speLock = true;
                    special = true;
                }
            }
            else
            {
                speLock = false;
            }
        }
        else
        {
            if (Input.GetKey(lightKey))
            {
                if (!litLock)
                {
                    litLock = true;
                    lite = true;
                }
            }
            else
            {
                litLock = false;
            }
            if (Input.GetKey(mediumKey))
            {
                if (!medLock)
                {
                    medLock = true;
                    medium = true;
                }
            }
            else
            {
                medLock = false;
            }
            if (Input.GetKey(heavyKey))
            {
                if (!hevLock)
                {
                    hevLock = true;
                    heavy = true;
                }
            }
            else
            {
                hevLock = false;
            }
            if (Input.GetKey(specialKey))
            {
                if (!speLock)
                {
                    speLock = true;
                    special = true;
                }
            }
            else
            {
                speLock = false;
            }
        }

		if (joy)
		{
			if (Input.GetKey(upKey) || JoyScript.Up)
			{
				up1 = true;
				if (!upLock)
				{
					upLock = true;
					up = true;
				}
			}
			else
			{
				upLock = false;
			}

			if (Input.GetKey(leftKey) || JoyScript.Left)
			{
				left1 = true;
				if (!leftLock)
				{
					leftLock = true;
					left = true;
				}
			}
			else
			{
				leftLock = false;
			}

			if (Input.GetKey(downKey) || JoyScript.Down)
			{
				down1 = true;
				if (!downLock)
				{
					downLock = true;
					down = true;
				}
			}
			else
			{
				downLock = false;
			}

			if (Input.GetKey(rightKey) || JoyScript.Right)
			{
				right1 = true;
				if (!rightLock)
				{
					rightLock = true;
					right = true;
				}
			}
			else
			{
				rightLock = false;
			}
		}
		else
		{
			if (Input.GetKey(upKey))
			{
				up1 = true;
				if (!upLock)
				{
					upLock = true;
					up = true;
				}
			}
			else
			{
				upLock = false;
			}

			if (Input.GetKey(leftKey))
			{
				left1 = true;
				if (!leftLock)
				{
					leftLock = true;
					left = true;
				}
			}
			else
			{
				leftLock = false;
			}

			if (Input.GetKey(downKey))
			{
				down1 = true;
				if (!downLock)
				{
					downLock = true;
					down = true;
				}
			}
			else
			{
				downLock = false;
			}

			if (Input.GetKey(rightKey))
			{
				right1 = true;
				if (!rightLock)
				{
					rightLock = true;
					right = true;
				}
			}
			else
			{
				rightLock = false;
			}
		}

		if (up1 && right1)
		{
			if (facingRight)
			{
				heldState = 9;
				jumpPass = 9;
			}
			else
			{
				heldState = 7;
				jumpPass = 7;
			}
		}
		else if (right1 && down1)
		{
			heldState = 3;
		}
		else if (down1 && left1)
		{
			heldState = 1;
		}
		else if (left1 && up1)
		{
			if (facingRight)
			{
				heldState = 7;
				jumpPass = 7;
			}
			else
			{
				heldState = 9;
				jumpPass = 9;
			}
		}
		else if (up1)
		{
			heldState = 8;
			jumpPass = 8;
		}
		else if (right1)
		{
			if (facingRight)
			{
				heldState = 6;
				jumpPass = 9;
			}
			else
			{
				heldState = 4;
				jumpPass = 7;
			}
		}
		else if (down1)
		{
			heldState = 2;
		}
		else if (left1)
		{
			if (facingRight)
			{
				heldState = 4;
				jumpPass = 7;
			}
			else
			{
				heldState = 6;
				jumpPass = 9;
			}
		}
		else
		{
			heldState = 5;
		}

		if (up && right)
        {
            if (facingRight)
            {
                iState = 9;
			}
            else
            {
                iState = 7;
			}
        }
        else if (right && down)
        {
            iState = 3;
        }
        else if (down && left)
        {
            iState = 1;
        }
        else if (left && up)
        {
            if (facingRight)
            {
                iState = 7;
            }
            else
            {
                iState = 9;
            }
        }
        else if (up)
        {
            iState = 8;
        }
        else if (right)
        {
            if (facingRight)
            {
                iState = 6;
			}
            else
            {
                iState = 4;
			}
        }
        else if (down)
        {
            iState = 2;
        }
        else if (left)
        {
            if (facingRight)
            {
                iState = 4;
			}
            else
            {
                iState = 6;
			}
        }
        else
        {
            iState = 5;
        }

        if (lite)
        {
            iAttack = LIGHT_ATTACK;
        }
        else if (medium)
        {
            iAttack = MEDIUM_ATTACK;
        }
        else if (heavy)
        {
            iAttack = HEAVY_ATTACK;
        }else if (special)
		{
			iAttack = SPECIAL_ATTACK;
		}
        else
        {
            iAttack = NO_ATTACK_STRENGTH;
        }

		//ADD INPUTS TO HISTORYYYYYYYYY
		//
		if (iState != 5)
		{
			history.Add(iState);
			delays.Add(inputTimer);
			inputTimer = 0;
			history.RemoveAt(0);
			delays.RemoveAt(0);
		}

		if (iAttack != NO_ATTACK_STRENGTH)
		{
			history.Add(iAttack);
			delays.Add(inputTimer);
			inputTimer = 0;
			history.RemoveAt(0);
			delays.RemoveAt(0);
		}

		if (iAttack == NO_ATTACK_STRENGTH && iState == 5)
		{
			++inputTimer;
		}
		//

        if (air)
        {
            airLock = true;
        }

		

		//floor check
		

		

		if (x() < -64f)
        {
            setX(-64);
        }else if (x() > 64f)
        {
            setX(64);
        }

        if (!air)
        {
            hVelocity = 0;
		}


		vVelocity += gravity;

		stateCheck();

		historyCheck();

		moveX(hVelocity);
		moveY(vVelocity);

		if (y() < FLOOR_HEIGHT)//ground snap
		{
			if (air)
			{
				state = 5;
				shutdown();
				if (waitForGround)
				{
					waitForGround = false;
					executeAction(Behaviors.aTurn, false);
				}
			}
			dashed = false;
			air = false;
			airLock = false;
			vVelocity = 0;
			setY(FLOOR_HEIGHT);
		}
		else
		{
			air = true;
		}

		if (action)//wow this is actually a lot simpler than before to send anim states to controller
        {
            animInt(ANIM_STATE, actionState + 10);
        }
        else
        {
            animInt(ANIM_STATE, state);
        }
    }

	private void historyCheck()
	{
		int[,] moves = new int[,]
		{
			{4,4},
			{6,6}
		};
		int[,] times = new int[,]
		{
			{16,-1},
			{16,-1}
		};
		int[] index = new int[]
		{
			Behaviors.aBDash,
			Behaviors.aDash,
		};
		for (int m = 0; m < moves.GetLength(0); ++m)//m i sthe variable that counts thorugh the moves array starting from 0 and going to the end
		{
			//the counter counts up every time through the i loop
			int counter = 0;
			//the i loop goes through history, starting at the last position and going down by a number of moves required by the move trying to be activated (2)
			for (int i = 5; i > 3; --i)
			{
				if (history[i] == moves[m,counter])
				{
					if (counter == moves.GetLength(1) - 1)
					{
						if (!dashed) {
							executeAction(index[m], false);
							dashed = true;
						}
						for (int j = history.Count - 1; j > history.Count - 1 - moves.GetLength(1); --j)
						{
							history[j] = 5;
						}
						break;
						
					}
				}
				else
				{
					break;
				}
				if (delays[i] > times[m, counter])
				{
					break;
				}
				++counter;
			}
		}
	}

	private void incrementFrame()
	{
		++actionCounter;
		int old = type;
		type = attackTypes[actionCounter];
		if (old != 1 && type == 1)
		{
			++damageCounter;
			damagePass = damages[damageCounter];
		}
		if (type == 0 || type == 2 || type == 3)
		{
			hitbox.enabled = false;
			hitbox.size = new Vector2(0f,0f);
		}
		else
		{
			hitbox.enabled = true;
		}
	}

    private void stateCheck() //casual loop
    {
		if (action) {
			if (!infiniteAction) {
				if (actionCounter == actionFrames - 1)//end an action by counting down the action timer
				{
					actionEnd();
				}
				else
				{
					incrementFrame();
				}
			}
		}

		if (actionState == Behaviors.aDash)
		{
			if (heldState != 6)
			{
				shutdown();
			}
		}

		if (!action) {//set velocity when moving forward and backward
			if (state == 6)
			{
				if (facingRight)
				{
					hVelocity = forwardSpeed;
				}
				else
				{
					hVelocity = -forwardSpeed;
				}
			}
			else if (state == 4)
			{
				if (facingRight)
				{
					hVelocity = -backwardSpeed;
				}
				else
				{
					hVelocity = backwardSpeed;
				}
			}
		}

		else{//set velocity when dashing
			if (actionState == Behaviors.aBDash)
			{
				if (facingRight)
				{
					hVelocity = -forwardSpeed * 1.5f;
					vVelocity = 0;
				}
				else
				{
					hVelocity = forwardSpeed * 1.5f;
					vVelocity = 0;
				}
			}
			else if (actionState == Behaviors.aDash)
			{
				if (facingRight)
				{
					hVelocity = forwardSpeed * 2f;
					vVelocity = 0;
				}
				else
				{
					hVelocity = -forwardSpeed * 2f;
					vVelocity = 0;
				}
			}
		}

		if (state < 4)//set the height for crouching
		{
			height = baseHeight / 2;
		}
		else
		{
			height = baseHeight;
		}

		if (!airLock)
		{
			state = heldState;//set the god damned state
			if (state > 6)//jump yo
			{
				executeAction(Behaviors.aJump, false);
			}
		}
	
		if (iAttack != NO_ATTACK_STRENGTH)
		{
			executeAction(iAttack, true);//check if you're attacking then do that
		}

	}

    private void executeAction(int strength, bool attacking)//new and improved! lots of canceling!
    {
		int place;//derive which place in the list of actions the requested action is
		if (attacking)
		{
			place = (state - 1) * 3 + strength;
		}
		else
		{
			place = strength;
		}

		bool executeAction_pass = true;//make sure the attack in cancelable basically
		if (action)
		{
			executeAction_pass = cancel.Contains(place) && type == RECOVERY;
		}

		if (executeAction_pass)
		{

			Action act = behaviors.getAction(place);
	
			if (act != null)
			{
				action = true;
				actionState = place;

                lvl = act.level;
				attackTypes = act.frames;
				actionCounter = -1;
				damageCounter = -1;
				actionFrames = attackTypes.Length;
				infiniteAction = act.infinite;
				incrementFrame();

				cancel = new List<int>(act.cancels);

				if (attacking)
				{
					damages = act.damage;
					damagePass = damages[0];
				}
			}
		}
		else
		{
			if (type == RECOVERY)//check if you can buffer then do that
			{
				storedAttackStrength = iAttack;
			}
		}
    }

    private void actionEnd()
    {
		Debug.Log("stored: "+storedAttackStrength);
		delBox();
		if (waitForEnd)
		{
			waitForEnd = false;
			if (state < 4)
			{
				executeAction(Behaviors.aCTurn, false);
			}
			else
			{
				executeAction(Behaviors.aTurn, false);
			}
		}
		else if (actionState == Behaviors.aTurn || actionState == Behaviors.aCTurn)
		{
			shutdown();
			facingRight = passDir;
		}
		else if (actionState == Behaviors.aJump)
		{
			shutdown();
			state = jumpPass;
			if (jumpPass == 8)
			{
				airLock = true;
				vVelocity = jumpSpeed;
			}
			else if (jumpPass == 9)
			{
				airLock = true;
				vVelocity = jumpSpeed;
				if (facingRight)
				{
					hVelocity = forwardSpeed * 1.2f;
				}
				else
				{
					hVelocity = -forwardSpeed * 1.2f;
				}
			}
			else if (jumpPass == 7)
			{
				airLock = true;
				vVelocity = jumpSpeed;
				if (facingRight)
				{
					hVelocity = -backwardSpeed * 1.2f;
				}
				else
				{
					hVelocity = backwardSpeed * 1.2f;
				}
			}
		}
        else if (storedAttackStrength != NO_ATTACK_STRENGTH)
        {
			if (!air) {
				state = heldState;
			}
            executeAction(storedAttackStrength, true);
		}
		else
		{
			shutdown();
			if (state == 6)
			{
				if (heldState != 6)
				{
					state = 5;
				}
			}
			else if (state == 4)
			{
				if (heldState != 4)
				{
					state = 5;
				}
			}
			else if (state < 4)
			{
				if (heldState > 3)
				{
					state = 5;
				}
			}
			else if (state > 6)
			{
				if (!air)
				{
					state = 5;
				}
			}
		}
		storedAttackStrength = NO_ATTACK_STRENGTH;
    }

	private void shutdown()
	{
		actionState = NO_ATTACK_INDEX;
		action = false;
		damagePass = 0;
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

    public void flip(bool dir)
    {
		if (air) {
			waitForGround = true;
		}
		else if (action)
		{
			waitForEnd = true;
		}
		else
		{
			waitForEnd = false;
			waitForGround = false;
			if (state < 4) {
				executeAction(Behaviors.aCTurn, false);
			}
			else
			{
				executeAction(Behaviors.aTurn, false);
			}
		}
		passDir = dir;
    }

	public void damage(int ammount)
	{
		Debug.Log(ammount);
		health -= ammount;
	}

	public void block()
	{
		//executeAction();
	}

	public void delBox()
	{
		hurtbox.enabled = false;
		/*BoxCollider2D[] list = GetComponents<BoxCollider2D>();
		foreach (BoxCollider2D b in list)
		{
			if (b.tag.Equals("h"))
			{
				Destroy(b);
			}
		}*/
	}

    public float level(int wanted)
    {
       return levelScaling[lvl,wanted];
    }

    /*public void genBox(String s)//fuck you unity for forcing one variable
	{
		char[] yeet = s.ToCharArray();
		BoxCollider2D box = new BoxCollider2D();
		box.offset.Set((yeet[0] - 48) , (yeet[0] - 48) );
		box.size.Set((yeet[0] - 48) , (yeet[0] - 48) );
		box.tag = "h";
	}*/

}
