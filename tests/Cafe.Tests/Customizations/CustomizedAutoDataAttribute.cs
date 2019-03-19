using AutoFixture;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cafe.Tests.Customizations
{
    public class CustomizedAutoDataAttribute : AutoDataAttribute
    {
        /// <summary>
        /// The customizations are static so we only traverse the assembly types once.
        /// </summary>
        private static readonly IList<ICustomization> _customizations;

        static CustomizedAutoDataAttribute()
        {
            var currentAssembly = Assembly
                .GetExecutingAssembly();

            _customizations = currentAssembly
                .GetTypes()
                .Where(t => typeof(ICustomization).IsAssignableFrom(t))
                .Select(t => (ICustomization)Activator.CreateInstance(t))
                .ToList();
        }

        public CustomizedAutoDataAttribute()
            : base (() =>
            {
                // We need to create a new fixture with each new attribute
                // otherwise there is a risk of AutoFixture generating the same data when
                // executing tests in parallel which results in unwanted collissions
                var fixture = new Fixture();

                foreach (var customization in _customizations)
                {
                    fixture.Customize(customization);
                }

                return fixture;
            })
        {
        }
    }
}
