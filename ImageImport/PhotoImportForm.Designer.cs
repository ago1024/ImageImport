namespace ImageImport
{
    partial class PhotoImportForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
			this.ScanDevices = new System.Windows.Forms.Button();
			this.DeviceList = new System.Windows.Forms.ListView();
			this.Import = new System.Windows.Forms.Button();
			this.Progress = new System.Windows.Forms.Label();
			this.TargetFolder = new System.Windows.Forms.TextBox();
			this.ItemList = new System.Windows.Forms.ListView();
			this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.source = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.target = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Log = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// ScanDevices
			// 
			this.ScanDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ScanDevices.Location = new System.Drawing.Point(12, 293);
			this.ScanDevices.Name = "ScanDevices";
			this.ScanDevices.Size = new System.Drawing.Size(308, 23);
			this.ScanDevices.TabIndex = 0;
			this.ScanDevices.Text = "Geräte suchen";
			this.ScanDevices.UseVisualStyleBackColor = true;
			this.ScanDevices.Click += new System.EventHandler(this.ScanDevices_Click);
			// 
			// DeviceList
			// 
			this.DeviceList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.DeviceList.HideSelection = false;
			this.DeviceList.Location = new System.Drawing.Point(12, 12);
			this.DeviceList.Name = "DeviceList";
			this.DeviceList.Size = new System.Drawing.Size(308, 275);
			this.DeviceList.TabIndex = 1;
			this.DeviceList.UseCompatibleStateImageBehavior = false;
			this.DeviceList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.DeviceList_ItemSelectionChanged);
			// 
			// Import
			// 
			this.Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.Import.Location = new System.Drawing.Point(12, 322);
			this.Import.Name = "Import";
			this.Import.Size = new System.Drawing.Size(308, 23);
			this.Import.TabIndex = 2;
			this.Import.Text = "Importieren";
			this.Import.UseVisualStyleBackColor = true;
			this.Import.Click += new System.EventHandler(this.Import_Click);
			// 
			// Progress
			// 
			this.Progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Progress.Location = new System.Drawing.Point(12, 521);
			this.Progress.Name = "Progress";
			this.Progress.Size = new System.Drawing.Size(990, 17);
			this.Progress.TabIndex = 3;
			// 
			// TargetFolder
			// 
			this.TargetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.TargetFolder.Location = new System.Drawing.Point(12, 351);
			this.TargetFolder.Name = "TargetFolder";
			this.TargetFolder.Size = new System.Drawing.Size(308, 20);
			this.TargetFolder.TabIndex = 4;
			this.TargetFolder.Text = "D:\\Pictures";
			// 
			// ItemList
			// 
			this.ItemList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ItemList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.name,
            this.source,
            this.target});
			this.ItemList.HideSelection = false;
			this.ItemList.Location = new System.Drawing.Point(327, 13);
			this.ItemList.Name = "ItemList";
			this.ItemList.Size = new System.Drawing.Size(675, 358);
			this.ItemList.TabIndex = 5;
			this.ItemList.UseCompatibleStateImageBehavior = false;
			this.ItemList.View = System.Windows.Forms.View.Details;
			// 
			// name
			// 
			this.name.Text = "Name";
			this.name.Width = 150;
			// 
			// source
			// 
			this.source.Text = "Quelle";
			this.source.Width = 250;
			// 
			// target
			// 
			this.target.Text = "Ziel";
			this.target.Width = 250;
			// 
			// Log
			// 
			this.Log.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Log.Location = new System.Drawing.Point(12, 377);
			this.Log.Multiline = true;
			this.Log.Name = "Log";
			this.Log.ReadOnly = true;
			this.Log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.Log.Size = new System.Drawing.Size(990, 141);
			this.Log.TabIndex = 6;
			// 
			// PhotoImportForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1014, 547);
			this.Controls.Add(this.Log);
			this.Controls.Add(this.ItemList);
			this.Controls.Add(this.TargetFolder);
			this.Controls.Add(this.Progress);
			this.Controls.Add(this.Import);
			this.Controls.Add(this.DeviceList);
			this.Controls.Add(this.ScanDevices);
			this.Name = "PhotoImportForm";
			this.Text = "PhotoImport";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ScanDevices;
        private System.Windows.Forms.ListView DeviceList;
        private System.Windows.Forms.Button Import;
        private System.Windows.Forms.Label Progress;
        private System.Windows.Forms.TextBox TargetFolder;
        private System.Windows.Forms.ListView ItemList;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader source;
        private System.Windows.Forms.ColumnHeader target;
		private System.Windows.Forms.TextBox Log;
	}
}

