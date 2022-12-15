using System;
using System.Collections.Immutable;

namespace EnpassJSONViewer.Models
{
    public class EnpassAttachment : IEquatable<EnpassAttachment>
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Kind { get; }
        public ImmutableArray<byte> Data { get; }
        public DateTime UpdatedUTC { get; }
        public DateTime UpdatedLocal => UpdatedUTC.ToLocalTime();
        public int Order { get; }

        public EnpassAttachment(Guid id, string name, string kind, ImmutableArray<byte> data, DateTime updated, int order)
        {
            Id = id;
            Name = name;
            Kind = kind ?? string.Empty;
            Data = data;
            UpdatedUTC = updated;
            Order = order;
        }

        public override int GetHashCode() => Id.GetHashCode();
        public bool Equals(EnpassAttachment other) => other != null && Id.Equals(other.Id);
        public override bool Equals(object obj) => Equals(obj as EnpassAttachment);

        public override string ToString() => Name;
    }
}
