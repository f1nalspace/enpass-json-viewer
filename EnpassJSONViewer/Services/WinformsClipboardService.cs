using System.Windows.Forms;

namespace EnpassJSONViewer.Services
{
    class WinformsClipboardService : IClipboardService
    {
        public void SetText(string text)
        {
            if (!string.IsNullOrEmpty(text))
                Clipboard.SetText(text);
        }
    }
}
