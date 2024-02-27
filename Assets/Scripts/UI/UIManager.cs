using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject craftingContainer;

    public void ScreenOn(GameObject screen,bool cursorUnlock)
    {
        UIScreen[] screens = GetComponentsInChildren<UIScreen>();
        
        foreach(UIScreen obj in screens)
        {
            obj.gameObject.SetActive(false);
        }

        screen.SetActive(true);


        if (cursorUnlock)
        {
            Cursor.lockState = CursorLockMode.None;
            PlayerController.instance.mouseLock = true;
        }
    
    }

    public void ScreensOn(GameObject[] screen, bool cursorUnlock)
    {
        UIScreen[] screens = GetComponentsInChildren<UIScreen>();

        foreach (UIScreen obj in screens)
        {
            obj.gameObject.SetActive(false);
        }

        foreach(GameObject obj in screen)
        {
            obj.SetActive(true);
        }

        if (cursorUnlock)
        {
            Cursor.lockState = CursorLockMode.None;
            PlayerController.instance.mouseLock = true;

        }

    }

    public void ScreenOff()
    {
        UIScreen[] screens = GetComponentsInChildren<UIScreen>();

        foreach (UIScreen obj in screens)
        {
            obj.gameObject.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Locked;
        PlayerController.instance.mouseLock = false;

    }
}
