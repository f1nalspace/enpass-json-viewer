using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace EnpassJSONViewer.Models
{
    public class EnpassItem : IEquatable<EnpassItem>
    {
        public EnpassFolder Folder { get; }
        public IEnumerable<EnpassFolder> Folders => _folders;
        private readonly ImmutableArray<EnpassFolder> _folders;

        public Guid Id { get; }
        public string Title { get; }
        public string Subtitle { get; }
        public string Note { get; }
        public DateTime UpdatedAtUTC { get; }
        public DateTime UpdatedAtLocal => UpdatedAtUTC.ToLocalTime();

        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public string Url { get; private set; }

        public int FieldCount => _fields.Count;
        public IEnumerable<EnpassField> Fields => _fields;
        private ImmutableList<EnpassField> _fields = ImmutableList<EnpassField>.Empty;

        public int AttachmentCount => _attachments.Count;
        public IEnumerable<EnpassAttachment> Attachments => _attachments;
        private ImmutableList<EnpassAttachment> _attachments = ImmutableList<EnpassAttachment>.Empty;

        public EnpassItem(IEnumerable<EnpassFolder> folders, Guid id, string title, string subtitle, string note, DateTime updatedAt)
        {
            Id = id;
            _folders = folders.ToImmutableArray();
            Folder = _folders.FirstOrDefault();
            Title = title;
            Subtitle = subtitle;
            Note = note;
            UpdatedAtUTC = updatedAt;
            Username = Password = Email = Url = null;
        }

        public void AddFields(ImmutableArray<EnpassField> fields)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            _fields = _fields.AddRange(fields);
            Username = _fields.Where(f => "username".Equals(f.Type)).Select(f => f.Value).FirstOrDefault();
            Password = _fields.Where(f => "password".Equals(f.Type)).Select(f => f.Value).FirstOrDefault();
            Email = _fields.Where(f => "email".Equals(f.Type)).Select(f => f.Value).FirstOrDefault();
            Url = _fields.Where(f => "url".Equals(f.Type)).Select(f => f.Value).FirstOrDefault();
        }

        public void AddAttachments(ImmutableArray<EnpassAttachment> attachments)
        {
            if (attachments == null)
                throw new ArgumentNullException(nameof(attachments));
            _attachments = _attachments.AddRange(attachments);
        }

        public override int GetHashCode() => Id.GetHashCode();
        public bool Equals(EnpassItem other) => other != null && Id.Equals(other.Id);
        public override bool Equals(object obj) => Equals(obj as EnpassItem);

        public override string ToString() => Title;
    }
}
