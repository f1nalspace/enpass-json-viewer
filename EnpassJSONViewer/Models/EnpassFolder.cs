using System;
using System.Diagnostics;

namespace EnpassJSONViewer.Models
{
    [DebuggerDisplay("{Title} [{UpdatedAt}/{UUID}]")]
    class EnpassFolder : IEquatable<EnpassFolder>
    {
        public EnpassFolder Parent { get; internal set; }
        public Guid Id { get; }
        public string Title { get; }
        public DateTime UpdatedAt { get; }

        public EnpassFolder(Guid uuid, string title, DateTime updatedAt)
        {
            Parent = null;
            Id = uuid;
            Title = title;
            UpdatedAt = updatedAt;
        }

        public override string ToString() => Title;

        public bool Equals(EnpassFolder other)
            => other != null && Id.Equals(other.Id);
        public override bool Equals(object obj) => Equals(obj as EnpassFolder);
        public override int GetHashCode() => Id.GetHashCode();
    }
}
