using System;
using System.Collections;
using System.Collections.Generic;
using Studiouvu.Runtime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CameraTest : MonoBehaviour
{
    [SerializeField] private InGameCamera _inGameCamera;
    [SerializeField] private Target _targetTestPrefab;
    [SerializeField] private Target _targetTestBigPrefab;
    [SerializeField] private Projectile _projectilePrefab;

    [SerializeField] private float _outFocusDistance = 0.5f;
    [SerializeField] private float _targetCreateDistance = 10f;

    [SerializeField] private float speed = 0.2f;

    private void OnEnable()
    {
        _inGameCamera.AddCameraTarget(transform, 2);
    }

    public void TestAddTarget()
    {
        var position = Random.insideUnitCircle * _targetCreateDistance;
        var target = Instantiate(_targetTestPrefab, position, Quaternion.identity);
        target.onDamaged += ShakeSmall;
        target.onDestroy += Shake;
        target.onDestroy += () => _inGameCamera.RemoveCameraTarget(target.transform);
        _inGameCamera.AddCameraTarget(target.transform, 1);
    }

    public void TestAddBigTarget()
    {
        var position = Random.insideUnitCircle * _targetCreateDistance;
        var target = Instantiate(_targetTestBigPrefab, position, Quaternion.identity);
        target.onDamaged += ShakeSmall;
        target.onDestroy += Shake;
        target.onDestroy += () => _inGameCamera.RemoveCameraTarget(target.transform);
        _inGameCamera.AddCameraTarget(target.transform, 3);
    }

    public void Shake()
    {
        _inGameCamera.Shake(2f);
    }

    public void ShakeSmall()
    {
        _inGameCamera.Shake(0.5f);
    }

    public void Shot()
    {
        var mousePosition = _inGameCamera.Camera.ScreenToWorldPoint(Input.mousePosition);
        var direction = (mousePosition - transform.position).Copy(z: 0).normalized;

        var projectile = Instantiate(_projectilePrefab, transform.position, quaternion.identity);
        projectile.SetDirection(direction);

        _inGameCamera.OutFocus(direction * _outFocusDistance);
    }

    private void Update()
    {
        transform.localPosition = new Vector3(Mathf.Sin(Time.time * speed) * 2f, Mathf.Cos(Time.time * speed * 2), 0) * 5f;
    }
}
