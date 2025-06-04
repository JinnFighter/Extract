using Leopotam.Ecs;

namespace Logic.ActionRequests
{
    public interface IActionRequest
    {
        void AcceptEntity(EcsEntity entity);
    }
}
