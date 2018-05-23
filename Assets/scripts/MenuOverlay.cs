﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOverlay : MonoBehaviour
{
    //broken record studios?

    public GameObject selector;

    private InputManager inputManager, inputManager2;

    public static int state = 0;
    public static int state2 = 1;
    public int menu = 0;

    void Start ()
    {
        inputManager = new InputManager(1);
        inputManager2 = new InputManager(2);
        InputManager.isInputEnabled = true;
	}

    void Update()
    {

        inputManager.pollInput(0, 1);
        inputManager2.pollInput(0, 2);

        if (inputManager2.currentInput[8])
        {
            state2--;
        }
        else if (inputManager2.currentInput[9])
        {
            state2++;
        }

        if (inputManager.currentInput[menu < 2 ? 12 : 8])
        {
            state--;
        }
        else if (inputManager.currentInput[menu < 2 ? 13 : 9])
        {
            state++;
        }
        else if (inputManager.currentInput[4])
        {
            switch (menu)
            {
                case 0:
                    switch (state)
                    {
                        // play button
                        case 0:
                            GetComponentInParent<MenuScript>().triggerEvent(1);
                            selector.SetActive(false);
                            state = 0;
                            menu = 2;
                            break;
                        // settings button
                        case 1:
                            GetComponentInParent<MenuScript>().triggerEvent(15);
                            state = 0;
                            menu = 1;
                            break;
                    }
                    break;
                case 1:
                    switch (state)
                    {
                        case 0:
                            GetComponentInParent<MenuScript>().triggerEvent(16);
                            break;
                        case 1:
                            GetComponentInParent<MenuScript>().triggerEvent(17);
                            break;
                        case 2:
                            GetComponentInParent<MenuScript>().triggerEvent(18);
                            break;
                        case 3:
                            GetComponentInParent<MenuScript>().triggerEvent(19);
                            break;
                        case 4:
                            GetComponentInParent<MenuScript>().triggerEvent(20);
                            break;
                    }
                    break;
            }
        }
        else if (inputManager.currentInput[5])
            switch (menu)
            {
                case 1:
                    GetComponentInParent<MenuScript>().triggerEvent(0);
                    state = 1;
                    menu = 0;
                    break;
                case 2:
                    GetComponentInParent<MenuScript>().triggerEvent(0);
                    state = 0;
                    menu = 0;
                    selector.SetActive(true);
                    break;
            }

        switch (menu)
        {
            case 0:
                if (state > 1)
                    state = 0;
                else if (state < 0)
                    state = 1;
                break;
            case 1:
                if (state > 4)
                    state = 0;
                else if (state < 0)
                    state = 4;
                break;
            case 2:
                if (state > 1)
                    state = 0;
                else if (state < 0)
                    state = 1;


                if (state2 > 1)
                    state2 = 0;
                else if (state2 < 0)
                    state2 = 1;
                break;
        }

        moveSelector();

    }

    void moveSelector()
    {
        switch (menu)
        {
            // main menu
            case 0:
                switch (state)
                {
                    // play button
                    case 0:
                        selector.transform.position = new Vector3(0.3f, -3.1f, 0);
                        selector.transform.localScale = new Vector3(11, 42, 0);
                        break;
                    // settings button
                    case 1:
                        selector.transform.position = new Vector3(0.2f, -7, 0);
                        selector.transform.localScale = new Vector3(13.5f, 33, 0);
                        break;
                }
                break;
            // settings menu
            case 1:
                switch (state)
                {
                    case 0:
                        selector.transform.position = new Vector3(-10.5f, 4, 0);
                        selector.transform.localScale = new Vector3(7.9f, 21.5f, 0);
                        break;
                    case 1:
                        selector.transform.position = new Vector3(-10.5f, 0, 0);
                        selector.transform.localScale = new Vector3(7.9f, 21.5f, 0);
                        break;
                    case 2:
                        selector.transform.position = new Vector3(-10.5f, -4, 0);
                        selector.transform.localScale = new Vector3(7.9f, 21.5f, 0);
                        break;
                    case 3:
                        selector.transform.position = new Vector3(5.5f, 3, 0);
                        selector.transform.localScale = new Vector3(8.24f, 25, 0);
                        break;
                    case 4:
                        selector.transform.position = new Vector3(5.5f, -3.51f, 0);
                        selector.transform.localScale = new Vector3(8.24f, 25, 0);
                        break;
                }
                break;
            case 2:
                switch (state)
                {
                    case 0:
                        GetComponentInParent<MenuScript>().triggerEvent(9);
                        GetComponentInParent<MenuScript>().characterButtons[0].GetComponent<ComponentScript>().click();
                        break;
                    case 1:
                        GetComponentInParent<MenuScript>().triggerEvent(9);
                        GetComponentInParent<MenuScript>().characterButtons[1].GetComponent<ComponentScript>().click();
                        break;
                }
                switch (state2)
                {
                    case 0:
                        GetComponentInParent<MenuScript>().triggerEvent(10);
                        GetComponentInParent<MenuScript>().characterButtons[0].GetComponent<ComponentScript>().click();
                        break;
                    case 1:
                        GetComponentInParent<MenuScript>().triggerEvent(10);
                        GetComponentInParent<MenuScript>().characterButtons[1].GetComponent<ComponentScript>().click();
                        break;
                }
                break;
        }
    }

}
