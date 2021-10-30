using CompLang.BLL.Interfaces.Providers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CompLang
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class WordInputDialog : Window
    {
        private readonly IWordProvider _wordProvider;
        private readonly string _title;
        private readonly string _description;

        public string Word { get; private set; }
        public bool IsConfirmed => !string.IsNullOrWhiteSpace(Word);
        public bool ToValidate { get; private set; }

        public WordInputDialog(string title, string description, IWordProvider wordProvider, bool toValidate = false)
        {
            this._wordProvider = wordProvider;
            this._title = title;
            this._description = description;
            this.ToValidate = toValidate;
            InitializeComponent();

        }
        private void OnLoad(object sender, EventArgs e)
        {
            Description.Text = _description;
            Title = _title;
            ConfirmButton.IsEnabled = !ToValidate;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Word = WordTextBox.Text;
            CloseHandle();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseHandle();
        }

        private void CloseHandle()
        {
            Close();
        }

        private void WordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ToValidate)
            {
                ConfirmButton.IsEnabled = _wordProvider.ValidateWord((sender as TextBox).Text);
            }
        }
    }
}
