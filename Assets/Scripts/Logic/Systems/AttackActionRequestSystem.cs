using System.Collections.Generic;
using Logic.ActionEvents;
using Logic.ActionRequests;

namespace Logic.Systems
{
    public class AttackActionRequestSystem : BaseLogicSystem
    {
        protected override IEnumerator<ActionEvent> RunPrepareLogicInner(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            yield return new ActionEventSequenceStart
            {
                SequenceType = EActionSequenceType.Attack
            };
        }

        protected override IEnumerator<ActionEvent> RunLogicInner(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            var attackRequest = rootRequest as ActionRequestAttack;
            var target = modelServer.UnitEntities[attackRequest.TargetId];
            target.Properties[EPropertyType.Health] -= 5;
            yield return new ActionEventPropertyUpdate
            {
                EntityType = EEntityType.Unit,
                EntityId = target.Id,
                PropertyType = EPropertyType.Health,
                Value = target.Properties[EPropertyType.Health]
            };
        }

        protected override IEnumerator<ActionEvent> RunCancelLogicInner(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            yield return new ActionEventSequenceEnd()
            {
                SequenceType = EActionSequenceType.Attack
            };
        }

        protected override IEnumerator<ActionEvent> RunFinishLogicInner(ActionRequest rootRequest, LogicModelServer modelServer)
        {
            yield return new ActionEventSequenceEnd()
            {
                SequenceType = EActionSequenceType.Attack
            };
        }
    }
}