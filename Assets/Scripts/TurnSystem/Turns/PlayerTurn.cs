using PlayerCore;
using TurnSystem.Turns.Base;

namespace TurnSystem.Turns
{
    public class PlayerTurn : Turn
    {
        protected override void OnStart(Player currentPlayer)
        {
            //Debug.Log("Start Player Turn");
        }

        protected override void OnUpdate(Player currentPlayer)
        {
            //Debug.Log("Update Player Turn");
        }

        protected override void OnEnd(Player currentPlayer)
        {
            //Debug.Log("End Player Turn");
        }
    }
}
