using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Styling;

namespace Atlas.UI
{
    public class Window : Avalonia.Controls.Window, IStyleable
    {
        Type IStyleable.StyleKey => typeof(Window);

        private Button _minimizeButton;
        private Button _maximizeButton;
        private Button _closeButton;

        private double _resizeBorderThickness = 5;
        private WindowEdge? _currentResizeEdge;

        public static readonly StyledProperty<bool> ShowCaptionBarSeparatorProperty =
            AvaloniaProperty.Register<Window, bool>(nameof(ShowCaptionBarSeparator));

        public bool ShowCaptionBarSeparator
        {
            get => GetValue(ShowCaptionBarSeparatorProperty);
            set => SetValue(ShowCaptionBarSeparatorProperty, value);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                var point = e.GetCurrentPoint(this);
                var pos = point.Position;

                var width = Bounds.Width;
                var height = Bounds.Height;

                if (pos.X <= _resizeBorderThickness &&
                    pos.Y <= _resizeBorderThickness)
                {
                    _currentResizeEdge = WindowEdge.NorthWest;
                    Cursor = new Cursor(StandardCursorType.TopLeftCorner);
                }
                else if (pos.X > _resizeBorderThickness && pos.X < width - _resizeBorderThickness &&
                         pos.Y <= _resizeBorderThickness)
                {
                    _currentResizeEdge = WindowEdge.North;
                    Cursor = new Cursor(StandardCursorType.TopSide);
                }
                else if (pos.X >= width - _resizeBorderThickness &&
                         pos.Y <= _resizeBorderThickness)
                {
                    _currentResizeEdge = WindowEdge.NorthEast;
                    Cursor = new Cursor(StandardCursorType.TopRightCorner);
                }
                else if (pos.X >= width - _resizeBorderThickness &&
                         pos.Y > _resizeBorderThickness && pos.Y < height - _resizeBorderThickness)
                {
                    _currentResizeEdge = WindowEdge.East;
                    Cursor = new Cursor(StandardCursorType.RightSide);
                }
                else if (pos.X >= width - _resizeBorderThickness &&
                         pos.Y >= height - _resizeBorderThickness)
                {
                    _currentResizeEdge = WindowEdge.SouthEast;
                    Cursor = new Cursor(StandardCursorType.BottomRightCorner);
                }
                else if (pos.X > _resizeBorderThickness && pos.X < width - _resizeBorderThickness &&
                         pos.Y >= height - _resizeBorderThickness)
                {
                    _currentResizeEdge = WindowEdge.South;
                    Cursor = new Cursor(StandardCursorType.BottomSide);
                }
                else if (pos.X <= _resizeBorderThickness &&
                         pos.Y >= height - _resizeBorderThickness)
                {
                    _currentResizeEdge = WindowEdge.SouthWest;
                    Cursor = new Cursor(StandardCursorType.BottomLeftCorner);
                }
                else if (pos.X <= _resizeBorderThickness &&
                         pos.Y < height - _resizeBorderThickness)
                {
                    _currentResizeEdge = WindowEdge.West;
                    Cursor = new Cursor(StandardCursorType.LeftSide);
                }
                else
                {
                    _currentResizeEdge = null;
                    Cursor = new Cursor(StandardCursorType.Arrow);
                }
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (_currentResizeEdge != null)
            {
                BeginResizeDrag(_currentResizeEdge.Value, e);
            }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            var captionBarBorder = e.NameScope.Find<Border>("PART_CaptionBar");

            _minimizeButton = e.NameScope.Find<Button>("PART_MinimizeButton");
            _maximizeButton = e.NameScope.Find<Button>("PART_MaximizeButton");
            _closeButton = e.NameScope.Find<Button>("PART_CloseButton");

            _minimizeButton.Click += (s, ev) =>
            {
                WindowState = WindowState.Minimized;
            };

            _maximizeButton.Click += (s, ev) =>
            {
                ToggleMaximize();
            };

            _closeButton.Click += (s, ev) =>
            {
                Close();
            };

            captionBarBorder.PointerPressed += (s, ev) =>
            {
                if (_currentResizeEdge != null)
                    return;

                if (_minimizeButton.IsPointerOver || _maximizeButton.IsPointerOver || _closeButton.IsPointerOver)
                    return;

                var updateKind = ev.GetCurrentPoint(this).Properties.PointerUpdateKind;
                if (updateKind == PointerUpdateKind.LeftButtonPressed)
                {
                    BeginMoveDrag(ev);
                }
            };

            captionBarBorder.DoubleTapped += (s, ev) =>
            {
                if (_minimizeButton.IsPointerOver || _maximizeButton.IsPointerOver || _closeButton.IsPointerOver)
                    return;
                
                if (captionBarBorder.IsPointerOver)
                {
                    ToggleMaximize();
                }
            };

            base.OnTemplateApplied(e);
        }

        private void ToggleMaximize()
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
    }
}