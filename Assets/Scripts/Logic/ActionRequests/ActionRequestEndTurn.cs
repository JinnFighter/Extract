using Leopotam.Ecs;

namespace Logic.ActionRequests
{
    public struct ActionRequestEndTurn : IActionRequest
    {
        public int CasterId { get; set; }

        public void AcceptEntity(EcsEntity entity)
        {
            entity.Replace(this);
        }
    }
}