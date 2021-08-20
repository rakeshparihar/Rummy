using UnityEngine;
using UnityEngine.Events;

namespace FrolicRummy.Cam
{

    public class CameraAnchor : MonoBehaviour
    {
        public UnityEvent BecameVisible = new UnityEvent();

        private void OnBecameVisible()
        {
            BecameVisible.Invoke();
        }
    }

}