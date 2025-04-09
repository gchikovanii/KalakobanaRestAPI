using Kalakoana.Notifications.API.Brokers;
using Kalakoana.Notifications.API.Services;
using Kalakobana.Notifications.API.Infrastructure.Configurations;
using MassTransit;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<GoogleAppSettings>(builder.Configuration.GetSection("GoogleAppSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddControllers();

#region MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(i =>
{
    //Resend Three times in 3 interval if error occured - Using Circuit breaker to avoid flooding
    i.AddConsumer<EmailConsumer>(config =>
    {
        config.UseMessageRetry(retryConf =>
        {
            retryConf.Interval(3, TimeSpan.FromSeconds(3));
        });
        config.UseCircuitBreaker(cb =>
        {
            cb.TrackingPeriod = TimeSpan.FromMinutes(1);// Time to track failures
            cb.TripThreshold = 70; // 70% of messages must fail to open the circuit
            cb.ActiveThreshold = 10; //At least 10 messages must be processed in the time
            cb.ResetInterval = TimeSpan.FromSeconds(30); // Wait 30 seconds before trying again after circuit is open
        });
    });
    // Register the consumer

    i.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");

        cfg.ReceiveEndpoint("email_queue", e =>
        {
            e.ConfigureConsumer<EmailConsumer>(context);
        });
    });
});
#endregion
builder.Services.AddAuthorization();
//Adding serilog configuration which is stored in appsettings.json
builder.Host.UseSerilog((context, configration) =>
    configration.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

