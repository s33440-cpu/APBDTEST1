using Microsoft.Data.SqlClient;
using Test1.DTOs;
using System.Data;

namespace Test1.Repositories
{
    public class MakerRepository : IMakerRepository
    {
        private readonly string _connectionString;

        public MakerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Missing connection string.");
        }

        public async Task<MakerResponse?> GetMaker(int makerId)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            MakerResponse? maker = null;
            var products = new Dictionary<int, ProductResponse>();

            var query = @"SELECT m.Id, m.Name, p.Id, p.Name, p.Description, p.StickerPrice, pt.Id, pt.Name, v.Code, v.Name, vp.Amount, vp.PricePerUnit
                            FROM Makers m
                            LEFT JOIN Products p ON p.MakerId = m.Id
                            LEFT JOIN ProductTypes pt ON pt.Id = p.ProductTypeId
                            LEFT JOIN VendorProducts vp ON vp.ProductId = p.Id
                            LEFT JOIN Vendors v ON v.Code = vp.VendorCode
                            WHERE m.Id = @id
                            ORDER BY p.Id";

            await using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", makerId);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
                return null;

            while (await reader.ReadAsync())
            {
                if (maker == null)
                {
                    maker = new MakerResponse
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Products = new List<ProductResponse>()
                    };
                }

                if (reader.IsDBNull(2)) continue;

                int productId = reader.GetInt32(2);

                if (!products.TryGetValue(productId, out var product))
                {
                    product = new ProductResponse
                    {
                        Id = productId,
                        Name = reader.GetString(3),
                        Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                        StickerPrice = reader.GetDecimal(5),
                        ProductType = new ProductType
                        {
                            Id = reader.GetInt32(6),
                            Name = reader.GetString(7)
                        },
                        Vendors = new List<VendorResponse>()
                    };

                    products.Add(productId, product);
                    maker.Products.Add(product);
                }

                if (!reader.IsDBNull(8))
                {
                    product.Vendors.Add(new VendorResponse
                    {
                        Code = reader.GetString(8),
                        Name = reader.GetString(9),
                        Amount = reader.GetInt32(10),
                        PricePerUnit = reader.GetDecimal(11)
                    });
                }
            }

            return maker;
        }

        public async Task<int> CreateMaker(CreateMakerRequest request)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var makerCmd = new SqlCommand(
                    "INSERT INTO Makers(Name) OUTPUT INSERTED.Id VALUES(@name)",
                    connection, (SqlTransaction)transaction);

                makerCmd.Parameters.AddWithValue("@name", request.Name);

                int makerId = (int)await makerCmd.ExecuteScalarAsync();

                if (request.Products != null)
                {
                    foreach (var p in request.Products)
                    {
                        var typeCmd = new SqlCommand(@"IF NOT EXISTS (SELECT 1 FROM ProductTypes WHERE Name = @name)
                                                        INSERT INTO ProductTypes(Name) VALUES(@name);
                                                        SELECT Id FROM ProductTypes WHERE Name = @name;", connection, (SqlTransaction)transaction);
                        typeCmd.Parameters.AddWithValue("@name", p.Type);
                        int typeId = (int)await typeCmd.ExecuteScalarAsync();

                        var prodCmd = new SqlCommand(@"INSERT INTO Products(Name, Description, StickerPrice, ProductTypeId, MakerId) VALUES(@n, @d, @p, @t, @m)",
                            connection, (SqlTransaction)transaction);

                        prodCmd.Parameters.AddWithValue("@n", p.Name);
                        prodCmd.Parameters.AddWithValue("@d", (object?)p.Description ?? DBNull.Value);
                        prodCmd.Parameters.AddWithValue("@p", p.StrickerPrice);
                        prodCmd.Parameters.AddWithValue("@t", typeId);
                        prodCmd.Parameters.AddWithValue("@m", makerId);

                        await prodCmd.ExecuteNonQueryAsync();
                    }
                }

                await transaction.CommitAsync();
                return makerId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}