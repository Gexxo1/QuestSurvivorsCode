using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Pool;
public class ObjectPoolManager : MonoBehaviour {
    public static List<PooledObjectInfo> ObjectPools = new();
    public static List<PooledBulletInfo> BulletPools = new();
    public static List<PooledAudioSourceInfo> AudioPools = new();
    //public static List<PooledParticleSystemInfo> ParticleSystemPools = new();
    private static GameObject gameObjectsEmpty; private GameObject gameObjectsEmptyHolder;
    private GameObject playerBulletEmptyHolder; private static GameObject playerBulletEmpty;
    private GameObject enemyEmptyHolder; private static GameObject enemyEmpty;  
    private GameObject enemyBulletEmptyHolder; private static GameObject enemyBulletEmpty;
    private GameObject particleSystemEmptyHolder; private static GameObject particleSystemEmpty;
    private GameObject collectablesEmptyHolder; private static GameObject collectablesEmpty;
    private GameObject AudioSourceSFXEmptyHolder; private static GameObject AudioSourceSFXEmpty;
    public enum PoolType {
        GameObject,
        PlayerBullet,
        Enemy,
        EnemyBullet,
        ParticleSystem,
        Collectables,
        AudioSourceSFX,
        None,
    }
    public static PoolType PoolingType;
    private void Awake() {
        SetupEmpties();
        AudioPools = new();
    }
    private void SetupEmpties() {
        gameObjectsEmptyHolder = new GameObject("GameObjectsEmptyHolder");
        gameObjectsEmpty = new GameObject("GameObjectsEmpty");
        gameObjectsEmpty.transform.SetParent(gameObjectsEmptyHolder.transform);

        playerBulletEmptyHolder = new GameObject("PlayerBulletEmptyHolder");
        playerBulletEmpty = new GameObject("PlayerBulletEmpty");
        playerBulletEmpty.transform.SetParent(playerBulletEmptyHolder.transform);

        enemyEmptyHolder = new GameObject("EnemyEmptyHolder");
        enemyEmpty = new GameObject("EnemyEmpty");
        enemyEmpty.transform.SetParent(enemyEmptyHolder.transform);

        enemyBulletEmptyHolder = new GameObject("EnemyBulletEmptyHolder");
        enemyBulletEmpty = new GameObject("EnemyBulletEmpty");
        enemyBulletEmpty.transform.SetParent(enemyBulletEmptyHolder.transform);

        particleSystemEmptyHolder = new GameObject("ParticleSystemEmptyHolder");
        particleSystemEmpty = new GameObject("ParticleSystemEmpty");
        particleSystemEmpty.transform.SetParent(particleSystemEmptyHolder.transform);

        collectablesEmptyHolder = new GameObject("CollectablesEmptyHolder");
        collectablesEmpty = new GameObject("CollectablesEmpty");
        collectablesEmpty.transform.SetParent(collectablesEmptyHolder.transform);

        AudioSourceSFXEmptyHolder = new GameObject("AudioSourceSFXEmptyHolder");
        AudioSourceSFXEmpty = new GameObject("AudioSourceSFXEmpty");
        AudioSourceSFXEmpty.transform.SetParent(AudioSourceSFXEmptyHolder.transform);
    }
    //can be used for everything (often used for spawning enemies)
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None) {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
        //If the pool doesn't exist, create it
        if (pool == null) { 
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }
        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault(); // Get the first inactive object
        
        if(spawnableObject == null) {
            GameObject parentObject = GetParentObject(poolType);
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            if(parentObject != null) 
                spawnableObject.transform.SetParent(parentObject.transform);
        }
        else {
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }
    //USED ONLY FOR MAIN WEAPONS AND FOR ABILITIES
    public static Bullet SpawnBullet(Bullet bulletToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, ProjectileWeapon wpn, 
        bool hasManaGain, PoolType poolType = PoolType.None) {
        PooledBulletInfo pool = BulletPools.Find(p => p.LookupString == bulletToSpawn.name);
        //If the pool doesn't exist, create it
        if (pool == null) { 
            pool = new PooledBulletInfo() { LookupString = bulletToSpawn.name };
            BulletPools.Add(pool);
        }
        Bullet spawnableObject = pool.InactiveBullets.FirstOrDefault(); // Get the first inactive object
        //if there is not any inactive object, create a new one
        if(spawnableObject == null) {
            GameObject parentObject = GetParentObject(poolType);
            bulletToSpawn.gameObject.SetActive(false);
            spawnableObject = Instantiate(bulletToSpawn, spawnPosition, spawnRotation);
            if(parentObject != null) 
                spawnableObject.transform.SetParent(parentObject.transform);
        }
        else { //if there is an inactive object, just set it up
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveBullets.Remove(spawnableObject);
        }
        spawnableObject.Setup(wpn.stats, wpn, hasManaGain);
        
        spawnableObject.gameObject.SetActive(true);
        return spawnableObject;
    }
    //with ProjectileWeaponStats
    public static Bullet SpawnBullet(Bullet bulletToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, 
        WeaponStats stats, bool hasManaGain, PoolType poolType = PoolType.None) {

        PooledBulletInfo pool = BulletPools.Find(p => p.LookupString == bulletToSpawn.name);
        //If the pool doesn't exist, create it
        if (pool == null) { 
            pool = new PooledBulletInfo() { LookupString = bulletToSpawn.name };
            BulletPools.Add(pool);
        }
        Bullet spawnableObject = pool.InactiveBullets.FirstOrDefault(); // Get the first inactive object
        //if there is not any inactive object, create a new one
        if(spawnableObject == null) {
            GameObject parentObject = GetParentObject(poolType);
            bulletToSpawn.gameObject.SetActive(false);
            spawnableObject = Instantiate(bulletToSpawn, spawnPosition, spawnRotation);
            if(parentObject != null) 
                spawnableObject.transform.SetParent(parentObject.transform);
        }
        else { //if there is an inactive object, just set it up
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveBullets.Remove(spawnableObject);
        }
        spawnableObject.Setup(stats, null, hasManaGain);
        spawnableObject.gameObject.SetActive(true);

        //additional objective counter
        if(poolType == PoolType.PlayerBullet) {
            GameManager.instance.IncreaseProjectileShotCounter();
        }

        return spawnableObject;
    }
    //with BaseWeaponStats --> USED FOR BASE BULLETS (example: explosion)
    public static Bullet SpawnBullet(Bullet bulletToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, BaseWeaponStats stats, bool hasManaGain, PoolType poolType = PoolType.None) {
        PooledBulletInfo pool = BulletPools.Find(p => p.LookupString == bulletToSpawn.name);
        //If the pool doesn't exist, create it
        if (pool == null) { 
            pool = new PooledBulletInfo() { LookupString = bulletToSpawn.name };
            BulletPools.Add(pool);
        }
        Bullet spawnableObject = pool.InactiveBullets.FirstOrDefault(); // Get the first inactive object
        //if there is not any inactive object, create a new one
        if(spawnableObject == null) {
            GameObject parentObject = GetParentObject(poolType);
            bulletToSpawn.gameObject.SetActive(false);
            spawnableObject = Instantiate(bulletToSpawn, spawnPosition, spawnRotation);
            if(parentObject != null) 
                spawnableObject.transform.SetParent(parentObject.transform);
        }
        else { //if there is an inactive object, just set it up
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveBullets.Remove(spawnableObject);
        }
        spawnableObject.Setup(stats, hasManaGain);
        spawnableObject.gameObject.SetActive(true);
        return spawnableObject;
    }
    public static Bullet SpawnBullet(Bullet bulletToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, BaseWeaponStats stats, bool hasManaGain, Fighter wielder = null, PoolType poolType = PoolType.None) {
        PooledBulletInfo pool = BulletPools.Find(p => p.LookupString == bulletToSpawn.name);
        //If the pool doesn't exist, create it
        if (pool == null) { 
            pool = new PooledBulletInfo() { LookupString = bulletToSpawn.name };
            BulletPools.Add(pool);
        }
        Bullet spawnableObject = pool.InactiveBullets.FirstOrDefault(); // Get the first inactive object
        //if there is not any inactive object, create a new one
        if(spawnableObject == null) {
            GameObject parentObject = GetParentObject(poolType);
            bulletToSpawn.gameObject.SetActive(false);
            spawnableObject = Instantiate(bulletToSpawn, spawnPosition, spawnRotation);
            if(parentObject != null) 
                spawnableObject.transform.SetParent(parentObject.transform);
        }
        else { //if there is an inactive object, just set it up
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveBullets.Remove(spawnableObject);
        }
        spawnableObject.Setup(stats, hasManaGain, wielder);
        spawnableObject.gameObject.SetActive(true);
        return spawnableObject;
    }
    /*
    //this object pool just plays one sound per kind
    public static void SpawnAudioSource(AudioClip audioClip, AudioSource sourceTemplate, string clipName, Vector3 spawnPosition, float volume, PoolType poolType = PoolType.AudioSourceSFX) {
        PooledAudioSourceInfo pool = AudioPools.Find(p => p.LookupString == clipName);
        if (pool == null) { 
            pool = new PooledAudioSourceInfo() { LookupString = clipName };
            AudioPools.Add(pool);
        }
        AudioSource newSource = pool.AudioSources.FirstOrDefault(); // Get the first object
        if(newSource == null) {
            GameObject parentObject = SetParentObject(poolType);
            newSource = Instantiate(sourceTemplate, spawnPosition, Quaternion.identity);
            newSource.name = clipName;
            pool.AudioSources.Add(newSource);
            if(parentObject != null) newSource.transform.SetParent(parentObject.transform);
            
        }
        else {
            newSource.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity); 
        }
        
        //newSource.clip = audioClip;
        //newSource.volume = volume;
        newSource.PlayOneShot(audioClip,volume);
        Debug.Log("Playing " + audioClip + " on source: " + newSource);
        return;
    }
    */
    private const int MAX_AUDIO_SOURCES_PER_CLIP = 3;
    public static void SpawnAudioSource(AudioClip audioClip, AudioSource sourceTemplate, string clipName, Vector3 spawnPosition, float volume, PoolType poolType = PoolType.AudioSourceSFX) {
        PooledAudioSourceInfo pool = AudioPools.Find(p => p.LookupString == clipName);
        if (pool == null) { 
            pool = new PooledAudioSourceInfo() { LookupString = clipName };
            AudioPools.Add(pool);
        }
        AudioSource newSource = pool.InactiveAudioSources.FirstOrDefault(); // Get the first inactive object
        //to do: trovare il numero di audio source attivi con la stessa clip, e controllare se il counter è corretto
        //Debug.Log("Current Audio Source Number [" + pool.availableAudioSources.Count + "] --> [" + pool.LookupString + "]");
        if(newSource == null) {
            if(pool.availableAudioSources.Count >= MAX_AUDIO_SOURCES_PER_CLIP) {
                //Debug.Log("Max audio sources reached for clip [" + audioClip + "]");
                return;
            }
            GameObject parentObject = GetParentObject(poolType);
            newSource = Instantiate(sourceTemplate, spawnPosition, Quaternion.identity);
            newSource.name = clipName;
            if(parentObject != null) newSource.transform.SetParent(parentObject.transform);
            pool.availableAudioSources.Add(newSource);
        }
        else {
            newSource.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity); 
            pool.InactiveAudioSources.Remove(newSource);
        }
        newSource.clip = audioClip;
        newSource.volume = volume;
        newSource.gameObject.SetActive(true);
        newSource.Play();
        return;
    }
    public static void ReturnObjectToPool(GameObject obj) {
        string goName = obj.name.Substring(0, obj.name.Length - 7); //Remove the "(Clone)" from the name
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

        if(pool == null)
            Debug.LogWarning("Trying to release an object that is not pooled " + obj.name);
        else {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }
    public static bool TryReturnObjectToPool(GameObject obj) {
        if(obj.name.Length < 7)
            return false;
        string goName = obj.name.Substring(0, obj.name.Length - 7); //Remove the "(Clone)" from the name
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

        if(pool == null)
            return false;
        else {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
            return true;
        }
    }
    public static void ReturnObjectToPool(Bullet obj) {
        string goName = obj.name.Substring(0, obj.name.Length - 7); //Remove the "(Clone)" from the name
        PooledBulletInfo pool = BulletPools.Find(p => p.LookupString == goName);

        if(pool == null)
            Debug.LogWarning("Trying to release an object that is not pooled " + obj.name);
        else {
            obj.gameObject.SetActive(false);
            pool.InactiveBullets.Add(obj);
        }
    }
    public static void ReturnObjectToPool(AudioSource obj) {
        string goName = obj.name; 
        PooledAudioSourceInfo pool = AudioPools.Find(p => p.LookupString == goName);

        if(pool == null)
            Debug.LogWarning("Trying to release an object that is not pooled " + obj.name);
        else {
            obj.gameObject.SetActive(false);
            pool.InactiveAudioSources.Add(obj);
        }
    }
    private static GameObject GetParentObject(PoolType poolType) {
        switch(poolType) {
            case PoolType.GameObject:
                return gameObjectsEmpty;
            case PoolType.PlayerBullet:
                return playerBulletEmpty;
            case PoolType.Enemy:
                return enemyEmpty;
            case PoolType.EnemyBullet:
                return enemyBulletEmpty;
            case PoolType.ParticleSystem:
                return particleSystemEmpty;
            case PoolType.Collectables:
                return collectablesEmpty;
            case PoolType.AudioSourceSFX:
                return AudioSourceSFXEmpty;
            case PoolType.None:
                return null;
            default:
                return null;
        }
    }

}

public class PooledObjectInfo {
    public string LookupString;
    public List<GameObject> InactiveObjects = new();
}

public class PooledBulletInfo {
    public string LookupString;
    public List<Bullet> InactiveBullets = new();
}

public class PooledAudioSourceInfo {
    public string LookupString;
    public List<AudioSource> availableAudioSources = new();
    public List<AudioSource> InactiveAudioSources = new();
}

