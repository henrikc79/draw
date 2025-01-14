﻿using NLog;
using System.Threading.Tasks;
using System.Timers;

namespace Draw.Server.Game.Rooms
{
    internal class RoomStateWordChoice : IRoomState
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Player activePlayer;
        private Room room;
        private RoomStatePlayerTurn roomStatePlayerTurn;
        private GameTimer wordChoiceTimer;

        private Word word1;
        private Word word2;
        private Word word3;

        private bool wordChoiceDone = false;

        public RoomStateWordChoice(Player player, Room room, RoomStatePlayerTurn roomStatePlayerTurn)
        {
            this.activePlayer = player;
            this.room = room;
            this.roomStatePlayerTurn = roomStatePlayerTurn;
        }

        public async Task Enter()
        {
            int timeout = 12;
            (word1, word2, word3) = room.GetNext3Words();
            await room.SendPlayer(activePlayer, "ActivePlayerWordChoice", word1.ToWordDTO(), word2.ToWordDTO(), word3.ToWordDTO(), timeout);
            await room.SendAllExcept(activePlayer, "PlayerWordChoice", activePlayer.ToPlayerDTO(), timeout);
            wordChoiceTimer = new GameTimer(timeout * 1000, WordChoiceTimerElapsed);
            wordChoiceTimer.Start();
        }

        public Task AddPlayer(Player player)
        {
            int timeRemaining = (int)(wordChoiceTimer.TimeRemaining / 1000);
            return room.SendPlayer(player, "PlayerWordChoice", activePlayer.ToPlayerDTO(), timeRemaining);
        }

        public Task RemovePlayer(Player player)
        {
            if (player.Equals(activePlayer))
            {
                room.RoomState = roomStatePlayerTurn;
            }
            return Task.CompletedTask;
        }

        private void WordChoiceTimerElapsed(object sender, ElapsedEventArgs e)
        {
            WordChosen(1, activePlayer);
        }

        internal void WordChosen(int wordIndex, Player player)
        {
            lock (this)
            {
                // Guard against user choosing word exactly when timer runs out.
                if (wordChoiceDone)
                {
                    return;
                }
                wordChoiceDone = true;
                wordChoiceTimer.Dispose();

                if (this.activePlayer.Equals(player))
                {
                    Word word;
                    switch (wordIndex)
                    {
                        case 1:
                            word = word1;
                            room.AddRejectedWord(word2);
                            room.AddRejectedWord(word3);
                            break;
                        case 2:
                            word = word2;
                            room.AddRejectedWord(word1);
                            room.AddRejectedWord(word3);
                            break;
                        case 3:
                        default:
                            word = word3;
                            room.AddRejectedWord(word1);
                            room.AddRejectedWord(word2);
                            break;
                    };

                    room.RoomState = new RoomStateDrawing(player, room, word, roomStatePlayerTurn);
                }
                else
                {
                    logger.Warn("Someone else (" + player.Name + ") than active player (" + player.Name + ") tried to choose a word.");
                }
            }
        }
    }
}