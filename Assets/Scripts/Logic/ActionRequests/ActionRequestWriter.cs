using FishNet.Serializing;

namespace Logic.ActionRequests
{
    //DO NOT REMOVE -> SERIALIZER IS USED BY FISH NET THROUGH REFLECTION
    public static class ActionRequestSerializer
    {
        public static void WriteActionRequest(this Writer writer, ActionRequest value)
        {
            writer.WriteInt32((int)value.ActionRequestType);
            switch (value)
            {
                case ActionRequestEndTurn endTurn:
                    writer.Write(endTurn);
                    break;
                case ActionRequestMove move:
                    writer.Write(move);
                    break;
                default:
                    return;
            }
        }

        public static ActionRequest ReadActionRequest(this Reader reader)
        {
            var typeInt = reader.ReadInt32();
            return (EActionRequestType)typeInt switch
            {
                EActionRequestType.EndTurn => reader.Read<ActionRequestEndTurn>(),
                EActionRequestType.Move => reader.Read<ActionRequestMove>(),
                _ => default
            };
        }
    }
}