using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Type", menuName = "Shooting/Ammo Type")]

public class AmmoSO : ScriptableObject 
{
    public float Damage = 10f;
    public float Speed = 2000f;

    [Tooltip("This multiplier affects how strongly the bullet knocks back a player.")]
    public float KnockBackForce = 1f;

    public float LifeTime = 3f;
    public float PowerWhenTouchWithPlayer = 1000f;

    [Tooltip("How much ammo of this type that players start with, not counting ammo in the gun already.")]
    public int StartingAmmo = 30;

    [Tooltip("The max amount of ammo of this type that can be in a player's stash at any one time.")]
    public int MaxAmmo = 100;

    public GameObject BulletHitParticle;

    [Tooltip("If true, this ammo type is infinite and never runs out.")]
    public bool HasInfiniteAmmo = false;
}
