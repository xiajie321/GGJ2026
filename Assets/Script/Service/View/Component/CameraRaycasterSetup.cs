using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.Service.View.Component
{
    /// <summary>
    /// 自动为相机设置 Physics2DRaycaster
    /// 用于世界空间对象的拖拽事件检测
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraRaycasterSetup : MonoBehaviour
    {
        private void Awake()
        {
            // 检查是否已有 PhysicsRaycaster 或 Physics2DRaycaster
            var raycaster2D = GetComponent<Physics2DRaycaster>();
            var raycaster3D = GetComponent<PhysicsRaycaster>();
            
            if (raycaster2D == null && raycaster3D == null)
            {
                // 根据项目是 2D 还是 3D 自动添加合适的 Raycaster
                // 这里默认添加 Physics2DRaycaster（适用于 2D 游戏）
                gameObject.AddComponent<Physics2DRaycaster>();
                Debug.Log($"[CameraRaycasterSetup] 已为相机 {gameObject.name} 添加 Physics2DRaycaster");
            }
            
            // 确保场景中有 EventSystem
            if (FindObjectOfType<EventSystem>() == null)
            {
                var eventSystemObj = new GameObject("EventSystem");
                eventSystemObj.AddComponent<EventSystem>();
                eventSystemObj.AddComponent<StandaloneInputModule>();
                Debug.Log("[CameraRaycasterSetup] 已创建 EventSystem");
            }
        }
    }
}
