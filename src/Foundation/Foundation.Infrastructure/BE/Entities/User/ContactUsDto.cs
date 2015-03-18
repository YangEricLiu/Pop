using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class ContactUsDto
    {
        [NotNullValidator(Ruleset = "create", MessageTemplate = "999")]
        public string Name { get; set; }

        [NotNullValidator(Ruleset = "create", MessageTemplate = "999")]
        public string Telephone { get; set; }

        [NotNullValidator(Ruleset = "create", MessageTemplate = "999")]
        public string CustomerName { get; set; }

        public string Title { get; set; }

        public string Comment { get; set; }
    }
}