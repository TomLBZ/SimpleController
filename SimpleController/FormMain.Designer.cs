namespace SimpleController
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            GrpControls = new GroupBox();
            LblState = new Label();
            BtnStop = new Button();
            BtnStart = new Button();
            PicMain = new PictureBox();
            TimFrame = new System.Windows.Forms.Timer(components);
            GrpControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PicMain).BeginInit();
            SuspendLayout();
            // 
            // GrpControls
            // 
            GrpControls.Controls.Add(LblState);
            GrpControls.Controls.Add(BtnStop);
            GrpControls.Controls.Add(BtnStart);
            GrpControls.Dock = DockStyle.Bottom;
            GrpControls.Location = new Point(0, 612);
            GrpControls.Name = "GrpControls";
            GrpControls.Size = new Size(1002, 100);
            GrpControls.TabIndex = 1;
            GrpControls.TabStop = false;
            GrpControls.Text = "Controls";
            // 
            // LblState
            // 
            LblState.AutoSize = true;
            LblState.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblState.Location = new Point(264, 30);
            LblState.Name = "LblState";
            LblState.Size = new Size(80, 22);
            LblState.TabIndex = 2;
            LblState.Text = "State: ";
            // 
            // BtnStop
            // 
            BtnStop.Location = new Point(138, 30);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new Size(120, 58);
            BtnStop.TabIndex = 1;
            BtnStop.Text = "Stop";
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Click += StopSimulation;
            // 
            // BtnStart
            // 
            BtnStart.Location = new Point(12, 30);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(120, 58);
            BtnStart.TabIndex = 0;
            BtnStart.Text = "Start";
            BtnStart.UseVisualStyleBackColor = true;
            BtnStart.Click += StartSimulation;
            // 
            // PicMain
            // 
            PicMain.Dock = DockStyle.Fill;
            PicMain.Location = new Point(0, 0);
            PicMain.Name = "PicMain";
            PicMain.Size = new Size(1002, 612);
            PicMain.TabIndex = 2;
            PicMain.TabStop = false;
            PicMain.SizeChanged += PicSizeChanged;
            PicMain.Paint += PicMainPaint;
            PicMain.MouseDown += PicMainMouseDown;
            // 
            // TimFrame
            // 
            TimFrame.Tick += UpdateFrame;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1002, 712);
            Controls.Add(PicMain);
            Controls.Add(GrpControls);
            MinimumSize = new Size(1024, 768);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Simple Controller";
            GrpControls.ResumeLayout(false);
            GrpControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PicMain).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox GrpControls;
        private PictureBox PicMain;
        private Button BtnStart;
        private System.Windows.Forms.Timer TimFrame;
        private Button BtnStop;
        private Label LblState;
    }
}
