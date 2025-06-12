using Logic.States;

namespace Client.States
{
    public class NullBattleState : BattleState
    {
        public override EBattleStateId Id => EBattleStateId.None;
    }
}