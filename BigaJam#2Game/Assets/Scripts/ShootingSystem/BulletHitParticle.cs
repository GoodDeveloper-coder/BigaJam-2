using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitParticle : MonoBehaviour
{
    [SerializeField] private float _destoryAfter;
    private Animator _animator;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        Destroy(this.gameObject, _destoryAfter);
    }
}
