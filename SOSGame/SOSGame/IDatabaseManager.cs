using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSGame
{
    public interface IDatabaseManager
    {
        public bool InitializeDatabase();           // self explanatory ; returns true if successful, false otherwise
        public void RecordMove(MoveInfo moveToAdd); // writes instance data of moveToAdd to a tuple in database table
        public List<MoveInfo> GetRecordedMoves();   // returns the contents of  table as a list of MoveInfo objects
        public void ClearRecordedMoves();           // erases all tuples in the database table
        public int GetNumberOfRecordedMoves();      // returns the number of tuples in the database table

    }
}
