using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
namespace Software_Application_Assignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool _wall = false;
        private bool _openings = false;
        private bool _isDrawing = false;
        private bool _openingsWindow = false;
        Line LineType;
        Polygon DoorType;
        PointCollection polygonPoints;
        Line NewLine;
        Polygon DoorPolygon;
        Polygon WindowPolygon;
        List<Line> WallLines = new();
        List<Polygon> DoorLines = new();
        List<Polygon> WindowLines = new();
        // White and black Brush For Door And Window
        SolidColorBrush WhiteBrush = new() { Color = Colors.White};
        SolidColorBrush blackBrush = new() { Color = Colors.Black};
        Point Start;
        Point End;
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_wall)//for Wall Radio Button 
            {

                if (!_isDrawing)
                {
                    Start = e.GetPosition(this);
                    _isDrawing=true;
                    return;
                }
                else
                {
                    //termination of Wall drawing 
                    if (NewLine != null&& !WallLines.Contains(NewLine))
                    {
                        WallLines.Add(NewLine);
                    }
                    Redraw();

                    _isDrawing = false;
                    return;
                }
            }
            if (_openings)//for Openings Radio Button 
            {
                
                
                if (e.Source is Line)
                {
                    if (!_isDrawing)
                    {
                        LineType = (Line)e.Source;
                        Start = e.GetPosition(this);
                        _isDrawing = true;
                        
                        return;
                    }


                }
                else if (e.Source is Polygon)
                {
                    if (!_isDrawing&&!_openingsWindow)
                    {
                        if (DoorLines.Contains((Polygon)e.Source))
                        {
                            DoorType = (Polygon)e.Source;
                            Start = e.GetPosition(this);
                            DoorLines.Remove((Polygon)e.Source);
                            _isDrawing = true;
                            _openingsWindow = true;
                            return ;
                        }
                    }

                    
                }
                
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_wall)
            {

                if (!_isDrawing)
                {
                    return; 
                }
                if (_isDrawing)
                {
                    Redraw();
                    End = e.GetPosition(this);
                    NewLine = new Line
                    {
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };

                    var Angle= Math.Round(180/Math.PI*Math.Atan(Math.Abs(End.Y - Start.Y) / Math.Abs(End.X - Start.X)),1);
                    if(Math.Abs(Angle) <= 45)
                    {
                        NewLine.X1 = Start.X;
                        NewLine.Y1 = Start.Y;
                        NewLine.X2 = End.X;
                        NewLine.Y2 = Start.Y;
                        DrawingCanvas.Children.Add(NewLine);
                    }
                    else
                    {
                        NewLine.X1 = Start.X;
                        NewLine.Y1 = Start.Y;
                        NewLine.X2 = Start.X;
                        NewLine.Y2 = End.Y;
                        DrawingCanvas.Children.Add(NewLine);
                    }
                  
                }
            }
            if (_openings)
            {
                if (!_isDrawing)
                {
                    return;
                }
                if (_isDrawing)
                {
                    if (_openingsWindow)//window
                    {
                        Redraw();

                        End = e.GetPosition(this);
                        if (DoorType.Points[0].Y==DoorType.Points[1].Y)
                        {
                            if (End.Y <= DoorType.Points[1].Y)
                            {
                                if (End.X <= DoorType.Points[1].X)
                                {
                                    CreateWindow(DoorType.Points[1], -1, -1, true);
                                }
                                else
                                {

                                    CreateWindow(DoorType.Points[1], 1, -1, true);
                                }

                            }
                            else
                            {
                                if (End.X <= DoorType.Points[1].X)
                                {
                                    CreateWindow(DoorType.Points[1], -1, 1, true);

                                }
                                else
                                {
                                    CreateWindow(DoorType.Points[1], 1, 1, true);
                                }

                            }


                        }
                        else
                        {
                            if (End.X <= DoorType.Points[1].X)
                            {
                                if (End.Y <= DoorType.Points[1].Y)
                                {
                                    CreateWindow(DoorType.Points[1], -1, -1, false);
                                }
                                else
                                {

                                    CreateWindow(DoorType.Points[1], 1, -1, false);
                                }

                            }
                            else
                            {
                                if (End.Y <= DoorType.Points[1].Y)
                                {
                                    CreateWindow(DoorType.Points[1], -1, 1, false);

                                }
                                else
                                {
                                    CreateWindow(DoorType.Points[1], 1, 1, false);
                                }

                            }

                        }

                    }
                    else//door  
                    {

                        Redraw();

                        End = e.GetPosition(this);
                    
                        if (LineType.Y1 == LineType.Y2)
                        {
                            if (End.Y <= Start.Y)
                            {
                                if (End.X <= Start.X)
                                {
                                    CreateDoor(Start, -60, -5, 1, true);
                                }
                                else 
                                {

                                    CreateDoor(Start, -60, -5, -1, true);
                                }

                            }
                            else 
                            {
                                if (End.X <= Start.X)
                                {
                                    CreateDoor(Start, 60, 5, -1, true);

                                }
                                else 
                                {
                                    CreateDoor(Start, 60, 5, 1, true);
                                }

                            }

                        }
                        else
                        {
                            if (End.X <= Start.X)
                            {
                                if (End.Y <= Start.Y)
                                {
                                    CreateDoor(Start, -60, -5, 1, false);
                                }
                                else 
                                {

                                    CreateDoor(Start, -60, -5, -1, false);
                                }

                            }
                            else 
                            {
                                if (End.Y <= Start.Y)
                                {
                                    CreateDoor(Start, 60, 5, -1, false);

                                }
                                else 
                                {
                                    CreateDoor(Start, 60, 5, 1, false);
                                }

                            }

                        }
                    }

                }

            }

        }
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            if (_openings)
            {
                if (_isDrawing)
                {
                    if (_openingsWindow)//window
                    {
                        Redraw();

                        End = e.GetPosition(this);
                        if (DoorType.Points[0].Y == DoorType.Points[1].Y)
                        {
                            if (End.Y <= DoorType.Points[1].Y)
                            {
                                if (End.X <= DoorType.Points[1].X)
                                {
                                    CreateWindow(DoorType.Points[1], -1, -1, true);
                                }
                                else
                                {

                                    CreateWindow(DoorType.Points[1], 1, -1, true);
                                }

                            }
                            else
                            {
                                if (End.X <= DoorType.Points[1].X)
                                {
                                    CreateWindow(DoorType.Points[1], -1, 1, true);

                                }
                                else
                                {
                                    CreateWindow(DoorType.Points[1], 1, 1, true);
                                }

                            }


                        }
                        else
                        {
                            if (End.X <= DoorType.Points[1].X)
                            {
                                if (End.Y <= DoorType.Points[1].Y)
                                {
                                    CreateWindow(DoorType.Points[1], -1, -1, false);
                                }
                                else
                                {

                                    CreateWindow(DoorType.Points[1], 1, -1, false);
                                }

                            }
                            else
                            {
                                if (End.Y <= DoorType.Points[1].Y)
                                {
                                    CreateWindow(DoorType.Points[1], -1, 1, false);

                                }
                                else
                                {
                                    CreateWindow(DoorType.Points[1], 1, 1, false);
                                }

                            }

                        }
                        //termination of Window drawing 
                        if (WindowPolygon != null && !WindowLines.Contains(WindowPolygon))
                        {
                            WindowLines.Add(WindowPolygon);

                        }
                        _isDrawing = false;
                        _openingsWindow = false;
                        DoorType = new Polygon();
                        return;
                    }
                    else
                    {

                        Redraw();

                        End = e.GetPosition(this);

                        if (LineType.Y1 == LineType.Y2)
                        {
                            if (End.Y <= Start.Y)
                            {
                                if (End.X <= Start.X)
                                {
                                    CreateDoor(Start, -60, -5, 1, true);

                                }
                                else
                                {

                                    CreateDoor(Start, -60, -5, -1, true);

                                }

                            }
                            else
                            {
                                if (End.X <= Start.X)
                                {
                                    CreateDoor(Start, 60, 5, -1, true);


                                }
                                else
                                {
                                    CreateDoor(Start, 60, 5, 1, true);

                                }

                            }

                        }
                        else//door
                        {
                            if (End.X <= Start.X)
                            {
                                if (End.Y <= Start.Y)
                                {
                                    CreateDoor(Start, -60, -5, 1, false);

                                }
                                else
                                {

                                    CreateDoor(Start, -60, -5, -1, false);

                                }

                            }
                            else
                            {
                                if (End.Y <= Start.Y)
                                {
                                    CreateDoor(Start, 60, 5, -1, false);


                                }
                                else
                                {
                                    CreateDoor(Start, 60, 5, 1, false);

                                }

                            }

                        }
                        //termination of Door drawing 
                        if (DoorPolygon != null && !DoorLines.Contains(DoorPolygon))
                        {
                            DoorLines.Add(DoorPolygon);

                        }
                        _isDrawing = false;
                        LineType = new Line();
                        return;
                    }
                }
            }
        }

        private void CreateDoor(Point p,int r,int offset,int mir,bool horizontal )
        {
            if (!horizontal)
            {
                // Create a collection of points for a polygon
                polygonPoints = new PointCollection
                {
                    new Point(p.X, p.Y + (offset * mir)),
                    new Point(p.X, p.Y),
                    new Point(p.X + r, p.Y),
                    new Point(p.X + r, p.Y + (offset * mir)),
                    new Point(p.X + offset/* +5 so it wont bug*/, p.Y + (offset * mir))
                };

                // Create a Polygon  
                DoorPolygon = new Polygon
                {
                    Stroke = blackBrush,
                    Fill = WhiteBrush,
                    StrokeThickness = 1
                };
                // Create a collection of points for a polygon  
                for (int i = 0; i <= 90; i += 9)
                {
                    polygonPoints.Add(
                    new Point() { X =  p.X + (r * Math.Cos(Math.PI * i / 180.0)) , Y =p.Y + (offset * mir) + ((r * mir) * Math.Sin(Math.PI * i / 180.0))});
                }
                // Set Polygon.Points properties  
                DoorPolygon.Points = polygonPoints;
                DoorPolygon.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
                DoorPolygon.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
                DoorPolygon.MouseRightButtonDown += Canvas_MouseRightButtonDown;
                DrawingCanvas.Children.Add(DoorPolygon);
            }
            else
            {
                // Create a collection of points for a polygon
                polygonPoints = new PointCollection
                {
                    new Point(p.X + (offset * mir), p.Y),
                    new Point(p.X, p.Y),
                    new Point(p.X, p.Y + r),
                    new Point(p.X + (offset * mir), p.Y + r),
                    new Point(p.X + (offset * mir), p.Y + offset/* +5 so it wont bug*/)
                };

                // Create a Polygon  
                DoorPolygon = new Polygon
                {
                    Stroke = blackBrush,
                    Fill = WhiteBrush,
                    StrokeThickness = 1
                };
                // Create a collection of points for a polygon  
                for (int i = 0; i <= 90; i += 9)
                {
                    polygonPoints.Add(
                    new Point() { X = p.X + (offset*mir) + ((r*mir) * Math.Sin(Math.PI * i / 180.0)), Y = p.Y + (r * Math.Cos(Math.PI * i / 180.0)) });
                }
                // Set Polygon.Points properties  
                DoorPolygon.Points = polygonPoints;
                DoorPolygon.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
                DoorPolygon.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
                DoorPolygon.MouseRightButtonDown += Canvas_MouseRightButtonDown;
                DrawingCanvas.Children.Add(DoorPolygon);
            }
           


        }
        private void CreateWindow(Point p, int mirX,int mirY, bool horizontal)
        {
            if (!horizontal)
            {
                // Create a collection of points for a polygon 
                polygonPoints = new PointCollection
                {
                    new Point(p.X + (5 * mirY), p.Y + (5 * mirX)),
                    new Point(p.X + (10 * mirY), p.Y + (5 * mirX)),
                    new Point(p.X + (10 * mirY), p.Y + (0 * mirX)),
                    new Point(p.X + (0 * mirY), p.Y + (0 * mirX)),
                    new Point(p.X + (0 * mirY), p.Y + (5 * mirX)),
                    new Point(p.X + (5 * mirY), p.Y + (5 * mirX)),
                    new Point(p.X + (5 * mirY), p.Y + (65 * mirX)),
                    new Point(p.X + (0 * mirY), p.Y + (65 * mirX)),
                    new Point(p.X + (0 * mirY), p.Y + (5 * mirX)),
                    new Point(p.X + (5 * mirY), p.Y + (5 * mirX)),
                    new Point(p.X + (10 * mirY), p.Y + (5 * mirX)),
                    new Point(p.X + (10 * mirY), p.Y + (65 * mirX)),
                    new Point(p.X + (5 * mirY) , p.Y +(65 * mirX)),
                    new Point(p.X + (5 * mirY), p.Y +(5 * mirX)),
                    new Point(p.X + (10 * mirY), p.Y +(5 * mirX)),
                    new Point(p.X + (10 * mirY), p.Y + (70 * mirX)),
                    new Point(p.X + (0 * mirY), p.Y + (70 * mirX)),
                    new Point(p.X + (0 * mirY), p.Y + (65 * mirX)),
                    new Point(p.X + (5 * mirY), p.Y + (65 * mirX)),
                    new Point(p.X + (10 * mirY), p.Y + (65 * mirX)),
                    new Point(p.X + (10 * mirY), p.Y + (5 * mirX)),
                    new Point(p.X + (5 * mirY), p.Y + (5 * mirX))
                };


                // Create a Polygon  
                WindowPolygon = new Polygon
                {
                    Stroke = blackBrush,
                    Fill = WhiteBrush,
                    StrokeThickness = 1,
                     

                    // Set Polygon.Points properties  
                    Points = polygonPoints
                };
                WindowPolygon.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
                WindowPolygon.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
                WindowPolygon.MouseRightButtonDown += Canvas_MouseRightButtonDown; 
                DrawingCanvas.Children.Add(WindowPolygon);

            }
            else
            {
                // Create a collection of points for a polygon
                polygonPoints = new PointCollection
                {
                    new Point(p.X + (5 * mirX), p.Y + (mirY * 5)),
                    new Point(p.X + (5 * mirX), p.Y + (mirY * 10)),
                    new Point(p.X + (0 * mirX), p.Y + (mirY * 10)),
                    new Point(p.X + (0 * mirX), p.Y + (mirY * 0)),
                    new Point(p.X + (5 * mirX), p.Y + (mirY * 0)),
                    new Point(p.X + (5 * mirX), p.Y + (mirY * 5)),
                    new Point(p.X + (65 * mirX), p.Y + (mirY * 5)),
                    new Point(p.X + (65 * mirX), p.Y + (mirY * 0)),
                    new Point(p.X + (5 * mirX), p.Y + (mirY * 0)),
                    new Point(p.X + (5 * mirX), p.Y + (mirY * 5)),
                    new Point(p.X + (5 * mirX), p.Y + (mirY * 10)),
                    new Point(p.X + (65 * mirX), p.Y + (mirY * 10)),
                    new Point(p.X + (65 * mirX), p.Y + (mirY * 5)),
                    new Point(p.X + (5 * mirX), p.Y + (mirY * 5)),
                    new Point(p.X + (5 * mirX), p.Y + (mirY * 10)),
                    new Point(p.X + (70 * mirX), p.Y + (mirY * 10)),
                    new Point(p.X + (70 * mirX), p.Y + (mirY * 0)),
                    new Point(p.X + (65 * mirX), p.Y + (mirY * 0)),
                    new Point(p.X + (65 * mirX), p.Y + (mirY * 5)),
                    new Point(p.X + (65 * mirX), p.Y + (mirY * 10)),
                    new Point(p.X + (5 * mirX), p.Y + (mirY * 10)),
                    new Point(p.X + (5 * mirX), p.Y + (mirY * 5))   
                };

                // Create a Polygon  
                WindowPolygon = new Polygon
                {
                    Stroke = blackBrush,
                    Fill = WhiteBrush,
                    StrokeThickness = 1,  

                    // Set Polygon.Points properties  
                    Points = polygonPoints
                };
                WindowPolygon.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
                WindowPolygon.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
                WindowPolygon.MouseRightButtonDown += Canvas_MouseRightButtonDown;
                DrawingCanvas.Children.Add(WindowPolygon);
            }
           

        }
        private void CreateWall(object sender, RoutedEventArgs e)
        {

            _wall = true;
            _openings = false;
            _openingsWindow = false;
            _isDrawing=false;
            Redraw();

        }

        private void CreateOpenings(object sender, RoutedEventArgs e)
        {
            _openings = true;
            _wall = false;
            _openingsWindow = false;
            _isDrawing = false;
            Redraw();
        }
        private void Redraw()
        {
            DrawingCanvas.Children.Clear();
            foreach (Line line in WallLines)
            {
                DrawingCanvas.Children.Add(line);
            }
            foreach (Polygon polygon in DoorLines)
            {
                DrawingCanvas.Children.Add(polygon);
            }
            foreach (Polygon polygon in WindowLines)
            {
                DrawingCanvas.Children.Add(polygon);
            }

        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _openingsWindow = false;
            _isDrawing = false;
            if (e.Source is Line)
            {
                if (WallLines.Contains((Line)e.Source))
                {
                    WallLines.Remove((Line)e.Source);
                    Redraw();
                    return;
                }
            }
            else if (e.Source is Polygon)
            {
                if (DoorLines.Contains((Polygon)e.Source))
                {
                    DoorLines.Remove((Polygon)e.Source);
                    Redraw();
                    return;
                }
                else if (WindowLines.Contains((Polygon)e.Source))
                {
                    WindowLines.Remove((Polygon)e.Source);
                    Redraw();
                    return;

                }
            }
            Redraw();
        }

        private void ClearCanvas(object sender, RoutedEventArgs e)
        {
            _openingsWindow = false;
            _isDrawing = false;
            WallLines.Clear();
            DoorLines.Clear();
            WindowLines.Clear();
            DrawingCanvas.Children.Clear();
            RadioButtonWall.IsChecked = true;
        }
    }
}

