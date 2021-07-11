using System;
using System.Collections.Generic;
using System.Drawing;

namespace WalmartEngine
{
    class StateManager
    {
        private const int NONE = -1;

        private Dictionary<int, GameState> gameStates;

        private int currentStateID = NONE;
        private int pendingStateID = NONE;

        private bool stateActive = false;

        private bool StateExists(int stateID)
        {
            return gameStates.ContainsKey(stateID);
        }

        public StateManager()
        {
            gameStates = new Dictionary<int, GameState>();
        }

        public void ChangeState(int stateID)
        {
            if (StateExists(stateID))
            {
                if (!stateActive)
                {
                    pendingStateID = stateID;
                    ExecuteStateChange();
                }
                else
                {
                    pendingStateID = stateID;
                }
            }
            else
            {
                throw new ArgumentException($"GameState [{stateID}] doesn't exist");
            }
        }

        private bool StateCanUpdate()
        {
            return currentStateID != NONE && gameStates[currentStateID].Update != null;
        }

        private bool StateCanDraw()
        {
            return currentStateID != NONE && gameStates[currentStateID].Draw != null;
        }

        public void UpdateCurrentState(GameContainer gc, float deltaTime)
        {
            if (StateCanUpdate())
            {
                stateActive = true;
                gameStates[currentStateID].Update(gc, deltaTime);
            }
        }

        private void ExecuteStateChange()
        {
            currentStateID = pendingStateID;
            pendingStateID = NONE;

            Log.Info(this, $"Changed state to [{currentStateID}]");
        }

        public void AddState(int id, Action<GameContainer, float> update, Action<Graphics> draw)
        {
            try
            {
                gameStates.Add(id, new GameState(update, draw));
            }
            catch (ArgumentException)
            {
                Console.WriteLine("GameState ID already exists.");
            }
        }

        public void DrawCurrentState(Graphics gfx)
        {
            if (StateCanDraw())
            {
                gameStates[currentStateID].Draw(gfx);
                stateActive = false;
            }

            if (pendingStateID != NONE)
            {
                ExecuteStateChange();
            }
        }
    }
}