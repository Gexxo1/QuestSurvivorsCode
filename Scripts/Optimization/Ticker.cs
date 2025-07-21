using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ticker : MonoBehaviour
{
    /* How to use:
        put this on the script you want to use:
        private void OnEnable() {
            Ticker.OnTickAction += Tick;
        }
        private void OnDisable() {
            Ticker.OnTickAction -= Tick;
        }
        private void Tick() {
            //do something
        }
    */
    public static float tickTime = 0.2f;
    private float _tickerTimer;
    public delegate void TickAction();
    public static event TickAction OnTickAction;
    private void Update()
    {
        _tickerTimer += Time.deltaTime;
        if (_tickerTimer >= tickTime)
        {
            _tickerTimer = 0;
            TickEvent();
        }
    }

    private void TickEvent() {
        OnTickAction?.Invoke();
    }
}
