using System;

namespace _Script
{
    /// <inheritdoc />
    /// <summary>
    /// Player state change event handler.
    /// </summary>
    public class PlayerStateChangeEventArgs : EventArgs
    {
        public readonly PlayerState PreviousState;
        public readonly PlayerState NextPlayerState;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="nextPlayerState"></param>
        public PlayerStateChangeEventArgs(PlayerState previousState, PlayerState nextPlayerState)
        {
            PreviousState = previousState;
            NextPlayerState = nextPlayerState;
        }
    }
}