using AutoFixture;
using Cafe.Core.AuthContext.Commands;
using System.Net.Mail;

namespace Cafe.Tests.Customizations
{
    public class RegisterCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Register>(composer =>
                composer.With(c => c.Email, fixture.Create<MailAddress>().Address));
        }
    }
}
