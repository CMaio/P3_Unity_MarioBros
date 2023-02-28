using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Score")]
public class Score : ScriptableObject
{
    public float points;
    public void score()
    {
        IScoreManager score = DependencyContainer.GetDependency<IScoreManager>();
        score.addPoints(points);
    }
}

