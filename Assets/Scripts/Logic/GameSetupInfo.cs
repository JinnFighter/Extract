using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public struct GameSetupInfo
    {
        public List<PlayerSetupInfo> PlayersSetupInfo;
        public List<TileSetupInfo> TilesSetupInfo;
        public List<UnitSetupInfo> UnitsSetupInfo;
    }

    public struct PlayerSetupInfo
    {
        public int Id;
    }

    public struct TileSetupInfo
    {
        public Vector2Int TilePosition;
        public Vector3 WorldPosition;
    }

    public struct UnitSetupInfo
    {
        public int Id;
        public string NameId;
        public int OwnerId;
        public int TeamId;
        public Vector3 SpawnPosition;
    }
}