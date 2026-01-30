using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using QFramework;
using Service.View.UI.Panel;  
using UnityEngine;
namespace Script.Service.System
{
    public class CameraEdgeScrollingSystem : AbstractSystem
    {
        private Camera _mainCamera;
        private Transform _bg;
        private Vector2 _edgeBorder = Vector2.zero;  //x 最左 y 最右  
        private bool _isEnabled = true;
        private float _screenEdgeSize = 50f;  // 屏幕边缘检测区域大小（像素）

        /// <summary>
        /// 相机移动速度
        /// </summary>
        public float CameraMoveSpeed { get; set; } = 10f;

        private CancellationTokenSource _cts;

        protected override void OnInit()
        {

        }

        public void InitCameraSystem()
        {
            Debug.Log("[cjh test] 初始化相机系统...1");
            _mainCamera = Camera.main;
            SetBg();
            // 计算相机视野占场景的比例
            float cameraViewWidth = _mainCamera.orthographicSize * _mainCamera.aspect * 2f;
            float sceneWidth = _edgeBorder.y - _edgeBorder.x;
            float cameraViewRatio = cameraViewWidth / sceneWidth;

            UIKit.OpenPanel<UIMiniMap>(UILevel.Common, new UIMiniMapData
            {
                CameraViewRatio = cameraViewRatio
            });
            // 启动更新循环
            _cts = new CancellationTokenSource();
            UpdateLoop(_cts.Token).Forget();
            Debug.Log("[cjh test] 初始化相机系统...2");
        }

        public void SetBg()
        {
            //场景中寻找 Bg 背景图
            var bg = GameObject.Find("Bg");
            if (bg == null)
            {
                Debug.LogError("Bg 背景图未找到");
                return;
            }
            _bg = bg.transform;
            // 计算场景左右边界
            float cameraWidth = _mainCamera.orthographicSize * _mainCamera.aspect;
            _edgeBorder = new Vector2(
                _bg.position.x - _bg.localScale.x / 2f,  // 最左边界
                _bg.position.x + _bg.localScale.x / 2f   // 最右边界
            );

            Debug.Log($"[cjh test] 初始化相机系统...6--{_edgeBorder}");
        }

        /// <summary>
        /// 获取鼠标是否在屏幕边缘
        /// </summary>
        public bool IsMouseAtScreenEdge()
        {
            Vector3 mousePos = Input.mousePosition;

            return mousePos.x < _screenEdgeSize ||
                   mousePos.x > Screen.width - _screenEdgeSize;
        }

        /// <summary>
        /// 停止更新循环
        /// </summary>
        public void StopUpdateLoop()
        {
            _cts?.Cancel();
            _isEnabled = false;
        }

        /// <summary>
        /// 重新启动更新循环
        /// </summary>
        public void RestartUpdateLoop()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            _isEnabled = true;
            UpdateLoop(_cts.Token).Forget();
        }

        /// <summary>
        /// 持续更新循环 - 使用 UniTask
        /// </summary>
        private async UniTaskVoid UpdateLoop(CancellationToken cancellationToken)
        {
            Debug.Log("[cjh test] 初始化相机系统...3");
            while (!cancellationToken.IsCancellationRequested)
            {
                // 等待下一帧
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);

                if (!_isEnabled || _mainCamera == null)
                    continue;

                UpdateCameraPosition();
            }
        }

        /// <summary>
        /// 更新相机位置
        /// </summary>
        private void UpdateCameraPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 moveDirection = CalculateMoveDirection(mousePos);
            Debug.Log($"[cjh test] 初始化相机系统...4--{moveDirection.magnitude}");
            // 如果有移动方向，则移动相机
            if (moveDirection.magnitude > 0)
            {
                Vector3 newPosition = _mainCamera.transform.position +
                    moveDirection * CameraMoveSpeed * Time.deltaTime;

                // 应用场景边界限制
                newPosition = ClampCameraPosition(newPosition);

                _mainCamera.transform.position = newPosition;

                Debug.Log("[cjh test] 初始化相机系统...5");
            }
        }

        /// <summary>
        /// 计算移动方向（只支持左右移动）
        /// </summary>
        private Vector3 CalculateMoveDirection(Vector3 mousePos)
        {
            Vector3 moveDirection = Vector3.zero;

            // 左边缘
            if (mousePos.x < _screenEdgeSize)
            {
                moveDirection.x = -1f;
            }
            // 右边缘
            else if (mousePos.x > Screen.width - _screenEdgeSize)
            {
                moveDirection.x = 1f;
            }

            return moveDirection;
        }

        /// <summary>
        /// 限制相机位置在场景边界内（只限制左右）
        /// </summary>
        private Vector3 ClampCameraPosition(Vector3 position)
        {
            if (_mainCamera == null)
                return position;

            // 计算相机可见宽度
            float cameraWidth = _mainCamera.orthographicSize * _mainCamera.aspect;

            // 使用 _edgeBorder 限制（x 是最左，y 是最右）
            float minX = _edgeBorder.x + cameraWidth;  // 最左边界 + 相机半宽
            float maxX = _edgeBorder.y - cameraWidth;  // 最右边界 - 相机半宽

            // 如果场景小于相机视野，则居中
            if (minX >= maxX)
            {
                position.x = (_edgeBorder.x + _edgeBorder.y) * 0.5f;
            }
            else
            {
                position.x = Mathf.Clamp(position.x, minX, maxX);
            }

            return position;
        }
        /// <summary>
        /// 获取场景边界（供小地图使用）
        /// </summary>
        public Vector2 GetSceneBounds()
        {
            return _edgeBorder;
        }

        /// <summary>
        /// 移动相机到指定世界 X 坐标（供小地图调用）
        /// </summary>
        public void MoveCameraToWorldX(float targetWorldX)
        {
            if (_mainCamera == null)
                return;

            // 应用边界限制
            float cameraWidth = _mainCamera.orthographicSize * _mainCamera.aspect;
            float minX = _edgeBorder.x + cameraWidth;
            float maxX = _edgeBorder.y - cameraWidth;

            targetWorldX = Mathf.Clamp(targetWorldX, minX, maxX);

            // 移动相机
            Vector3 newPos = _mainCamera.transform.position;
            newPos.x = targetWorldX;
            _mainCamera.transform.position = newPos;

            Debug.Log($"[Camera] 相机移动到 X={targetWorldX:F1}");
        }
    }
}