using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Studiouvu.Runtime
{
    public class InGameCamera : MonoBehaviour, IIngameCameraService
    {
        public Camera Camera => _camera;

        [SerializeField] private Camera _camera;
        [SerializeField] private float _defaultSize = 14f;

        [SerializeField] private Vector2 _cameraSize;
        [SerializeField] private Vector2 _roomSize;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private float _speed = 0.1f;

        [SerializeField] private float _shakeSpeed = 10f;
        [SerializeField] private float _shakeTremble = 9f;
        [SerializeField] private float _shakeDisappearSpeed = 0.1f;
        [SerializeField] private float _outFocusDisappearSpeed = 0.3f;

        [SerializeField] private Transform _resultPoint;
        [SerializeField] private Transform _targetPoint;
        // [SerializeField] private Transform _shakenPoint;

        private readonly Dictionary<Transform, float> _targetSet = new();
        private Vector3 _targetPosition;
        private Vector3 _cameraPosition;
        private float _shakePower;
        private float _shakeTime;
        private float _zoom;
        private Vector3 _outFocusVector;
        private float _outFocusTime;

        private float _defaultOrthographicSize;

        private void Start()
        {
            var ratio = 1;
            // _battleRenderController.ScreenDefaultRatio / _battleRenderController.ScreenCurrentRatio;
            _defaultOrthographicSize = _defaultSize / ratio;
            _camera.orthographicSize = _defaultOrthographicSize;
        }

        private void FixedUpdate()
        {
            if (_targetSet.Count == 0)
                return;

            var position = Vector3.zero;

            foreach (var t in _targetSet)
                position += (t.Key.position.Copy(z: 0) * t.Value);

            var centerPosition = (position / _targetSet.Values.Sum());

            var roomSizeWidthHalf = Mathf.Max((_roomSize.x - _cameraSize.x) / 2, 0);
            var roomSizeHeightHalf = Mathf.Max((_roomSize.y - _cameraSize.y) / 2, 0);

            centerPosition = new Vector3(Mathf.Clamp(centerPosition.x, -roomSizeWidthHalf, roomSizeWidthHalf)
                , Mathf.Clamp(centerPosition.y, -roomSizeHeightHalf, roomSizeHeightHalf));

            _targetPosition = centerPosition;

            _targetPoint.position = transform.position.Copy(z: 0);

            _cameraPosition = Vector2.Lerp(_cameraPosition, centerPosition, _speed);

            var shakeTime = Time.time * _shakeSpeed;
            var shakeMul = Mathf.Sin(Time.time * _shakeTremble);
            var shakePos = new Vector3(Mathf.Sin(shakeTime), Mathf.Cos(shakeTime)) * ((0.2f + shakeMul) * _shakePower);

            var shakenPosition = _cameraPosition + shakePos;

            if (_shakePower > 0)
                _shakePower = Mathf.Lerp(_shakePower, 0, _shakeDisappearSpeed);

            if (_zoom > 0)
                _zoom = Mathf.Lerp(_zoom, 0, _shakeDisappearSpeed);

            _outFocusTime += Time.deltaTime;

            var calOutFocusVector = _outFocusVector * Mathf.Sin(_outFocusTime * _shakeTremble);
            _outFocusVector = Vector3.Lerp(_outFocusVector, Vector3.zero, _outFocusDisappearSpeed);

            var resultPosition = shakenPosition + calOutFocusVector;

            transform.position = resultPosition.Copy(z: transform.position.z);

            _resultPoint.position = transform.position.Copy(z: 0);

            _camera.orthographicSize = _defaultOrthographicSize * (1 - _zoom);
        }

        public void AddCameraTarget(Transform target, float weight)
        {
            _targetSet.Add(target, weight);
        }

        public void RemoveCameraTarget(Transform target)
        {
            _targetSet.Remove(target);
        }

        public void Shake(float power, float zoom = 0)
        {
            if (power > _shakePower)
                _shakePower = power;

            if (zoom > _zoom)
                _zoom = zoom;
        }

        public void OutFocus(Vector3 vector)
        {
            _outFocusTime = 0;
            _outFocusVector += vector;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, _cameraSize);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero, _roomSize);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_targetPosition, 0.5f);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_cameraPosition, 0.5f);
        }


    }

    public static class VectorExtension
    {
        public static Vector3 Copy(this Vector3 v, float? x = null, float? y = null, float? z = null)
            => new Vector3(x ?? v.x, y ?? v.y, z ?? v.z);
    }
}
