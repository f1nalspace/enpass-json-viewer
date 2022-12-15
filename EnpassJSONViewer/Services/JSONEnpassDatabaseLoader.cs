using EnpassJSONViewer.Extensions;
using EnpassJSONViewer.Models;
using EnpassJSONViewer.Types;
using EnpassJSONViewer.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing.Text;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EnpassJSONViewer.Services
{
    class JSONEnpassDatabaseLoader : IEnpassDatabaseLoader
    {
        public Result<EnpassDatabase> Load(Stream stream, string name)
        {
            if (stream == null)
                return new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(name))
                return new ArgumentNullException(nameof(name));
            if (!stream.CanRead)
                return new EndOfStreamException($"The stream '{name}' is not readable");


            JsonDocument doc = null;
            try
            {
                doc = JsonDocument.Parse(stream);
            }
            catch (Exception e)
            {
                return new FormatException($"The JSON stream '{name}' could not be parsed!", e);
            }

            EnpassDatabase db = new EnpassDatabase();

            List<EnpassFolder> folders = new List<EnpassFolder>();
            Dictionary<Guid, EnpassFolder> folderMap = new Dictionary<Guid, EnpassFolder>();
            Dictionary<EnpassFolder, Guid> parentLinkMap = new Dictionary<EnpassFolder, Guid>();
            if (doc.RootElement.TryGetProperty("folders", out JsonElement foldersElement) && 
                foldersElement.ValueKind == JsonValueKind.Array)
            {
                JsonElement.ArrayEnumerator folderIterator = foldersElement.EnumerateArray();
                foreach (JsonElement folderElement in folderIterator)
                {
                    string title = folderElement.GetString("title");
                    Guid parentId = folderElement.GetGuid("parent_uuid");
                    Guid id = folderElement.GetGuid("uuid");
                    DateTime updatedAt = folderElement.GetUnixTimestamp("updated_at");
                    EnpassFolder folder = new EnpassFolder(id, title, updatedAt);
                    folders.Add(folder);
                    folderMap.Add(id, folder);
                    parentLinkMap.Add(folder, parentId);
                }
            }

            foreach (EnpassFolder folder in folders)
            {
                if (parentLinkMap.TryGetValue(folder, out Guid parentId) &&
                    folderMap.TryGetValue(parentId, out EnpassFolder parent))
                    folder.Parent = parent;
            }

            db.AddFolders(folders.ToImmutableArray());

            List<EnpassItem> items = new List<EnpassItem>();
            if (doc.RootElement.TryGetProperty("items", out JsonElement itemsElement) 
                && itemsElement.ValueKind == JsonValueKind.Array)
            {
                JsonElement.ArrayEnumerator itemIterator = itemsElement.EnumerateArray();
                foreach (JsonElement itemElement in itemIterator)
                {
                    string title = itemElement.GetString("title");
                    string subtitle = itemElement.GetString("subtitle");
                    string note = itemElement.GetString("note");
                    Guid id = itemElement.GetGuid("uuid");
                    DateTime updatedAt = itemElement.GetUnixTimestamp("updated_at");

                    List<EnpassFolder> folderLinks = new List<EnpassFolder>();
                    if (itemElement.TryGetProperty("folders", out JsonElement folderLinksElement) &&
                        folderLinksElement.ValueKind == JsonValueKind.Array)
                    {
                        JsonElement.ArrayEnumerator folderLinkIterator = folderLinksElement.EnumerateArray();
                        foreach (JsonElement folderLink in folderLinkIterator)
                        {
                            if (folderLink.ValueKind == JsonValueKind.String)
                            {
                                string folderLinkIdText = folderLink.GetString();
                                if (Guid.TryParse(folderLinkIdText, out Guid folderLinkId) &&
                                    folderMap.TryGetValue(folderLinkId, out EnpassFolder linkedFolder))
                                    folderLinks.Add(linkedFolder);
                            }
                        }
                    }

                    List<EnpassField> fields = new List<EnpassField>();
                    if (itemElement.TryGetProperty("fields", out JsonElement fieldsElement) &&
                        fieldsElement.ValueKind == JsonValueKind.Array)
                    {
                        JsonElement.ArrayEnumerator fieldsElementsIterator = fieldsElement.EnumerateArray();
                        foreach (JsonElement fieldElement in fieldsElementsIterator)
                        {
                            string label = fieldElement.GetString("label");
                            string type = fieldElement.GetString("type");
                            string value = fieldElement.GetString("value");
                            int order = fieldElement.GetInt32("order");
                            uint uid = fieldElement.GetUInt32("uid");
                            DateTime fieldUpdatedAt = itemElement.GetUnixTimestamp("updated_at");
                            DateTime valueUpdatedAt = itemElement.GetUnixTimestamp("value_updated_at");
                            bool isDeleted = fieldElement.GetUInt32("deleted") == 1;
                            bool isSensitive = fieldElement.GetUInt32("sensitive") == 1;
                            EnpassField field = new EnpassField(uid, label, type, value, fieldUpdatedAt, valueUpdatedAt, order, isDeleted, isSensitive);
                            fields.Add(field);
                        }
                    }

                    List<EnpassAttachment> attachments = new List<EnpassAttachment>();
                    if (itemElement.TryGetProperty("attachments", out JsonElement attachmentsElement) &&
                        attachmentsElement.ValueKind == JsonValueKind.Array)
                    {
                        JsonElement.ArrayEnumerator attachmentsElementIterator = attachmentsElement.EnumerateArray();
                        foreach (JsonElement attachmentElement in attachmentsElementIterator)
                        {
                            string attachmentName = attachmentElement.GetString("name");
                            string attachmentKind = attachmentElement.GetString("kind");
                            string dataRaw = attachmentElement.GetString("data");
                            int order = attachmentElement.GetInt32("order");
                            Guid attachmentId = attachmentElement.GetGuid("uuid");
                            DateTime updated = attachmentElement.GetUnixTimestamp("updated");

                            ImmutableArray<byte> data = ByteUtils.GetFromBase64(dataRaw);

                            EnpassAttachment attachment = new EnpassAttachment(attachmentId, attachmentName, attachmentKind, data, updated, order);
                            attachments.Add(attachment);
                        }
                    }

                    EnpassItem item = new EnpassItem(folderLinks, id, title, subtitle, note, updatedAt);

                    item.AddFields(fields.ToImmutableArray());

                    item.AddAttachments(attachments.ToImmutableArray());

                    items.Add(item);
                }
            }

            db.AddItems(items.ToImmutableArray());

            return db;
        }

        public Result<EnpassDatabase> Load(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return new Result<EnpassDatabase>(new ArgumentNullException(nameof(filePath)));
            if (!File.Exists(filePath))
                return new Result<EnpassDatabase>(new FileNotFoundException($"The JSON file '{filePath}' was not found", filePath));
            using (FileStream stream = File.OpenRead(filePath))
                return Load(stream, filePath);
        }
    }
}
