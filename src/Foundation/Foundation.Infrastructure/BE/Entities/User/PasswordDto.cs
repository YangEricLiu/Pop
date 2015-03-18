using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.Validators;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class PasswordDto
    {
        [IdValidator(Ruleset = "update", MessageTemplate = FieldCode.ID)]
        public long? Id { get; set; }
        [PasswordValidator(Ruleset = "create", MessageTemplate = FieldCode.Password)]
        [PasswordValidator(Ruleset = "update", MessageTemplate = FieldCode.Password)]
        public string Password { get; set; }
        [PasswordValidator(Ruleset = "create", MessageTemplate = FieldCode.Password)]
        [PasswordValidator(Ruleset = "update", MessageTemplate = FieldCode.Password)]
        public string NewPassword { get; set; }
        [VersionValidator(Ruleset = "create", MessageTemplate = FieldCode.VERSION)]
        [VersionValidator(Ruleset = "update", MessageTemplate = FieldCode.VERSION)]
        public long? Version { get; set; }
    }
}
