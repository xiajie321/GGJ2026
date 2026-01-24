using System;
using Cysharp.Threading.Tasks;
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Service.System
{
    public interface ISceneSwitch
    {
        /// <summary>
        /// 正在加载时会调用的方法
        /// </summary>
        /// <param name="progress">加载的进度</param>
        public void OnLoad(float progress);

        /// <summary>
        /// 加载完成时会调用的方法
        /// </summary>
        /// <param name="progress">加载的进度</param>
        public bool OnLoadCompleted(float progress);
    }

    public class SceneSwitchSystem : AbstractSystem
    {
        private UIPanel _panel;
        private ISceneSwitch _sceneSwitch;

        protected override void OnInit()
        {
            Debug.Log("[SceneSwitchSystem] 加载完成...");
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="loadSceneMode">加载模式</param>
        /// <typeparam name="TUIPanel">实现了 ISceneSwitch 接口的 UIPanel</typeparam>
        public void LoadSceneAsync<TUIPanel>(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
            where TUIPanel : UIPanel, ISceneSwitch
        {
            UniTaskLoadSceneAsync<TUIPanel>(sceneName, loadSceneMode).Forget();
        }

        /// <summary>
        /// 异步卸载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <typeparam name="TUIPanel">实现了 ISceneSwitch 接口的 UIPanel</typeparam>
        public void UnloadSceneAsync<TUIPanel>(string sceneName) where TUIPanel : UIPanel, ISceneSwitch
        {
            UniTaskUnloadSceneAsync<TUIPanel>(sceneName).Forget();
        }

        /// <summary>
        /// 异步加载场景的具体实现
        /// </summary>
        private async UniTask UniTaskLoadSceneAsync<TUIPanel>(string sceneName, LoadSceneMode loadSceneMode)
            where TUIPanel : UIPanel, ISceneSwitch
        {
            Type tUIPanel = typeof(TUIPanel);
            if (_panel == null || tUIPanel != _panel.GetType())
            {
                if (_panel)
                {
                    UIKit.ClosePanel(_panel);
                }

                _panel = UIKit.OpenPanel<TUIPanel>();
                _sceneSwitch = (ISceneSwitch)_panel;
            }
            else
            {
                UIKit.OpenPanel<TUIPanel>();
            }

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            if (operation == null) return;
            operation.allowSceneActivation = false;
            while (!operation.isDone)
            {
                _sceneSwitch.OnLoad(operation.progress);
                if (operation.progress >= 0.9f)
                {
                    if (_sceneSwitch.OnLoadCompleted(operation.progress))
                    {
                        operation.allowSceneActivation = true;
                    }
                }

                await UniTask.Yield();
            }
        }

        /// <summary>
        /// 异步卸载场景的具体实现
        /// </summary>
        private async UniTask UniTaskUnloadSceneAsync<TUIPanel>(string sceneName)
            where TUIPanel : UIPanel, ISceneSwitch
        {
            Type tUIPanel = typeof(TUIPanel);
            if (_panel == null || tUIPanel != _panel.GetType())
            {
                if (_panel)
                {
                    UIKit.ClosePanel(_panel);
                }

                _panel = UIKit.OpenPanel<TUIPanel>();
                _sceneSwitch = (ISceneSwitch)_panel;
            }
            else
            {
                UIKit.OpenPanel<TUIPanel>();
            }

            AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
            
            if (operation == null) return;
            operation.allowSceneActivation = false;
            while (!operation.isDone)
            {
                if (operation.progress >= 0.9f)
                {
                    if (_sceneSwitch.OnLoadCompleted(operation.progress))
                    {
                        operation.allowSceneActivation = true;
                    }
                }
                await UniTask.Yield();
            }
        }
    }
}