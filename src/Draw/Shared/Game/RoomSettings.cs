﻿
namespace Draw.Shared.Game
{
    public class RoomSettings
    {
        public RoomSettings() { }
        public RoomSettings(Language language, 
                            int rounds, 
                            int drawingTime, 
                            int minWordDifficulty, 
                            int maxWordDifficulty, 
                            bool isPrivateRoom,
                            string password)
        {
            Language = language;
            Rounds = rounds;
            DrawingTime = drawingTime;
            MinWordDifficulty = minWordDifficulty;
            MaxWordDifficulty = maxWordDifficulty;
            IsPrivateRoom = isPrivateRoom;
            Password = password;
        }

        public RoomSettings(RoomSettings settings)
        {
            if (settings != null)
            {
                Language = settings.Language;
                Rounds = settings.Rounds;
                DrawingTime = settings.DrawingTime;
                MinWordDifficulty = settings.MinWordDifficulty;
                MaxWordDifficulty = settings.MaxWordDifficulty;
                IsPrivateRoom = settings.IsPrivateRoom;
                Password = settings.Password;
            }
        }

        public Language Language { get; set; } = Language.English;
        public int Rounds { get; set; } = 3;
        public int DrawingTime { get; set; } = 90;
        public WordDifficulty MinWordDifficulty { get; set; } = 1;
        public WordDifficulty MaxWordDifficulty { get; set; } = 6;
        public int WordDifficultyDelta => MaxWordDifficulty - MinWordDifficulty;
        public bool IsPrivateRoom { get; set; } = false;
        public string Password { get; set; } = "";
    }
}
