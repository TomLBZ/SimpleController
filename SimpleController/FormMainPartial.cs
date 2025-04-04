using System.Numerics;
using System.Drawing.Drawing2D;

namespace SimpleController;
public partial class FormMain : Form
{
    private const float FPS = 60.0f;
    private const int UNITS = 5; // number of units in the picture box
    private const float MAX_ACCEL = 9.8f; // units per second
    private const float MAX_ARROW_LENGTH = 0.25f; // length of the arrow representing the maximum accelerationtrength
    private const float ARROW_HEAD_LENGTH = 0.05f * UNITS; // length of the arrowhead
    private const float ARROW_HEAD_ANGLE = (float)Math.PI / 4f; // angle of the arrowhead
    private const float DRAG_COEFF = 0.5f; // dragForce coefficient
    private const float RUNNER_V = 0.01f; // target velocity in units per second
    private const float RUNNER_HALF_LENGTH = 0.5f * UNITS; // half the length of the target point travel path
    private const float PEN_WIDTH = 0.01f * UNITS; // width of the pen used to draw the lines
    private const float OBJ_RADIUS = 0.02f * UNITS; // radius of the objects in units
    private const int HISTORY_SIZE = 1024; // size of the history array

    private readonly Queue<Vector2> chaserHistory = new(HISTORY_SIZE); // queue to store the history of start points
    private readonly Queue<Vector2> runnerHistory = new(HISTORY_SIZE); // queue to store the history of target points
    struct ObjectState(PointF p)
    {
        public Vector2 Position = new(p.X, p.Y); // position vector of the point
        public Vector2 Velocity = new(0, 0); // velocity vector of the point
        public Vector2 Acceleration = new(0, 0); // accelerationtrength vector of the point
        public void Next(Vector2 acceleration)
        {
            Position += Velocity / FPS; // update the position based on the velocity and time step
            Velocity += Acceleration / FPS; // update the velocity based on the accelerationtrength and time step
            float speed = Velocity.Length(); // calculate the speed of the point
            float dragForce = speed * speed * DRAG_COEFF; // calculate the deceleration based on the velocity squared and dragForce coefficient
            Vector2 decceleration = speed == 0 ? new(0, 0) : Vector2.Normalize(Velocity) * dragForce; // calculate the deceleration vector based on the accelerationtrength and dragForce
            Acceleration = acceleration - decceleration; // update the accelerationtrength based on the target accelerationtrength and deceleration
        }
        public override readonly string ToString()
        {
            string posStr = $"({    Position.X:00.000000;-0.000000}, {      Position.Y:00.000000;-0.000000})"; // convert the position vector to a string
            string velStr = $"({    Velocity.X:00.000000;-0.000000}, {      Velocity.Y:00.000000;-0.000000})"; // convert the velocity vector to a string
            string accStr = $"({Acceleration.X:00.000000;-0.000000}, {  Acceleration.Y:00.000000;-0.000000})"; // convert the accelerationtrength vector to a string
            string positionString       = "====== Position ======";
            string velocityString       = "====== Velocity ======";
            string accelerationString   = "==== Acceleration ====";
            return $"{positionString} | {velocityString} | {accelerationString}\n{posStr}   {velStr}   {accStr}"; // format the string with the position, velocity, and accelerationtrength vectors'
        }
    }
    private void DrawBackground(Graphics g)
    {
        g.Clear(Color.White); // clear the picture box
        using Pen dashedPen = new(Color.LightGray, PEN_WIDTH) { DashStyle = DashStyle.Dot }; // create a dashed pen with light gray color
        for (int i = -UNITS; i <= UNITS; i++) // draw the grid lines in both directions
        {
            g.DrawLine(dashedPen, i, -UNITS, i, UNITS); // draw vertical lines
            g.DrawLine(dashedPen, -UNITS, i, UNITS, i); // draw horizontal lines
        }
    }
    private void RedrawToGraphics(Graphics g)
    {
        DrawBackground(g); // draw the background
        DrawLines(g, runnerHistory.ToArray(), Color.Pink, DashStyle.Dot); // draw the history of the target points with blue color
        DrawLines(g, chaserHistory.ToArray(), Color.Cyan, DashStyle.Dot); // draw the history of the start points with red color
        DrawLine(g, chaser.Position, runner.Position, Color.Black, DashStyle.Dash); // draw a dashed line from the start point to the target point with black color
        DrawCircle(g, runner.Position, OBJ_RADIUS, Color.HotPink); // draw a circle at the target point with radius 0.05 units and blue color
        DrawCircle(g, chaser.Position, OBJ_RADIUS, Color.DarkCyan); // draw a circle at the start point with radius 0.05 units and red color
        float accelerationArrowHead = chaser.Acceleration.Length() * MAX_ARROW_LENGTH / MAX_ACCEL; // calculate the length of the accelerationtrength arrowhead
        DrawArrow(g, chaser.Position, chaser.Position + accelerationArrowHead * chaser.Acceleration, Color.Red); // draw an arrow representing the accelerationtrength vector with red color
        float velocityArrowHead = chaser.Velocity.Length() * MAX_ARROW_LENGTH / MAX_ACCEL; // calculate the length of the velocity arrowhead
        DrawArrow(g, chaser.Position, runner.Position + velocityArrowHead * chaser.Velocity, Color.Blue); // draw an arrow representing the velocity vector with blue color
    }
    private void RunnerMove()
    {
        if (runner.Position.X == -RUNNER_HALF_LENGTH)
        {
            if (runner.Position.Y < RUNNER_HALF_LENGTH) runner.Position.Y += RUNNER_V; // move the target point up
            else runner.Position.Y = RUNNER_HALF_LENGTH; // stop moving the target point
        } else if (runner.Position.X == RUNNER_HALF_LENGTH)
        {
            if (runner.Position.Y > -RUNNER_HALF_LENGTH) runner.Position.Y -= RUNNER_V; // move the target point down
            else runner.Position.Y = -RUNNER_HALF_LENGTH; // stop moving the target point
        }
        if (runner.Position.Y == -RUNNER_HALF_LENGTH)
        {
            if (runner.Position.X > -RUNNER_HALF_LENGTH) runner.Position.X -= RUNNER_V; // move the target point left
            else runner.Position.X = -RUNNER_HALF_LENGTH; // stop moving the target point
        } else if (runner.Position.Y == RUNNER_HALF_LENGTH)
        {
            if (runner.Position.X < RUNNER_HALF_LENGTH) runner.Position.X += RUNNER_V; // move the target point right
            else runner.Position.X = RUNNER_HALF_LENGTH; // stop moving the target point
        }
    }
    private void UpdatePhysics()
    {
        if (chaserHistory.Count >= HISTORY_SIZE) chaserHistory.Dequeue(); // remove the oldest point from the queue if the queue is full
        chaserHistory.Enqueue(chaser.Position); // add the current position to the queue
        if (runnerHistory.Count >= HISTORY_SIZE) runnerHistory.Dequeue(); // remove the oldest point from the queue if the queue is full
        runnerHistory.Enqueue(runner.Position); // add the current position to the queue
        RunnerMove(); // move the target point
        Vector2 displacement = runner.Position - chaser.Position; // calculate the displacement vector from the start point to the target point
        float distance = displacement.Length(); // calculate the distance between the start point and the target point
        // maps the distance from [0, infinity] to the range [0, MAX_ACCEL] that asymptotically approaches MAX_ACCEL
        float accelerationtrength = MAX_ACCEL * (float)(Math.Atan(distance) * 2 / Math.PI); // calculate the accelerationtrength based on the distance
        Vector2 purePursuitAcceleration = Vector2.Normalize(displacement) * accelerationtrength; // calculate the accelerationtrength vector towards the target point based on the maximum accelerationtrength
        Vector2 adjustedAcceleration = purePursuitAcceleration - chaser.Velocity; // calculate the accelerationtrength vector to adjust the velocity to zero
        float adjustedAccelerationLength = adjustedAcceleration.Length(); // calculate the length of the adjusting accelerationtrength vector
        if (adjustedAccelerationLength > MAX_ACCEL) adjustedAcceleration = Vector2.Normalize(adjustedAcceleration) * MAX_ACCEL; // limit the length of the adjusting accelerationtrength vector to the maximum accelerationtrength
        chaser.Next(adjustedAcceleration); // update the start point based on the target point
    }

}