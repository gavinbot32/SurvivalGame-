using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    [SerializeField] Image img_health;
    [SerializeField] Image img_hunger;
    [SerializeField] Image img_stam;
    [SerializeField] Image img_progess;

    public void updateStats()
    {
        img_health.fillAmount = (float)PlayerController.instance.health / 100;
        img_stam.fillAmount = (float)PlayerController.instance.stamina / 1000;
        img_hunger.fillAmount = (float)PlayerController.instance.hunger / 100;
    }

    // Update is called once per frame
    void Update()
    {
        updateStats();
        img_progess.transform.parent.gameObject.SetActive(PlayerController.instance.b_equiping);
    }

    public void progressUpdate(float curTime, float maxTime)
    {
        img_progess.fillAmount = curTime / maxTime;
        
    }


}
