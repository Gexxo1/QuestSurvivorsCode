using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnAudioSourceToPool : MonoBehaviour
{
    
    private AudioSource audioSource;
    //private Coroutine returnToPoolCoroutine;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable() {
        StartCoroutine(ReturnToPool());
    }
    private void OnDisable() {
        StopAllCoroutines();
        //ObjectPoolManager.ReturnObjectToPool(audioSource);
    }
    private IEnumerator ReturnToPool() {
        yield return new WaitForSeconds(audioSource.clip.length);
        ObjectPoolManager.ReturnObjectToPool(audioSource);
        yield return null;
    }
    
}
