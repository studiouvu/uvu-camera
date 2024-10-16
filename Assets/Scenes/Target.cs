using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Target : MonoBehaviour
{
    [FormerlySerializedAs("t")] [SerializeField] private float distance = 5f;
    [SerializeField] private float speed = 0.2f;

    public event Action onDestroy;

    private Vector3 _defaultPosition;
    private float _seed;

    private void Awake()
    {
        _defaultPosition = transform.localPosition;
        _seed = Random.Range(0, 10f);
        UpdatePosition();
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.localPosition = _defaultPosition + new Vector3(Mathf.Sin(Time.time * speed + _seed) * 2.5f, Mathf.Sin(Mathf.Cos(Time.time * speed + _seed * 2)), 0) * distance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        onDestroy?.Invoke();
        Destroy(gameObject);
    }
}
