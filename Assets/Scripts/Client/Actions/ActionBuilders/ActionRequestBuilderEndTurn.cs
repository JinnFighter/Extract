using Logic.ActionRequests;

namespace Client.Actions.ActionBuilders
{
    public class ActionRequestBuilderEndTurn : BaseActionRequestBuilder
    {
        public override void Reset()
        {
            CasterId = -1;
        }

        protected override bool ValidateInner()
        {
            return CasterId >= 0;
        }

        protected override ActionRequest BuildActionInner()
        {
            return new ActionRequestEndTurn
            {
                CasterId = CasterId
            };
        }
    }
}