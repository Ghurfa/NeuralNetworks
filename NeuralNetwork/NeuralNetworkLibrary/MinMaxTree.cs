using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetworkLibrary
{
    public interface IMinMaxGameState
    {
        int Value { get; set; }
        List<IMinMaxGameState> Children { get; set; }
        void GenerateChildren();
    }

    public static class MinMaxFuncs
    {
        public static void GenerateTree(IMinMaxGameState startState)
        {
            Queue<IMinMaxGameState> states = new Queue<IMinMaxGameState>();
            states.Enqueue(startState);
            while(states.Count > 0)
            {
                IMinMaxGameState state = states.Dequeue();
                state.GenerateChildren();
                foreach(IMinMaxGameState child in state.Children)
                {
                    states.Enqueue(child);
                }
            }
        }
        public static int MinMax(bool isMax, IMinMaxGameState gameState)
        {
            if(gameState.Children.Count == 0)
            {
                return gameState.Value;
            }

            if(isMax)
            {
                int maxVal = int.MinValue;
                foreach (IMinMaxGameState child in gameState.Children)
                {
                    int childVal = MinMax(false, child);
                    maxVal = maxVal > childVal ? maxVal : childVal;
                }
                return maxVal;
            }
            else
            {
                int minVal = int.MaxValue;
                foreach (IMinMaxGameState child in gameState.Children)
                {
                    int childVal = MinMax(true, child);
                    minVal = minVal < childVal ? minVal : childVal;
                }
                return minVal;
            }
        }
    }
}
