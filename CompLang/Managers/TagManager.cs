using CompLang.DAL.Entities;
using System.Windows.Controls;

namespace CompLang.Managers
{
    public class TagManager : TextBox
    {
        private WordUsageEntity _wordUsage;

        public string Word { get => _wordUsage.Word.Name; }
        public string WordTag 
        {
            get => _wordUsage.Tag;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    this.Text = WordTag;
                    return;
                }
                this.Text = value;
                _wordUsage.Tag = value;
            }
        }

        public TagManager(WordUsageEntity wordUsageEntity): base()
        {
            this._wordUsage = wordUsageEntity;
        }

        public void Reset()
        {
            this.Text = this.WordTag;
        }
    }
}
