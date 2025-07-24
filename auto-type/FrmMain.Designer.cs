namespace auto_type
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cmbGroups = new System.Windows.Forms.ComboBox();
            this.cmsGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAddGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.miRenameGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeleteGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.cmsButton = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAddButton = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditButton = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeleteButton = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnStopAutoTyping = new System.Windows.Forms.Button();
            this.cmsGroup.SuspendLayout();
            this.cmsButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbGroups
            // 
            this.cmbGroups.ContextMenuStrip = this.cmsGroup;
            this.cmbGroups.FormattingEnabled = true;
            this.cmbGroups.Location = new System.Drawing.Point(12, 8);
            this.cmbGroups.Name = "cmbGroups";
            this.cmbGroups.Size = new System.Drawing.Size(150, 21);
            this.cmbGroups.TabIndex = 0;
            this.cmbGroups.SelectedIndexChanged += new System.EventHandler(this.cmbGroups_SelectedIndexChanged);
            // 
            // cmsGroup
            // 
            this.cmsGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddGroup,
            this.miRenameGroup,
            this.miDeleteGroup});
            this.cmsGroup.Name = "cmsGroup";
            this.cmsGroup.Size = new System.Drawing.Size(154, 70);
            // 
            // miAddGroup
            // 
            this.miAddGroup.Name = "miAddGroup";
            this.miAddGroup.Size = new System.Drawing.Size(153, 22);
            this.miAddGroup.Text = "Add Group";
            this.miAddGroup.Click += new System.EventHandler(this.miAddGroup_Click);
            // 
            // miRenameGroup
            // 
            this.miRenameGroup.Name = "miRenameGroup";
            this.miRenameGroup.Size = new System.Drawing.Size(153, 22);
            this.miRenameGroup.Text = "Rename Group";
            this.miRenameGroup.Click += new System.EventHandler(this.miRenameGroup_Click);
            // 
            // miDeleteGroup
            // 
            this.miDeleteGroup.Name = "miDeleteGroup";
            this.miDeleteGroup.Size = new System.Drawing.Size(153, 22);
            this.miDeleteGroup.Text = "Delete Group";
            this.miDeleteGroup.Click += new System.EventHandler(this.miDeleteGroup_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.AutoScroll = true;
            this.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlButtons.ContextMenuStrip = this.cmsButton;
            this.pnlButtons.Location = new System.Drawing.Point(12, 35);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(360, 319);
            this.pnlButtons.TabIndex = 3;
            // 
            // cmsButton
            // 
            this.cmsButton.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddButton,
            this.miEditButton,
            this.miDeleteButton});
            this.cmsButton.Name = "cmsGroup";
            this.cmsButton.Size = new System.Drawing.Size(147, 70);
            // 
            // miAddButton
            // 
            this.miAddButton.Name = "miAddButton";
            this.miAddButton.Size = new System.Drawing.Size(146, 22);
            this.miAddButton.Text = "Add Button";
            this.miAddButton.Click += new System.EventHandler(this.miAddButton_Click);
            // 
            // miEditButton
            // 
            this.miEditButton.Name = "miEditButton";
            this.miEditButton.Size = new System.Drawing.Size(146, 22);
            this.miEditButton.Text = "Edit Button";
            this.miEditButton.Click += new System.EventHandler(this.miEditButton_Click);
            // 
            // miDeleteButton
            // 
            this.miDeleteButton.Name = "miDeleteButton";
            this.miDeleteButton.Size = new System.Drawing.Size(146, 22);
            this.miDeleteButton.Text = "Delete Button";
            this.miDeleteButton.Click += new System.EventHandler(this.miDeleteButton_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(286, 7);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(66, 23);
            this.btnSettings.TabIndex = 4;
            this.btnSettings.Text = "Settings...";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnStopAutoTyping
            // 
            this.btnStopAutoTyping.Location = new System.Drawing.Point(182, 7);
            this.btnStopAutoTyping.Name = "btnStopAutoTyping";
            this.btnStopAutoTyping.Size = new System.Drawing.Size(98, 23);
            this.btnStopAutoTyping.TabIndex = 5;
            this.btnStopAutoTyping.Text = "Stop Auto Typing";
            this.btnStopAutoTyping.UseVisualStyleBackColor = true;
            this.btnStopAutoTyping.Click += new System.EventHandler(this.btnStopAutoTyping_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.btnStopAutoTyping);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.cmbGroups);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "FrmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Auto Type v1.0";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.cmsGroup.ResumeLayout(false);
            this.cmsButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbGroups;
        private System.Windows.Forms.FlowLayoutPanel pnlButtons;
        private System.Windows.Forms.ContextMenuStrip cmsGroup;
        private System.Windows.Forms.ToolStripMenuItem miAddGroup;
        private System.Windows.Forms.ToolStripMenuItem miRenameGroup;
        private System.Windows.Forms.ToolStripMenuItem miDeleteGroup;
        private System.Windows.Forms.ContextMenuStrip cmsButton;
        private System.Windows.Forms.ToolStripMenuItem miAddButton;
        private System.Windows.Forms.ToolStripMenuItem miEditButton;
        private System.Windows.Forms.ToolStripMenuItem miDeleteButton;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnStopAutoTyping;
    }
}

