using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

public enum CollectionChangeAction
{
    Add,
    Remove
}

public class ObservableCollection<T> : Collection<T>
{
    public event EventHandler<CollectionChangedEventArgs<T>> CollectionChanged;

    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        OnCollectionChanged(new CollectionChangedEventArgs<T>(CollectionChangeAction.Add, item));
    }

    protected override void RemoveItem(int index)
    {
        T removedItem = this[index];
        base.RemoveItem(index);
        OnCollectionChanged(new CollectionChangedEventArgs<T>(CollectionChangeAction.Remove, removedItem));
    }

    protected virtual void OnCollectionChanged(CollectionChangedEventArgs<T> e)
    {
        CollectionChanged?.Invoke(this, e);
    }
}

public class CollectionChangedEventArgs<T> : EventArgs
{
    public CollectionChangeAction Action { get; }
    public T Item { get; }

    public CollectionChangedEventArgs(CollectionChangeAction action, T item)
    {
        Action = action;
        Item = item;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        ObservableCollection<string> collection = new ObservableCollection<string>();
        collection.CollectionChanged += Collection_CollectionChanged;

        collection.Add("Apple");
        collection.Add("Banana");
        collection.Remove("Apple");
    }

    private static void Collection_CollectionChanged(object sender, CollectionChangedEventArgs<string> e)
    {
        string message = "";

        if (e.Action == CollectionChangeAction.Add)
        {
            message = $"Element '{e.Item}' is added to the collection";
        }
        else if (e.Action == CollectionChangeAction.Remove)
        {
            message = $"Element '{e.Item}' is removed from the collection";
        }

        Console.WriteLine(message);
       
    }
}
