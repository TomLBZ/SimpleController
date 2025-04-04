namespace SimpleController;
using System.Drawing.Drawing2D;
using System.Numerics;

public partial class FormMain : Form
{
    private Matrix worldToPixel, pixelToWorld;
    private ObjectState chaser, runner;
    public FormMain()
    {
        InitializeComponent();
        (worldToPixel, pixelToWorld) = GetMatrices(); // initialize the transformation matrices
        TimFrame.Interval = (int)(1000.0f / FPS); // set the timer interval to 1/FPS seconds
        UpdateStateText(); // update the label with the current state of the start point
        runner = new ObjectState(new(RUNNER_HALF_LENGTH, RUNNER_HALF_LENGTH)); // initialize the target point to the center of the picture box
        chaser = new ObjectState(new(-RUNNER_HALF_LENGTH, RUNNER_HALF_LENGTH)); // initialize the start point to the origin
        PicMain.Refresh(); // refresh the picture box
    }

    private (Matrix, Matrix) GetMatrices()
    {
        int minLength = Math.Min(PicMain.Width, PicMain.Height); // the number of pixels in the shortest side of the picture box
        float unitSize = minLength / 2.0f / UNITS; // the size of one unit in pixels
        worldToPixel = new Matrix(unitSize, 0, 0, -unitSize, PicMain.Width / 2.0f, PicMain.Height / 2.0f); // transformation matrix to convert from world coordinates to pixel coordinates
        pixelToWorld = worldToPixel.Clone(); // clone the transformation matrix
        pixelToWorld.Invert(); // invert the transformation matrix
        return (worldToPixel, pixelToWorld); // return the transformation matrices
    }
    private static void DrawCircle(Graphics g, Vector2 point, float radius, Color color)
    {
        g.FillEllipse(new SolidBrush(color), point.X - radius, point.Y - radius, 2 * radius, 2 * radius); // draw a circle at the given point with the given radius and color
    }
    private static void DrawArrow(Graphics g, Vector2 tail, Vector2 head, Color color, float headLength = ARROW_HEAD_LENGTH, float headAngle = ARROW_HEAD_ANGLE)
    {
        Vector2 direction = head - tail; // calculate the direction vector from the tail to the head
        float length = direction.Length(); // calculate the length of the direction vector
        if (length == 0) return; // if the length is zero, do nothing
        direction /= length; // normalize the direction vector
        g.DrawLine(new Pen(color, PEN_WIDTH), tail.X, tail.Y, head.X, head.Y); // draw a line from the tail to the head
        Vector2 headPoint1 = head + Vector2.Transform(-direction, Matrix3x2.CreateRotation(-headAngle)) * headLength; // rotate the direction vector by -headAngle radians
        Vector2 headPoint2 = head + Vector2.Transform(-direction, Matrix3x2.CreateRotation(headAngle)) * headLength; // rotate the direction vector by headAngle radians
        g.DrawLine(new Pen(color, PEN_WIDTH), head.X, head.Y, headPoint1.X, headPoint1.Y); // draw a line from the head to the first arrowhead point
        g.DrawLine(new Pen(color, PEN_WIDTH), head.X, head.Y, headPoint2.X, headPoint2.Y); // draw a line from the head to the second arrowhead point
    }
    private static void DrawLine(Graphics g, Vector2 start, Vector2 end, Color color, DashStyle style = DashStyle.Solid)
    {
        using Pen dashedPen = new(color, PEN_WIDTH) { DashStyle = style }; // create a dashed pen with the given color
        g.DrawLine(dashedPen, start.X, start.Y, end.X, end.Y); // draw a dashed line from the start point to the end point
    }
    private static void DrawLines(Graphics g, Vector2[] points, Color color, DashStyle style = DashStyle.Solid)
    {
        using Pen dashedPen = new(color, PEN_WIDTH) { DashStyle = style }; // create a dashed pen with the given color
        for (int i = 0; i < points.Length - 1; i++)
        {
            g.DrawLine(dashedPen, points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y); // draw a dashed line from the current point to the next point
        }
    }
    private PointF PointToWorld(PointF pt)
    {
        PointF[] pts = { pt };
        pixelToWorld.TransformPoints(pts); // transform the point to the coordinate system of the picture box
        return pts[0]; // return the transformed point
    }
    private void PicMainPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.Transform = worldToPixel; // set the transformation matrix to the graphics object
        RedrawToGraphics(e.Graphics); // redraw the picture box
    }
    private void UpdateStateText()
    {
        LblState.Text = chaser.ToString(); // update the label with the current state of the start point
    }
    private void UpdateFrame(object sender, EventArgs e) // occurs at a fixed rate
    {
        UpdatePhysics(); // update the physics
        UpdateStateText(); // update the label with the current state of the start point
        PicMain.Refresh(); // refresh the picture box
    }
    private void PicSizeChanged(object sender, EventArgs e)
    {
        (worldToPixel, pixelToWorld) = GetMatrices(); // update the transformation matrices
        PicMain.Refresh(); // refresh the picture box
    }
    private void StartSimulation(object sender, EventArgs e)
    {
        TimFrame.Enabled = true; // start the timer
    }
    private void StopSimulation(object sender, EventArgs e)
    {
        TimFrame.Enabled = false; // stop the timer
    }
    private void PicMainMouseDown(object sender, MouseEventArgs e)
    {
        switch (e.Button)
        {
            case MouseButtons.Left:
                chaser = new(PointToWorld(e.Location)); // set the start point to the clicked point
                break;
            case MouseButtons.None:
                break;
            case MouseButtons.Right:
                runner = new(PointToWorld(e.Location)); // set the target point to the clicked point
                break;
            case MouseButtons.Middle:
                break;
            case MouseButtons.XButton1:
                break;
            case MouseButtons.XButton2:
                break;
            default:
                break;
        }
        PicMain.Refresh(); // refresh the picture box
    }
}
