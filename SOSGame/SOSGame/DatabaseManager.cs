using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSGame
{
    public class DatabaseManager : IDatabaseManager
    {
        public DatabaseManager()
        {

        }

        public bool InitializeDatabase()
        {
            return true;
        }


        public List<MoveInfo> GetRecordedMoves()
        {
            throw new NotImplementedException();
        }


        public void RecordMove(MoveInfo moveToAdd)
        {
            throw new NotImplementedException ();
        }


        public void ClearRecordedMoves()
        { 
            throw new NotImplementedException (); 
        }


        public int GetNumberOfRecordedMoves()
        {
            throw new NotImplementedException ();
        }


    }
}
