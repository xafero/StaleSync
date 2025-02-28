namespace StaleSync
{
	partial class MainForm
	{
		private System.ComponentModel.IContainer components = null;
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		private void InitializeComponent()
		{
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Text = "StaleSync Agent";
			this.Name = "MainForm";
		}
	}
}