using System;
using System.Collections.Generic;
using FishNet.Serializing;

namespace Logic.ActionRequests
{
    //DO NOT REMOVE -> SERIALIZER IS USED BY FISH NET THROUGH REFLECTION
    public static class IActionRequestWriter
    {
        private static readonly Dictionary<Type, (Action<Writer, IActionRequest> writeAction,
            Action<Reader, IActionRequest>
            readAction)> Actions = new()
        {
            { typeof(ActionRequestEndTurn), (WriteEndTurnSpecificData, ReadEndTurnSpecificData) },
        };

        public static void WriteIActionRequest(this Writer writer, IActionRequest value)
        {
            switch (value)
            {
                case ActionRequestEndTurn:
                    writer.WriteUInt8Unpacked(1);
                    break;
                default:
                    return;
            }

            if (Actions.TryGetValue(value.GetType(), out var actions))
            {
                actions.writeAction.Invoke(writer, value);
            }
        }

        public static IActionRequest ReadIActionRequest(this Reader reader)
        {
            var typeByte = reader.ReadUInt8Unpacked();
            IActionRequest action;
            switch (typeByte)
            {
                case 1:
                    action = reader.Read<ActionRequestEndTurn>();
                    break;
                default:
                    return default;
            }

            if (Actions.TryGetValue(action.GetType(), out var actions))
            {
                actions.readAction.Invoke(reader, action);
            }

            return action;
        }

        private static void ReadEndTurnSpecificData(Reader reader, IActionRequest actionRequest)
        {
            var actionRequestEndTurn = (ActionRequestEndTurn)actionRequest;
            actionRequestEndTurn.CasterId = reader.ReadInt32();
        }

        private static void WriteEndTurnSpecificData(Writer writer, IActionRequest actionRequest)
        {
            var actionRequestEndTurn = (ActionRequestEndTurn)actionRequest;
            writer.WriteInt32(actionRequestEndTurn.CasterId);
        }
    }
}
