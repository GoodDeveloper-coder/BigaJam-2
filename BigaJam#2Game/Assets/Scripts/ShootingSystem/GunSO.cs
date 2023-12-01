using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Shooting/Gun")]
public class GunSO : ScriptableObject
{
    public Sprite gunSprite;
    public int MaxGunAmmo;
    public float Cooldown;
}
