using System;
using System.Collections.Immutable;

namespace EnpassJSONViewer.Models
{
    class EnpassAttachment
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Kind { get; }
        public ImmutableArray<byte> Data { get; }
        public DateTime Updated { get; }
        public int Order { get; }

        public EnpassAttachment(Guid id, string name, string kind, ImmutableArray<byte> data, DateTime updated, int order)
        {
            Id = id;
            Name = name;
            Kind = kind;
            Data = data;
            Updated = updated;
            Order = order;
        }

        public override string ToString() => Name;
    }
}
