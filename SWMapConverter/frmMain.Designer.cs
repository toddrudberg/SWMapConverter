namespace SWMapConverter
{
    partial class frmMain
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
      btnSWMapsToSW = new Button();
      btnConvertSWPointsToLatLon = new Button();
      lblSWMapsFile = new LinkLabel();
      lblSWPointsFile = new LinkLabel();
      lblSWMapsPointsCount = new Label();
      lblSWPointsCount = new Label();
      lblInstructions = new Label();
      btnOtherSWMapsToSW = new Button();
      lblOtherSWMapsFile = new LinkLabel();
      lblOtherSWMapsPointsCount = new Label();
      SuspendLayout();
      // 
      // btnSWMapsToSW
      // 
      btnSWMapsToSW.Location = new Point(235, 85);
      btnSWMapsToSW.Margin = new Padding(2);
      btnSWMapsToSW.Name = "btnSWMapsToSW";
      btnSWMapsToSW.Size = new Size(503, 45);
      btnSWMapsToSW.TabIndex = 0;
      btnSWMapsToSW.Text = "Basis SWMaps To SolidWorks Points";
      btnSWMapsToSW.UseVisualStyleBackColor = true;
      btnSWMapsToSW.Click += btnSWMapsToSW_Click;
      // 
      // btnConvertSWPointsToLatLon
      // 
      btnConvertSWPointsToLatLon.Location = new Point(235, 280);
      btnConvertSWPointsToLatLon.Margin = new Padding(2);
      btnConvertSWPointsToLatLon.Name = "btnConvertSWPointsToLatLon";
      btnConvertSWPointsToLatLon.Size = new Size(503, 45);
      btnConvertSWPointsToLatLon.TabIndex = 1;
      btnConvertSWPointsToLatLon.Text = "Convert SolidWorks Points to SWMaps CSV";
      btnConvertSWPointsToLatLon.UseVisualStyleBackColor = true;
      btnConvertSWPointsToLatLon.Click += btnConvertSWPointsToLatLon_Click;
      // 
      // lblSWMapsFile
      // 
      lblSWMapsFile.Location = new Point(237, 49);
      lblSWMapsFile.Name = "lblSWMapsFile";
      lblSWMapsFile.Size = new Size(1308, 44);
      lblSWMapsFile.TabIndex = 3;
      lblSWMapsFile.TabStop = true;
      lblSWMapsFile.Text = "linkLabel1";
      lblSWMapsFile.LinkClicked += lblSWMapsFile_LinkClicked;
      // 
      // lblSWPointsFile
      // 
      lblSWPointsFile.Location = new Point(237, 243);
      lblSWPointsFile.Name = "lblSWPointsFile";
      lblSWPointsFile.Size = new Size(1248, 44);
      lblSWPointsFile.TabIndex = 4;
      lblSWPointsFile.TabStop = true;
      lblSWPointsFile.Text = "linkLabel1";
      lblSWPointsFile.LinkClicked += linkLabel1_LinkClicked;
      // 
      // lblSWMapsPointsCount
      // 
      lblSWMapsPointsCount.AutoSize = true;
      lblSWMapsPointsCount.Location = new Point(759, 95);
      lblSWMapsPointsCount.Name = "lblSWMapsPointsCount";
      lblSWMapsPointsCount.Size = new Size(139, 32);
      lblSWMapsPointsCount.TabIndex = 5;
      lblSWMapsPointsCount.Text = "Not Loaded";
      // 
      // lblSWPointsCount
      // 
      lblSWPointsCount.AutoSize = true;
      lblSWPointsCount.Location = new Point(759, 286);
      lblSWPointsCount.Name = "lblSWPointsCount";
      lblSWPointsCount.Size = new Size(139, 32);
      lblSWPointsCount.TabIndex = 6;
      lblSWPointsCount.Text = "Not Loaded";
      // 
      // lblInstructions
      // 
      lblInstructions.Location = new Point(149, 402);
      lblInstructions.Name = "lblInstructions";
      lblInstructions.Size = new Size(1336, 516);
      lblInstructions.TabIndex = 7;
      lblInstructions.Text = "First Load the SWMaps File.";
      // 
      // btnOtherSWMapsToSW
      // 
      btnOtherSWMapsToSW.Location = new Point(237, 180);
      btnOtherSWMapsToSW.Margin = new Padding(2);
      btnOtherSWMapsToSW.Name = "btnOtherSWMapsToSW";
      btnOtherSWMapsToSW.Size = new Size(503, 45);
      btnOtherSWMapsToSW.TabIndex = 8;
      btnOtherSWMapsToSW.Text = "Other SWMaps To SolidWorks Points";
      btnOtherSWMapsToSW.UseVisualStyleBackColor = true;
      btnOtherSWMapsToSW.Click += btnOtherSWMapsToSW_Click;
      // 
      // lblOtherSWMapsFile
      // 
      lblOtherSWMapsFile.Location = new Point(237, 143);
      lblOtherSWMapsFile.Name = "lblOtherSWMapsFile";
      lblOtherSWMapsFile.Size = new Size(1248, 44);
      lblOtherSWMapsFile.TabIndex = 9;
      lblOtherSWMapsFile.TabStop = true;
      lblOtherSWMapsFile.Text = "linkLabel1";
      // 
      // lblOtherSWMapsPointsCount
      // 
      lblOtherSWMapsPointsCount.AutoSize = true;
      lblOtherSWMapsPointsCount.Location = new Point(759, 187);
      lblOtherSWMapsPointsCount.Name = "lblOtherSWMapsPointsCount";
      lblOtherSWMapsPointsCount.Size = new Size(139, 32);
      lblOtherSWMapsPointsCount.TabIndex = 10;
      lblOtherSWMapsPointsCount.Text = "Not Loaded";
      // 
      // frmMain
      // 
      AutoScaleDimensions = new SizeF(13F, 32F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(1643, 1273);
      Controls.Add(lblOtherSWMapsPointsCount);
      Controls.Add(lblOtherSWMapsFile);
      Controls.Add(btnOtherSWMapsToSW);
      Controls.Add(lblInstructions);
      Controls.Add(lblSWPointsCount);
      Controls.Add(lblSWMapsPointsCount);
      Controls.Add(lblSWPointsFile);
      Controls.Add(lblSWMapsFile);
      Controls.Add(btnConvertSWPointsToLatLon);
      Controls.Add(btnSWMapsToSW);
      Margin = new Padding(2);
      Name = "frmMain";
      Text = "Form1";
      Load += frmMain_Load;
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Button btnSWMapsToSW;
    private Button btnConvertSWPointsToLatLon;
    private LinkLabel lblSWMapsFile;
    private LinkLabel lblSWPointsFile;
    private Label lblSWMapsPointsCount;
    private Label lblSWPointsCount;
    private Label lblInstructions;
    private Button btnOtherSWMapsToSW;
    private LinkLabel lblOtherSWMapsFile;
    private Label lblOtherSWMapsPointsCount;
  }
}
