using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MiniTube
{
    public partial class OpenDialog : Form
    {
        public OpenDialog()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private Regex _YoutubeRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");

        private void GoBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UrlTxtBox.Text))
            {
                string url = UrlTxtBox.Text;
                Match youtubeMatch = _YoutubeRegex.Match(url);
                if (youtubeMatch.Success)
                {
                    MainWindow.Current.Open(youtubeMatch.Groups[1].Value);
                }
            }
            this.Close();
        }
    }
}
