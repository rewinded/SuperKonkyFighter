﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScript : MonoBehaviour {

    public PlayerScript p1s;
    public PlayerScript p2s;
    public PlayerScript[] player;

    // Use this for initialization
    void Start () { }
	
    public void setScripts(PlayerScript p1s, PlayerScript p2s)
    {
        this.p1s = p1s;
        this.p2s = p2s;

        player = new PlayerScript[]{ p1s, p2s, p1s};  
    }

	// Update is called once per frame
	void Update () {

        // Point 0 - left, Point 2 - right
        Vector2[] points1 = p1s.hitbox.GetComponent<PolygonCollider2D>().points;
        Vector2[] points2 = p2s.hitbox.GetComponent<PolygonCollider2D>().points;

        pushing();
        damage();
        knockback();
        checkCollisions();

        player[0].decreaseHitboxLifespan();
        player[1].decreaseHitboxLifespan();
        player[0].decreaseHurtboxLifespan();
        player[1].decreaseHurtboxLifespan();
    }

    public void damage()
    {
        
    }

    private void knockback()
    {

    }

    private void pushing()
    {
        float[] diff = new float[2];

        if (p1s.coll)
        {

            if (player[0].hPush < 0)
                player[0].hPush = 0;
            if (player[1].hPush < 0)
                player[1].hPush = 0;

            if (player[0].vPush < 0)
                player[0].vPush = 0;
            if (player[1].vPush < 0)
                player[1].vPush = 0;

            if (Mathf.Abs(player[0].x()) > 64)
                player[1].hPush = -player[1].hPush;
            if (Mathf.Abs(player[1].x()) > 64)
                player[1].hPush = -player[0].hPush;

            for (int i = 0; i < 2; i++)
            {
                float xPos = player[i].hitbox.transform.position.x,
                xPosFuture = xPos + (player[i].facingRight ? player[i].hVelocity - player[i].hPush : -player[i].hVelocity + player[i].hPush),
                otherXPos = player[i + 1].hitbox.transform.position.x,
                otherXPosFuture = otherXPos + (player[i + 1].facingRight ? player[i + 1].hVelocity - player[i + 1].hPush : -player[i + 1].hVelocity + player[i + 1].hPush),
                hitboxWidth = player[i].hitbox.GetComponent<PolygonCollider2D>().bounds.size.x,
                otherHitboxWidth = player[i + 1].hitbox.GetComponent<PolygonCollider2D>().bounds.size.x;

                if (Mathf.Abs((xPosFuture) - (otherXPosFuture)) <= (hitboxWidth / 2 + otherHitboxWidth / 2))
                {
                    diff[i] = ((hitboxWidth / 2 + otherHitboxWidth / 2) - Mathf.Abs((xPosFuture) - (otherXPosFuture)));
                }
            }
        }
        else
        {
            diff[0] = 0;
            diff[1] = 0;
        }

        for (int i = 0; i < 2; i++)
        {
            if (Mathf.Abs(player[i].hVelocity) > Mathf.Abs(player[i + 1].hVelocity))
            {
                if (Mathf.Abs(player[i + 1].x()) < 64)
                    diff[i] = 0;
            }
            else if (player[i].hVelocity == player[i + 1].hVelocity)
                diff[i] = diff[i] / 2;
        }
        for (int i = 0; i < 2; i++)
        {
            if (player[i].updateEnd != 0)
            {
                player[i].hPush += diff[i];

                if (player[i].updateEnd == 2)
                {
                    player[i].updateEnd = 0;
                    player[i].UpdateEnd();
                }
            }
        }
    }

    private void checkCollisions()
    {
        foreach (BoxCollider2D hitbox in p1s.GetComponentsInChildren<BoxCollider2D>())
        {
            if (hitbox.tag.Equals("hitbox1") || hitbox.tag.Equals("hitbox2"))
            {
                foreach (BoxCollider2D hurtbox in p2s.GetComponentsInChildren<BoxCollider2D>())
                {
                    if (hurtbox.tag.Equals("hurtbox1") || hurtbox.tag.Equals("hurtbox2"))
                    {
                        if (hitbox.IsTouching(hurtbox))
                        {
                            Debug.Log(hitbox.name + " collided with " + hurtbox.name);
                        }
                    }
                }
            }
        }
    }

    private Vector2 diffBetweenPoints(Vector2 point1, Vector2 point2)
    {
        return new Vector2(point1.x - point2.x, point1.y - point2.y);
    }

}