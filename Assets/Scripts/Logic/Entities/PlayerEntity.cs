using System.Collections.Generic;
using Logic.ActionRequests;

namespace Logic.Entities
{
    public class PlayerEntity : BaseEntity
    {
        public int NetId { get; set; }
        public int OwnerId { get; set; }
        public Dictionary<EActionRequestType, ActionRequestOption> CurrentOptions { get; } = new();
    }
}