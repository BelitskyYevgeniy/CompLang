using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CompLang
{
    /// <summary>
    /// Interaction logic for FilesListWindow.xaml
    /// </summary>
    public partial class FilesListWindow : Window
    {
        private IEnumerable<string> _titles;
        public FilesListWindow(IEnumerable<string> titles)
        {
            this._titles = titles;
            InitializeComponent();
        }
        private void OnLoad(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (var title in _titles)
            {
                sb.Append(title).Append('\n');
            }
            Titles_TB.Text = sb.ToString();

        }
    }
}
