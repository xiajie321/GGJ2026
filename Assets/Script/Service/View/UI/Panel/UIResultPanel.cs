using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Script.SODataScript.TbConfig;
using Script.Service.Model;
using Script.Service.System;
using Script.Service.View.Game;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Script.Service.Architecture;
using Script.Service.View.UI.Panel;

namespace Service.View.UI.Panel
{
	public class UIResultPanelData : UIPanelData
	{
		public LevelData LevelData;
		public bool HasNextLevel;
		public int CurrentLevelIndex;
	}
	public partial class UIResultPanel : QFramework.UIPanel, QFramework.IController
	{
		public QFramework.IArchitecture GetArchitecture()
		{
			return GameArchitecture.Interface;
		}

		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIResultPanelData ?? new UIResultPanelData();
			
			NextLevelBtn.onClick.AddListener(() =>
			{
				// 加载下一关
				int nextLevelIndex = mData.CurrentLevelIndex + 1;
				EnemyExitMono.Index = nextLevelIndex;
				
				this.GetSystem<SceneSwitchSystem>().LoadSceneAsync<UILoadingPanel>($"Level{nextLevelIndex}", v =>
				{
					Run(v).Forget();
				});
			});

			MainMenuBtn.onClick.AddListener(() =>
			{
				this.GetSystem<SceneSwitchSystem>().LoadSceneAsync<UILoadingPanel>("GameBoot", v =>
				{
					UIKit.CloseAllPanel();
					UIKit.OpenPanel<UIHomePanel>();
				});
			});

			TryAgainBtn.onClick.AddListener(() =>
			{
				// 重试当前关卡
				int currentLevelIndex = mData.CurrentLevelIndex;
				// 恢复 Index
				EnemyExitMono.Index = currentLevelIndex;
				this.GetSystem<SceneSwitchSystem>().LoadSceneAsync<UILoadingPanel>($"Level{currentLevelIndex}", v =>
				{
					Run(v).Forget();
				});
			});
			
			CloseBtn.onClick.AddListener(() =>
			{
				// 关闭按钮的行为可能与主菜单相同，或者只是关闭面板（但在结算时通常意味着离开）
				MainMenuBtn.onClick.Invoke();
			});
		}
		
		private async UniTask Run(UIPanel panel)
		{
			await UniTask.Delay(500);
			this.GetSystem<LevelSystem>().StartLevel(EnemyExitMono.Index - 1); 
			await UniTask.Yield();
			UIKit.CloseAllPanel();
			UIKit.OpenPanel<UIMouseCursorPanel>(UILevel.PopUI);
			UIKit.OpenPanel<UIHUDPanel>();
			await UniTask.Yield();
			if(panel != null && panel.gameObject != null)
				UIKit.ClosePanel(panel);
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			if (uiData != null)
			{
				mData = uiData as UIResultPanelData ?? new UIResultPanelData();
			}
			PlayResultAnimation().Forget();
		}
		
		private async UniTaskVoid PlayResultAnimation()
		{
			var levelModel = this.GetModel<LevelModel>();
			var levelData = mData.LevelData;

			if (levelData == null) return;

			// 初始状态：隐藏星星，文本设为0
			UIResultStar_1.gameObject.SetActive(false);
			UIResultStar_2.gameObject.SetActive(false);
			UIResultStar_3.gameObject.SetActive(false);
			ScoreText.text = "0";
			
			// 初始隐藏按钮
			NextLevelBtn.gameObject.SetActive(false);
			TryAgainBtn.gameObject.SetActive(false);
			MainMenuBtn.gameObject.SetActive(false);
			CloseBtn.gameObject.SetActive(false);

			// 计算目标值
			float currentMoney = levelModel.Money;
			float targetProfit = currentMoney - (levelData.PerStarCost * levelData.MaxStar);
			float targetScore = currentMoney;

			// 计算星级
			int stars = 0;
			if (targetScore >= levelData.QualifiedLine)
			{
				stars = 1;
				if (levelData.PerStarCost > 0)
				{
					stars += Mathf.FloorToInt((targetScore - levelData.QualifiedLine) / levelData.PerStarCost);
				}
			}
			stars = Mathf.Min(stars, levelData.MaxStar);

			// 胜利/失败标题显示
			if (targetProfit >= 0)
			{
				TitleImage.gameObject.SetActive(false);
			}
			else
			{
				TitleImage.gameObject.SetActive(true);
			}

			// 1. 数字滚动动画 (0.5秒)
			float duration = 0.5f;
			float timer = 0f;
			
			while (timer < duration)
			{
				timer += Time.deltaTime;
				float t = timer / duration;
				
				float currentProfit = Mathf.Lerp(0, targetProfit, t);
				float currentScoreVal = Mathf.Lerp(0, targetScore, t);
				
				ScoreText.text = $"{Mathf.RoundToInt(currentScoreVal)}";

				await UniTask.Yield();
			}
			
			// 确保最终值正确
			ScoreText.text = $"{targetScore}";
			
			// 2. 星星逐个显示动画
			if (stars >= 1)
			{
				await UniTask.Delay(200);
				UIResultStar_1.gameObject.SetActive(true);
				// 简单的缩放效果
				PlayScaleAnimation(UIResultStar_1.transform).Forget();
				// 播放音效
				if (levelData.WinSound) AudioSource.PlayClipAtPoint(levelData.WinSound, Vector3.zero);
			}
			
			if (stars >= 2)
			{
				await UniTask.Delay(200);
				UIResultStar_2.gameObject.SetActive(true);
				PlayScaleAnimation(UIResultStar_2.transform).Forget();
				if (levelData.WinSound) AudioSource.PlayClipAtPoint(levelData.WinSound, Vector3.zero);
			}
			
			if (stars >= 3)
			{
				await UniTask.Delay(200);
				UIResultStar_3.gameObject.SetActive(true);
				PlayScaleAnimation(UIResultStar_3.transform).Forget();
				if (levelData.WinSound) AudioSource.PlayClipAtPoint(levelData.WinSound, Vector3.zero);
			}

			// 失败音效
			if (stars == 0 && levelData.LoseSound)
			{
				AudioSource.PlayClipAtPoint(levelData.LoseSound, Vector3.zero);
			}

			// 3. 显示按钮
			await UniTask.Delay(200);
			NextLevelBtn.gameObject.SetActive(mData.HasNextLevel);
			TryAgainBtn.gameObject.SetActive(true);
			MainMenuBtn.gameObject.SetActive(true);
			CloseBtn.gameObject.SetActive(true);
		}

		private async UniTaskVoid PlayScaleAnimation(Transform target)
		{
			float duration = 0.3f;
			float timer = 0f;
			Vector3 originalScale = Vector3.one;
			target.localScale = Vector3.zero;

			while (timer < duration)
			{
				timer += Time.deltaTime;
				float t = timer / duration;
				// 简单的弹跳效果曲线
				float scale = Mathf.Sin(t * Mathf.PI) * 0.2f + 1f; 
				if (t >= 1) scale = 1f;
				
				target.localScale = originalScale * Mathf.Lerp(0, scale, t);
				await UniTask.Yield();
			}
			target.localScale = originalScale;
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
