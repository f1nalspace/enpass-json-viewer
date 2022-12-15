using System;

namespace EnpassJSONViewer.Models
{
    public class EnpassField : IEquatable<EnpassField>
    {
        public uint Id { get; }
        public string Label { get; }
        public string Type { get; }
        public string Value { get; }
        public DateTime UpdatedAtUTC { get; }
        public DateTime UpdatedAtLocal => UpdatedAtUTC.ToLocalTime();
        public DateTime ValueUpdatedAtUTC { get; }
        public DateTime ValueUpdatedAtLocal => ValueUpdatedAtUTC.ToLocalTime();
        public int Order { get; }
        public bool IsDeleted { get; }
        public bool IsSensitive { get; }

        public EnpassField(uint id, string label, string type, string value, DateTime updatedAt, DateTime valueUpdatedAt, int order, bool isDeleted, bool isSensitive)
        {
            Id = id;
            Label = label;
            Type = type;
            Value = value;
            UpdatedAtUTC = updatedAt;
            ValueUpdatedAtUTC = valueUpdatedAt;
            Order = order;
            IsDeleted = isDeleted;
            IsSensitive = isSensitive;
        }

        public override int GetHashCode() => Id.GetHashCode();
        public bool Equals(EnpassField other) => other != null && Id.Equals(other.Id);
        public override bool Equals(object obj) => Equals(obj as EnpassField);

        public override string ToString() => $"{Label} => {Value}";
    }
}
