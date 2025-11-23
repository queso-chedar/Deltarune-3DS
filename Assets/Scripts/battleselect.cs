using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class battleselect : MonoBehaviour
{
    public GameObject[] Objectstodeactivate;
    public GameObject[] ObjectstoActivate;
	private Image spriter_fightbar;
	private Image spriter_aimer_bg;
	private Image spriter_PresserHint;
    public Battle_handle BattleHandler;

    Text changetext;

    void Start()
    {
		spriter_fightbar = BattleHandler.Battle_Aimer.GetComponent<UnityEngine.UI.Image>();
		spriter_aimer_bg = BattleHandler.Battle_Aimer_bg.GetComponent<UnityEngine.UI.Image>();
		spriter_PresserHint = BattleHandler.PresserHint.GetComponent<UnityEngine.UI.Image>();
    }

    public void ButtonPressed()
    {
        foreach (GameObject Object in ObjectstoActivate)
        {
            Object.SetActive(true);
            BattleHandler.Battle_Aimer.GetComponent<FightBar>().MoveFightBar = true;
            BattleHandler.Battle_Aimer.GetComponent<FightBar>().CanAttack = true;
            BattleHandler.Battle_Aimer.GetComponent<FightBar>().Collider.enabled = false;
            BattleHandler.Battle_Aimer.GetComponent<FightBar>().GetComponent<RectTransform>().anchoredPosition = BattleHandler.Battle_Aimer.GetComponent<FightBar>().savedpos;
            BattleHandler.playeranimator.Play("fightreadyup");

            spriter_fightbar.color = new Color(spriter_fightbar.color.r, spriter_fightbar.color.g, spriter_fightbar.color.b, 1);
			spriter_aimer_bg.color = new Color(spriter_aimer_bg.color.r, spriter_aimer_bg.color.g, spriter_aimer_bg.color.b, 1);
			spriter_PresserHint.color = new Color(spriter_PresserHint.color.r, spriter_PresserHint.color.g, spriter_PresserHint.color.b, 1);

        }
        foreach (GameObject Object in Objectstodeactivate)
        {
            Object.SetActive(false);
        }
    }

    void Update()
    {
    }
}
