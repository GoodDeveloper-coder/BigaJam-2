using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class Bullet : MonoBehaviour
{
    [SerializeField] AmmoSO _AmmoType;

    private Rigidbody2D rb;
    private PoolObject _poolObject;


    void Awake()
    {
            
    }

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
        rb.velocity = transform.right * _AmmoType.Speed * Time.deltaTime;
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(_AmmoType.LifeTime);
        _poolObject.ReturnPool();
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();

        if (pm != null)
        {
            // Apply _knockBackForce force per second.
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(_AmmoType.KnockBackForce * Time.deltaTime, 0f), ForceMode2D.Impulse);
            pm.TakeDamage(_AmmoType.Damage);
        }
        Instantiate(_AmmoType.BulletHitParticle, this.gameObject.transform.position, Quaternion.identity);
        _poolObject.ReturnPool();
    }

    public void SetAmmoType(AmmoSO ammoType)
    {
        _AmmoType = ammoType;
    }
}
