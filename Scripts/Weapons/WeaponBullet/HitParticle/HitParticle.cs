using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class HitParticle : MonoBehaviour
{
    private float timeToDestroy;
    public void Setup(float time, Vector3 scale, SpriteLibraryAsset hitSkin) {
        this.timeToDestroy = time;
        transform.localScale = scale;
        if(gameObject.TryGetComponent(out SpriteLibrary sl))
            sl.spriteLibraryAsset = hitSkin;
    }

    private void Start() {
        
        Destroy(gameObject,timeToDestroy);
    }
}
