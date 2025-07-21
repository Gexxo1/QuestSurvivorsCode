using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : Item
{
    [Header("Base Ability")]
    [SerializeField] protected int manaCost = 1;
    [SerializeField] protected float cooldown = 5;
    protected float lastUsed = -999;
    protected Player player;
    protected bool isReady;
    protected virtual void Start() {
        player = GameManager.instance.player;
        //MenuManager.instance.SetAbilityFill(1);
    }
    protected virtual void Update() {
        Update1();
        Update2();
    }
    abstract protected void UseSkill();
    protected bool CooldownCondition() {
        return Time.time - lastUsed > cooldown;
    }
    private void Update1() {
        if(CooldownCondition()) {
            if(!isReady) {
                isReady = true;
                //MenuManager.instance.SetAbilityFill(0);
            }
        }
        else {
            if(isReady) isReady = false;
            //MenuManager.instance.UpdateAbilityFill(cooldown);
        }
    }
    private void Update2() {
        if(LaunchCondition()) {
            lastUsed = Time.time;
            UseSkill();
        }
    }
    protected bool LaunchCondition() {
        return isReady && Input.GetKeyDown(KeyCode.LeftShift) 
                && GameManager.instance.player.currMp >= manaCost;
    }
}
