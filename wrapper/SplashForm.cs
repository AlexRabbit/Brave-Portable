using System.Drawing;

namespace BraveNightlyPortable;

internal sealed class SplashForm : Form
{
    private readonly Label _status;
    private readonly ProgressBar _progress;

    public SplashForm(string title, string status)
    {
        Text = title;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        ControlBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        TopMost = true;
        ShowInTaskbar = false;
        ClientSize = new Size(420, 130);
        BackColor = Color.FromArgb(30, 30, 30);
        ForeColor = Color.White;

        var heading = new Label
        {
            Text = "Brave Nightly Portable",
            Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
            AutoSize = false,
            Size = new Size(400, 28),
            Location = new Point(10, 12),
            ForeColor = Color.White,
        };

        _status = new Label
        {
            Text = status,
            Font = new Font("Segoe UI", 9F),
            AutoSize = false,
            Size = new Size(400, 40),
            Location = new Point(10, 44),
            ForeColor = Color.Gainsboro,
        };

        _progress = new ProgressBar
        {
            Style = ProgressBarStyle.Marquee,
            MarqueeAnimationSpeed = 30,
            Size = new Size(400, 18),
            Location = new Point(10, 92),
        };

        Controls.Add(heading);
        Controls.Add(_status);
        Controls.Add(_progress);
    }

    public void SetStatus(string status)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetStatus(status));
            return;
        }
        _status.Text = status;
        Refresh();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        CenterOnScreen();
    }

    private void CenterOnScreen()
    {
        var screen = Screen.FromPoint(Cursor.Position).WorkingArea;
        Location = new Point(
            screen.Left + (screen.Width - Width) / 2,
            screen.Top + (screen.Height - Height) / 2);
    }
}
