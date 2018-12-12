using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xam.Plugins.Forms.FloatingEntry
{
    public class FloatingEntry : ContentView, IDisposable
    {
        public static readonly BindableProperty KeyboardTypeProperty =
            BindableProperty.Create(nameof(KeyboardType),
                typeof(Keyboard),
                typeof(FloatingEntry),
                default(Keyboard));

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text),
                typeof(string),
                typeof(FloatingEntry),
                default(string),
                BindingMode.TwoWay);

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder),
                typeof(string),
                typeof(FloatingEntry),
                default(string));

        public static readonly BindableProperty ValidationErrorProperty =
            BindableProperty.Create(nameof(ValidationError),
                typeof(string),
                typeof(FloatingEntry),
                default(string));

        public static readonly BindableProperty HasValidationErrorProperty =
            BindableProperty.Create(nameof(HasValidationError),
                typeof(bool),
                typeof(FloatingEntry),
                default(bool));

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor),
                typeof(Color),
                typeof(FloatingEntry),
                default(Color));

        public static readonly BindableProperty FloatingPlaceholderColorProperty =
            BindableProperty.Create(nameof(FloatingPlaceholderColor),
                typeof(Color),
                typeof(FloatingEntry),
                default(Color));

        public static readonly BindableProperty ValidationErrorColorProperty =
            BindableProperty.Create(nameof(ValidationErrorColor),
                typeof(Color),
                typeof(FloatingEntry),
                default(Color));

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor),
                typeof(Color),
                typeof(FloatingEntry),
                default(Color));

        public static readonly BindableProperty EntryFontFamilyProperty =
            BindableProperty.Create(nameof(EntryFontFamily),
                typeof(string),
                typeof(FloatingEntry),
                default(string));

        public static readonly BindableProperty PlaceholderFontFamilyProperty =
            BindableProperty.Create(nameof(PlaceholderFontFamily),
                typeof(string),
                typeof(FloatingEntry),
                default(string));

        public static readonly BindableProperty PlaceholderFontSizeProperty =
            BindableProperty.Create(nameof(PlaceholderFontSize),
                typeof(double),
                typeof(FloatingEntry),
                default(double));

        public static readonly BindableProperty EntryFontSizeProperty =
            BindableProperty.Create(nameof(EntryFontSize),
                typeof(double),
                typeof(FloatingEntry),
                default(double));

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(nameof(ImageSource),
                typeof(ImageSource),
                typeof(FloatingEntry),
                default(ImageSource));

        public static readonly BindableProperty ValidateOnFocusLostProperty =
            BindableProperty.Create(nameof(ValidateOnFocusLost),
                typeof(bool),
                typeof(FloatingEntry),
                default(bool));

        private readonly BoxView _border;
        private readonly TintableImage _image;
        private readonly StackLayout _imageAndEntryStackLayout;
        private readonly Label _infoLabel;
        private readonly TapGestureRecognizer _tapGestureRecognizer;
        private readonly BorderlessEntry _textEntry;
        private bool _entryWasEmpty;
        private int _focusCount;

        public FloatingEntry()
        {
            var containerStackLayout = new StackLayout {VerticalOptions = LayoutOptions.StartAndExpand, Spacing = 2};
            _infoLabel = new Label
            {
                Opacity = 0,
                HorizontalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Start
            };
            _textEntry = new BorderlessEntry {HorizontalOptions = LayoutOptions.FillAndExpand};
            containerStackLayout.Children.Add(_infoLabel);
            
            _imageAndEntryStackLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0
            };
            
            _image = new TintableImage
            {
                HorizontalOptions = LayoutOptions.Start,
                IsVisible = false
            };
            
            _imageAndEntryStackLayout.Children.Add(_image);
            _imageAndEntryStackLayout.Children.Add(_textEntry);
            
            _border = new BoxView
            {
                Margin = new Thickness(0, 5, 0, 0),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                HeightRequest = 1
            };
            
            containerStackLayout.Children.Add(_imageAndEntryStackLayout);
            containerStackLayout.Children.Add(_border);
            _tapGestureRecognizer = new TapGestureRecognizer();
            _tapGestureRecognizer.Tapped += Tapped;
            containerStackLayout.GestureRecognizers.Add(_tapGestureRecognizer);
            Content = containerStackLayout;
            _entryWasEmpty = true;
            _textEntry.TextChanged += TextEntryOnTextChanged;
            _textEntry.PropertyChanged += TextEntryChanged;
            _textEntry.Focused += TextEntryOnFocused;
            _textEntry.Unfocused += TextEntryOnUnfocused;
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Color PlaceholderColor
        {
            get => (Color) GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public Color ValidationErrorColor
        {
            get => (Color) GetValue(ValidationErrorColorProperty);
            set => SetValue(ValidationErrorColorProperty, value);
        }

        public Color FloatingPlaceholderColor
        {
            get => (Color) GetValue(FloatingPlaceholderColorProperty);
            set => SetValue(FloatingPlaceholderColorProperty, value);
        }

        public Color TextColor
        {
            get => (Color) GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public string PlaceholderFontFamily
        {
            get => (string) GetValue(PlaceholderFontFamilyProperty);
            set => SetValue(PlaceholderFontFamilyProperty, value);
        }

        public string EntryFontFamily
        {
            get => (string) GetValue(EntryFontFamilyProperty);
            set => SetValue(EntryFontFamilyProperty, value);
        }

        public double PlaceholderFontSize
        {
            get => (double) GetValue(PlaceholderFontSizeProperty);
            set => SetValue(PlaceholderFontSizeProperty, value);
        }

        public double EntryFontSize
        {
            get => (double) GetValue(EntryFontSizeProperty);
            set => SetValue(EntryFontSizeProperty, value);
        }

        public string ValidationError
        {
            get => (string) GetValue(ValidationErrorProperty);
            set => SetValue(ValidationErrorProperty, value);
        }

        public bool HasValidationError
        {
            get => (bool) GetValue(HasValidationErrorProperty);
            set => SetValue(HasValidationErrorProperty, value);
        }

        public bool ValidateOnFocusLost
        {
            get => (bool) GetValue(ValidateOnFocusLostProperty);
            set => SetValue(ValidateOnFocusLostProperty, value);
        }

        public string Placeholder
        {
            get => (string) GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public ImageSource ImageSource
        {
            get => (ImageSource) GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public Keyboard KeyboardType
        {
            get => (Keyboard) GetValue(KeyboardTypeProperty);
            set => SetValue(KeyboardTypeProperty, value);
        }

        public void Dispose()
        {
            _textEntry.TextChanged -= TextEntryOnTextChanged;
            _textEntry.PropertyChanged -= TextEntryChanged;
            _textEntry.Focused -= TextEntryOnFocused;
            _textEntry.Unfocused -= TextEntryOnUnfocused;
            _tapGestureRecognizer.Tapped -= Tapped;
        }

        private void Tapped(object sender, EventArgs e)
        {
            _textEntry.Focus();
        }

        public new void Focus()
        {
            _textEntry.Focus();
        }

        private void TextEntryOnUnfocused(object sender, FocusEventArgs focusEventArgs)
        {
            if (_focusCount == 0)
                return;
            if (HasValidationError)
            {
                _border.BackgroundColor = ValidationErrorColor;
                _image.Tint = ValidationErrorColor;
                _infoLabel.Text = ValidationError?.ToUpperInvariant();
                _infoLabel.TextColor = ValidationErrorColor;
            }
            else
            {
                _border.BackgroundColor = PlaceholderColor;
                _image.Tint = PlaceholderColor;
                _infoLabel.Text = Placeholder?.ToUpperInvariant();
                _infoLabel.TextColor = FloatingPlaceholderColor;
            }
        }

        private void TextEntryOnFocused(object sender, FocusEventArgs focusEventArgs)
        {
            if (ValidateOnFocusLost)
            {
                _border.BackgroundColor = PlaceholderColor;
                _image.Tint = PlaceholderColor;
                _infoLabel.Text = Placeholder?.ToUpperInvariant();
                _infoLabel.TextColor = FloatingPlaceholderColor;
            }
            else
            {
                _border.BackgroundColor = PlaceholderColor;
                _image.Tint = PlaceholderColor;
            }

            _focusCount++;
        }

        private void TextEntryChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != HeightProperty.PropertyName) return;
            if (_image.Source != null)
                _image.HeightRequest = _textEntry.Height;
        }

        private void TextEntryOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            Text = textChangedEventArgs.NewTextValue;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == TextProperty.PropertyName)
            {
                UpdateControl();
            }
            else if (propertyName == TextColorProperty.PropertyName)
            {
                _textEntry.TextColor = TextColor;
            }
            else if (propertyName == PlaceholderProperty.PropertyName)
            {
                if (!HasValidationError) _infoLabel.Text = Placeholder?.ToUpperInvariant();
                _textEntry.Placeholder = Placeholder;
            }
            else if (propertyName == PlaceholderColorProperty.PropertyName)
            {
                _textEntry.PlaceholderColor = PlaceholderColor;
                _border.BackgroundColor = PlaceholderColor;
            }
            else if (propertyName == HasValidationErrorProperty.PropertyName)
            {
                if (HasValidationError && (!ValidateOnFocusLost || ValidateOnFocusLost && !_textEntry.IsFocused))
                {
                    if (ValidateOnFocusLost && _focusCount == 0)
                        return;
                    _infoLabel.Text = ValidationError?.ToUpperInvariant();
                    _infoLabel.TextColor = ValidationErrorColor;
                    _border.BackgroundColor = ValidationErrorColor;
                }
                else
                {
                    _infoLabel.Text = Placeholder?.ToUpperInvariant();
                    _infoLabel.TextColor = FloatingPlaceholderColor;
                    _border.BackgroundColor = _textEntry.IsFocused ? FloatingPlaceholderColor : PlaceholderColor;
                }
            }
            else if (propertyName == ValidationErrorProperty.PropertyName)
            {
                if (string.IsNullOrEmpty(ValidationError) || ValidateOnFocusLost && _textEntry.IsFocused) return;
                _infoLabel.Text = ValidationError?.ToUpperInvariant();
                _infoLabel.TextColor = ValidationErrorColor;
            }
            else if (propertyName == EntryFontFamilyProperty.PropertyName)
            {
                _textEntry.FontFamily = EntryFontFamily;
            }
            else if (propertyName == PlaceholderFontFamilyProperty.PropertyName)
            {
                _infoLabel.FontFamily = PlaceholderFontFamily;
            }
            else if (propertyName == EntryFontSizeProperty.PropertyName)
            {
                _textEntry.FontSize = EntryFontSize;
            }
            else if (propertyName == PlaceholderFontSizeProperty.PropertyName)
            {
                _infoLabel.FontSize = PlaceholderFontSize;
            }
            else if (propertyName == ImageSourceProperty.PropertyName)
            {
                _image.Source = ImageSource;
                _imageAndEntryStackLayout.Spacing = ImageSource == null ? 0 : 10;
                _image.IsVisible = _image.Source != null;
                if (_image.Source == null)
                {
                    _image.HeightRequest = 0;
                    _image.WidthRequest = 0;
                }
                else
                {
                    _image.HeightRequest = _textEntry.Height;
                }

                _imageAndEntryStackLayout.ForceLayout();
            }
            else if (propertyName == FloatingPlaceholderColorProperty.PropertyName)
            {
                _infoLabel.TextColor = FloatingPlaceholderColor;
            }
            else if (propertyName == KeyboardTypeProperty.PropertyName)
            {
                _textEntry.Keyboard = KeyboardType;
            }
        }

        private async void UpdateControl()
        {
            _textEntry.Text = Text;
            if (!string.IsNullOrEmpty(Text) && _entryWasEmpty)
                await Task.WhenAll(
                    _infoLabel.FadeTo(1),
                    _infoLabel.TranslateTo(0, 0)
                );

            else if (string.IsNullOrEmpty(Text))
                await Task.WhenAll(
                    _infoLabel.FadeTo(0),
                    _infoLabel.TranslateTo(0, _textEntry.Height / 2)
                );

            _entryWasEmpty = string.IsNullOrEmpty(Text);
        }
    }
}