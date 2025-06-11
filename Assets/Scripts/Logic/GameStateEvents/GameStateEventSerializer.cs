using FishNet.Serializing;

namespace Logic.GameStateEvents
{
    //DO NOT REMOVE -> SERIALIZER IS USED BY FISH NET THROUGH REFLECTION
    public static class GameStateEventSerializer
    {
        public static void WriteGameStateEvent(this Writer writer, GameStateEvent gameStateEvent)
        {
            writer.Write((int)gameStateEvent.GetEventType());
            switch (gameStateEvent)
            {
                case GameStateEventGameStarted gameStarted:
                    writer.Write(gameStarted);
                    break;
                case GameStateEventFullEntity fullEntity:
                    writer.Write(fullEntity);
                    break;
                case GameStateEventPlayerChanged playerChanged:
                    writer.Write(playerChanged);
                    break;
                case GameStateEventGameEnded gameEnded:
                    writer.Write(gameEnded);
                    break;
            }
        }
        public static GameStateEvent ReadGameStateEvent(this Reader reader)
        {
            var id = reader.ReadInt32();
            switch ((EGameStateEventType)id)
            {
                case EGameStateEventType.GameStart:
                    return reader.Read<GameStateEventGameStarted>();
                case EGameStateEventType.FullEntity:
                    return reader.Read<GameStateEventFullEntity>();
                case EGameStateEventType.PlayerTurn:
                    return reader.Read<GameStateEventPlayerChanged>();
                case EGameStateEventType.GameEnd:
                    return reader.Read<GameStateEventGameEnded>();
                default:
                    return null;
            }
        }
    }
}