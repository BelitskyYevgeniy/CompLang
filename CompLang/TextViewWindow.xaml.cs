using CompLang.BLL.Models;
using CompLang.DAL.Entities;
using CompLang.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for TextViewWindow.xaml
    /// </summary>
    public partial class TextViewWindow : Window
    {
        private readonly TextEntity _text;
        public bool ToSave { get; private set; } = false;

        private void Exit(bool toSave)
        {
            this.ToSave = toSave;
            Close();
        }
        private void FillPlain()
        {
            foreach(var wordUsage in _text.WordUsages)
            {
                Main_TB.Inlines.Add(wordUsage.Word.Name);
                if (char.IsLetter(wordUsage.Word.Name[0]))
                {
                    var tagTextBox = new TagManager(wordUsage);
                    tagTextBox.Width = 50;
                    tagTextBox.Text = wordUsage.Tag;
                    tagTextBox.KeyDown += SaveTag;
                    Main_TB.Inlines.Add(tagTextBox);
                }
            }
        }

        public TextViewWindow(TextEntity text)
        {
            this._text = text;
            InitializeComponent();
        }

        
        private void SaveTag(object sender, KeyEventArgs e)
        {
            var tagManager = (TagManager)sender;
            if(e.Key == Key.Enter)
            {
                if(MessageBox.Show(
                    $"Do you seriously want to change tag \"{tagManager.WordTag}\" for word \"{ tagManager.Word }\"?",
                    "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    tagManager.Reset();
                    return;
                }
                tagManager.WordTag = tagManager.Text;
            }
        }

        
        private void OnLoad(object sender, EventArgs e)
        {
            FillPlain();
        }
        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            Exit(true);
        }
        private void CancelChanges(object sender, RoutedEventArgs e)
        {
            Exit(false);
        }

        private void Window_Closed(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show(
                    $"Do you seriously want to exit? All unsaved changes will be lost!",
                    "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
        }
    }
}
