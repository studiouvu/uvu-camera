using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Target : MonoBehaviour
{
    [SerializeField] private float t = 5f;
    [SerializeField] private float speed = 0.2f;

    private int hp = 3;

    public event Action onDestroy;
    public event Action onDamaged;

    private Vector3 _defaultPosition;
    private float _a;

    private void Awake()
    {
        _defaultPosition = transform.localPosition;
        _a = Random.Range(0, 10f);
    }

    private void Update()
    {
        transform.localPosition = _defaultPosition + new Vector3(Mathf.Sin(Time.time * speed + _a) * 2.5f, Mathf.Sin(Mathf.Cos(Time.time * speed + _a * 2)), 0) * t;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        //
        // hp -= 1;
        //
        // onDamaged?.Invoke();
        //
        // if (hp > 0)
        //     return;

        onDestroy?.Invoke();
        Destroy(gameObject);
    }
}
