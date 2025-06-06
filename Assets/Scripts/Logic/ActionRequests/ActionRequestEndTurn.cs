using Leopotam.Ecs;

namespace Logic.ActionRequests
{
    public struct ActionRequestEndTurn : IActionRequest
    {
        public int CasterId;

        public void AcceptEntity(EcsEntity entity)
        {
            entity.Replace(this);
        }
    }
}