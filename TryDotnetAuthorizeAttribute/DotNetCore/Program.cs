using CommonAuthService;
using DotNetCore.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSystemWebAdapters();
builder.Services.AddHttpForwarder();

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

// Add the ExamplePolicy
builder.Services.AddAuthorization(options =>
{
    // Add the CustomAuthorizeRequirement policy
    options.AddPolicy("CustomAuthorize", policy =>
        policy.Requirements.Add(new CustomAuthorizeRequirement("User1,User2", "Admin,Manager")));
});

// Register the CustomAuthorizeHandler
builder.Services.AddSingleton<IAuthorizationHandler, CustomAuthorizeHandler>();

UserManager.AddUser("User1", "Password1");
UserManager.AddRoleToUser("User1", "Admin");

UserManager.AddUser("User2", "Password2");
UserManager.AddRoleToUser("User2", "Manager");

UserManager.AddUser("User3", "Password3");
UserManager.AddRoleToUser("User3", "User");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//app.UseSystemWebAdapters();

app.MapDefaultControllerRoute();
app.MapForwarder("/{**catch-all}", app.Configuration["ProxyTo"]).Add(static builder => ((RouteEndpointBuilder)builder).Order = int.MaxValue);

//// Apply the ExamplePolicy to an endpoint
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers().RequireAuthorization("ExamplePolicy");
//});

app.Run();
