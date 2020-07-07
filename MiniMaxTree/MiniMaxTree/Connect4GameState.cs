using NeuralNetworkLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMaxTree
{
    enum Connect4Color
    {
        None,
        Red,
        Yellow
    }

    class Connect4GameState : IMinMaxGameState
    {
        public int Value { get; set; }
        public List<IMinMaxGameState> Children { get; set;}

        private Connect4Color[,] gameState;
        
        public Connect4GameState(Connect4Color[,] gameState)
        {
            this.gameState = gameState;
        }
        public void GenerateChildren()
        {
            for(int x = 0; x < gameState.GetLength(0); x++)
            {
                for(int y = 0; y < gameState.GetLength(1); y++)
                {
                    Connect4Color color = gameState[x, y];
                    if(color == Connect4Color.None)
                    {
                        Connect4Color[,] newGameState = new Connect4Color[gameState.GetLength(0), gameState.GetLength(1)];
                        gameState.CopyTo(newGameState, 0);
                    }
                }
            }
        }
    }
}
