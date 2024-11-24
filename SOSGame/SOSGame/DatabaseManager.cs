using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SQLite;

namespace SOSGame
{
    public static class DatabaseManager
    {
        // variables

        // hardcoding DB name may be bad practice, but it shouldn't change
        private static readonly string _connectionString = "Data Source=RecordedMoves.db;Version=3;";


        // methods

        public static bool InitializeDatabase()
        {
            try
            {
                // open a connection to the DB
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    // query to create a table to store moves
                    String tableQuery = @"
                        CREATE TABLE IF NOT EXISTS RecordedMoves (
                            RowIndex INTEGER,
                            ColumnIndex INTEGER,
                            MoveLetter CHAR(1),
                            PRIMARY KEY (RowIndex, ColumnIndex)
                        );";

                    // execute the query
                    using (SQLiteCommand command = new SQLiteCommand(tableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                // if the DB initialized correctly, return true
                return true;
            }

            catch (Exception ex)
            {
                // if there was an error initializing DB
                Console.WriteLine($"Error initializing DB ; {ex.StackTrace}");
                return false;
            }
        }


        public static List<MoveInfo> GetRecordedMoves()
        {
            List<MoveInfo> recordedMoves = new List<MoveInfo>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection( _connectionString))
                {
                    connection.Open();

                    // create SELECT query
                    String query = @"
                        SELECT RowIndex, ColumnIndex, MoveLetter
                        FROM RecordedMoves;";


                    // execute and interate through the rows returned by the query
                    // to create and add MoveInfo objects to the return list
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MoveInfo moveToAdd = new MoveInfo(
                                rowIndex: reader.GetInt32(0),
                                columnIndex: reader.GetInt32(1),
                                moveLetter: reader.GetString(2)
                                );
                           
                            recordedMoves.Add(moveToAdd);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                // if there was an error querying the recorded moves
                Console.WriteLine($"Error querying recorded moves ; {ex.StackTrace}");
            }

            return recordedMoves;
        }


        public static bool RecordMove(MoveInfo moveToAdd)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    // create INSERT query
                    String query = @"
                        INSERT INTO RecordedMoves (RowIndex, ColumnIndex, MoveLetter)
                        VALUES (@RowIndex, @ColumnIndex, @MoveLetter);";

                    // execute query
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RowIndex", moveToAdd.RowIndex);
                        command.Parameters.AddWithValue("@ColumnIndex", moveToAdd.ColumnIndex);
                        command.Parameters.AddWithValue("@MoveLetter", moveToAdd.MoveLetter);

                        command.ExecuteNonQuery();
                    }
                }

                // if insertion successful, return true
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting move into table {ex.StackTrace}");
                return false;
            }
        }


        public static bool ClearRecordedMoves()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection( _connectionString))
                {
                    connection.Open();

                    String query = @"
                        DELETE FROM RecordedMoves;";


                    using (SQLiteCommand command = new SQLiteCommand(query, connection)) { command.ExecuteNonQuery(); }
                }

                // return true if successful
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing table {ex.StackTrace}");
                return false;
            }
        }


        public static int GetNumberOfRecordedMoves()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    String query = @"
                    SELECT COUNT(*)
                    FROM RecordedMoves;";

                    // return the number of rows
                    using (SQLiteCommand command = new SQLiteCommand(query, connection)) { return Convert.ToInt32(command.ExecuteScalar()); }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error retriving number of tuples {ex.StackTrace}");
                return -1;
            }
            
        }


    }
}
