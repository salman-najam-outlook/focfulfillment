using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    internal class LoggingBackgroundJob : IJob
    {
        private readonly ILogger<LoggingBackgroundJob> _logger;
        private readonly string connectionString = "Server=34.133.133.136; Database=focfulfillment-test; User Id=sa; Password=Techie@1234; Integrated Security=False;Max Pool Size=100;Trust Server Certificate=True";
        public LoggingBackgroundJob(ILogger<LoggingBackgroundJob> logger)
        {
                _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            List<String> Emails = new List<String>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CheckThirtyDaysSubscription", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string email = "No Subscription Expired";
                            if (reader["Email"] != null)
                            {
                                email = reader["Email"].ToString();

                            }
                            //var username = reader["Username"] != null ? reader["Username"].ToString() : "No Subscription is over"; // Assuming "Username" is the column name in your database
                            Emails.Add(email);
                        }
                    }
                }
            }
            Emails.ForEach(email => {
                _logger.LogInformation("{Email} is expired", email);
                _logger.LogInformation("{UtcNow}", DateTime.UtcNow);
            });

            return Task.CompletedTask;
        }
    }
}
