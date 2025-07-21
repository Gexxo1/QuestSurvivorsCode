using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drop", menuName = "ScriptableObjects/Drop/DropSingle")]
public class Drop : ScriptableObject
{
    [SerializeField] public Collectable item;
    [SerializeField] [Range(0, 100)] public int percent;
    [SerializeField] private int quantity = 1;
    [SerializeField] private int quantityMaxInclusive = 0; //Se non c'è alcun intervallo va lasciato a 0
    //IMPORTANTE: Non ricavare MAI direttamente quantity, ma getQuantity(), in modo che se c'è un intervallo da ricavarcelo
    public int getQuantity() {
        if(quantityMaxInclusive <= quantity)
            return quantity;
        return Random.Range(quantity,quantityMaxInclusive+1);
    }

}
