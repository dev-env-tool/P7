using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using P7CreateRestApi.Data;
using P7CreateRestApi.DTO;
using P7CreateRestApi.Filters;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;
using P7CreateRestApi.Profiles;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Xunit;
using User = P7CreateRestApi.Domain.User;


namespace P7CreateRestApiUsersServicesUnitTests
{

    /// <Summary>
    /// Creating an SQL datatabse context for all tests needing sql
    /// <Summary>
    public class DatabaseFixture
    {


        public class UserUnitTests
        {
            private ApplicationDbContext GetInMemoryDbContext()
            {
                var configuration = new ConfigurationBuilder();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                return new ApplicationDbContext(options, configuration);
            }


            [Fact]
            public async Task IUserService_CreateUserWithRegisterModel_ShouldAdd_1User()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel",
                    Email = "registerModel@gmail.com",
                    Password = "test123.Pass",
                    Role = "",
                };

                ///Act
                var createUser = await iUserService.CreateUserWithRegisterModel(registerModel);
                UserDto userDtoFound = iUserService.GetUserDtoByEmail(registerModel.Email).Result.Select(u => u).Last();

                ///Assert
                Xunit.Assert.True(createUser.Succeeded);
                Xunit.Assert.Equivalent(registerModel.UserName, userDtoFound.UserName);
                Xunit.Assert.Equivalent("Member", userDtoFound.Role);
                await iUserService.DeleteUserByEmail(registerModel.Email);
            }



            [Fact]
            public async Task IUserService_DeleteUserByEmail_ShouldDelete_1User()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel",
                    Email = "registerModel@gmail.com",
                    Password = "test123.Pass",
                    Role = "Member",
                };


                var createUser = await iUserService.CreateUserWithRegisterModel(registerModel);
                UserDto userDtoFound = iUserService.GetUserDtoByEmail(registerModel.Email).Result.Select(u => u).Last();

                ///Act
                var deleteUser = await iUserService.DeleteUserByEmail(registerModel.Email);
                int userDtoFoundafterDeletetionCount = iUserService.GetUserDtoByEmail(registerModel.Email).Result.Select(u => u).Count();

                ///Assert
                Xunit.Assert.True(createUser.Succeeded);
                Xunit.Assert.Equivalent(registerModel.UserName, userDtoFound.UserName);
                Xunit.Assert.Equivalent(registerModel.Role, userDtoFound.Role);
                Xunit.Assert.True(deleteUser.Succeeded);
                Xunit.Assert.Equal(0, userDtoFoundafterDeletetionCount);

            }


            [Fact]
            public async Task IUserService_GetAllUsersDto_ShouldGet_2Users()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                //var userValidator = new List<IUserValidator<User>>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new IPasswordValidator<User>[0];
                var validator = new PasswordValidator<User>();
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel1",
                    Email = "registerModel1@gmail.com",
                    Password = "test123.Pass",
                    Role = "Member",
                };

                P7CreateRestApi.Models.RegisterModel registerModel2 = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel2",
                    Email = "registerModel2@gmail.com",
                    Password = "test123.Pass",
                    Role = "Member",
                };

                ///Act
                var createUser1 = await iUserService.CreateUserWithRegisterModel(registerModel1);
                var createUser2 = await iUserService.CreateUserWithRegisterModel(registerModel2);
                IList<UserDto> userDtoFound = iUserService.GetAllUsersDto().Result.Select(u => u).ToList();
                int userDtoFoundafterDeletetionCount = iUserService.GetAllUsersDto().Result.Select(u => u).Count();

                ///Assert
                Xunit.Assert.True(createUser1.Succeeded);
                Xunit.Assert.True(createUser2.Succeeded);
                Xunit.Assert.Equivalent(registerModel1.UserName, userDtoFound[0].UserName);
                Xunit.Assert.Equivalent(registerModel1.Role, userDtoFound[0].Role);
                Xunit.Assert.Equivalent(registerModel2.UserName, userDtoFound[1].UserName);
                Xunit.Assert.Equivalent(registerModel2.Role, userDtoFound[1].Role);
                Xunit.Assert.Equal(2, userDtoFoundafterDeletetionCount);
                await iUserService.DeleteUserByEmail(registerModel1.Email);
                await iUserService.DeleteUserByEmail(registerModel2.Email);
            }


            [Fact]
            public async Task IBidListService_CreateBidListWithBidListDto_ShouldGet_1UserById()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel1",
                    Email = "test123456789abcd@gmail.com",
                    Password = "test123.Pass",
                    Role = "Member",
                };

                P7CreateRestApi.Models.RegisterModel registerModel2 = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel2",
                    Email = "test1abcd@gmail.com",
                    Password = "test123.Pass",
                    Role = "Member",
                };

                ///Act
                var createUser1 = await iUserService.CreateUserWithRegisterModel(registerModel1);
                var createUser2 = await iUserService.CreateUserWithRegisterModel(registerModel2);
                UserDto userDtoFound1 = iUserService.GetUserDtoByEmail(registerModel1.Email).Result.Select(u => u).First();




                ///Assert
                Xunit.Assert.True(createUser1.Succeeded);
                Xunit.Assert.True(createUser2.Succeeded);
                Xunit.Assert.Equivalent(registerModel1.UserName, userDtoFound1.UserName);
                Xunit.Assert.Equivalent(registerModel1.Role, userDtoFound1.Role);
                await iUserService.DeleteUserByEmail(registerModel1.Email);
                await iUserService.DeleteUserByEmail(registerModel2.Email);
            }

            [Fact]
            public async Task IBidListService_UpdateUserWithUpdateGeneralInfosModel_ShouldUpdate_1User()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel = new P7CreateRestApi.Models.RegisterModel
                {
                    Email = "test@gmail.com",
                    UserName = "register",
                    Password = "passW1.ordtest",
                    Role = "Member",
                };

                P7CreateRestApi.Models.UpdateGeneralInfosModel updateGeneralInfosModelForUpdate = new P7CreateRestApi.Models.UpdateGeneralInfosModel
                {
                    UserName = "testnewname",
                    Role = "Admin",
                };

                ///Act
                var createUser = await iUserService.CreateUserWithRegisterModel(registerModel);
                var updateUser = await iUserService.UpdateUserWithUpdateGeneralInfosModel(registerModel.Email, updateGeneralInfosModelForUpdate);
                // Persistency = staus before/during/after the action. _userManager.UpdateAsync does not tell the new database instance about the updates in test environement.
                // We must use context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == registerModel.Email); to retrieve the updated user
                var userDtoFoundwithoutPertisency = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == registerModel.Email);


                ///Assert
                Xunit.Assert.True(createUser.Succeeded);
                Xunit.Assert.True(updateUser.Succeeded);
                Xunit.Assert.Equivalent(updateGeneralInfosModelForUpdate.UserName, userDtoFoundwithoutPertisency.UserName);
                Xunit.Assert.Equivalent(updateGeneralInfosModelForUpdate.Role, userDtoFoundwithoutPertisency.Role);
                var test1 = await userManager.GetUsersInRoleAsync("Member");
                var test2 = await userManager.GetUsersInRoleAsync("Admin");
                Xunit.Assert.True(test2.ElementAt(0).UserName == userDtoFoundwithoutPertisency.UserName);

                await iUserService.DeleteUserByEmail(registerModel.Email);

            }


            [Fact]
            public async Task IBidListService_UpdateUserWithUpdatePasswordModel_ShouldUpdate_1PasswordUser()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel = new P7CreateRestApi.Models.RegisterModel
                {
                    Email = "test@gmail.com",
                    UserName = "register",
                    Password = "passW1.ordtest",
                    Role = "Member",
                };

                P7CreateRestApi.Models.UpdatePasswordModel updatePasswordModelForUpdate = new P7CreateRestApi.Models.UpdatePasswordModel
                {
                    CurrentPassword = "passW1.ordtest",
                    NewPassword = "NewPassword123.",
                };

                ///Act
                var createUser = await iUserService.CreateUserWithRegisterModel(registerModel);
                var updateUser = await iUserService.UpdateUserPasswordWithUpdatePasswordModel(registerModel.Email, updatePasswordModelForUpdate);
                // Persistency = staus before/during/after the action. _userManager.UpdateAsync does not tell the new database instance about the updates in test environement.
                // We must use context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == registerModel.Email); to retrieve the updated user
                var userDtoFoundwithoutPertisency = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == registerModel.Email);


                ///Assert
                Xunit.Assert.True(createUser.Succeeded);
                Xunit.Assert.True(updateUser.Succeeded);
                Xunit.Assert.Equivalent(updatePasswordModelForUpdate.NewPassword, userDtoFoundwithoutPertisency.Password);

                await iUserService.DeleteUserByEmail(registerModel.Email);

            }


            [Fact]
            public async Task IBidListService_CreateUserWithRegisterModel_ShouldNotCreate_2IdenticalUsers()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
                {
                    Email = "test@gmail.com",
                    UserName = "test@gmail.com",
                    Password = "passwordTest1.",
                    Role = "Member",
                };

                P7CreateRestApi.Models.RegisterModel registerModel2 = new P7CreateRestApi.Models.RegisterModel
                {
                    Email = "test@gmail.com",
                    UserName = "test@gmail.com",
                    Password = "passwordTest1.",
                    Role = "",
                };
                ///Act
                var createUser = await iUserService.CreateUserWithRegisterModel(registerModel1);
                var createUser2 = await iUserService.CreateUserWithRegisterModel(registerModel2);
                UserDto userDtoFound1 = iUserService.GetUserDtoByEmail(registerModel1.Email).Result.Select(u => u).First();
                int userDtoFoundCount = iUserService.GetAllUsersDto().Result.Select(u => u).Count();



                ///Assert
                Xunit.Assert.Equivalent(registerModel1.Email, userDtoFound1.Email);
                Xunit.Assert.Equivalent(registerModel1.UserName, userDtoFound1.UserName);
                Xunit.Assert.Equivalent(registerModel1.Role, userDtoFound1.Role);
                Xunit.Assert.Equal(1, userDtoFoundCount);
                Xunit.Assert.False(createUser2.Succeeded);
                await iUserService.DeleteUserByEmail(registerModel1.Email);

            }


            [Fact]
            public async Task IBidListService_CreateUserWithRegisterModel_ShouldNotCreate_1User()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
                {
                    Email = "test@gmail.com",
                    UserName = "test@gmail.com",
                    Password = "",
                    Role = "Member",
                };


                ///Act
                var createUser = await iUserService.CreateUserWithRegisterModel(registerModel1);
                //UserDto userDtoFound1 = iUserService.GetUserDtoByEmail(registerModel1.Email).Result.Select(u => u).First();
                int userDtoFoundCount = iUserService.GetAllUsersDto().Result.Select(u => u).Count();



                ///Assert
                Xunit.Assert.Equal(0, userDtoFoundCount);


            }


            [Fact]
            public async Task IBidListService_CreateBidListWithBidListDto_ShouldReturn_4Errors()
            {
                /// Arrange
                Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = new LoggerFactory();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DtoProfile>();
                }, loggerFactory);
                IMapper mapper = config.CreateMapper();

                var context = GetInMemoryDbContext();
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase6")
                    .Options;
                var configuration = new ConfigurationBuilder();
                var db = new ApplicationDbContext(options, configuration);
                var userstore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                //var optionsDb = Options.Create(new IdentityOptions());
                var optionsDb = Options.Create(new IdentityOptions
                {
                    Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireNonAlphanumeric = true,
                        RequireUppercase = true,
                        RequiredLength = 10,
                        RequiredUniqueChars = 1
                    },
                    User = new UserOptions
                    {
                        RequireUniqueEmail = true,
                    },

                });
                var passwordHasher = new PasswordHasher<User>();
                var userValidator = new List<IUserValidator<User>>();
                var userValidatorItem = new UserValidator<User>();
                userValidator.Add(userValidatorItem);
                int count = userValidator.Count();
                var passwordValidator = new List<IPasswordValidator<User>>();
                var passwordValidatorItem = new PasswordValidator<User>();
                passwordValidator.Add(passwordValidatorItem);
                var lookupNormalizer = new UpperInvariantLookupNormalizer();
                var identityErrorDescriber = new IdentityErrorDescriber();
                var iServiceProvider = new Mock<IServiceProvider>().Object;
                var iLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<User>>>().Object;
                var userManager = new UserManager<P7CreateRestApi.Domain.User>(userstore, optionsDb, passwordHasher, userValidator, passwordValidator, lookupNormalizer, identityErrorDescriber, iServiceProvider, iLogger);
                var roleValidator = new List<IRoleValidator<IdentityRole>>();
                var iLoggerRole = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;
                var roleManager = new RoleManager<IdentityRole>(roleStore, roleValidator, lookupNormalizer, identityErrorDescriber, iLoggerRole);
                IUserRepository iUserRepository = new UserRepository(context, userManager, roleManager);
                IUserService iUserService = new UserService(iUserRepository, mapper);
                CustomValidationAttribute customValidationAttribute = new CustomValidationAttribute();

                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                P7CreateRestApi.Models.RegisterModel registerModel1 = new P7CreateRestApi.Models.RegisterModel
                {
                    UserName = "registerModel1",
                    Email = "registerModel1@gmail.com",
                    Password = "pp",
                    Role = "Member",
                };



                ///Act
                var createUser1 = await iUserService.CreateUserWithRegisterModel(registerModel1);
                int userDtoFoundCount = iUserService.GetAllUsersDto().Result.Select(u => u).Count();
                string err1 = createUser1.Errors.ElementAt(0).Description;
                string err2 = createUser1.Errors.ElementAt(1).Description;
                string err3 = createUser1.Errors.ElementAt(2).Description;
                string err4 = createUser1.Errors.ElementAt(3).Description;


                ///Assert
                Xunit.Assert.Equal(0, userDtoFoundCount);
                Xunit.Assert.Equal(4, createUser1.Errors.Count());
                Xunit.Assert.Contains(err1, "Passwords must be at least 10 characters.");
                Xunit.Assert.Contains(err2, "Passwords must have at least one non alphanumeric character.");
                Xunit.Assert.Contains(err3, "Passwords must have at least one digit ('0'-'9').");
                Xunit.Assert.Contains(err4, "Passwords must have at least one uppercase ('A'-'Z').");
                await iUserService.DeleteUserByEmail(registerModel1.Email);
            }

        }

    }










    
}















