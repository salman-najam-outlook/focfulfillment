using Microsoft.EntityFrameworkCore;

namespace LocalDropshipping.Web.Data.Entities
{
    [Keyless]
    public class FocSetting
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
        public string Description { get; set; }
    }
}
