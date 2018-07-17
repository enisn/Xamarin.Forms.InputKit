# SelectionView
SelectionView is a dynamic control. It needs a **ItemsSource** to generate views.
It can handle single selections nad multi selections. You can bind **SelectedItem** for single selection type,
and bind  **SelectecItems** for multi selections.

<hr />

## SelectionType
*Default value is Button*.
### Button
It's simple. Generates buttons for each object in your collection and able to select single of them.
You can get selected object with binding **SelectedItem ** property.

### RadioButton
Same with Button. User able to select one of selection. You can get selected one with **SelectedItem**.

### CheckBox
Generates CheckBox for each object in ItemsSource and able to select multiple selections.
You can get selected objects with binding **SelectedIems**.

<hr/>
<hr />
<br />
<br />
<br />
<br />
<hr />

## ColumnNumber

*Default value is 2*. <br />
That provides to decide inputs will be displayed in how many columns. 
<hr />
<br />
<br />
<br />
<br />
<hr />

## IsDisabledPropertyName
That is a propertyname. This property in your class must be a boolean, and decides that property is disabled or not.

 For example you have an class like that:


```csharp
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool OutOfStock { get; set; }
        public override string ToString() => Name;
    }
```

Just generate that list in a property:


```csharp
    public IList<Product> Products { get; set; } = new[]
           {
                new Product { Id = 1, Name ="Blue T-Shirt", OutOfStock = false },
                new Product { Id = 2, Name ="Red Jacket", OutOfStock = false },
                new Product { Id = 3, Name ="Black Pants", OutOfStock = true },
                new Product { Id = 4, Name ="Yellow T-Shirt", OutOfStock = false },
            };
```

Then bind it like, set **IsDisabledPropertyName** as *"OutOfStock"*, and when that property is true, it'll be disabled control. It can't be choosed.


```xml
      <input:SelectionView ColumnNumber="1" SelectionType="RadioButton" ItemsSource="{Binding Products}" IsDisabledPropertyName="OutOfStock" />

```

And the result:

<img src="https://scontent-frx5-1.xx.fbcdn.net/v/t1.15752-0/p280x280/37340493_2014315578579545_5894925369189859328_n.png?_nc_cat=0&oh=d8942e7bb79f86ff32d54b6aa1e1eb7f&oe=5BC5875B" height="480" />

<hr />
<br />
<br />
<br />
<br />
<hr />

## DisabledSource
It's a source to disable controls. It must keep items from ItemsSource, and disables them.

For example you have an class like that:


```csharp
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool OutOfStock { get; set; }
        public override string ToString() => Name;
    }
```

Just generate that list in a property:


```csharp
    public IList<Product> Products { get; set; } = new[]
           {
                new Product { Id = 1, Name ="Blue T-Shirt", OutOfStock = false },
                new Product { Id = 2, Name ="Red Jacket", OutOfStock = false },
                new Product { Id = 3, Name ="Black Pants", OutOfStock = true },
                new Product { Id = 4, Name ="Yellow T-Shirt", OutOfStock = false },
            };
        
    public IList<Product> DisabledProducts { get; set; }= new ObservableCollection<Product>();
```

And add some data to DisabledProducts from Products:


```csharp
     public MainViewModel() //Constructor
        {
            DisabledProducts.Add(Products[0]);
            DisabledProducts.Add(Products[2]);
        }
```

And just set DisabledSource from XAML:

```xml
         <input:SelectionView ColumnNumber="1" SelectionType="RadioButton" ItemsSource="{Binding Products}" DisabledSource="{Binding DisabledProducts}" />
```

And the result:

<img src="https://scontent.xx.fbcdn.net/v/t1.15752-0/p280x280/37242386_2014329231911513_6608474343640924160_n.png?_nc_cat=0&_nc_ad=z-m&_nc_cid=0&oh=9f45eb89a8bdb1f31b1641732c11b9e2&oe=5BDE9D13" height="480" />
<hr />
<br />
<br />
<br />
<br />



