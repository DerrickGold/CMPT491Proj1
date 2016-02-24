namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.locationTree = new System.Windows.Forms.TreeView();
            this.dateTree = new System.Windows.Forms.TreeView();
            this.productTree = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // locationTree
            // 
            this.locationTree.HideSelection = false;
            this.locationTree.Location = new System.Drawing.Point(27, 49);
            this.locationTree.Name = "locationTree";
            this.locationTree.Size = new System.Drawing.Size(163, 264);
            this.locationTree.TabIndex = 0;
            this.locationTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // dateTree
            // 
            this.dateTree.HideSelection = false;
            this.dateTree.Location = new System.Drawing.Point(395, 49);
            this.dateTree.Name = "dateTree";
            this.dateTree.Size = new System.Drawing.Size(173, 264);
            this.dateTree.TabIndex = 2;
            this.dateTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.dateTree_AfterSelect);
            // 
            // productTree
            // 
            this.productTree.HideSelection = false;
            this.productTree.Location = new System.Drawing.Point(209, 49);
            this.productTree.Name = "productTree";
            this.productTree.Size = new System.Drawing.Size(168, 264);
            this.productTree.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(218, 319);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 33);
            this.button1.TabIndex = 11;
            this.button1.Text = "Report Fixed Dates";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Location";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(392, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Time Frame";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(206, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Item";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(27, 399);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(541, 162);
            this.richTextBox1.TabIndex = 20;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 573);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.productTree);
            this.Controls.Add(this.dateTree);
            this.Controls.Add(this.locationTree);
            this.Name = "Form1";
            this.Text = "Data Warehouse";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView locationTree;
        private System.Windows.Forms.TreeView dateTree;
        private System.Windows.Forms.TreeView productTree;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox richTextBox1;

    }
}

