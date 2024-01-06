using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("The Current Health of the Enemy")]
    public int Health;

    [HideInInspector]
    public bool CanMove = true;

    [Header("Enemy Settings")]

    [Tooltip("The Maximumn Health of the Enemy")]
    public int MaxHealth;

    [Tooltip("The Possible Enemy Drops")]
    public EnemyDrop[] Drops;

    //Since these are interface references they can't be serialized by the Unity Engine in the inspector, so all you have to do is attach a module and it will be detected in the Start() function
    public IEnemyMoveModule MovementModule;
    public IEnemyAttackModule AttackModule;
    public IEnemyHealthModule HealthModule;
    public IEnemyHealthBarModule HealthBarModule;
    public IEnemyDeathModule DeathModule;

    void Start()
    {
        if (GetComponent<IEnemyMoveModule>() != null)
            MovementModule = GetComponent<IEnemyMoveModule>();
        else
            Debug.LogError("Enemy " + transform.name + " does not contain a Movement Module");

        if (GetComponent<IEnemyAttackModule>() != null)
            AttackModule = GetComponent<IEnemyAttackModule>();
        else
            Debug.LogError("Enemy " + transform.name + " does not contain an Attack Module");

        if (GetComponent<IEnemyHealthModule>() != null)
            HealthModule = GetComponent<IEnemyHealthModule>();
        else
            HealthModule = gameObject.AddComponent<DefaultHealthModule>();

        if (GetComponent<IEnemyHealthBarModule>() != null)
            HealthBarModule = GetComponent<IEnemyHealthBarModule>();
        else
            HealthBarModule = gameObject.AddComponent<DefaultHealthBarModule>();

        if (GetComponent<IEnemyDeathModule>() != null)
            DeathModule = GetComponent<IEnemyDeathModule>();
        else
            DeathModule = gameObject.AddComponent<DefaultDeathModule>();

    }

    public void TakeDamage(int Amount)
    {
        Health -= Amount;

        if (Health <= 0)
        {
            DeathModule.Die();
            HealthBarModule.Die();
        }
        else
            HealthBarModule.UpdateHealth(Health/MaxHealth);
    }
}

[System.Serializable]
public struct EnemyDrop 
{
    [Tooltip("The item to drop, in form of a prefab")]
    public GameObject Drop;
    [Tooltip("Chance of a drop, form 0 to 1")]
    public float Chance;
    [Tooltip("The minimumn number of dropped items")]
    public int MinDrops;
    [Tooltip("The maximumn number of dropped items")]
    public int MaxDrops;
}

//Any interface here will be a module type that the enemy can use
public interface IEnemyAttackModule //Used to control an enemies attacks
{
    public void Attack(PlayerController Player);
}

public interface IEnemyDeathModule //Used to control what happens to an enemy on death
{
    public void Die();
}

public interface IEnemyHealthModule //Used to control the health of an enemy
{

}

public interface IEnemyHealthBarModule //Used to control the health bar of an enemy
{
    public void UpdateHealth(float Health);
    public void Die();
}

public interface IEnemyMoveModule //Used to control how the enemy moves
{
    public float MovementFreeze
    {
        get;
        set;
    }
}