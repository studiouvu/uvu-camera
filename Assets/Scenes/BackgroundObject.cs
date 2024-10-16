using System;
using UnityEngine;
namespace Studiouvu.Runtime
{
    public class BackgroundObject : MonoBehaviour
    {
        [SerializeField] private InGameCamera _inGameCamera;
        [SerializeField] private float _distance;

        private Vector3 _defaultPosition;

        private void Awake()
        {
            _defaultPosition = transform.localPosition;
        }

        private void LateUpdate()
        {
            transform.localPosition = _defaultPosition + _inGameCamera.Camera.transform.position.Copy(z: 0) * _distance;
        }
    }
}
