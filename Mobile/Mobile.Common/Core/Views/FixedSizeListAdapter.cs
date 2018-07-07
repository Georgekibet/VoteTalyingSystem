using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Mobile.Common.Core.Data;
using Object = Java.Lang.Object;


namespace Mobile.Common.Core.Views
{

    // A ListAdapter that displays a maximum of 100 items. It also pre-loads pages in both directions
    // and adds or removes them automatically depending on the lists scroll poistion ie Infinite Scrolling
    // Users of this class should call Initialise, passing in their base IDataSource. This class will
    // ask the IDataSource for the correct page, depending on where the list scoll position currently is. 
    // You can call Initialise multiple times which is useful for when the User changes the filter parameters. 
    public class FixedSizeListAdapter<T> : ArrayAdapter<T>, AbsListView.IOnScrollListener where T : new()
    {
        private List<T> _currentItems = new List<T>();
        private List<T> _previousPage;
        private List<T> _nextPage;

        private const int PageSize = 50;
        //The offset into the underlying database table.
        private int _offset;
        private int _firstVisibleItem;
       
        private IDataSource<T> _dataSource;
        private IRemovableRepository<T> _removeRepository;

        public IRemovableRepository<T> Repository
        {
            get { return _removeRepository; }
        }


        public FixedSizeListAdapter(Context context)
            : base(context, Android.Resource.Layout.SimpleListItem1, new List<T>())
        {
        }

     

        // Resolve a dependency from the container
        public void Initialise(IDataSource<T> dataSource, IRemovableRepository<T> repository = null)
        {
            _dataSource = dataSource;
            _previousPage = new List<T>();
            _nextPage = new List<T>();
            _offset = 0;
            _firstVisibleItem = 0;
            _removeRepository = repository;

            _currentItems = _dataSource.Fetch(_offset, PageSize*2);
            //Move the offset so that we will advance through the IDataSource records
            _offset = PageSize * 2;

            FetchNextPage();

            UpdateItems();
        }

        private async void FetchNextPage()
        {
            _nextPage = await _dataSource.FetchAsync(_offset, PageSize);
        }

        private async void FetchPreviousPage()
        {
            var previousPageOffset = _offset - PageSize * 3;
            _previousPage = await _dataSource.FetchAsync(previousPageOffset, PageSize);
        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            _firstVisibleItem = firstVisibleItem;
        }

        public void OnScrollStateChanged(AbsListView view, ScrollState scrollState)
        {
            //Are we scrolling forwards, beyond three-quarters of the way throught the list?
            var threshold = (PageSize * 2) * 0.75;

            if (_firstVisibleItem >= threshold && _nextPage.Any())
            {
                //Get the precise position of the item currently showing at the top of the ListView
                var currentPosition = RecordCurrentPosition(view);
                RemoveFromStart();
                AddToEnd();
                //Restore the item to the top of the ListView
                RestorePosition(view, currentPosition, PageSize);
            }
            //Are we scrolling backwards, into the last quarter of items and have previous pages to reshow?
            else if (_firstVisibleItem < PageSize * 0.5 && _previousPage.Any())
            {
                var currentPosition = RecordCurrentPosition(view);
                RemoveFromEnd();
                AddToStart();
                RestorePosition(view, currentPosition, -PageSize);
            }
        }


        private void UpdateItems()
        {
            Clear();
            AddAll(_currentItems);
        }

        private ViewItemPosition RecordCurrentPosition(AbsListView view)
        {
            var pos = view.FirstVisiblePosition;
            var topItem = view.GetChildAt(0);
            var top = topItem == null ? 0 : (topItem.Top - view.ListPaddingTop);
            return new ViewItemPosition(pos, top);
        }

        private void RestorePosition(AbsListView view, ViewItemPosition position, int pageOffset)
        {
            view.SetSelectionFromTop(position.Position - pageOffset, position.Offset);
        }

        private void AddToStart()
        {
            _currentItems.InsertRange(0, _previousPage);
            if (_offset > PageSize * 2)
            {
                FetchPreviousPage();
            }
            else
            {
                _previousPage = new List<T>();
            }
            UpdateItems();
        }

        private void RemoveFromEnd()
        {
            var count = _currentItems.Count - PageSize;
            _nextPage = _currentItems.GetRange(PageSize, count);
            _currentItems.RemoveRange(PageSize, count);
            _offset -= PageSize;
        }

        private void AddToEnd()
        {
            _currentItems.AddRange(_nextPage);
            _offset += PageSize;
            FetchNextPage();
            UpdateItems();
        }

        private void RemoveFromStart()
        {
            _previousPage = _currentItems.GetRange(0, PageSize);
            _currentItems.RemoveRange(0, PageSize);
        }

        public void ConfirmDialog(View parent, string title, string message, Action onYesAction, Action onNoAction)
        {
           
            var builder = new AlertDialog.Builder(parent.Context);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetPositiveButton("   Yes  ", (senderAlert, args) =>
            {
                onYesAction?.Invoke();
               

            });
            builder.SetNegativeButton("   No  ", (senderAlert, args) =>
            {
              onNoAction?.Invoke();
             
            
            });

          builder.Create().Show();
         
        }

        public void AlertDialog(View parent, string title, string message, Action onYesAction)
        {
            var builder = new AlertDialog.Builder(parent.Context);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetPositiveButton("   OK  ", (senderAlert, args) =>
            {
                onYesAction?.Invoke();
            });
            builder.Show();
        }
    }

    public class ViewItemPosition
    {
        public ViewItemPosition(int position, int offset)
        {
            Position = position;
            Offset = offset;
        }

        public int Position { get; private set; }
        public int Offset { get; private set; }
    }

    public interface IRemovableRepository<T>
    {
        void Delete(T entity);
    }
}