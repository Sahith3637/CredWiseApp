using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CredWiseCustomer.Api.Controllers
{
    [ApiController]
    [Route("api/meta")]
    public class MetaController : ControllerBase
    {
        public class OptionDto
        {
            public string Value { get; set; }
            public string Label { get; set; }
        }

        [HttpGet("genders")]
        public ActionResult<IEnumerable<OptionDto>> GetGenders()
        {
            var genders = new List<OptionDto>
            {
                new OptionDto { Value = "Male", Label = "Male" },
                new OptionDto { Value = "Female", Label = "Female" },
                new OptionDto { Value = "Other", Label = "Other" }
            };
            return Ok(genders);
        }

        [HttpGet("employment-types")]
        public ActionResult<IEnumerable<OptionDto>> GetEmploymentTypes()
        {
            var types = new List<OptionDto>
            {
                new OptionDto { Value = "Salaried", Label = "Salaried" },
                new OptionDto { Value = "Self-Employed", Label = "Self-Employed" }
            };
            return Ok(types);
        }
    }
} 