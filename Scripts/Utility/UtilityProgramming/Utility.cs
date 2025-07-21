using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {
    public static GameObject FindGameObjectInChildWithTag (GameObject parent, string tag)
    {
        Transform t = parent.transform;
        for (int i = 0; i < t.childCount; i++) 
        {
            Debug.Log(t.GetChild(i).gameObject.tag);
            if(t.GetChild(i).gameObject.tag == tag)
            {
                return t.GetChild(i).gameObject;
            }
        }
        return null;
    }

    public static Vector3 GetMouseWorldPosition() {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ() {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
            return n;
    }

    public static GameObject FindChildWithName(GameObject parent, string name) {
        Transform trans = parent.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null) 
            return childTrans. gameObject;
        else 
            return null;
    }

    public static GameObject FindChildWithTag(GameObject parent, string tag) {
        GameObject child = null;
 
        foreach(Transform transform in parent.transform) {
            Debug.Log("Parent: " + parent.transform.name);
            Debug.Log("Child: " + transform.name);
            if(transform.CompareTag(tag)) {
                child = transform.gameObject;
                break;
            }
        }
        Debug.Log(child);
        return child;
    }

    public static int generateNonDuplicate(int valueNotEqual, int min, int max) {
        int r;
        do {
            r = Random.Range(min,max);
        } while(r == valueNotEqual);

        return r;
        
    }

    public static int generateNonDuplicate(int valueNotEqual1, int valueNotEqual2, int min, int max) {
        int r;
        do {
            r = Random.Range(min,max);
        } while(r == valueNotEqual1 || r == valueNotEqual2);

        return r;
        
    }

    public static string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999)) return "";
        if (number < 1) return string.Empty;            
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900); 
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);            
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);

        return "Impossible state reached";
    }
    public static void PrintArray<T>(T[] array) {
        string result = string.Join("\n", array);
        Debug.Log("Print Array --> [" + result + "]");
    }
    public static void PrintArray<T>(T[,] array)
    {
        int rows = array.GetLength(0);
        int columns = array.GetLength(1);
        Debug.Log("Print Array: " + rows + "x" + columns + " -->");
        for (int i = 0; i < rows; i++) {
            string row = "";
            for (int j = 0; j < columns; j++) 
                row += array[i, j] + " ";
            Debug.Log(row);
        }
    }
    
    public static void PrintList<T>(List<T> list) {
        string result = string.Join("\n", list);
        Debug.Log("Print List --> [" +result + "]");
    }
    public static string GetSumFromList(List<string> list) {
        string sum = "";
        foreach(string element in list) 
            sum += element;
        return sum;
    }
    public static int GetSumFromList(List<int> list) {
        int sum = 0;
        foreach(int element in list) 
            sum += element;
        return sum;
    }
    public static float GetSumFromList(List<float> list) {
        float sum = 0;
        foreach(float element in list) 
            sum += element;
        return sum;
    }
    public static float GetAverageFromList(List<float> list) {
        return GetSumFromList(list) / list.Count;
    }
    public static int GetAverageFromList(List<int> list) {
        return (int)System.Math.Ceiling((double)GetSumFromList(list) / list.Count);
    }

    public static Color32 GetAverageColorFromList(List<Color32> list) {
        if(list.Count == 1)
            return list[0];
        Color32 color = list[0];
        for(int i=1; i < list.Count; i++) 
            color = Color.Lerp(color, list[i], 0.5f);
        return color;
    }

    public static WeaponStats GetSumFromTwoStats(WeaponStats w1, WeaponStats w2) {
        WeaponStats stats = ScriptableObject.CreateInstance<WeaponStats>();
        List<StatusEffectData> statusEffects = new(w1.statusEffects);
        statusEffects.AddRange(w2.statusEffects);
        stats.SetWeaponStats(w1.baseDamage + w2.baseDamage,
            w1.knockback + w2.knockback,
            w1.atkCd - w2.atkCd,
            w1.projectileSpeed + w2.projectileSpeed,
            w1.weaponSize + w2.weaponSize,
            w1.projectilePierce + w2.projectilePierce,
            w1.projectileNumber + w2.projectileNumber,
            w1.projectileBounce + w2.projectileBounce,
            statusEffects);
        return stats;
    }

    public static BaseWeaponStats GetSumFromTwoStats(BaseWeaponStats w1, WeaponStats w2) {
        BaseWeaponStats stats = BaseWeaponStats.CreateInstance<WeaponStats>();
        stats.SetWeaponStats(
            w1.baseDamage + w2.baseDamage,
            w1.knockback + w2.knockback,
            w1.atkCd - w2.atkCd,
            w1.weaponSize + w2.weaponSize);
        return stats;
    }

    public static bool AreGameObjectsSameIgnoringClone(GameObject obj1, GameObject obj2) {
        string name1 = obj1.name.Replace("(Clone)", "").Trim();
        string name2 = obj2.name.Replace("(Clone)", "").Trim();
        return name1 == name2;
    }
    public static bool AreGameObjectsSameIgnoringClone(string obj1, string obj2) {
        string name1 = obj1.Replace("(Clone)", "").Trim();
        string name2 = obj2.Replace("(Clone)", "").Trim();
        return name1 == name2;
    }

    public static string GetPercentStrFromDecimalValue(float value) {
        return (value * 100).ToString() + "%";
    }
}