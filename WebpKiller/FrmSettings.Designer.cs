namespace WebpKiller;

partial class FrmSettings
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
        LbFolders = new ListBox();
        CbEnabled = new CheckBox();
        CbRecursive = new CheckBox();
        CbScanOnStartup = new CheckBox();
        BtnRemove = new Button();
        BtnAdd = new Button();
        BtnOk = new Button();
        BtnCancel = new Button();
        FBD = new FolderBrowserDialog();
        CbDeleteWebp = new CheckBox();
        CbShowConversionMsg = new CheckBox();
        SuspendLayout();
        // 
        // LbFolders
        // 
        LbFolders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        LbFolders.FormattingEnabled = true;
        LbFolders.Location = new Point(12, 12);
        LbFolders.Name = "LbFolders";
        LbFolders.Size = new Size(275, 199);
        LbFolders.TabIndex = 0;
        LbFolders.SelectedIndexChanged += LbFolders_SelectedIndexChanged;
        // 
        // CbEnabled
        // 
        CbEnabled.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        CbEnabled.AutoSize = true;
        CbEnabled.Location = new Point(293, 12);
        CbEnabled.Name = "CbEnabled";
        CbEnabled.Size = new Size(68, 19);
        CbEnabled.TabIndex = 3;
        CbEnabled.Text = "&Enabled";
        CbEnabled.UseVisualStyleBackColor = true;
        // 
        // CbRecursive
        // 
        CbRecursive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        CbRecursive.AutoSize = true;
        CbRecursive.Location = new Point(294, 62);
        CbRecursive.Name = "CbRecursive";
        CbRecursive.Size = new Size(142, 19);
        CbRecursive.TabIndex = 5;
        CbRecursive.Text = "&Include subdirectories";
        CbRecursive.UseVisualStyleBackColor = true;
        // 
        // CbScanOnStartup
        // 
        CbScanOnStartup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        CbScanOnStartup.AutoSize = true;
        CbScanOnStartup.Location = new Point(293, 37);
        CbScanOnStartup.Name = "CbScanOnStartup";
        CbScanOnStartup.Size = new Size(125, 19);
        CbScanOnStartup.TabIndex = 4;
        CbScanOnStartup.Text = "&Full scan at startup";
        CbScanOnStartup.UseVisualStyleBackColor = true;
        // 
        // BtnRemove
        // 
        BtnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        BtnRemove.Location = new Point(93, 226);
        BtnRemove.Name = "BtnRemove";
        BtnRemove.Size = new Size(75, 23);
        BtnRemove.TabIndex = 2;
        BtnRemove.Text = "&Remove";
        BtnRemove.UseVisualStyleBackColor = true;
        BtnRemove.Click += BtnRemove_Click;
        // 
        // BtnAdd
        // 
        BtnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        BtnAdd.Location = new Point(12, 226);
        BtnAdd.Name = "BtnAdd";
        BtnAdd.Size = new Size(75, 23);
        BtnAdd.TabIndex = 1;
        BtnAdd.Text = "&Add";
        BtnAdd.UseVisualStyleBackColor = true;
        BtnAdd.Click += BtnAdd_Click;
        // 
        // BtnOk
        // 
        BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnOk.Location = new Point(313, 226);
        BtnOk.Name = "BtnOk";
        BtnOk.Size = new Size(75, 23);
        BtnOk.TabIndex = 8;
        BtnOk.Text = "&Ok";
        BtnOk.UseVisualStyleBackColor = true;
        BtnOk.Click += BtnOk_Click;
        // 
        // BtnCancel
        // 
        BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnCancel.DialogResult = DialogResult.Cancel;
        BtnCancel.Location = new Point(394, 226);
        BtnCancel.Name = "BtnCancel";
        BtnCancel.Size = new Size(75, 23);
        BtnCancel.TabIndex = 9;
        BtnCancel.Text = "&Cancel";
        BtnCancel.UseVisualStyleBackColor = true;
        BtnCancel.Click += BtnCancel_Click;
        // 
        // FBD
        // 
        FBD.AutoUpgradeEnabled = false;
        FBD.Description = "Select folder to monitor";
        // 
        // CbDeleteWebp
        // 
        CbDeleteWebp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        CbDeleteWebp.AutoSize = true;
        CbDeleteWebp.Location = new Point(293, 87);
        CbDeleteWebp.Name = "CbDeleteWebp";
        CbDeleteWebp.Size = new Size(179, 19);
        CbDeleteWebp.TabIndex = 6;
        CbDeleteWebp.Text = "&Delete webp after conversion";
        CbDeleteWebp.UseVisualStyleBackColor = true;
        // 
        // CbShowConversionMsg
        // 
        CbShowConversionMsg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        CbShowConversionMsg.AutoSize = true;
        CbShowConversionMsg.Location = new Point(293, 112);
        CbShowConversionMsg.Name = "CbShowConversionMsg";
        CbShowConversionMsg.Size = new Size(170, 19);
        CbShowConversionMsg.TabIndex = 7;
        CbShowConversionMsg.Text = "Show conversion messages";
        CbShowConversionMsg.UseVisualStyleBackColor = true;
        // 
        // FrmSettings
        // 
        AcceptButton = BtnOk;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = BtnCancel;
        ClientSize = new Size(484, 261);
        Controls.Add(CbShowConversionMsg);
        Controls.Add(LbFolders);
        Controls.Add(CbDeleteWebp);
        Controls.Add(CbEnabled);
        Controls.Add(BtnCancel);
        Controls.Add(CbRecursive);
        Controls.Add(BtnOk);
        Controls.Add(CbScanOnStartup);
        Controls.Add(BtnAdd);
        Controls.Add(BtnRemove);
        MinimumSize = new Size(500, 300);
        Name = "FrmSettings";
        Text = "WebpKiller Settings";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ListBox LbFolders;
    private CheckBox CbEnabled;
    private CheckBox CbRecursive;
    private CheckBox CbScanOnStartup;
    private Button BtnCancel;
    private Button BtnOk;
    private Button BtnAdd;
    private Button BtnRemove;
    private FolderBrowserDialog FBD;
    private CheckBox CbDeleteWebp;
    private CheckBox CbShowConversionMsg;
}