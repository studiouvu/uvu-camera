using UnityEngine;
namespace Studiouvu.Runtime
{
    public interface IIngameCameraService
    {
        Camera Camera { get; }

        public void AddCameraTarget(Transform target, float weight);
        
        void RemoveCameraTarget(Transform target);

        public void Shake(float power, float zoom = 0);
    }
}
