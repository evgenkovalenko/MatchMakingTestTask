using MatchMaking.Common.App;
using MatchMaking.Common.Types;
using MatchMaking.Shared.DataContracts;
using MatchMaking.Worker.CustomServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging();
builder.Logging.AddConsole();

builder.Services.ApplicationDefaultRegistrations(builder.Configuration);

builder.Services.AddHostedService<PrepareMatchResultsWorker>();

builder.Services.AddTransient<BaseService<MatchSearchRequest, BaseResponse>, PrepareMatchService>();
builder.Services.AddTransient<BaseService<MatchCompleteRequest, MatchCompleteResponse>, CompleteMatchCreateService>();

var app = builder.Build();

app.WarmUp(app.Services);

app.Run();
