using UnityEngine;

namespace GameCore
{
    public class CameraRenderer : MonoBehaviour
    {
        public Vector2 DefaultResolution = new Vector2(720, 1280);
        [Range(0f, 1f)] public float WidthOrHeight;

        private Camera componentCamera;

        private float initialSize;
        private float targetAspect;

        private float initialFov;
        private float horizontalFov = 120f;

        private void Start()
        {
            componentCamera = GetComponent<Camera>();
            initialSize = componentCamera.orthographicSize;

            targetAspect = DefaultResolution.x / DefaultResolution.y;

            initialFov = componentCamera.fieldOfView;
            horizontalFov = CalcVerticalFov(initialFov, 1 / targetAspect);
        }

        private void Update()
        {
            if (componentCamera.orthographic)
            {
                var constantWidthSize = initialSize * (targetAspect / componentCamera.aspect);
                componentCamera.orthographicSize = Mathf.Lerp(constantWidthSize, initialSize, WidthOrHeight);
            }
            else
            {
                var constantWidthFov = CalcVerticalFov(horizontalFov, componentCamera.aspect);
                componentCamera.fieldOfView = Mathf.Lerp(constantWidthFov, initialFov, WidthOrHeight);
            }
        }

        private float CalcVerticalFov(float hFovInDeg, float aspectRatio)
        {
            var hFovInRads = hFovInDeg * Mathf.Deg2Rad;

            var vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

            return vFovInRads * Mathf.Rad2Deg;
        }
    }
}