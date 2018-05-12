﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
	public Canvas canvas;
	public UIScript uis;

	public GameObject playerPrefab;
    public GameObject ghost;
	public GameObject background;
	public GameObject ground;
	public Camera self;

	public GameObject player1;
    public PlayerScript p1s;
    public CollisionScript p1h;
	public GameObject player2;
	public PlayerScript p2s;
	public CollisionScript p2h;

	public Sprite background0;
	public Sprite background1;
	public Sprite background2;
	public Sprite background3;
	public Sprite background4;
	public Sprite background5;
	public Sprite background6;

	public Sprite ground0;
	public Sprite ground1;
    public Sprite ground2;
    public Sprite ground3;


    public Sprite[] Background;
	public Sprite[] Ground;

	public JoyScript JoyScript;

	public Button lightButton;
	public Button mediumButton;
	public Button heavyButton;

    public Transform cameraLeft, cameraRight;
    public Transform leftEdge, rightEdge, topEdge, bottomEdge;

    public IntVariable time;

    float vertExtent, horzExtent;

	private int megaKek;

	public float shakeX;
	public float shakeY;
    public bool lastP1Side;
    public bool lastP2Side;

    public float magnitude, roughness, fadeIn, fadeOut;

    private bool justShook;
    private Vector3 preShakePos;

	void Start()
	{
        uis = canvas.GetComponent<UIScript>();

		player1 = Instantiate(playerPrefab);

		setX(player1, -16f);
		p1s = player1.GetComponent<PlayerScript>();
        p1h = player1.GetComponentInChildren<CollisionScript>();
		p1s.facingRight = true;
		p1s.playerID = 1;
		p1s.JoyScript = JoyScript;
        p1s.cameraLeft = cameraLeft;
        p1s.cameraRight = cameraRight;
        //p1s.lightButton = lightButton;
        //p1s.mediumButton = mediumButton;
        //p1s.heavyButton = heavyButton;

        player2 = Instantiate(playerPrefab);
		setX(player2, 16f);
		p2s = player2.GetComponent<PlayerScript>();
        p2h = player2.GetComponentInChildren<CollisionScript>();
        p2s.facingRight = false;
		p2s.playerID = 2;
        p2s.cameraLeft = cameraLeft;
        p2s.cameraRight = cameraRight;

        p1s.otherPlayer = player2;
		p2s.otherPlayer = player1;

        ghost.GetComponent<BackGroundScript>().setScripts(p1s, p2s);

        Background = new Sprite[] { background0, background1, background2, background3 };
		Ground = new Sprite[] { ground0, ground1, ground2, ground3 };

        GetComponentInParent<Follow>().setTargets(player1.transform, player2.transform);

        vertExtent = GetComponentInParent<Follow>().vertExtent;
        horzExtent = GetComponentInParent<Follow>().horzExtent;

        int stage = PlayerPrefs.GetInt("stage");

        background.GetComponent<SpriteRenderer>().sprite = Background[stage < 2 ? stage : stage - 1];
        ground.GetComponent<SpriteRenderer>().sprite = Ground[stage < 2 ? stage : stage - 1];

        Debug.LogFormat("STAGE {0}", stage);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }        

        if (getX(player1) < getX(player2) - 1)
        {
            p1s.playerSide = true;
            p2s.playerSide = false;
        }
        else if (getX(player1) > getX(player2) + 1)
        {
            p1s.playerSide = false;
            p2s.playerSide = true;
        }

        uis.health1.maxValue = p1s.maxHealth;
        uis.health1.minValue = 0;
        uis.health2.maxValue = p2s.maxHealth;
        uis.health2.minValue = 0;
        uis.meter1.maxValue = p1s.maxMeter;
        uis.meter1.minValue = 0;
        uis.meter2.maxValue = p2s.maxMeter;
        uis.meter2.minValue = 0;
        uis.health1.value = p1s.health;
        uis.meter1.value = p1s.meterCharge;
		uis.health2.value = p2s.health;
        uis.meter2.value = p2s.meterCharge;

        if (p1s.health <= 0 || p2s.health <= 0 || time.value < 1)
        {
            PlayerPrefs.SetInt("menu_state", 1);
            SceneManager.LoadScene("Menu");
        }

        if (ghost.GetComponent<BackGroundScript>().shake)
		{   
            if (justShook == false)
            {
                preShakePos = transform.position;
                justShook = true;
            }

            shakeX = Random.Range(-0.75f, 0.75f);
			shakeY = Random.Range(-0.75f, 0.75f);

            float camX = transform.parent.transform.position.x + shakeX;
            if (camX - (horzExtent ) < leftEdge.position.x)
                camX = leftEdge.position.x + (horzExtent );
            else if (camX + (horzExtent ) > rightEdge.position.x)
                camX = rightEdge.position.x - (horzExtent );
            setX(self, camX);
			setY(self, 12 + shakeY);
            
		}
        else if (justShook)
        {
            transform.position = preShakePos;
            justShook = false;
        }
	}

    private float getX(GameObject o)
    {
        return o.transform.position.x;
    }
    private float getX(Camera o)
    {
        return o.transform.position.x;
    }

    private float getY(GameObject o)
    {
        return o.transform.position.y;
    }
    private float getY(Camera o)
    {
        return o.transform.position.y;
    }

    private void moveX(Camera o, float amm)
    {
        Vector3 position = o.transform.position;
        position.x += amm;
    }

    private void setX(Camera o, float amm)
    {
        Vector3 position = o.transform.position;
        position.x = amm;
        o.transform.position = position;
    }

    private void setX(GameObject o, float amm)
    {
        Vector3 position = o.transform.position;
        position.x = amm;
        o.transform.position = position;
    }

    private void setY(GameObject o, float amm)
    {
        Vector3 position = o.transform.position;
        position.y = amm;
        o.transform.position = position;
    }

    private void setY(Camera o, float amm)
	{
		Vector3 position = o.transform.position;
		position.y = amm;
		o.transform.position = position;
	}
}
