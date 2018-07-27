using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ComplaintBookApp.UserControls.CustomControls
{
    public class InfiniteListView : ListView
    {
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create<InfiniteListView, ICommand>(bp => bp.LoadMoreCommand, default(ICommand));

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }
        public InfiniteListView()
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
            ItemTapped += InfiniteListView_ItemTapped;
        }


        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = ItemsSource as IList;
            if (items == null || e.Item != items[items.Count - 1] || items.Count <= 3)
            {
                return;
            }

            if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
            {
                LoadMoreCommand.Execute(null);
            }
        }

        void InfiniteListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row
            if (e.Item == null) return;
            // do something with e.SelectedItem
            ((ListView)sender).SelectedItem = null; // de-select the row after ripple effect
        }
    }
}
