using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSGame
{
    public class GeneralGameRecording : GeneralGameInstance
    {

        private List<MoveInfo> moves;

        public GeneralGameRecording(GUIHandler GUIRef, GameLogicHandler gameLogicHandler, int recordedSize) : base(GUIRef, gameLogicHandler, recordedSize)
        {

            // make sure the player cannot interfere with the recording
            // while it's playing out
            ToggleUnpressedButtons(false);

            // get a list of all the moves
            this.moves = DatabaseManager.GetRecordedMoves();

            // play back the recorded moves if there are any
            if (this.moves.Count > 0) { HandleRecordedMoves(); }
            else
            {
                GUIRef.GetNewGameButton().Enabled = true;
                GUIRef.GetReplayButton().Enabled = true;
            }
        }

        // add slight delay in between processing each move
        public void HandleRecordedMoves()
        {
            Timer timer = new Timer(500);

            timer.Elapsed += (s, e) =>
            {
                timer.Stop();
                timer.Dispose();

                InternalMoveHandler();
            };

            timer.Start();
        }


        private void InternalMoveHandler()
        {
            // pop the first move from the list of recorded moves
            MoveInfo currentMove = moves[0];
            moves.RemoveAt(0);

            // handle the move
            HandleMove(currentMove);

            // get SOS information and handle it
            SOSInfo sosInfo = this.gameLogicHandler.GetSOSInfo(currentMove);
            HandleSOS(sosInfo);

            // update the GUI turn label
            this.GUIRef.UpdateTurnLabel(this.gameLogicHandler.IsRedTurn());

            // if there are moves left, process the next one
            if (moves.Count > 0) { HandleRecordedMoves(); }

            // otherwise, yield control back to the user
            else
            {
                GUIRef.GetNewGameButton().Enabled = true;
                GUIRef.GetReplayButton().Enabled = true;
            }
        }
    }
}
