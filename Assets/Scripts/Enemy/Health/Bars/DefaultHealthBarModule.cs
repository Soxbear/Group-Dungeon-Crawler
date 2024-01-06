using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class DefaultHealthBarModule : MonoBehaviour, IEnemyHealthBarModule
{
    Enemy Enemy;

    public void UpdateHealth(float Health)
    {

    }

    public void Die()
    {

    }

    void Awake()
    {
        this.hideFlags = HideFlags.HideInInspector;
        Enemy = GetComponent<Enemy>();
    }
}
