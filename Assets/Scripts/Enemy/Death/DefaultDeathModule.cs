using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class DefaultDeathModule : MonoBehaviour, IEnemyDeathModule
{
    Enemy Enemy;

    public void Die()
    {

    }

    void Awake()
    {
        this.hideFlags = HideFlags.HideInInspector;
        Enemy = GetComponent<Enemy>();
    }
}
