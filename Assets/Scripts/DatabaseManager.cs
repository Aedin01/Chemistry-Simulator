using System;
using System.Data.SQLite;

public static class DatabaseManager
{
    private static string _connectionString;

    // Set the connection string once for all static methods
    public static void Initialize(string databaseFilePath)
    {
        _connectionString = $"Data Source={databaseFilePath};Version=3;";
    }

    private static SQLiteConnection CreateConnection()
    {
        return new SQLiteConnection(_connectionString);
    }

    public static string CheckReaction(string reactant1, string reactant2, float temperature)
    {
        using (var connection = CreateConnection())
        {
            connection.Open();

            string query = @"
                SELECT Product 
                FROM ChemicalReactionFormat 
                WHERE ((Reactant1 = @Reactant1 AND Reactant2 = @Reactant2) 
                    OR (Reactant1 = @Reactant2 AND Reactant2 = @Reactant1)) 
                    AND TemperatureRequired <= @Temperature
                LIMIT 1";

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Reactant1", reactant1);
                command.Parameters.AddWithValue("@Reactant2", reactant2);
                command.Parameters.AddWithValue("@Temperature", temperature);

                return command.ExecuteScalar() as string;
            }
        }
    }

    public static float GetMeltingPoint(ChemicalInformation.ChemicalType chemical)
    {
        using (var connection = CreateConnection())
        {
            connection.Open();

            string query = "SELECT MeltingPoint FROM ChemicalFormat WHERE Chemical = @Chemical LIMIT 1";

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Chemical", Convert.ToString(chemical));

                var result = command.ExecuteScalar();
                return result != null ? Convert.ToSingle(result) : float.NaN;
            }
        }
    }
}