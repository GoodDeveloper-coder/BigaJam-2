using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBGScript : MonoBehaviour
{
    [SerializeField] private float _speed = 0.5f;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(Time.time * _speed, 0);
        rend.material.mainTextureOffset = offset;
    }  
}
