using FishNet.Connection;
using Logic.GameStateEvents;
using Channel = FishNet.Transporting.Channel;

namespace Logic.States
{
    public class BattleStateInit : BattleState
    {
        private int _setupCount;
        public override EBattleStateId Id => EBattleStateId.Init;

        protected override void Subscribe()
        {
            Owner.NetworkService.SubscribeClientBroadcast<GameSetupInfo>(HandleBroadcastGameSetupInfo);
            if (Owner.BattleInstance.IsHostStarted)
                Owner.NetworkService.SubscribeServerBroadcast<BroadcastSetupComplete>(
                    HandleBroadcastSetupCompleteReceived);
        }

        protected override void Unsubscribe()
        {
            Owner.NetworkService.UnsubscribeClientBroadcast<GameSetupInfo>(HandleBroadcastGameSetupInfo);
            Owner.NetworkService.UnsubscribeServerBroadcast<BroadcastSetupComplete>(
                HandleBroadcastSetupCompleteReceived);
        }

        protected override void EnterInner()
        {
            Owner.UserDataService.LocalPlayer.SetReady(true);
            if (Owner.BattleInstance.IsHostStarted) Owner.BattleInstance.SetupGameServer();
        }

        protected override void ExitInner()
        {
            _setupCount = 0;
        }

        private void HandleBroadcastGameSetupInfo(GameSetupInfo arg1, Channel arg2)
        {
            SetupGame(arg1);
        }

        private void HandleBroadcastSetupCompleteReceived(NetworkConnection arg1, BroadcastSetupComplete arg2,
            Channel arg3)
        {
            _setupCount++;
            if (_setupCount != Owner.LobbyService.Players.Count) return;

            Owner.BattleInstance.SendGameEvent(new GameStateEventGameStarted
            {
                EventId = 0,
                TurnNumber = 0,
            });
        }

        private void SetupGame(GameSetupInfo gameSetupInfo)
        {
            foreach (var tileSetup in Owner.GameFieldSetup.TileSetups) Owner.BattleInstance.Model.AddTile(tileSetup);

            foreach (var unitSetupInfo in gameSetupInfo.UnitsSetupInfo)
                Owner.BattleInstance.Model.AddUnit(unitSetupInfo);
            Owner.BattleInstance.Model.SetCurrentPlayer(gameSetupInfo.StartingPlayerId);

            Owner.NetworkService.SendClientBroadcast(new BroadcastSetupComplete
            {
                UserId = Owner.UserDataService.LocalPlayer.Id
            });
            Complete();
        }
    }
}