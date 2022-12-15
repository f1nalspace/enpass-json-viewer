using System;

namespace EnpassJSONViewer.Models
{
    class EnpassField
    {
        public uint Id { get; }
        public string Label { get; }
        public string Type { get; }
        public string Value { get; }
        public DateTime UpdatedAt { get; }
        public DateTime ValueUpdatedAt { get; }
        public int Order { get; }
        public bool IsDeleted { get; }
        public bool IsSensitive { get; }

        public EnpassField(uint id, string label, string type, string value, DateTime updatedAt, DateTime valueUpdatedAt, int order, bool isDeleted, bool isSensitive)
        {
            Id = id;
            Label = label;
            Type = type;
            Value = value;
            UpdatedAt = updatedAt;
            ValueUpdatedAt = valueUpdatedAt;
            Order = order;
            IsDeleted = isDeleted;
            IsSensitive = isSensitive;
        }

        public override string ToString() => $"{Label} => {Value}";
    }
}
