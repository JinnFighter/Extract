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
                case ActionEventGameStarted gameStarted:
                    writer.Write(gameStarted);
                    break;
                case ActionEventFullEntity fullEntity:
                    writer.Write(fullEntity);
                    break;
                case ActionEventPositionChanged positionChanged:
                    writer.Write(positionChanged);
                    break;
                case ActionEventPlayerChanged playerChanged:
                    writer.Write(playerChanged);
                    break;
                case ActionEventGameEnded gameEnded:
                    writer.Write(gameEnded);
                    break;
            }
        }
        public static ActionEvent ReadActionEvent(this Reader reader)
        {
            var id = reader.ReadInt32();
            switch ((EActionEventType)id)
            {
                case EActionEventType.GameStart:
                    return reader.Read<ActionEventGameStarted>();
                case EActionEventType.FullEntity:
                    return reader.Read<ActionEventFullEntity>();
                case EActionEventType.PositionChanged:
                    return reader.Read<ActionEventPositionChanged>();
                case EActionEventType.PlayerTurn:
                    return reader.Read<ActionEventPlayerChanged>();
                case EActionEventType.GameEnd:
                    return reader.Read<ActionEventGameEnded>();
                default:
                    return null;
            }
        }
    }
}