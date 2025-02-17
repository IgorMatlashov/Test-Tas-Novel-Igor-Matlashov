using Naninovel;
using UnityEngine;

public class CustomCommands
{
    [CommandAlias("startMiniGame")]
    public class StartMiniGame : Command
    {
        public override async UniTask ExecuteAsync (AsyncToken asyncToken = default)
        {
            var gameManager = Object.FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.StartGame();
            
                await UniTask.WaitUntil(() => gameManager.IsGameOver());
            }
            await UniTask.CompletedTask;
        }
    }
}
