using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Atlas.UI
{
    public class Window : Avalonia.Controls.Window
    {
        private double _resizeBorderThickness = 6.5;
        private WindowEdge? _currentResizeEdge;

        protected override void OnPointerMoved(PointerEventArgs e)
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
            
            captionBarBorder.PointerPressed += (s, ev) =>
            {
                if (_currentResizeEdge != null)
                    return;
                
                if (ev.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
                {
                    BeginMoveDrag(ev);
                }
            };

            base.OnTemplateApplied(e);
        }
    }
}