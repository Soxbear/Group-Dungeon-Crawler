using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactAttackModule : MonoBehaviour, IEnemyAttackModule
{
    public int Damage;
    public float AttackMovementFreeze;

    // Start is called before the first frame update
    public void Attack(PlayerController Player)
    {
        Player.TakeDamage(Damage);
    }

    void OnCollisionEnter2D(Collision2D Col)
    {
        if (Col.transform.tag == "Player")
            Attack(Col.transform.GetComponent<PlayerController>());
    }
}
