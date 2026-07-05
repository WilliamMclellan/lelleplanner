using System;
using System.IO;
using System.Text.Json;

namespace Lelleplanner.Core
{
    public static class Persistence
    {
        public static GameState LoadOrCreate()
        {
            string filename = "gamestate.json";
            string path = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Lelleplanner", filename );
            string jsonString = "";

            if ( File.Exists( path ) )
            {
                jsonString = File.ReadAllText( path );
            }

            if ( !string.IsNullOrEmpty( jsonString ) )
            {
                GameState loadedState = JsonSerializer.Deserialize<GameState>( jsonString )!;
                loadedState.RolloverIfNeeded();
                loadedState.WeeklyRolloverIfNeeded();
                return loadedState;
            }

            GameState createdState = new GameState();
            createdState.RolloverIfNeeded();
            createdState.WeeklyRolloverIfNeeded();
            return createdState;
        }
        
        public static void Save(GameState gameState)
        {
            string saveState = JsonSerializer.Serialize<GameState>( gameState );
            string fileName = "gamestate.json";
            string folderPath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Lelleplanner" );
            string filePath = Path.Combine( folderPath, fileName );
            if ( !Directory.Exists( folderPath ) )
            {
                Directory.CreateDirectory( folderPath );
            }

            File.WriteAllText(filePath, saveState);
        }
}
}
