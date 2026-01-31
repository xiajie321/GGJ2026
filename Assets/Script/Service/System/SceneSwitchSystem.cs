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
        /// 正在 (加载/卸载) 场景时会调用的方法,每帧都会返回一个当前的加载进度,如果进度>=0.9则不会不会执行该方法
        /// </summary>
        /// <param name="progress">加载的进度</param>
        /// <param name="isLoad">用于判断当前调用属于加载还是卸载</param>
        public void OnLoad(float progress,bool isLoad);

        /// <summary>
        ///  (加载/卸载) 场景完成时会调用的方法,如果返回值为false每帧都会执行一次,如果返回值为true则会将加载好的场景载入。
        /// </summary>
        /// <param name="progress">加载的进度</param>
        /// <param name="isLoad">用于判断当前调用属于加载还是卸载</param>
        public bool OnLoadCompleted(float progress,bool isLoad);
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
        public void LoadSceneAsync<TUIPanel>(string sceneName,Action end, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
            where TUIPanel : UIPanel, ISceneSwitch
        {
            UniTaskLoadSceneAsync<TUIPanel>(sceneName,end ,loadSceneMode).Forget();
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
        private async UniTask UniTaskLoadSceneAsync<TUIPanel>(string sceneName,Action end, LoadSceneMode loadSceneMode)
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
                _sceneSwitch.OnLoad(operation.progress,true);
                if (operation.progress >= 0.9f)
                {
                    if (_sceneSwitch.OnLoadCompleted(operation.progress,true))
                    {
                        operation.allowSceneActivation = true;
                        end?.Invoke();
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
                _sceneSwitch.OnLoad(operation.progress,false);
                if (operation.progress >= 0.9f)
                {
                    if (_sceneSwitch.OnLoadCompleted(operation.progress,false))
                    {
                        operation.allowSceneActivation = true;
                    }
                }
                await UniTask.Yield();
            }
        }
    }
}