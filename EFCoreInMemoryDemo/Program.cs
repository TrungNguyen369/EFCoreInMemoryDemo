using EFCoreInMemoryDemo.DatabaseContext;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Cấu hình Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        },
        columnOptions: GetSqlColumnOptions())
    .CreateLogger();

// Thêm Serilog vào Logging
builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyDatabaseContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

ColumnOptions GetSqlColumnOptions()
{
    var columnOptions = new ColumnOptions();

    // Tạo thêm các cột tùy chỉnh nếu cần thiết
    columnOptions.Store.Remove(StandardColumn.Properties);
    columnOptions.Store.Add(StandardColumn.LogEvent);

    columnOptions.AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn { ColumnName = "UserName", DataType = SqlDbType.NVarChar, DataLength = 128 },
        new SqlColumn { ColumnName = "Environment", DataType = SqlDbType.NVarChar, DataLength = 128 }
    };

    return columnOptions;
}