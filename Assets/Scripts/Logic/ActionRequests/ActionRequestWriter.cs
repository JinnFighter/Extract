using System;
using System.Collections.Generic;
using FishNet.Serializing;
using UnityEngine;

namespace Logic.ActionRequests
{
    //DO NOT REMOVE -> SERIALIZER IS USED BY FISH NET THROUGH REFLECTION
    public static class ActionRequestWriter
    {
        private static readonly Dictionary<Type, (Action<Writer, IActionRequest> writeAction,
            Action<Reader, IActionRequest>
            readAction)> Actions = new()
        {
            { typeof(ActionRequestEndTurn), (WriteEndTurnSpecificData, ReadEndTurnSpecificData) },
        };

        public static void WriteActionRequest(this Writer writer, IActionRequest value)
        {
            writer.WriteString(value.GetType().ToString());

            if (Actions.TryGetValue(value.GetType(), out var actions))
            {
                actions.writeAction.Invoke(writer, value);
            }
        }

        public static IActionRequest ReadActionRequest(this Reader reader)
        {
            var typeString = reader.ReadStringAllocated();
            var type = Type.GetType(typeString);
            var action = (IActionRequest)Activator.CreateInstance(type);

            if (Actions.TryGetValue(type, out var actions))
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
