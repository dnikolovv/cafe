using AutoFixture;
using Cafe.Core.Auth.Commands;
using System.Net.Mail;

namespace Cafe.Tests.Customizations
{
    public class LoginCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Login>(composer =>
                composer.With(c => c.Email, fixture.Create<MailAddress>().Address));
        }
    }
}
