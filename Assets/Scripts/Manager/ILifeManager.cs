using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILifeManager
{
    void addLife(float f);
    void doDamage(float f);
    float getLife();
    event LifeChanged lifeChangedDelegate;
}
public delegate void LifeChanged(ILifeManager scoreManager);
