#r "System.Configuration"
#r "System.Data"

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

public static async Task Run(TimerInfo CleanupTimer, TraceWriter log)
{
    var constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    using(var sqlConn = new SqlConnection(constr))
    {
        sqlConn.Open();
        var args = "DELETE FROM VerificationTokens WHERE Expires < GETDATE()";

        using(var cmd = new SqlCommand(args, sqlConn))
        {
            var result = await cmd.ExecuteNonQueryAsync();
            log.Verbose($"Deleted {result} expired tokens.");
        }
    }
}