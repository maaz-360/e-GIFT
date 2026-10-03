using eGift.WebAPI.Common;
using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Helpers;
using eGift.WebAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;

public static class LoginEndpoint
{
    public static RouteGroupBuilder MapLoginEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/login").WithTags("Login");

        #region Default CRUD Endpoints

        // GET: api/login
        group.MapGet("/", async (AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("LoginEndpoint");

            try
            {
                var logins = await (
                    from login in context.Logins
                    where !login.IsDeleted
                    select new
                    {
                        Id = login.Id,
                        RefId = login.RefId,
                        RefType = login.RefType,
                        UserName = login.UserName,
                        Password = login.Password,
                        RoleId = login.RoleId,
                        IsActive = login.IsActive,
                        LogInDate = login.LogInDate,
                        LastLoginDate = login.LastLoginDate,
                        CreatedDate = login.CreatedDate
                    }
                )
                .AsNoTracking()
                .ToListAsync();

                return logins is null
                    ? Results.NotFound()
                    : Results.Ok(logins);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in LoginEndpoint: /api/login GET: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while retrieving logins.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // GET: api/login/{id}
        group.MapGet("/{id:int}", async (int id, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("LoginEndpoint");

            try
            {
                var login = await (
                    from l in context.Logins
                    where l.Id == id && !l.IsDeleted
                    select new
                    {
                        Id = l.Id,
                        RefId = l.RefId,
                        RefType = l.RefType,
                        UserName = l.UserName,
                        Password = l.Password,
                        RoleId = l.RoleId,
                        IsActive = l.IsActive,
                        LogInDate = l.LogInDate,
                        LastLoginDate = l.LastLoginDate
                    }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();

                return login is null
                    ? Results.NotFound()
                    : Results.Ok(login);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in LoginEndpoint: /api/login/{id} GET: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while retrieving the login with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // POST: api/login
        group.MapPost("/", async (LoginDto dto, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("LoginEndpoint");

            try
            {
                var login = dto.ToEntity();

                login.Password = PasswordHelper.HashPassword(login.Password);

                context.Logins.Add(login);
                await context.SaveChangesAsync();

                return Results.Created(
                    $"/api/login/{login.Id}",
                    login
                );
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in LoginEndpoint: /api/login POST: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while creating the login.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // PUT: api/login/{id}
        group.MapPut("/{id:int}", async (int id, EditLoginDto dto, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("LoginEndpoint");

            try
            {
                var existingLogin = await context.Logins.FindAsync(id);

                if (existingLogin is null)
                {
                    return Results.NotFound();
                }

                existingLogin.ToEntity(dto);

                existingLogin.Password = PasswordHelper.HashPassword(existingLogin.Password);

                context.Logins.Update(existingLogin);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in LoginEndpoint: /api/login/{id} PUT: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while updating the login with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // DELETE: api/login/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (int id, int loginUserId, DateTime deletedDate, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("LoginEndpoint");

            try
            {
                var existingLogin = await context.Logins.FindAsync(id);

                if (existingLogin is null)
                {
                    return Results.NotFound();
                }

                existingLogin.IsDeleted = true;
                existingLogin.UpdatedBy = loginUserId;
                existingLogin.UpdatedDate = deletedDate;

                context.Logins.Update(existingLogin);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in LoginEndpoint: /api/login/{id} DELETE: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the login with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        #endregion

        #region Login Employee Endpoints
        // GET: api/login/employee
        group.MapGet("/employee", async (string userName, string password, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("LoginEndpoint");

            try
            {
                var employeeLogin = await context.Logins.Where(x => x.UserName == userName && !x.IsDeleted && x.RefType == RefType.Employee.ToString()).FirstOrDefaultAsync();

                if (employeeLogin == null)
                {
                    return Results.Json(
                        new
                        {
                            Message = $"Employee doesn't exist."
                        },
                        statusCode: StatusCodes.Status404NotFound
                    );
                }
                else if (!employeeLogin.IsActive)
                {
                    return Results.Json(
                        new
                        {
                            Message = $"Employee is not active.",
                        },
                        statusCode: StatusCodes.Status200OK
                    );
                }

                var isValid = PasswordHelper.VerifyPassword(password, employeeLogin.Password);

                if (!isValid)
                {
                    return Results.Json(
                        new
                        {
                            Message = $"Username or password invalid.",
                        },
                        statusCode: StatusCodes.Status200OK
                    );
                }
                else
                {
                    employeeLogin.LastLoginDate = employeeLogin.LogInDate;
                    employeeLogin.LogInDate = DateTime.Now;

                    context.Logins.Update(employeeLogin);
                    await context.SaveChangesAsync();
                }

                return Results.Ok(new
                {
                    Message = "Login successfully.",
                    UserId = employeeLogin.RefId,
                    UserName = employeeLogin.UserName,
                    RoleId = employeeLogin.RoleId
                });
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in LoginEndpoint: /api/login/employee GET: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while the login employee with username {userName}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });
        #endregion

        #region Login Customer Endpoints
        // GET: api/login/customer
        group.MapGet("/customer", async (string userName,string password,AppDBContext context,ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("LoginEndpoint");

            try
            {
                var customerLogin = await context.Logins
                    .Where(x => x.UserName == userName && x.RefType == RefType.Customer.ToString() && !x.IsDeleted).FirstOrDefaultAsync();

                if (customerLogin == null)
                {
                    return Results.Json(
                        new
                        {
                            Message = "Username or password invalid."
                        },statusCode: StatusCodes.Status200OK
                    );
                }

                if (!customerLogin.IsActive)
                {
                    return Results.Json(
                        new
                        {
                            Message = "Customer is not active."
                        },statusCode: StatusCodes.Status200OK
                    );
                }

                var customer = await context.Customers
                    .Where(x =>x.Id == customerLogin.RefId &&!x.IsDeleted).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return Results.Json(
                        new
                        {
                            Message = "Username or password invalid."
                        },statusCode: StatusCodes.Status200OK
                    );
                }

                if (!customer.IsActive)
                {
                    return Results.Json(
                        new
                        {
                            Message = "Customer is not active."
                        },statusCode: StatusCodes.Status200OK
                    );
                }

                var isValid = PasswordHelper.VerifyPassword(password, customerLogin.Password);

                if (!isValid)
                {
                    return Results.Json(
                        new
                        {
                            Message = "Username or password invalid."
                        },statusCode: StatusCodes.Status200OK
                    );
                }

                customerLogin.LastLoginDate = customerLogin.LogInDate;
                customerLogin.LogInDate = DateTime.Now;

                context.Logins.Update(customerLogin);
                await context.SaveChangesAsync();

                return Results.Ok(new
                {
                    Message = "Login successfully.",
                    UserId = customerLogin.RefId,
                    UserName = customerLogin.UserName,
                    RoleId = customerLogin.RoleId
                });
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in LoginEndpoint: /api/login/customer GET: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message =$"An error occurred while the login customer with username {userName}.",error = ex.Message
                    },statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });
        #endregion

        return group;
    }
}