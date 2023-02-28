using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Life")]
public class Life : ScriptableObject
{
    public float lifePoints;
    public void life()
    {
        ILifeManager life = DependencyContainer.GetDependency<ILifeManager>();
        life.addLife(lifePoints);
    }
}
