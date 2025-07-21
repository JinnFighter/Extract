using FishNet.Serializing;

namespace Logic.ActionEvents
{
    //DO NOT REMOVE -> SERIALIZER IS USED BY FISH NET THROUGH REFLECTION
    public static class ActionEventSerializer
    {
        public static void WriteActionEvent(this Writer writer, ActionEvent actionEvent)
        {
            writer.Write((int)actionEvent.EventType);
            switch (actionEvent)
            {
                case ActionEventInitStarted initStarted:
                    writer.Write(initStarted);
                    break;
                case ActionEventGameStart gameStart:
                    writer.Write(gameStart);
                    break;
                case ActionEventFullEntity fullEntity:
                    writer.Write(fullEntity);
                    break;
                case ActionEventSequenceStart sequenceStart:
                    writer.Write(sequenceStart);
                    break;
                case ActionEventSequenceEnd sequenceEnd:
                    writer.Write(sequenceEnd);
                    break;
                case ActionEventPositionChanged positionChanged:
                    writer.Write(positionChanged);
                    break;
                case ActionEventPlayerChanged playerChanged:
                    writer.Write(playerChanged);
                    break;
                case ActionEventPropertyUpdate propertyUpdate:
                    writer.Write(propertyUpdate);
                    break;
                case ActionEventGameEnded gameEnded:
                    writer.Write(gameEnded);
                    break;
            }
        }
        public static ActionEvent ReadActionEvent(this Reader reader)
        {
            var id = reader.ReadInt32();
            return (EActionEventType)id switch
            {
                EActionEventType.InitStart => reader.Read<ActionEventInitStarted>(),
                EActionEventType.GameStart => reader.Read<ActionEventGameStart>(),
                EActionEventType.FullEntity => reader.Read<ActionEventFullEntity>(),
                EActionEventType.SequenceStart => reader.Read<ActionEventSequenceStart>(),
                EActionEventType.SequenceEnd => reader.Read<ActionEventSequenceEnd>(),
                EActionEventType.PositionChanged => reader.Read<ActionEventPositionChanged>(),
                EActionEventType.PlayerTurn => reader.Read<ActionEventPlayerChanged>(),
                EActionEventType.PropertyUpdate => reader.Read<ActionEventPropertyUpdate>(),
                EActionEventType.GameEnd => reader.Read<ActionEventGameEnded>(),
                _ => null
            };
        }
    }
}