using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class DefaultHealthModule : MonoBehaviour, IEnemyHealthModule
{
    Enemy Enemy;

    void Awake()
    {
        this.hideFlags = HideFlags.HideInInspector;
        Enemy = GetComponent<Enemy>();
        Enemy.Health = Enemy.MaxHealth;
    }
}

