using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}
public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    unit playerUnit;
    unit enemyUnit;

    public BattleHud playerHUD;
    public BattleHud enemyHUD;

    public Text dialogueText;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());
    }

    IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<unit>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<unit>();

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action: ";
    }

    IEnumerator PlayerAttack() 
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        // Update HUD to reflect damage taken.
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack succeeds!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else 
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn() 
    {
        dialogueText.text = enemyUnit.unitName + " is attacking!";

        yield return new WaitForSeconds(1f);
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else 
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle() 
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won!";
        }
        else if (state == BattleState.LOST) 
        {
            dialogueText.text = "You were defeated...";
        }
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN) return;

        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerHeal() 
    {
        playerUnit.Heal(5);
        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength.";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN) return;

        StartCoroutine(PlayerHeal());
    }
}