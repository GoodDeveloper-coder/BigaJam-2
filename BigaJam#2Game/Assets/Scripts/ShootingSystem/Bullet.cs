using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _lifeTime;
    [SerializeField] float _powerWhenTouchWithPlayer;

    private Rigidbody2D rb;
    private PoolObject _poolObject;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _poolObject = GetComponent<PoolObject>();
        StartCoroutine(Destroy());
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * _speed * Time.deltaTime;
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(_lifeTime);
        _poolObject.ReturnPool();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();

        if (pm != null)
        {
            pm.Repulsion(100f);
        }
        _poolObject.ReturnPool();
    }
}
