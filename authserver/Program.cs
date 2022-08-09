using authserver;
using SkyApm.Utilities.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var identityServerBuilder = builder.Services.AddIdentityServer(options =>
{
    options.EmitStaticAudienceClaim = true;

    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
})
    .AddTestUsers(Config.TestUsers)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients);

identityServerBuilder.AddDeveloperSigningCredential();

// ������֤�û���������
identityServerBuilder.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();
// �������÷��ظ��ͻ��˵��û�������Ϣ
identityServerBuilder.AddProfileService<CustomProfileService>();

builder.Services.AddSkyApmExtensions();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIdentityServer();

app.Run();
