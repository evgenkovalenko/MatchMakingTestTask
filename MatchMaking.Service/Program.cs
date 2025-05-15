using MatchMaking.Common.App;
using MatchMaking.Common.Types;
using MatchMaking.Service;
using MatchMaking.Service.CustomServices;
using MatchMaking.Shared.DataContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging();

builder.Logging.AddConsole();

builder.Services.AddControllers();

builder.Services.ApplicationDefaultRegistrations(builder.Configuration);

builder.Services.AddHostedService<CompleteMatchesConsumeWorker>();

builder.Services.AddTransient<BaseService<MatchSearchRequest, MatchSearchResponse>, MatchSearchService>();
builder.Services.AddTransient<BaseService<MatchRetreiveRequest, MatchCompleteResponse>, MatchRetreiveService>();
builder.Services.AddTransient<BaseService<MatchCompleteRequest, MatchCompleteResponse>, CompleteMatchStoreService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.WarmUp(app.Services);

app.Run();
