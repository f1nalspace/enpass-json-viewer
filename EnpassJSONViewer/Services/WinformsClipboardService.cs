using System.Windows.Forms;

namespace EnpassJSONViewer.Services
{
    class WinformsClipboardService : IClipboardService
    {
        public void SetText(string text) => Clipboard.SetText(text ?? string.Empty);
    }
}
