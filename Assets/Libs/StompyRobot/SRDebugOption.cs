// using Cysharp.Threading.Tasks;
// using DataManagement;
// using Game;
// using Game.DailyChallenge;
using System.ComponentModel;
using UnityEngine;

public partial class SROptions
{
// 	// private EResource resource;
// 	[Category("Reward")]
// 	// public EResource Resource
//  //    {
// 	// 	get => resource;
// 	// 	set => resource = value;
//  //    }
//
// 	private int value;
// 	[Category("Reward")]
// 	public int Value
// 	{
// 		get => value;
// 		set => this.value = value;
// 	}
//
//
// 	[Category("Reward")] // Options will be grouped by category
// 	public void GrantReward()
// 	{
// 		Game.RewardHandler.Grant(Resource, Value);
// 	}
//
// 	private string result;
// 	[Category("Painting")]
// 	public string Result
//     {
// 		get => result;
// 		set => result = value;
//     }
// 	[Category("Painting")]
// 	public void  FinishPainting()
// 	{
// 		var item=Service.ServiceHandler.GetService<Service.PlayerDataService>().PlayerSave.galleryData.GetCurrent();
// 		item.TotalPiece++;
// 		Result = item.id + " " + item.TotalPiece;
//         if (item.TotalPiece >= 5)
//         {
// 			var currentItem = Service.ServiceHandler.GetService<Service.DatabaseService>().GalleryTable.GetItem(item.id);
// 			var nextItem = Service.ServiceHandler.GetService<Service.DatabaseService>().GalleryTable.GetNext(currentItem);
//
// 			if (nextItem != null)
// 			{
// 				Service.ServiceHandler.GetService<Service.PlayerDataService>().PlayerSave.galleryData.PushItem(new DataManagement.GalleryItem() { id = nextItem.paintingID });
// 			}
// 			else
// 			{
// 				UI.PanelManager.CreateAsync<QuickNoticePopUp>(UI.Key.QuickNoticePopUp).ContinueWith(panel =>
// 				{
// 					panel.SetUp("No more painting");
// 				}).Forget();
// 			}
// 		}
// 	}
//
//
// }
//
//
//
// public partial class SROptions
// {
//     [Category("Time")]
//     public void Add1Day()
//     {
// 		TimeManager.CheatSecond = 60 * 60 * 24;
//
//         UI.PanelManager.CreateAsync<QuickNoticePopUp>(UI.Key.QuickNoticePopUp).ContinueWith(panel =>
//         {
//             panel.SetUp("TIME  " +TimeManager.Now );
//         }).Forget();
//
//     }
//     [Category("AD")]
//     public void WatchInter()
//     {
// 		UI.PanelManager.CreateAsync<QuickNoticePopUp>(UI.Key.QuickNoticePopUp).ContinueWith(panel => 
// 		{
// 			panel.SetUp("WATCH INTER " + Service.ServiceHandler.GetService<Service.AdService>().IncreaseInterCount());
// 		}).Forget();
//     }
//
//
//     [Category("ABTEST")]
// 	private string levelPack;
// 	[Category("ABTEST")]
// 	public string LevelPack
// 	{
// 		get => levelPack;
// 		set => this.levelPack = value;
// 	}
//
// 	[Category("ABTEST")]
// 	public void SetLevelPack()
//     {
// 		Game.GameConfig.RemoteConfigDataHandler.GetConfig().configs.SetValue(Game.GameConfig.ConfigKey.LevelPack,LevelPack);
//     }
//
//
// 	public static DevelopmentOptions.Options Options=new DevelopmentOptions.Options();
// 	[Category("Game")]
// 	public void Save()
// 	{
// 		Service.ServiceHandler.GetService<Service.PlayerDataService>().Save();
// 	}
//
// 	public void FinishRound()
// 	{
// 		Game.Controller.Instance.gameController.Finish();
// 	}
//     public void FinishLevel()
//     {
// 		var session = Game.Controller.Instance.gameController.Session;
//         var levelEntity = Service.ServiceHandler.GetService<Service.DatabaseService>().GetLevelData(session.GameMode).GetLevel(session.Level);
//
// 		session.SubLevel = levelEntity.GetTotalMap() - 1;
//         Game.Controller.Instance.gameController.Finish();
//     }
//
//     [Category("BlockAD")]
// 	public void BlockAD()
// 	{
// 		Service.ServiceHandler.GetService<Service.AdService>().BlockAd(System.TimeSpan.FromSeconds(100));
// 	}
// 	[Category("Game")]
// 	public void RemoveAd()
// 	{
// 		Service.ServiceHandler.GetService<Service.AdService>().RemoveAd();
// 	}
// 	[Category("Game")]
// 	public void RemoveBackGround()
//     {
// 		Options.backGroundEnabled = !Options.backGroundEnabled;
// 		Logger.Log("GAME " + Options.backGroundEnabled);
// 		Messenger.Broadcast(EventKey.DevelopmentOptionUpdated, Options);
//     }
// 	[Category("Game")]
// 	public void RemoveUI()
// 	{
// 		Options.gameUIEnabled = !Options.gameUIEnabled;
// 		Messenger.Broadcast(EventKey.DevelopmentOptionUpdated, Options);
// 	}
// 	[Category("Game")]
// 	public void UnlockAllHole()
// 	{
// 		Options.unlockAllHole = !Options.unlockAllHole;
// 		Messenger.Broadcast(EventKey.DevelopmentOptionUpdated, Options);
// 	}
// 	[Category("HideBoard")]
// 	public void HideBoard()
// 	{
// 		Options.hideBoard = !Options.hideBoard;
// 		Messenger.Broadcast(EventKey.DevelopmentOptionUpdated, Options);
// 	}
// 	private int testLevel;
// 	[Category("Play Test")]
// 	public int TestLevel
// 	{
// 		get => testLevel;
// 		set => this.testLevel = value;
// 	}
// 	private EGameMode testGameMode;
// 	[Category("Play Test")]
// 	public EGameMode TestGameMode
// 	{
// 		get => testGameMode;
// 		set => this.testGameMode = value;
// 	}
// 	[Category("Play Test")]
//
// 	public void PlayTestLevel()
//     {
// 		if(HomePanel.Instance!=null)
// 			HomePanel.Instance.Close();
// 		Game.Controller.Instance.StartLevel(testGameMode, new LevelSession() { Level = testLevel-1,GameMode=testGameMode }).Forget();
// 	}
//
//
//
// 	private int level;
// 	[Category("Level")]
// 	public int Level
// 	{
// 		get => level;
// 		set => this.level = value;
// 	}
// 	private EGameMode gameMode;
// 	[Category("Level")]
// 	public EGameMode GameMode
// 	{
// 		get => gameMode;
// 		set => this.gameMode = value;
// 	}
// 	[Category("Level")]
// 	public void SetLevel()
// 	{
// 		Service.ServiceHandler.GetService<Service.PlayerDataService>().PlayerSave.playData.GetData(GameMode).level = Level-1;
//     }
//     [Category("Level")]
//     public void CheckModeTier()
//     {
//         int progress = Service.ServiceHandler.GetService<Service.PlayerDataService>().PlayerSave.playData.GetData(GameMode).level;
// 		int[] unlockLevels = GetLevel(new ConfigValue("OneLine_UnlockTier").GetString());
//
// 		Check(progress, EGameMode.ScrewJam, unlockLevels);
// 		Check(progress, EGameMode.TrickyRope, unlockLevels);
// 		Check(progress, EGameMode.OneLine, unlockLevels);
// 		Check(progress, EGameMode.DailyChallenge, unlockLevels);
//     }
//     int[] GetLevel(string levels)
// 	{
// 		Logger.Log(levels);
// 		string[] splits = levels.Split(",");
// 		int[] result = new int[splits.Length];
// 		for (int i = 0; i < result.Length; i++)
// 		{
// 			result[i] = int.Parse(splits[i]);
// 		}
// 		return result;
// 	}
// 	void Check(int progress,EGameMode gameMode,int[] unlockLevels)
//     {
// 		int currentTier = Service.ServiceHandler.GetService<Service.PlayerDataService>().PlayerSave.playData.GetData(gameMode).maxTier;
// 		int currentLevel = Service.ServiceHandler.GetService<Service.PlayerDataService>().PlayerSave.playData.GetData(gameMode).level;
//
//         if (progress >= unlockLevels[currentTier])
//         {
// 			Service.ServiceHandler.GetService<Service.PlayerDataService>().PlayerSave.playData.GetData(gameMode).maxTier++;
// 			Logger.Log("UNLOCK TIER " + gameMode + " " + Service.ServiceHandler.GetService<Service.PlayerDataService>().PlayerSave.playData.GetData(gameMode).maxTier);
// 		}
//
//     }

}