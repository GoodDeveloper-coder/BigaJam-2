using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public void ReturnPool()
    {
        gameObject.SetActive(false);
    }
}
