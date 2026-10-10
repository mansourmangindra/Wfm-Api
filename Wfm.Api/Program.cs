using DataAccess.DBContexts.Wfm;
using DataAccess.Repositories.Base;
using DataAccess.UnitOfWorks._Base;
using DataAccess.UnitOfWorks.Wfm;
using Microsoft.EntityFrameworkCore;
using Wfm.Api.Service;
using Wfm.Api.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<WfmDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWfmUnitOfWork, WfmUnitOfWork>();

builder.Services.AddScoped<IScheduleService, ScheduleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
