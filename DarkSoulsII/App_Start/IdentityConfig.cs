using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using DataAccess;
using Model;

namespace DarkSoulsII
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    //Pass int type argument to base class:
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        //Add int type argument to constructor call:
        public ApplicationUserManager(IUserStore<ApplicationUser, int> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            //Pass custom application user store as constructor argument:
            var manager = new ApplicationUserManager(new ApplicationUserStore(context.Get<DarkSoulsIIContext>()));
            // Configure validation logic for usernames

            //Add int type argument to method call:
            manager.UserValidator = new UserValidator<ApplicationUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.

            //Add int type argument to method call:
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser, int>
            {
                MessageFormat = "Your security code is: {0}"
            });

            //Add int type argument to method call:
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser, int>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                //Add int type argument to method call:
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    /* We are replacing this implementation of ApplicationRoleManager with our own, which will use the ApplicationRole class instead of IdentityRole
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }
    */

    //Pass custom application role and int as type arguments to base:
    public class ApplicationRoleManager : RoleManager<ApplicationRole, int>
    {
        //Pass custom application role and int as type arguments to constructor:
        public ApplicationRoleManager(IRoleStore<ApplicationRole, int> roleStore)
            : base(roleStore)
        {

        }

        //Pass custom application role as type argument:
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(
                new ApplicationRoleStore(context.Get<DarkSoulsIIContext>()));
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            string hostAddress = ConfigurationManager.AppSettings["hostAddress"];
            int hostPort = Convert.ToInt32(ConfigurationManager.AppSettings["hostPort"]);
            string hostUserName = ConfigurationManager.AppSettings["hostUserName"];
            string hostPassword = ConfigurationManager.AppSettings["hostPassword"];
            string hostFromAddress = ConfigurationManager.AppSettings["hostFromAddress"];
            SmtpClient client = new SmtpClient(hostAddress, hostPort)
                {
                    Credentials = new NetworkCredential(hostUserName, hostPassword),
                    EnableSsl = true
                };
            MailMessage mailMessage = new MailMessage(hostFromAddress, message.Destination, message.Subject, message.Body);
            return client.SendMailAsync(mailMessage);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            string accountSid = ConfigurationManager.AppSettings["twilioAccountSid"];
            string authToken = ConfigurationManager.AppSettings["twilioAuthToken"];
            string twilioPhoneNumber = ConfigurationManager.AppSettings["twilioPhoneNumber"];
            var twilio = new Twilio.TwilioRestClient(accountSid, authToken);
            twilio.SendMessage(twilioPhoneNumber, message.Destination, message.Body);
            return Task.FromResult(0);
        }
    }

    // This is useful if you do not want to tear down the database each time you run the application.
    //public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<DarkSoulsIIContext>
    {
        protected override void Seed(DarkSoulsIIContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }
      
        public static void InitializeIdentityForEF(DarkSoulsIIContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            string name = ConfigurationManager.AppSettings["defaultAdminUserName"];
             string password = ConfigurationManager.AppSettings["defaultAdminPassword"];
             string roleName = ConfigurationManager.AppSettings["defaultAdminRoleName"];

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                //role = new IdentityRole(roleName);
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
    }

    //Pass int as type argument to base instead of string:
    public class ApplicationSignInManager : SignInManager<ApplicationUser, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}