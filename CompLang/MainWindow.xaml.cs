using CompLang.BLL;
using CompLang.BLL.Interfaces.Providers;
using CompLang.BLL.Interfaces.Services;
using CompLang.BLL.Models;
using CompLang.BLL.Services;
using CompLang.Interfaces.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CompLang
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly IWordProvider _wordProvider;
        private readonly ITextService _textService;
        private readonly ITextProvider _textProvider;
        private readonly IWordService _wordService;

        private IPaginationManager _paginationManager;
        private IEnumerable<string> _likeNames;
        private SortType _sortType = SortType.Name;
        private SortDirection _sortDirection = SortDirection.Ascending;

        private async Task FillDataGrid()
        {
            var data = await _wordProvider.GetAsync(
                        _paginationManager.GetPosition(),
                        _paginationManager.PageSize,
                        true,
                        _sortType,
                        _sortDirection,
                        _likeNames,
                        App.Ct).ConfigureAwait(false);
               
            Dispatcher.Invoke(() =>
            {
                WordsDG.ItemsSource = data;
                WordsDG.Columns[(int)_sortType].SortDirection = (ListSortDirection)((int)_sortDirection);
                
            });
        }
        private WordInputDialog MakeWordInputDialog(string title, string description, bool toValidate = true)
        {
            var wordInputDialog = new WordInputDialog(title, description, _wordProvider, toValidate);
            wordInputDialog.Owner = this;
            wordInputDialog.ShowDialog();
            return wordInputDialog;
        }

        private void RefreshPageTB()
        {
            Dispatcher.Invoke(() =>
            {
                PageTB.Text = (_paginationManager.CurrentPage + 1).ToString();
            });
        }
        private async Task RefreshMaxPageTB(CancellationToken ct = default)
        {
            var dataCount = await _wordProvider.GetCountByNamesAsync(_likeNames, ct);
            var dividedPages = (dataCount / _paginationManager.PageSize);
            _paginationManager.MaxPage = dividedPages + ((dividedPages * _paginationManager.PageSize < dataCount) ? 1 : 0);
            Dispatcher.Invoke(() =>
            {
                MaxPageTB.Text = (_paginationManager.MaxPage).ToString();
            });
        }
        private async Task RefreshPaginationInfo()
        {
            RefreshPageTB();
            await RefreshMaxPageTB(App.Ct).ConfigureAwait(false);
        }

        public MainWindow(IWordProvider wordProvider, IWordService wordService, 
            ITextService textService, ITextProvider textProvider,
            IPaginationManager paginationManager, 
            IConfiguration configuration)
        {
            this._wordProvider = wordProvider;
            this._textService = textService;
            this._wordService = wordService;
            this._textProvider = textProvider;
            
            this._likeNames = null;
            this._paginationManager = paginationManager;
            InitializeComponent();
        }


        /////////////////
        private async void OnLoad(object sender, EventArgs e)
        {
            await FillDataGrid().ConfigureAwait(false);
            await RefreshPaginationInfo().ConfigureAwait(false);
        }

        private void OpenDb_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private async void DropFile_Click(object sender, RoutedEventArgs e)
        {
            var wordInputDialog = MakeWordInputDialog("Find words", "Input part of word");

            if (!wordInputDialog.IsConfirmed)
            {
                return;
            }
            var text = await _textService.GetByTitleAsync(wordInputDialog.Title, App.Ct);
            if (text == null)
            {
                MessageBox.Show($"Text \"{wordInputDialog.Word}\" does not exist!");
                return;
            }
            using (var sw = new StreamWriter(text.Title, false))
            {
                sw.Write(text.Content);
                sw.Close();
            }
        }
        private async void ParseText_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();

            fileDialog.DefaultExt = ".txt";
            fileDialog.Filter = "TextEntity|*.txt|All|*.*";


            var result = fileDialog.ShowDialog();
            var readingTasks = new List<Task<Text>>(fileDialog.FileNames.Length);
            if (result == true)
            {
                foreach (var filename in fileDialog.FileNames)
                {
                    string text = "";
                    using (var sr = new StreamReader(fileDialog.FileName))
                    {
                        text = sr.ReadToEnd();
                        sr.Close();
                    }
                    await _textService.AddAsync(new Text { Title = Path.GetFileName(filename), Content = text }, App.Ct);
                }
                await FillDataGrid();
            }
        }
        private async void ParseTextsFromFolder_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo(@"D:\University\5_semester\ComputingLang\Texts\texts\");
            FileInfo[] files = d.GetFiles("*.txt"); //Getting Text files
           // Array.Reverse(files);
            foreach(var file in files)
            {
                string text = "";
                using (var sr = new StreamReader(file.FullName))
                {
                    text = sr.ReadToEnd();
                    sr.Close();
                }
                await _textService.AddAsync(new Text { Title = file.Name, Content = text }, App.Ct);
            }
            await FillDataGrid();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //////////////////
        private async void CreateWord_Click(object sender, RoutedEventArgs e)
        {
            var wordInputDialog = MakeWordInputDialog("Create word", "Input new word");

            if (!wordInputDialog.IsConfirmed)
            {
                return;
            }
            var word = await _wordProvider.CreateAsync(wordInputDialog.Word, App.Ct).ConfigureAwait(false);
            if (word != null)
            {
                MessageBox.Show($"Word \"{wordInputDialog.Word}\" already exists!");
                return;
            }
            
            await FillDataGrid().ConfigureAwait(false);
            await RefreshMaxPageTB(App.Ct).ConfigureAwait(false);
        }

        private async void EditWord_Click(object sender, RoutedEventArgs e)
        {
            var wordInputDialog = MakeWordInputDialog("Edit word", "Input uncorrect word");

            if (!wordInputDialog.IsConfirmed)
            {
                return;
            }
            var oldName = wordInputDialog.Word;

            wordInputDialog = MakeWordInputDialog("Edit word", "Input correct word");

            if (!wordInputDialog.IsConfirmed)
            {
                return;
            }

            var newName = wordInputDialog.Word;

            var word = await _wordService.EditAsync(oldName, newName, App.Ct).ConfigureAwait(false);
            if (word == null)
            {
                MessageBox.Show($"Word \"{wordInputDialog.Word}\" does not exist!");
                return;
            }
            await FillDataGrid().ConfigureAwait(false);
            await RefreshPaginationInfo().ConfigureAwait(false);
        }

        private async void DeleteWord_Click(object sender, RoutedEventArgs e)
        {
            var wordInputDialog = MakeWordInputDialog("Delete word", "Input word");

            if (!wordInputDialog.IsConfirmed)
            {
                return;
            }
            if (MessageBox.Show(
                $"Do you seriously want to delete word \"{wordInputDialog.Word}\"?",
                "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                
                return;
            }

            var isDeleted = await _wordProvider.DeleteAsync(wordInputDialog.Word, App.Ct).ConfigureAwait(false);
            if (!isDeleted)
            {
                MessageBox.Show($"Word \"{wordInputDialog.Word}\" does not exist!");
                return;
            }
            await FillDataGrid().ConfigureAwait(false);
            await RefreshPaginationInfo().ConfigureAwait(false);
        }



        ///////////////
        private async void ColumnDGHeader_Click(object sender, RoutedEventArgs e)
        {
            var header = (System.Windows.Controls.Primitives.DataGridColumnHeader)sender;
            if (header.SortDirection == null)
            {
                _sortDirection = (SortDirection)((int)ListSortDirection.Descending);
            }
            else
            {
                _sortDirection = (SortDirection)(((int)header.Column.SortDirection.Value+1)%2);
            }
            _sortType = (SortType)(header.DisplayIndex);
            await FillDataGrid().ConfigureAwait(false);


        }

        //////////////
        private async void FindLike_Click(object sender, RoutedEventArgs e)
        {
            var wordInputDialog = MakeWordInputDialog("Find words", "Input part of word");

            if (!wordInputDialog.IsConfirmed)
            {
                return;
            }
            _likeNames = new string[] { wordInputDialog.Word };
            _paginationManager.Reset();
            await FillDataGrid().ConfigureAwait(false);
            await RefreshPaginationInfo();
        }

        private async void ViewAll_Click(object sender, RoutedEventArgs e)
        {
            _likeNames = null;
            _paginationManager.Reset();
            await FillDataGrid().ConfigureAwait(false);
            await RefreshPaginationInfo().ConfigureAwait(false);
        }



        ///////////////////
        private async void PageBack_Click(object sender, RoutedEventArgs e)
        {
            _paginationManager.Back();
            await FillDataGrid().ConfigureAwait(false);
            RefreshPageTB();
        }

        private async void PageForward_Click(object sender, RoutedEventArgs e)
        {
            _paginationManager.Forward();
            await FillDataGrid().ConfigureAwait(false);
            RefreshPageTB();
        }
        private void PageTB_OnKeyDown(object sender, KeyEventArgs e)
        {
           /* if (e.Key == Key.Return && PageTB.IsFocused)
            {
               PageTB_TextChanged(PageTB, new RoutedEventArgs());
               await FillDataGrid().ConfigureAwait(false);
            }*/
        }

        private void PageTB_TextChanged(object sender, RoutedEventArgs e)
        {
            /*var regex = new Regex(@"[1-9]+\d*");
            var isMatch = regex.IsMatch(PageTB.Text);

            if (!isMatch)
            {
                PageTB.Text = "1";
            }
            _paginationManager.CurrentPage = Int32.Parse(PageTB.Text) - 1;
            await FillDataGrid().ConfigureAwait(false);
            RefreshPageTB();
           */
        }
        private async void ViewFilesList_Click(object sender, RoutedEventArgs e)
        {
            var files = await _textProvider.GetAllTitlesAsync(App.Ct);

            var filesWindow = new FilesListWindow(files);
            filesWindow.Show();
        }
        private async void ViewText_Click(object sender, RoutedEventArgs e)
        {
            var wordInputDialog = MakeWordInputDialog("Find text", "Input title", false);

            if (!wordInputDialog.IsConfirmed)
            {
                return;
            }

            var text = await _textProvider.GetByTitleAsync(wordInputDialog.Word, true, App.Ct).ConfigureAwait(false);
            if(text == null)
            {
                MessageBox.Show("Not found!");
                return;
            }
            TextViewWindow textViewWindow = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                textViewWindow = new TextViewWindow(text);
                textViewWindow.ShowDialog();
            });
            if(textViewWindow.ToSave)
            {
                await _textProvider.UpdateAsync(text, App.Ct);
            }
            
        }

    }
}
