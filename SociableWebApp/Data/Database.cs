using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace SociableWebApp.Data
{
    public class Database
    {
        public static async Task CreateDatabaseAsync(IAmazonDynamoDB client)
        {
            string tableName = "users";

            try
            {
                var response = await client.CreateTableAsync(new CreateTableRequest
                {
                    TableName = tableName,
                    AttributeDefinitions = new List<AttributeDefinition>()
                              {
                                  new AttributeDefinition
                                  {
                                      AttributeName = "email",
                                      AttributeType = "S"
                                  },
                              },
                    KeySchema = new List<KeySchemaElement>()
                              {
                                  new KeySchemaElement
                                  {
                                      AttributeName = "email",
                                      KeyType = "HASH"
                                  },
                              },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                });

                var tableDescription = response.TableDescription;

                string status = tableDescription.TableStatus;

                // Wait until table is created.
                while (status != "ACTIVE")
                {
                    Thread.Sleep(1000);
                    try
                    {
                        var res = await client.DescribeTableAsync(new DescribeTableRequest
                        {
                            TableName = tableName
                        });

                        status = res.Table.TableStatus;
                    }
                    // Try-catch to handle potential eventual-consistency issue.
                    catch (ResourceNotFoundException)
                    { }
                }
            }
            // Try-catch to handle table name already existing.
            catch (ResourceInUseException)
            {
                return;
            }
            //await Seed.SeedDate(client, clientS3);
        }
    }
}
