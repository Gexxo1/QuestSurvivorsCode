using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager instance;
    //onded
    public delegate void EnemyKilledEventHandler(Fighter fighter);
    public static event EnemyKilledEventHandler OnEnemyKilled;
    //onhit
    public delegate void EnemyHitEventHandler(Fighter fighter, HitSource hitSource);
    public static event EnemyHitEventHandler OnEnemyHit;
    //on bullet destroyed
    public delegate void BulletDestroyedEventHandler(HitSource hitSource);
    public static event BulletDestroyedEventHandler OnBulletDestroyed;
    //on player hp treshold
    public delegate void PlayerResourceTresholdEventHandler();
    public static event PlayerResourceTresholdEventHandler OnHpTreshold;
    //on enemy executed by certain bullet
    public delegate void EnemyExecutedEventHandler(HitSource hitSource);
    public static event EnemyExecutedEventHandler OnEnemyExecuted;
    //on ability casted
    public delegate void AbilityCastEventHandler(Player player, int manaCost);
    public static event AbilityCastEventHandler OnAbilityCast;
    //on player shoot
    public delegate void PlayerShootEventHandler(int shootCounter);
    public static event PlayerShootEventHandler OnPlayerShoot;
    private int shootCounter = 0;

    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Debug.LogWarning("Found another PowerupManager instance in scene");
    }
    // Metodo per chiamare l'evento quando un nemico è stato ucciso
    public void EnemyKilledEvent(Fighter f) {   
        OnEnemyKilled?.Invoke(f); 
    }
    
    public void EnemyHitEvent(Fighter f, HitSource hs) {   
        OnEnemyHit?.Invoke(f,hs); 
    }

    public void BulletDestroyedEvent(Bullet bullet) {   
        OnBulletDestroyed?.Invoke(bullet); 
    }

    public void PlayerResouceTresholdEvent() {
        OnHpTreshold?.Invoke();
    }

    public void EnemyExecutedEvent(HitSource hs) {   
        OnEnemyExecuted?.Invoke(hs); 
    }

    public void PlayerShootEvent(Fighter wielder) {   
        if(wielder is Player) 
            OnPlayerShoot?.Invoke(shootCounter++); 
    }

    public bool IsAbilityCastEventAvailable(Player player, int manaCost) {   
        if(OnAbilityCast == null)
            return false;
        
        bool hpRequirements = player.currHP > manaCost;
        //if(hpRequirements) OnAbilityCast.Invoke(player,manaCost); 
        return hpRequirements;
    }
    public void ConsumeAbilityCastEvent(Player player, int manaCost) {
        //if(!IsAbilityCastEventAvailable(player, manaCost)) return;
        OnAbilityCast?.Invoke(player, manaCost);
    }
}
