using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable : ICollidable
{
    void getHit(Damage damage, float hitCd);
}
