using System;

namespace Solution.Domain
{
    public record CompanyId
    {
        public string Value { get; }

        public CompanyId(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value), "Id cannot be null or whitespace.");

            Value = value;
        }

        public static implicit operator string(CompanyId companyId) => companyId.Value;
        public static implicit operator CompanyId(string value) => new(value);
    }
}
